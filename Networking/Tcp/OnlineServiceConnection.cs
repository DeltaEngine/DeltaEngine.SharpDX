using System;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
using DeltaEngine.Networking.Messages;

namespace DeltaEngine.Networking.Tcp
{
	/// <summary>
	/// The Current property will connect only once and is used by DeveloperOnlineContentLoader and
	/// NetworkLogger. The Editor will create its own connection and manages the connecting itself.
	/// </summary>
	public class OnlineServiceConnection : TcpSocket
	{
		//ncrunch: no coverage start
		internal static void RememberCreationDataForAppRunner(string apiKey, Settings settings,
			Action timeout, Action<string> errorHappened, Action ready, Action dataReceived)
		{
			rememberApiKey = apiKey;
			rememberIp = settings.OnlineServiceIp;
			rememberPort = settings.OnlineServicePort;
			serverTimeout = timeout;
			serverErrorHappened = errorHappened;
			contentReady = ready;
			contentReceived = dataReceived;
			projectName = AssemblyExtensions.GetEntryAssemblyForProjectName();
		}

		private static string rememberApiKey;
		private static string rememberIp;
		private static int rememberPort;
		private static Action serverTimeout;
		private static Action<string> serverErrorHappened;
		public static Action contentReady;
		private static Action contentReceived;
		private static string projectName;

		internal static OnlineServiceConnection CreateForEditor()
		{
			var connection = new OnlineServiceConnection();
			connection.DataReceived += connection.OnDataReceived;
			return connection;
		}

		internal OnlineServiceConnection()
		{
			if (rememberApiKey == null)
				return;
			Connected += () => Send(new LoginRequest(rememberApiKey, projectName));
			TimedOut += serverTimeout;
			DataReceived += OnDataReceived;
			Connect(rememberIp, rememberPort);
		}

		private void OnDataReceived(object message)
		{
			var serverError = message as ServerError;
			var unknownMessage = message as UnknownMessage;
			var ready = message as ContentReady;
			var content = message as UpdateContent;
			if (serverError != null && serverErrorHappened != null)
				serverErrorHappened(serverError.Error);
			else if (unknownMessage != null && serverErrorHappened != null)
				serverErrorHappened(unknownMessage.Text);
			else if (message is LoginSuccessful)
			{
				IsLoggedIn = true;
				if (LoggedIn != null)
					LoggedIn();
			}
			else if (ready != null)
			{
				loadContentMetaData();
				if (contentReady != null)
					contentReady();
			}
			else if (content != null && contentReceived != null)
				contentReceived();
		}

		public bool IsLoggedIn { get; private set; }
		public Action loadContentMetaData;

		public bool IsDemoUser
		{
			get { return string.IsNullOrEmpty(rememberApiKey) || rememberApiKey == Guid.Empty.ToString(); }
		}

		public Action LoggedIn;
	}
}