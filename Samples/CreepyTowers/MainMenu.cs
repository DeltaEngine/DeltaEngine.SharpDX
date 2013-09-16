using System;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers
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
			foreach (var uiObject in xmlParser.UIObjectList)
			{
				var image = ContentLoader.Load<Image>(uiObject.Name);
				var imageSize = remap.RemapCoordinateSpaces(uiObject.ObjectSize);
				var centerPos = remap.RemapCoordinateSpaces(uiObject.Position);
				var drawArea = Rectangle.FromCenter(centerPos, imageSize);
				CreateScene(uiObject, drawArea, image);
			}
			Show();
		}

		private void CreateScene(UIObject uiObject, Rectangle drawArea, Image image)
		{
			var material = new Material( Shader.Position2DUv, uiObject.Name);
			if (uiObject.Name.Equals(Names.ImageLogo))
			{
				var gameLogo = new Sprite(material, drawArea);
				gameLogo.AddTag(uiObject.Name);
				Add(gameLogo);
			}
			else if (uiObject.Name.Equals(Names.ImageMenuKid))
			{
				var kid = new Sprite(material, drawArea);
				kid.AddTag(uiObject.Name);
				Add(kid);
			}
			else
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
			return new Theme
			{
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
				button.Clicked += StartTutorial;
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

			case Names.ButtonCreditsMenuBack:
				button.Clicked += () =>
				{
					Dispose();
					new MainMenu();
				};
				break;
			}
		}

		private void StartTutorial()
		{
			Dispose();
			ChildsRoom = new LevelChildsRoom();
		}

		public Manager manager;
		public LevelChildsRoom ChildsRoom { get; private set; }

		private void DisplayOptions()
		{
			//Dispose();
		}

		private void DisplayCredits()
		{
			Dispose();
			CreateCreditsScene();
		}

		private void CreateCreditsScene()
		{
			SetBackground(new Material(Shader.Position2DUv,Names.ImageCredits));
			var creditsXmlParser = new UIXmlParser();
			creditsXmlParser.ParseXml(Names.XmlCreditsScene, "MainMenu");
			foreach (var uiObject in creditsXmlParser.UIObjectList)
			{
				var size = uiObject.ObjectSize;
				var position = uiObject.Position;
				var imageSize = remap.RemapCoordinateSpaces(size);
				var centerPos = remap.RemapCoordinateSpaces(position);
				var drawArea = Rectangle.FromCenter(centerPos, imageSize);

				var button = new InteractiveButton(CreateTheme(uiObject.Name), drawArea);
				AttachButtonEvents(uiObject.Name, button);
				Add(button);
			}
			Show();
		}

		private void ExitGame()
		{
			Dispose();
			Close();
			Game.EndGame();
		}

		public void Close()
		{
			escCommand.IsActive = false;
			Clear();
		}
	}
}