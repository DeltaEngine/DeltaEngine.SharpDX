using System;
using System.Threading;
using DeltaEngine.Core;
using DeltaEngine.Mocks;
using DeltaEngine.Networking.Tcp;
using Microsoft.Win32;
using NUnit.Framework;

namespace DeltaEngine.Logging.Tests
{
	[Category("Slow")]
	public class NetworkLoggerTests
	{
		//ncrunch: no coverage start
		[TestFixtureSetUp]
		public void StartLogServer()
		{
			server = new LocalhostLogServer(new TcpServer());
			server.Start();
			var ready = false;
			var connection = OnlineServiceConnection.CreateForEditor();
			connection.DataReceived += o => ready = true;
			connection.Connect("localhost", LocalhostLogServer.Port);
			logger = new NetworkLogger(connection);
			for (int timeoutMs = 1000; timeoutMs > 0 && !ready; timeoutMs -= 10)
				Thread.Sleep(10);
			Assert.IsTrue(ready);
		}

		private LocalhostLogServer server;

		public class ServerErrorReceived : Exception
		{
			public ServerErrorReceived(string error) : base(error) { }
		}

		private static string GetApiKeyFromRegistry()
		{
			string apiKey = "";
			using (var key = Registry.CurrentUser.OpenSubKey(@"Software\DeltaEngine\Editor", false))
				if (key != null)
					apiKey = (string)key.GetValue("ApiKey");
			if (string.IsNullOrEmpty(apiKey))
				throw new ApiKeyNotSetPleaseStartEditor();
			return apiKey;
		}

		private class ApiKeyNotSetPleaseStartEditor : Exception { }

		private NetworkLogger logger;

		[TestFixtureTearDown]
		public void ShutdownLogServer()
		{
			logger.Dispose();
			server.Dispose();
		}

		[Test]
		public void LogInfoMessage()
		{
			logger.Write(Logger.MessageType.Info, "Hello");
			ExpectThatServerHasReceivedMessage("Hello");
		}

		private void ExpectThatServerHasReceivedMessage(string messageText)
		{
			Assert.That(() => server.LastMessage.Text, Is.EqualTo(messageText).After(100, 5));
		}

		[Test]
		public void LogWarning()
		{
			logger.Write(Logger.MessageType.Warning, "Ohoh");
			ExpectThatServerHasReceivedMessage("Ohoh");
			logger.Write(Logger.MessageType.Warning, new NullReferenceException().ToString());
			ExpectThatServerLastMessageContains("NullReferenceException");
		}

		private void ExpectThatServerLastMessageContains(string messageText)
		{
			Assert.That(() => server.LastMessage.Text.Contains(messageText),
				Is.EqualTo(true).After(100, 5));
		}

		[Test]
		public void LogError()
		{
			logger.Write(Logger.MessageType.Error, new ArgumentException().ToString());
			Thread.Sleep(100);
			ExpectThatServerLastMessageContains("ArgumentException");
		}

		[Test, Ignore]
		public static void LogToRealLogServer()
		{
			var ready = false;
			OnlineServiceConnection.RememberCreationDataForAppRunner(GetApiKeyFromRegistry(),
				new MockSettings(), () => { }, error => { throw new ServerErrorReceived(error); },
				() => ready = true, () => { });
			using (var logClient = new NetworkLogger(new OnlineServiceConnection()))
			{
				for (int timeoutMs = 1000; timeoutMs > 0 && !ready; timeoutMs -= 10)
					Thread.Sleep(10);
				logClient.Write(Logger.MessageType.Info, "Hello TestWorld from " + Environment.MachineName);
			}
		}
	}
}