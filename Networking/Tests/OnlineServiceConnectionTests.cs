using System;
using System.Threading;
using DeltaEngine.Mocks;
using DeltaEngine.Networking.Tcp;
using Microsoft.Win32;
using NUnit.Framework;

namespace DeltaEngine.Networking.Tests
{
	[Ignore]
	public class OnlineServiceConnectionTests
	{
		//ncrunch: no coverage start
		[Test]
		public void ClientGetsConnectedAndSendsLoginRequest()
		{
			string errorReceived = "";
			bool readyReceived = false;
			OnlineServiceConnection.RememberCreationDataForAppRunner(GetApiKeyFromRegistry(),
				new MockSettings(), () => { }, text => errorReceived = text, () => readyReceived = true,
				() => { });
			var connection = new OnlineServiceConnection();
			Thread.Sleep(1000);
			Assert.IsTrue(connection.IsLoggedIn);
			Assert.AreEqual("", errorReceived);
			Assert.IsTrue(readyReceived);
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

		private class ApiKeyNotSetPleaseStartEditor : Exception {}

		[Test]
		public void ConnectToInvalidServerShouldTimeOut()
		{
			bool timedOut = false;
			var connection = OnlineServiceConnection.CreateForEditor();
			connection.TimedOut += () => timedOut = true;
			connection.Connect("localhost", 12345);
			Thread.Sleep(3500);
			Assert.IsTrue(timedOut);
			Assert.IsFalse(connection.IsLoggedIn);
		}

		[Test]
		public void ReceiveResultFromServer()
		{
			OnlineServiceConnection.RememberCreationDataForAppRunner(GetApiKeyFromRegistry(),
				new MockSettings(), () => { }, text => { }, () => { }, () => { });
			object lastMessageReceived = null;
			var connection = new OnlineServiceConnection();
			connection.DataReceived += message => lastMessageReceived = message;
			Assert.IsNull(lastMessageReceived);
			Thread.Sleep(1000);
			Assert.IsNotNull(lastMessageReceived);
		}
	}
}