using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Sprites;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace $safeprojectname$.GUI
{
	public class Hud : Scene
	{
		public Hud()
		{
			remap = new RemapCoordinates();
			CreateHud();
		}

		private readonly RemapCoordinates remap;
		private UIXmlParser hudXmlParser;

		private void CreateHud()
		{
			hudXmlParser = new UIXmlParser();
			hudXmlParser.ParseXml(Names.XmlGameHud, "GameHud");
			foreach (var uiObject in hudXmlParser.UiObjectList)
			{
				var image = ContentLoader.Load<Image>(uiObject.Name);
				var imageSize = remap.RemapCoordinateSpaces(uiObject.ObjectSize);
				var centerPos = remap.RemapCoordinateSpaces(uiObject.Position);
				var drawArea = Rectangle.FromCenter(centerPos, imageSize);
				CreateUiElement(uiObject, drawArea, image);
			}
		}

		private void CreateUiElement(UIObject uiObject, Rectangle drawArea, Image image)
		{
			var material = new Material(Shader.Position2DUv, uiObject.Name);
			if (uiObject.Type.Equals("Interactable"))
			{
				var button = new InteractiveButton(CreateTheme(uiObject.Name), drawArea);
				button.AddTag(uiObject.Name);
				Add(button);
				AttachButtonEvents(uiObject.Name, button);
			}
			var sprite = new Sprite(material, drawArea);
			sprite.AddTag(uiObject.Name);
			Add(sprite);
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
				case Names.UIOptions:
					button.Clicked += DisplayOptions;
					break;
				case Names.UIDragonSpecialAttackBreath:
					button.Clicked += DisplayOptions;
					break;
				case Names.UIDragonSpecialAttackCannon:
					button.Clicked += DisplayOptions;
					break;
			}
		}

		private void DisplayOptions()
		{
		}
	}
}