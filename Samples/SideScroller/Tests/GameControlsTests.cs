using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Platforms.Mocks;
using NUnit.Framework;

namespace SideScroller.Tests
{
	internal class GameControlsTests : TestWithMocksOrVisually
	{
		private void CreateGameControls()
		{
			gameControls = new GameControls();
		}

		private GameControls gameControls;

		[Test]
		public void TestAscendControls()
		{
			CreateGameControls();
			bool ascended = false;
			gameControls.Ascend += () => { ascended = true; };
			if (resolver.GetType() != typeof(MockResolver))
				return;
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.W, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(ascended);
		}

		[Test]
		public void TestSinkControls()
		{
			CreateGameControls();
			bool sinking = false;
			gameControls.Sink += () => { sinking = true; };
			if (resolver.GetType() != typeof(MockResolver))
				return;
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.S, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(sinking);
		}

		[Test]
		public void TestAccelerateControls()
		{
			CreateGameControls();
			bool accelerated = false;
			gameControls.Accelerate += () => { accelerated = true; };
			if (resolver.GetType() != typeof(MockResolver))
				return;
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.D, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(accelerated);
		}

		[Test]
		public void TestSlowDownControls()
		{
			CreateGameControls();
			bool slowingDown = false;
			gameControls.SlowDown += () => { slowingDown = true; };
			if (resolver.GetType() != typeof(MockResolver))
				return;
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.A, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(slowingDown);
		}

		[Test]
		public void TestShootingControls()
		{
			CreateGameControls();
			bool fireing = false;
			gameControls.Fire += () => { fireing = true; };
			gameControls.HoldFire += () => { fireing = false; };
			if (resolver.GetType() != typeof(MockResolver))
				return;
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.Space, State.Pressing);
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(fireing);
			keyboard.SetKeyboardState(Key.Space, State.Releasing);
			AdvanceTimeAndUpdateEntities();
			Assert.IsFalse(fireing);
		}
	}
}