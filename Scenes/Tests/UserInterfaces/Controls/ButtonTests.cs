using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class ButtonTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			button = new Button(Center, "Click Me");
			InitializeMouse();
			InitializeTouch();
			AdvanceTimeAndUpdateEntities();
		}

		private Button button;
		private static readonly Rectangle Center = Rectangle.FromCenter(0.5f, 0.5f, 0.3f, 0.1f);

		private void InitializeMouse()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse != null)
				mouse.SetPosition(Vector2D.Zero);
		}

		private MockMouse mouse;

		private void InitializeTouch()
		{
			var touch = Resolve<Touch>() as MockTouch;
			if (touch != null)
				touch.SetTouchState(0, State.Released, Vector2D.Zero);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderButtonWithRelativePosition()
		{
			button.Add(new List<FontText>
			{
				new FontText(Font.Default, "", new Rectangle(0.4f, 0.7f, 0.2f, 0.1f))
			});
			button.Start<UpdateTextWithRelativePosition>();
		}

		//ncrunch: no coverage start
		private class UpdateTextWithRelativePosition : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Button button in entities)
					button.Get<List<FontText>>()[0].Text = button.State.RelativePointerPosition.ToString();
			}
		} //ncrunch: no coverage end

		[Test]
		public void RenderOneButtonEnablingAndDisablingAnother()
		{
			var button2 = new Button(Rectangle.FromCenter(0.5f, 0.3f, 0.2f, 0.1f));
			button2.Clicked += () => button.IsEnabled = !button.IsEnabled;
		}

		[Test, CloseAfterFirstFrame]
		public void DefaultsToEnabled()
		{
			Assert.IsTrue(button.IsEnabled);
		}

		[Test, CloseAfterFirstFrame]
		public void BeginClickInside()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			SetMouseState(State.Pressing, Vector2D.Half);
			Assert.AreEqual(Color.LightBlue, button.Color);
		}

		private void SetMouseState(State state, Vector2D position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void BeginClickOutside()
		{
			SetMouseState(State.Pressing, Vector2D.One);
			Assert.IsFalse(button.State.IsPressed);
		}

		[Test, CloseAfterFirstFrame]
		public void BeginAndEndClickInside()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			bool clicked = false;
			button.Clicked += () => clicked = true;
			PressAndReleaseMouse(new Vector2D(0.53f, 0.52f), new Vector2D(0.53f, 0.52f));
			Assert.IsTrue(clicked);
			Assert.IsTrue(button.State.RelativePointerPosition.IsNearlyEqual(new Vector2D(0.6f, 0.7f)));
			Assert.AreEqual(Color.White, button.Color);
			Assert.IsFalse(button.State.IsPressed);
		}

		private void PressAndReleaseMouse(Vector2D pressPosition, Vector2D releasePosition)
		{
			SetMouseState(State.Pressing, pressPosition);
			SetMouseState(State.Releasing, releasePosition);
		}

		[Test, CloseAfterFirstFrame]
		public void BeginClickInsideAndEndOutside()
		{
			bool clicked = false;
			button.Clicked += () => clicked = true;
			PressAndReleaseMouse(Vector2D.Half, Vector2D.Zero);
			Assert.IsFalse(clicked);
			Assert.IsFalse(button.State.IsPressed);
		}

		[Test, CloseAfterFirstFrame]
		public void BeginClickOutsideAndEndInside()
		{
			bool clicked = false;
			button.Clicked += () => clicked = true;
			PressAndReleaseMouse(Vector2D.Zero, Vector2D.Half);
			Assert.IsFalse(clicked);
			Assert.IsFalse(button.State.IsPressed);
		}

		[Test, CloseAfterFirstFrame]
		public void DisabledControlDoesNotRespondToClick()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			button.IsEnabled = false;
			bool clicked = false;
			button.Clicked += () => clicked = true;
			PressAndReleaseMouse(new Vector2D(0.53f, 0.52f), new Vector2D(0.53f, 0.52f));
			Assert.IsFalse(clicked);
			Assert.IsFalse(button.State.IsInside);
			Assert.AreEqual(Vector2D.Zero, button.State.RelativePointerPosition);
			Assert.AreEqual(Color.Gray, button.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void HiddenControlDoesNotRespondToClick()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			button.IsVisible = false;
			bool clicked = false;
			button.Clicked += () => clicked = true;
			PressAndReleaseMouse(new Vector2D(0.53f, 0.52f), new Vector2D(0.53f, 0.52f));
			Assert.IsFalse(clicked);
			Assert.IsFalse(button.State.IsInside);
			Assert.AreEqual(Vector2D.Zero, button.State.RelativePointerPosition);
		}

		[Test]
		public void RenderButtonAttachedToMouse()
		{
			new Command(point => button.DrawArea = Rectangle.FromCenter(point, button.DrawArea.Size)).
				Add(new MouseMovementTrigger());
		}

		[Test, CloseAfterFirstFrame]
		public void LoadSceneWithAButton()
		{
			var loadedScene = ContentLoader.Load<Scene>("SceneWithAButton");
			var loadedButton = loadedScene.Controls[0] as Button;
			Assert.AreEqual(2, loadedButton.GetActiveBehaviors().Count);
			Assert.AreEqual("UpdateUVCalculations",
				loadedButton.GetActiveBehaviors()[0].GetShortNameOrFullNameIfNotFound());
			Assert.AreEqual("Interact",
				loadedButton.GetActiveBehaviors()[1].GetShortNameOrFullNameIfNotFound());
			Assert.AreEqual(1, loadedButton.GetDrawBehaviors().Count);
			Assert.AreEqual("SpriteBatchRenderer",
				loadedButton.GetDrawBehaviors()[0].GetShortNameOrFullNameIfNotFound());
		}
	}
}