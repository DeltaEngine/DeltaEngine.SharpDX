using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class MessagesTests : TestWithMocksOrVisually
	{
		[Test]
		public void CheckChildrensRoomMessages()
		{
			var msges = Messages.ChildsRoomMessages();
			Assert.AreEqual(10, msges.Length);
		}

		[Test]
		public void CheckBathRoomMessages()
		{
			var msges = Messages.BathRoomMessages();
			Assert.AreEqual(2, msges.Length);
		}

		[Test]
		public void CheckLivingRoommMessages()
		{
			var msges = Messages.LivingRoomMessages();
			Assert.AreEqual(2, msges.Length);
		}
	}
}