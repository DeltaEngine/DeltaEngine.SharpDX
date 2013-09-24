using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers.GUI
{
  public class Credits : Scene
  {
    public Credits()
    {
      remap = new RemapCoordinates();
      SetBackground(new Material(Shader.Position2DUv, Names.ImageCredits));
      CreateCreditsScene();
    }

    private readonly RemapCoordinates remap;

    private void CreateCreditsScene()
    {
      var creditsXmlParser = new UIXmlParser();
      creditsXmlParser.ParseXml(Names.XmlCreditsScene, "Credits");
      foreach (var uiObject in creditsXmlParser.UiObjectList)
      {
        var imageSize = remap.RemapCoordinateSpaces(uiObject.ObjectSize);
        var centerPos = remap.RemapCoordinateSpaces(uiObject.Position);
        var drawArea = Rectangle.FromCenter(centerPos, imageSize);
        var button = new InteractiveButton(CreateTheme(uiObject.Name), drawArea);
        button.Clicked += () =>
        {
          Dispose();
          new MainMenu();
        };
        Add(button);
      }
      Show();
    }

    private static Theme CreateTheme(string buttonImageName)
    {
      var appearance = new Theme.Appearance(buttonImageName);
      return new Theme
      {
        Button = appearance,
        ButtonMouseover = appearance,
        ButtonPressed = appearance,
        Font = ContentLoader.Load<Font>(Names.FontChelseaMarket14)
      };
    }

    protected override void DisposeData()
    {
      Clear();
    }
  }
}