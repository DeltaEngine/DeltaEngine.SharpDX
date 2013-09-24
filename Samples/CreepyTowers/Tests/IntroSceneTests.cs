using CreepyTowers.GUI;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Sprites;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
  public class IntroSceneTests : TestWithMocksOrVisually
  {
    [Test]
    public void ClickToStartIntroScreen()
    {
      Resolve<Window>();
      introScreen =
        new Sprite(new Material(Shader.Position2DColorUv, "ComicStripsStoryboardPanel1"),
          ScreenSpace.Current.Viewport);
      new Command(StartFadeOutEffect).Add(new MouseButtonTrigger(MouseButton.Left,
        State.Releasing));
    }

    private Sprite introScreen;

    private void StartFadeOutEffect()
    {
      introScreen.Add(new FadeEffect.TransitionData
      {
        Colour = new RangeGraph<Color>(Color.TransparentWhite, Color.Black),
        Duration = 1.0f,
      });
      introScreen.Start<FadeEffect>();
    }

    [Test]
    public void StartNextImageFadeEffectAfterPreviousEnds()
    {
      Resolve<Window>();
      introScreen =
        new Sprite(new Material(Shader.Position2DColorUv, "ComicStripsStoryboardPanel1"),
          ScreenSpace.Current.Viewport);
      new Command(StartFadeOutEffect).Add(new MouseButtonTrigger(MouseButton.Left,
        State.Releasing));
      EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver += StartFadeInEffect;
    }

    private static void StartFadeInEffect()
    {
      var image1 =
        new Sprite(new Material(Shader.Position2DColorUv, "ComicStripsStoryboardPanel2"),
          ScreenSpace.Current.Viewport);
      image1.ToggleVisibility(Visibility.Hide);
      image1.Add(new FadeEffect.TransitionData
      {
        Colour = new RangeGraph<Color>(Color.TransparentBlack, Color.White),
        Duration = 1.0f,
      });
      image1.Start<FadeEffect>();
      EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver -= StartFadeInEffect;
    }

    [Test]
    public void PlayIntroScene()
    {
      new Game(Resolve<Window>(), Resolve<Device>());
      //window.ViewportPixelSize = new Size(1920, 1080);
      new IntroScene();
    }

    [Test]
    public void FadeOutButton()
    {
      button = new InteractiveButton(CreateTheme(Names.ButtonIntroFlipLeft),
        ScreenSpace.Current.Viewport);
      button.UpdatePriority = Priority.Last;
      new Command(StartButtonFade).Add(new MouseButtonTrigger(MouseButton.Right,
        State.Releasing));
    }

    private InteractiveButton button;

    private void StartButtonFade()
    {
      button.Add(new FadeEffect.TransitionData
      {
        Colour = new RangeGraph<Color>(Color.TransparentWhite, Color.Black),
        Duration = 1.0f,
      });
      button.Start<FadeEffect>();
    }

    private static Theme CreateTheme(string buttonImageName)
    {
      var appearance = new Theme.Appearance(buttonImageName);
      return new Theme
      {
        Button = appearance,
        ButtonDisabled = new Theme.Appearance(buttonImageName, Color.Gray),
        ButtonMouseover = appearance,
        ButtonPressed = appearance
        //Font = ContentLoader.Load<Font>(Names.FontChelseaMarket14)
      };
    }
  }
}