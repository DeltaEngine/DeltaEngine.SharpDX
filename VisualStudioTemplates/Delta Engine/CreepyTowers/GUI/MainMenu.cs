using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Sprites;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace $safeprojectname$.GUI
{
	public class MainMenu : Scene
	{
		public MainMenu()
		{
			remap = new RemapCoordinates();
			AddEscCommand();
			AddMenuBackground();
			AddMenuItems();
		}

		private readonly RemapCoordinates remap;

		private void AddEscCommand()
		{
			escCommand = new Command(ExitGame).Add(new KeyTrigger(Key.Escape, State.Pressed));
		}

		private Command escCommand;

		private void AddMenuBackground()
		{
			SetBackground(new Material(Shader.Position2DUv, Names.BackgroundMainMenu));
		}

		private void AddMenuItems()
		{
			var xmlParser = new UIXmlParser();
			xmlParser.ParseXml(Names.XmlMenuScene, "MainMenu");
			foreach (var uiObject in xmlParser.UiObjectList)
			{
				var image = ContentLoader.Load<Image>(uiObject.Name);
				var imageSize = remap.RemapCoordinateSpaces(uiObject.ObjectSize);
				var centerPos = remap.RemapCoordinateSpaces(uiObject.Position);
				var drawArea = Rectangle.FromCenter(centerPos, imageSize);
				CreateUiElement(uiObject, drawArea, image);
			}
			Show();
		}

		private void CreateUiElement(UIObject uiObject, Rectangle drawArea, Image image)
		{
			var material = new Material(Shader.Position2DUv, uiObject.Name);
			if (uiObject.Name.Equals(Names.ImageLogo))
			{
				var gameLogo = new Sprite(material, drawArea);
				gameLogo.AddTag(uiObject.Name);
				Add(gameLogo);
			} else if (uiObject.Name.Equals(Names.ImageMenuKid))
			{
				var kid = new Sprite(material, drawArea);
				kid.AddTag(uiObject.Name);
				Add(kid);
			} else
			{
				var button = new InteractiveButton(CreateTheme(uiObject.Name), drawArea);
				button.AddTag(uiObject.Name);
				Add(button);
				AttachButtonEvents(uiObject.Name, button);
			}
		}

		private static Theme CreateTheme(string buttonImageName)
		{
			var appearance = new Theme.Appearance(buttonImageName);
			return new Theme {
				Button = appearance,
				ButtonMouseover = appearance,
				ButtonPressed = appearance,
				Font = ContentLoader.Load<Font>(Names.FontChelseaMarket14)
			};
		}

		private void AttachButtonEvents(string buttonName, InteractiveButton button)
		{
			switch (buttonName)
			{
				case Names.ButtonMainMenuPlay:
					button.Clicked += StartIntro;
					break;
				case Names.ButtonMainMenuHelpAndOptions:
					button.Clicked += DisplayOptions;
					break;
				case Names.ButtonMainMenuCredits:
					button.Clicked += DisplayCredits;
					break;
				case Names.ButtonMainMenuQuit:
					button.Clicked += ExitGame;
					break;
			}
		}

		private void StartIntro()
		{
			Dispose();
			new IntroScene();
		}

		public Manager manager;

		private void DisplayOptions()
		{
		}

		private void DisplayCredits()
		{
			Dispose();
			new Credits();
		}

		private void ExitGame()
		{
			Dispose();
			Game.EndGame();
		}

		protected override void DisposeData()
		{
			escCommand.IsActive = false;
			Clear();
		}
	}
}