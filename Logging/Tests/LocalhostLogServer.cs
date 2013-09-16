using System;
using DeltaEngine.Networking;
using DeltaEngine.Networking.Messages;

namespace DeltaEngine.Logging.Tests
{
	internal class LocalhostLogServer : IDisposable
	{
		public LocalhostLogServer(Server server)
		{
			this.server = server;
			server.ClientDataReceived += OnDataReceived;
		}

		private void OnDataReceived(Client client, object message)
		{
			var logInfo = message as LogInfoMessage;
			if (message is LoginRequest)
				client.Send(new LoginSuccessful("localhost"));
			if (logInfo != null)
				LastMessage = logInfo;
		}

		private readonly Server server;

		public LogInfoMessage LastMessage { get; private set; }

		public void Start()
		{
			server.Start(Port);
		}

		public const int Port = 800;

		public void Dispose()
		{
			server.Dispose();
		}
	}
}