using System.Collections.Generic;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Networking.Tests
{
	public class MessagingTests : TestWithMocksOrVisually
	{
		[SetUp, CloseAfterFirstFrame]
		public void SetUp()
		{
			serverSession = Messaging.StartSession(Port);
			clientSession = Messaging.JoinSession("localhost", Port);
		}

		private MessagingSession serverSession;
		private MessagingSession clientSession;
		private const int Port = 12345;

		[Test]
		public void UniqueIDsAssigned()
		{
			Assert.AreEqual(0, serverSession.UniqueID);
			Assert.AreEqual(1, clientSession.UniqueID);
		}

		[Test]
		public void ServerReportsOneParticipant()
		{
			Assert.AreEqual(1, serverSession.NumberOfParticipants);
		}

		[Test]
		public void ClientReportsParticipantsNotCalculated()
		{
			Assert.AreEqual(ClientMessagingSession.NumberOfParticipantsNotCalculated,
				clientSession.NumberOfParticipants);
		}

		[Test]
		public void NoMessagesInitially()
		{
			List<MessagingSession.Message> messages = serverSession.GetMessages();
			Assert.AreEqual(0, messages.Count);
			Assert.AreEqual(0, clientSession.GetMessages().Count);
		}

		[Test]
		public void SendMessagesServerToClient()
		{
			serverSession.SendMessage("hello");
			serverSession.SendMessage(9);
			List<MessagingSession.Message> messages = clientSession.GetMessages();
			Assert.AreEqual(2, messages.Count);
			VerifyMessageContents(messages[0], 0, "hello");
			VerifyMessageContents(messages[1], 0, 9);
		}

		private static void VerifyMessageContents(MessagingSession.Message message, int uniqueID,
			object data)
		{
			Assert.AreEqual(uniqueID, message.SenderUniqueID);
			Assert.AreEqual(data, message.Data);
		}

		[Test]
		public void MessagesAreOnlyReceivedOnce()
		{
			serverSession.SendMessage("hello");
			clientSession.SendMessage("hello");
			Assert.AreEqual(1, serverSession.GetMessages().Count);
			Assert.AreEqual(0, serverSession.GetMessages().Count);
			Assert.AreEqual(1, clientSession.GetMessages().Count);
			Assert.AreEqual(0, clientSession.GetMessages().Count);
		}

		[Test]
		public void SendMessagesClientToServer()
		{
			clientSession.SendMessage("hi");
			clientSession.SendMessage(1.2f);
			List<MessagingSession.Message> messages = serverSession.GetMessages();
			Assert.AreEqual(2, messages.Count);
			VerifyMessageContents(messages[0], 1, "hi");
			VerifyMessageContents(messages[1], 1, 1.2f);
		}

		[Test]
		public void WithTwoClientsWhenOneClientMessagesTheServerItIsEchoedToTheOtherClient()
		{
			var clientSession2 = Messaging.JoinSession("", Port);
			clientSession2.SendMessage("welcome");
			List<MessagingSession.Message> messages = clientSession.GetMessages();
			Assert.AreEqual(1, serverSession.GetMessages().Count);
			Assert.AreEqual(1, messages.Count);
			VerifyMessageContents(messages[0], 2, "welcome");
			Assert.AreEqual(2, serverSession.NumberOfParticipants);
		}

		[Test]
		public void DisconnectedClientNoLongerReceivesMessagesFromServer()
		{
			clientSession.Disconnect();
			serverSession.SendMessage("hello?");
			Assert.AreEqual(0, clientSession.GetMessages().Count);
		}

		[Test]
		public void DisconnectedServerNoLongerReceivesMessagesFromClients()
		{
			serverSession.Disconnect();
			clientSession.SendMessage("are you there?");
			Assert.AreEqual(0, serverSession.GetMessages().Count);
		}

		[Test]
		public void WhenTwoServersExistMessagesAreSentToTheCorrectOne()
		{
			MessagingSession serverSession2 = Messaging.StartSession(Port + 1);
			MessagingSession clientSession2 = Messaging.JoinSession("", Port + 1);
			clientSession.SendMessage("first");
			clientSession2.SendMessage("second");
			Assert.AreEqual("first", serverSession.GetMessages()[0].Data);
			Assert.AreEqual("second", serverSession2.GetMessages()[0].Data);
		}
	}
}