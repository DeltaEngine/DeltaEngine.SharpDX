using DeltaEngine.Datatypes;
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
			controlledPlayer = new PlayerPlane(Vector2D.Half);
			playerControls = new PlayerControls(controlledPlayer);
		}

		private PlayerControls playerControls;
		private PlayerPlane controlledPlayer;

		[Test]
		public void TestAscendControls()
		{
			CreateGameControls();
			var originalVeocity = controlledPlayer.Get<Velocity2D>().Velocity;
			if (resolver.GetType() != typeof(MockResolver))
				return;//ncrunch: no coverage
			var keyboard = (MockKeyboard)Resolve<Keyboard>();
			keyboard.SetKeyboardState(Key.W, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			Assert.Less(controlledPlayer.Get<Velocity2D>().Velocity.Y, originalVeocity.Y);
		}

		[Test]
		public void TestSinkControls()
		{
			CreateGameControls();
			var originalVeocity = controlledPlayer.Get<Velocity2D>().Velocity;
			if (resolver.GetType() != typeof(MockResolver))
				return;//ncrunch: no coverage
			var keyboard = (MockKeyboard)Resolve<Keyboard>();
			keyboard.SetKeyboardState(Key.S, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			Assert.Greater(controlledPlayer.Get<Velocity2D>().Velocity.Y, originalVeocity.Y);
		}

		[Test]
		public void TestAccelerateControls()
		{
			CreateGameControls();
			if (resolver.GetType() != typeof(MockResolver))
				return;//ncrunch: no coverage
			var keyboard = (MockKeyboard)Resolve<Keyboard>();
			keyboard.SetKeyboardState(Key.D, State.Pressed);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void TestSlowDownControls()
		{
			CreateGameControls();
			if (resolver.GetType() != typeof(MockResolver))
				return;//ncrunch: no coverage
			var keyboard = (MockKeyboard)Resolve<Keyboard>();
			keyboard.SetKeyboardState(Key.A, State.Pressed);
			AdvanceTimeAndUpdateEntities();
		}
	}
}