using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class InteractiveButtonTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			button = new InteractiveButton(Center, "Click Me");
			InitializeMouse();
			InitializeTouch();
			AdvanceTimeAndUpdateEntities();
		}

		private InteractiveButton button;
		private static readonly Size BaseSize = new Size(0.3f, 0.1f);
		private static readonly Rectangle Center = Rectangle.FromCenter(Point.Half, BaseSize);

		private void InitializeMouse()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse != null)
				mouse.SetPosition(Point.Zero);
		}

		private MockMouse mouse;

		private void InitializeTouch()
		{
			var touch = Resolve<Touch>() as MockTouch;
			if (touch != null)
				touch.SetTouchState(0, State.Released, Point.Zero);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderInteractiveButton() {}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderDisabledInteractiveButton()
		{
			button.IsEnabled = false;
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(Color.Gray, button.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void DefaultsToEnabled()
		{
			Assert.IsTrue(button.IsEnabled);
			Assert.AreEqual(Color.LightGray, button.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeBaseSize()
		{
			Assert.AreEqual(BaseSize, button.BaseSize);
			button.BaseSize = Size.Half;
			Assert.AreEqual(Size.Half, button.BaseSize);
		}

		[Test, CloseAfterFirstFrame]
		public void BeginningClickMakesItShrink()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			SetMouseState(State.Pressing, Point.Half);
			Assert.IsTrue(button.Size.Width < BaseSize.Width);
		}

		private void SetMouseState(State state, Point position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void FinishingClickMakesItGrow()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			SetMouseState(State.Pressing, Point.Half);
			SetMouseState(State.Releasing, Point.Half);
			Assert.IsTrue(button.Size.Width > BaseSize.Width);
		}

		[Test, CloseAfterFirstFrame]
		public void EnteringMakesItGrow()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			SetMouseState(State.Released, Point.Half);
			Assert.IsTrue(button.Size.Width > BaseSize.Width);
		}

		[Test, CloseAfterFirstFrame]
		public void ExitingMakesItNormalize()
		{
			SetMouseState(State.Released, Point.Half);
			SetMouseState(State.Released, Point.Zero);
			Assert.AreEqual(BaseSize, button.Size);
		}

		[Test, CloseAfterFirstFrame]
		public void BeginningClickDoesNothingIfDisabled()
		{
			button.IsEnabled = false;
			SetMouseState(State.Pressing, Point.Half);
			Assert.AreEqual(BaseSize.Width, button.Size.Width);
		}

		[Test]
		public void RenderInteractiveButtonAttachedToMouse()
		{
			new Command(point => button.DrawArea = Rectangle.FromCenter(point, button.DrawArea.Size)).
				Add(new MouseMovementTrigger());
		}
	}
}