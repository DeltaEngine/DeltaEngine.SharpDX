using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace CreepyTowers
{
  public class IntroScene : Scene
  {
    public IntroScene()
    {
      count = 0;
      introImageList = new List<Sprite>();
      InitializeIntroScene();
      InitializeImagesAndSetVisibility();
    }

    private int count;
    private readonly List<Sprite> introImageList;

    private void InitializeIntroScene()
    {
      remap = new RemapCoordinates();
      xmlParser = new UIXmlParser();
      xmlParser.ParseXml(Names.XmlIntroScene, "IntroMenu");
      CreateIntroScene();
    }

    private RemapCoordinates remap;
    private UIXmlParser xmlParser;

    private void CreateIntroScene()
    {
      foreach (var uiObject in xmlParser.UIObjectList)
      {
        var imageSize = remap.RemapCoordinateSpaces(uiObject.ObjectSize);
        var centerPos = remap.RemapCoordinateSpaces(uiObject.Position);
        var drawArea = Rectangle.FromCenter(centerPos, imageSize);
        CreateScene(uiObject, drawArea);
      }
    }

    private void CreateScene(UIObject uiObject, Rectangle drawArea)
    {
      var button = new InteractiveButton(CreateTheme(uiObject.Name), drawArea)
      {
        RenderLayer = (int)CreepyTowersRenderLayer.Dialogues + 1
      };
      button.AddTag(uiObject.Name);

      if (button.ContainsTag(Names.ButtonIntroFlipRight))
        forwardButton = button;

      if (button.ContainsTag(Names.ButtonIntroFlipLeft))
        backButton = button;
        backButton.ToggleVisibility(Visibility.Hide);

      Add(button);
      AttachButtonEvents(uiObject.Name, button);
    }

    private InteractiveButton forwardButton;
    private InteractiveButton backButton;

    private static Theme CreateTheme(string buttonImageName)
    {
      var appearance = new Theme.Appearance(buttonImageName);
      return new Theme
      {
        Button = appearance,
        ButtonDisabled = new Theme.Appearance(buttonImageName, Color.Gray),
        ButtonMouseover = appearance,
        ButtonPressed = appearance
      };
    }

    private void AttachButtonEvents(string buttonName, InteractiveButton button)
    {
      switch (buttonName)
      {
      case Names.ButtonIntroFlipRight:
        button.Clicked += MoveIntroSceneForward;
        break;

      case Names.ButtonIntroFlipLeft:
        button.Clicked += MoveIntroSceneBackward;
        break;

      case Names.ButtonIntroSkip:
        button.Clicked += () =>
        {
          Dispose();
          //new MainMenu();
        };
        break;
      }
    }

    private void InitializeImagesAndSetVisibility()
    {
      firstImage =
        new Sprite(new Material(Shader.Position2DColorUv, Names.ComicStripsStoryboardPanel1),
          ScreenSpace.Current.Viewport);
      secondImage =
        new Sprite(new Material(Shader.Position2DColorUv, Names.ComicStripsStoryboardPanel2),
          ScreenSpace.Current.Viewport);
      secondImage.ToggleVisibility(Visibility.Hide);
      thirdImage =
        new Sprite(new Material(Shader.Position2DColorUv, Names.ComicStripsStoryboardPanel3),
          ScreenSpace.Current.Viewport);
      thirdImage.ToggleVisibility(Visibility.Hide);
      fourthImage =
        new Sprite(new Material(Shader.Position2DColorUv, Names.ComicStripsStoryboardPanel4),
          ScreenSpace.Current.Viewport);
      fourthImage.ToggleVisibility(Visibility.Hide);
      fifthImage =
        new Sprite(new Material(Shader.Position2DColorUv, Names.ComicStripsStoryboardPanel5),
          ScreenSpace.Current.Viewport);
      fifthImage.ToggleVisibility(Visibility.Hide);

      introImageList.Add(firstImage);
      introImageList.Add(secondImage);
      introImageList.Add(thirdImage);
      introImageList.Add(fourthImage);
      introImageList.Add(fifthImage);
    }

    private Sprite firstImage;
    private Sprite secondImage;
    private Sprite thirdImage;
    private Sprite fourthImage;
    private Sprite fifthImage;

    private void MoveIntroSceneForward()
    {
      if (count == introImageList.Count - 1)
        return;

      FadeOut();
      count++;
      EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver += FadeIn;
      ToggleButtonStatesMovingForward();
    }

    private void FadeOut()
    {
      ShowFadeEffect(Color.TransparentWhite, Color.Black);
    }

    private void ShowFadeEffect(Color startColor, Color endColor)
    {
      var image = introImageList[count];

      if (image.Contains<FadeEffect.TransitionData>())
        image.Remove<FadeEffect.TransitionData>();

      image.Add(new FadeEffect.TransitionData
      {
        Colour = new RangeGraph<Color>(startColor, endColor),
        Duration = 1.0f,
      });
      image.Start<FadeEffect>();
      EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver -= FadeIn;
    }

    private void FadeIn()
    {
      if (count < introImageList.Count - 1)
        introImageList[count + 1].ToggleVisibility(Visibility.Hide);
      ShowFadeEffect(Color.TransparentBlack, Color.White);
    }

    private void ToggleButtonStatesMovingForward()
    {
      if (count > 0 && count < introImageList.Count - 1)
        backButton.ToggleVisibility(Visibility.Show);
      else
        forwardButton.ToggleVisibility(Visibility.Hide);
    }

    private void MoveIntroSceneBackward()
    {
      if (count == 0)
        return;

      FadeOut();
      count--;
      EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver += FadeIn;
      ToggleButtonStatesMovingBackwards();
    }

    private void ToggleButtonStatesMovingBackwards()
    {
      if (count == introImageList.Count - 2)
      {
        forwardButton.ToggleVisibility(Visibility.Show);
        backButton.ToggleVisibility(Visibility.Show);
      }
      else if (count == 0)
      {
        forwardButton.ToggleVisibility(Visibility.Show);
        backButton.ToggleVisibility(Visibility.Hide);
      }
    }
  }
}