using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class MouseTapTriggerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse != null)
				mouse.SetPosition(Vector2D.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		private MockMouse mouse;

		[Test, CloseAfterFirstFrame]
		public void Tap()
		{
			bool wasTapped = false;
			new Command(() => wasTapped = true).Add(new MouseTapTrigger(MouseButton.Left));
			SetMouseState(State.Pressing, Vector2D.Half);
			Assert.IsFalse(wasTapped);
			SetMouseState(State.Releasing, Vector2D.Half);
			Assert.IsTrue(wasTapped);
		}

		private void SetMouseState(State state, Vector2D position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}
	}
}
