using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
using DeltaEngine.Networking.Messages;

namespace DeltaEngine.Networking.Tcp
{
	/// <summary>
	/// Socket for a network connection.
	/// </summary>
	public class TcpSocket : Client
	{
		//ncrunch: no coverage start
		public TcpSocket()
			: this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {}

		public TcpSocket(Socket nativeSocket)
		{
			this.nativeSocket = nativeSocket;
			buffer = new byte[ReceiveBufferSize];
			dataCollector = new DataCollector();
			dataCollector.ObjectFinished += OnObjectFinished;
			isDisposed = false;
			Timeout = DefaultTimeout;
		}

		protected readonly Socket nativeSocket;
		private readonly byte[] buffer;
		/// <summary>
		/// Receive up to 8 low level packets with 1440 bytes (1500 is MTU -40 bytes for TCP/IP -20
		/// bytes for different needs, e.g. VPN uses some bytes). Bigger messages will be put together
		/// inside the TryReceiveBytes via <see cref="DataCollector" />. Most messages are small.
		/// </summary>
		internal const int ReceiveBufferSize = 1440 * 8;

		private readonly DataCollector dataCollector;
		private bool isDisposed;
		public float Timeout { get; set; }
		private const float DefaultTimeout = 3.0f;

		private void OnObjectFinished(MessageData dataContainer)
		{
			using (var dataStream = new MemoryStream(dataContainer.Data))
			using (var dataReader = new BinaryReader(dataStream))
			{
				object receivedMessage;
				try
				{
					receivedMessage = dataReader.Create();
				}
				catch (Exception ex)
				{
					receivedMessage =
						new ServerError(StackTraceExtensions.FormatExceptionIntoClickableMultilineText(ex));
				}
				if (DataReceived != null)
					DataReceived(receivedMessage);
				else
					throw new NobodyIsUsingTheDataReceivedEvent(receivedMessage);
			}
		}

		public event Action<object> DataReceived;

		private class NobodyIsUsingTheDataReceivedEvent : Exception
		{
			public NobodyIsUsingTheDataReceivedEvent(object receivedMessage)
				: base(receivedMessage.ToString()) {}
		}

		public void Connect(string serverAddress, int serverPort)
		{
			connectionTargetAddress = serverAddress + ":" + serverPort;
			try
			{
				TryConnect(NetworkExtensions.ToEndPoint(serverAddress, serverPort));
			}
			catch (SocketException)
			{
				Logger.Warning("An error has occurred when trying to request a connection " +
					"to the server (" + connectionTargetAddress + ")");
				if (TimedOut != null)
					TimedOut();
				Dispose();
			}
		}

		private string connectionTargetAddress;

		private void TryConnect(EndPoint targetAddress)
		{
			var socketArgs = new SocketAsyncEventArgs { RemoteEndPoint = targetAddress };
			socketArgs.Completed += SocketConnectionComplete;
			nativeSocket.ConnectAsync(socketArgs);
			if (TimedOut != null)
				ThreadExtensions.Start(() =>
				{
					Thread.Sleep((int)(Timeout * 1000));
					if (IsConnected)
						return;
					TimedOut();
					Dispose();
				});
		}

		private void SocketConnectionComplete(object sender,
			SocketAsyncEventArgs socketAsyncEventArgs)
		{
			lock (syncObject)
			{
				if (socketAsyncEventArgs.SocketError != SocketError.Success)
					return;
				WaitForData();
				if (Connected != null)
					Connected();
				if (IsConnected)
					TrySendAllMessagesInTheQueue();
			}
		}

		public event Action Connected;
		public event Action TimedOut;

		public void Send(object data)
		{
			try
			{
				SendOrEnqueueData(data);
			}
			catch (SocketException)
			{
				Dispose();
			}
		}

		private void SendOrEnqueueData(object data)
		{
			lock (syncObject)
			{
				if (IsConnected)
					SendDataThroughNativeSocket(data);
				else
					messages.Enqueue(data);
			}
		}

		private readonly Queue<object> messages = new Queue<object>();
		private readonly Object syncObject = new Object();

		private void SendDataThroughNativeSocket(object message)
		{
			if (nativeSocket == null || isDisposed)
				throw new SocketException();
			var byteData = BinaryDataExtensions.ToByteArrayWithLengthHeader(message);
			int numberOfSendBytes = nativeSocket.Send(byteData);
			if (numberOfSendBytes == 0)
				throw new SocketException();
			if (numberOfSendBytes != byteData.Length)
				Logger.Warning("Failed to send message " + message + ", numberOfSendBytes=" +
					numberOfSendBytes + ", messageLength=" + byteData.Length);
		}

		private void TrySendAllMessagesInTheQueue()
		{
			try
			{
				while (messages.Count > 0)
					SendDataThroughNativeSocket(messages.Dequeue());
			}
			catch (Exception ex)
			{
				Logger.Warning("Failed to send all messages in queue: " + ex.Message);
			}
		}

		public void WaitForData()
		{
			if (isDisposed)
				return;
			try
			{
				nativeSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivingBytes, null);
			}
			catch (SocketException)
			{
				Console.WriteLine("An error has occurred when setting the socket to receive data");
			}
		}

		private void ReceivingBytes(IAsyncResult asyncResult)
		{
			if (isDisposed)
				return;
			try
			{
				TryReceiveBytes(asyncResult);
			}
			catch (SocketException)
			{
				Dispose();
			}
			catch (ObjectDisposedException)
			{
				Dispose();
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		private void TryReceiveBytes(IAsyncResult asyncResult)
		{
			int numberOfReceivedBytes = nativeSocket.EndReceive(asyncResult);
			if (numberOfReceivedBytes == 0)
				throw new SocketException();
			dataCollector.ReadBytes(buffer, 0, numberOfReceivedBytes);
			WaitForData();
		}

		public string TargetAddress
		{
			get { return IsConnected ? nativeSocket.RemoteEndPoint.ToString() : connectionTargetAddress; }
		}

		public bool IsConnected
		{
			get { return nativeSocket != null && nativeSocket.Connected; }
		}

		public virtual void Dispose()
		{
			if (isDisposed)
				return;
			isDisposed = true;
			nativeSocket.Close();
			if (Disconnected != null)
				Disconnected();
		}

		public event Action Disconnected;

		public int UniqueID { get; set; }
	}
}