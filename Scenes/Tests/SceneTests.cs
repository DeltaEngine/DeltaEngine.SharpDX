using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests
{
	public class SceneTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			scene = new Scene();
			material = new Material(Shader.Position2DColorUV, "DeltaEngineLogo");
			window = Resolve<Window>();
		}

		private Scene scene;
		private Material material;
		private Window window;

		[Test, CloseAfterFirstFrame]
		public void LoadSceneWithoutAnyControls()
		{
			var loadedScene = ContentLoader.Load<Scene>("EmptyScene");
			Assert.AreEqual("EmptyScene", loadedScene.Name);
			Assert.AreEqual(0, loadedScene.Controls.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void LoadSceneWithAButton()
		{
			//TODO: Theme is ContentData and all control saving and loading bugs have been fixed (lots of little tests needed in each class)
			var loadedScene = ContentLoader.Load<Scene>("SceneWithAButton");
			Assert.AreEqual("SceneWithAButton", loadedScene.Name);
			Assert.AreEqual(1, loadedScene.Controls.Count);
			Assert.AreEqual(typeof(Button), loadedScene.Controls[0].GetType());
		}

		[Test, CloseAfterFirstFrame]
		public void AddingControlAddsToListOfControls()
		{
			Assert.AreEqual(0, scene.Controls.Count);
			var control = new EmptyControl();
			scene.Add(control);
			Assert.AreEqual(1, scene.Controls.Count);
			Assert.AreEqual(control, scene.Controls[0]);
		}

		[Test, CloseAfterFirstFrame]
		public void AddingListOfControl()
		{
			Assert.AreEqual(0, scene.Controls.Count);
			var controls = new List<EmptyControl> { new EmptyControl(), new EmptyControl() };
			scene.Add(controls);
			Assert.AreEqual(2, scene.Controls.Count);
		}

		private class EmptyControl : Entity2D
		{
			public EmptyControl()
				: base(Rectangle.Zero) {}
		}

		[Test, CloseAfterFirstFrame]
		public void AddingControlTwiceOnlyAddsItOnce()
		{
			var control = new EmptyControl();
			scene.Add(control);
			scene.Add(control);
			Assert.AreEqual(1, scene.Controls.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void AddingControlToActiveSceneActivatesIt()
		{
			var label = new Sprite(material, Rectangle.One);
			scene.Show();
			scene.Add(label);
			Assert.IsTrue(label.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void AddingControlToInactiveSceneDeactivatesIt()
		{
			var label = new Sprite(material, Rectangle.One) { IsActive = true };
			scene.Hide();
			scene.Add(label);
			Assert.IsFalse(label.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void RemovingControlRemovesFromListOfControls()
		{
			var label = new Sprite(material, Rectangle.One);
			scene.Add(label);
			scene.Remove(label);
			Assert.AreEqual(0, scene.Controls.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void RemovingControlDeactivatesIt()
		{
			var label = new Sprite(material, Rectangle.One) { IsActive = true };
			scene.Add(label);
			scene.Remove(label);
			Assert.IsFalse(label.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void ClearingControlsDeactivatesThem()
		{
			var label = new Sprite(material, Rectangle.One) { IsActive = true };
			var control = new EmptyControl { IsActive = true };
			scene.Add(label);
			scene.Add(control);
			scene.Clear();
			Assert.AreEqual(0, scene.Controls.Count);
			Assert.IsFalse(label.IsActive);
			Assert.IsFalse(control.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void HidingSceneDeactivatesControls()
		{
			var label = new Sprite(material, Rectangle.One) { IsActive = true };
			var control = new EmptyControl { IsActive = true };
			scene.Add(label);
			scene.Add(control);
			scene.Hide();
			scene.Hide();
			Assert.AreEqual(2, scene.Controls.Count);
			Assert.IsFalse(label.IsActive);
			Assert.IsFalse(control.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void ControlsDontRespondToInputWhenSceneIsHidden()
		{
			var button = CreateButton();
			scene.Add(button);
			scene.Hide();
			SetMouseState(State.Pressing, Vector2D.Half);
			Assert.AreEqual(NormalColor, button.Color);
		}

		private static readonly Color NormalColor = Color.LightGray;

		private static Button CreateButton()
		{
			var theme = new Theme
			{
				Button = new Theme.Appearance("DeltaEngineLogo", NormalColor),
				ButtonMouseover = new Theme.Appearance("DeltaEngineLogo", MouseoverColor),
				ButtonPressed = new Theme.Appearance("DeltaEngineLogo", PressedColor)
			};
			return new Button(theme, Small);
		}

		private static readonly Color MouseoverColor = Color.White;
		private static readonly Color PressedColor = Color.Red;

		private void SetMouseState(State state, Vector2D position)
		{
			Resolve<MockMouse>().SetPosition(position);
			Resolve<MockMouse>().SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void ControlsDontRespondToInputWhenInBackground()
		{
			var button = CreateButton();
			scene.Add(button);
			scene.ToBackground();
			SetMouseState(State.Pressing, Vector2D.Half);
			Assert.AreEqual(NormalColor, button.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void ControlsRespondToInputWhenBroughtBackToForeground()
		{
			var button = CreateButton();
			scene.Add(button);
			scene.ToBackground();
			scene.ToForeground();
			SetMouseState(State.Pressing, Vector2D.Half);
			Assert.AreEqual(PressedColor, button.Color);
		}

		[Test]
		public void DrawButtonWhichChangesColorAndSize()
		{
			var button = CreateButton();
			button.Start<ChangeSizeDynamically>();
			scene.Add(button);
		}

		private static readonly Rectangle Small = Rectangle.FromCenter(0.5f, 0.5f, 0.3f, 0.1f);
		private static readonly Rectangle Big = Rectangle.FromCenter(0.5f, 0.5f, 0.36f, 0.12f);

		private class ChangeSizeDynamically : UpdateBehavior
		{
			public ChangeSizeDynamically()
				: base(Priority.Low) {}

			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities.OfType<Control>())
					if (entity.State.IsInside && !entity.State.IsPressed)
						entity.Set(Big);
					else
						entity.Set(Small);
			}
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeBackgroundImage()
		{
			Assert.AreEqual(0, scene.Controls.Count);
			var background = new Material(Shader.Position2DColorUV, "SimpleMainMenuBackground");
			scene.SetQuadraticBackground(background);
			Assert.AreEqual(1, scene.Controls.Count);
			Assert.AreEqual(background, ((Sprite)scene.Controls[0]).Material);
			var logo = new Material(Shader.Position2DColorUV, "DeltaEngineLogo");
			scene.SetQuadraticBackground(logo);
			Assert.AreEqual(1, scene.Controls.Count);
			Assert.AreEqual(logo, ((Sprite)scene.Controls[0]).Material);
		}

		[Test]
		public void BackgroundImageChangesWhenButtonClicked()
		{
			scene.SetQuadraticBackground("SimpleSubMenuBackground");
			var button = CreateButton();
			button.Clicked += () => scene.SetQuadraticBackground("SimpleMainMenuBackground");
			scene.Add(button);
		}

		[Test]
		public void SetQuadraticBackgroundOnSquareWindow()
		{
			window.ViewportPixelSize = new Size(512, 512);
			scene.SetQuadraticBackground("CheckerboardImage512x512");
		}

		[Test]
		public void SetQuadraticBackgroundOnLandscapeWindow()
		{
			window.ViewportPixelSize = new Size(512, 288);
			scene.SetQuadraticBackground("CheckerboardImage512x512");
		}

		[Test]
		public void SetQuadraticBackgroundOnPortraitWindow()
		{
			window.ViewportPixelSize = new Size(288, 512);
			scene.SetQuadraticBackground("CheckerboardImage512x512");
		}

		[Test]
		public void SetLandscapeViewportBackgroundOnLandscapeWindow()
		{
			window.ViewportPixelSize = new Size(512, 288);
			scene.SetViewportBackground("CheckerboardImage512x288");
		}

		[Test]
		public void SetPortraitViewportBackgroundOnPortraitWindow()
		{
			window.ViewportPixelSize = new Size(288, 512);
			scene.SetViewportBackground("CheckerboardImage288x512");
		}
	}
}