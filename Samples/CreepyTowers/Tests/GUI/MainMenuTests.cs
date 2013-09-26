using CreepyTowers.GUI;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
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
      Menu.SetBackground(new Material(Shader.Position2DUv, "Background"));
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
        Menu.Controls[0].GetType().ToString());
    }

    [Test]
    public void AddDummyActiveButtonToScene()
    {
      Initialize(Resolve<Window>());
      var drawArea = Rectangle.FromCenter(Vector2D.Half, new Size(0.3f, 0.1f));
      var button = new InteractiveButton(CreateTheme("DeltaEngineLogo"), drawArea);
      Menu.Clear();
      Menu.Add(button);
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
      Menu.Clear();
      Assert.AreEqual(0, Menu.Controls.Count);
    }

    [Test]
    public void CreateMainMenu()
    {
      new Game(Resolve<Window>(), Resolve<Device>());
      new MainMenu();
    }
  }
}