using System.Linq;
using CreepyTowers.GUI;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace CreepyTowers.Tests.GUI
{
	public class MainMenuTests : TestWithMocksOrVisually
	{
		[Test]
		public void AddDummyBackgroundToScene()
		{
			Initialize(Resolve<Window>());
			Menu.scene.SetQuadraticBackground(new Material(Shader.Position2DUV, "Background"));
		}

		private void Initialize(Window w)
		{
			window = w;
			Menu = new MainMenu();
		}

		private Window window;
		private MainMenu Menu { get; set; }

		[Test]
		public void CheckMenuBackground()
		{
			Initialize(Resolve<Window>());
			Assert.AreEqual("DeltaEngine.Rendering2D.Sprites.Sprite",
				Menu.scene.Controls[0].GetType().ToString());
		}

		[Test]
		public void AddDummyActiveButtonToScene()
		{
			Initialize(Resolve<Window>());
			var drawArea = Rectangle.FromCenter(Vector2D.Half, new Size(0.3f, 0.1f));
			var button = new InteractiveButton(CreateTheme("DeltaEngineLogo"), drawArea);
			Menu.scene.Clear();
			Menu.scene.Add(button);
		}

		private static Theme CreateTheme(string buttonImageName)
		{
			var appearance = new Theme.Appearance(buttonImageName);
			return new Theme
			{
				Button = appearance,
				ButtonMouseover = appearance,
				ButtonPressed = appearance
			};
		}

		[Test]
		public void CheckDisposeClearsScene()
		{
			Initialize(Resolve<Window>());
			Menu.scene.Clear();
			Assert.AreEqual(0, Menu.scene.Controls.Count);
		}

		[Test]
		public void CreateMainMenu()
		{
			new Game(Resolve<Window>());
			new MenuManager();
			MenuManager.Current.AddScene(ContentLoader.Load<Scene>(""));
			MenuManager.Current.SetCurrentScene(MenuManager.Current.GetScenesOfType<MainMenu>().First());
			MenuManager.Current.AddScene(new Credits().scene);
			MenuManager.Current.AddScene(new OptionMenu().scene);
		}
	}
}