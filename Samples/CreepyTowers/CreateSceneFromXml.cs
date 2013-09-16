using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers
{
	public class CreateSceneFromXml : Scene
	{
		public CreateSceneFromXml(string xmlName, string[] messages)
		{
			sceneXml = xmlName;
			dialogMessages = messages;
			MessageCount = 0;
			ReadXml();
		}

		private readonly string sceneXml;
		private readonly string[] dialogMessages;
		public int MessageCount { get; set; }

		private void ReadXml()
		{
			InteractiveButtonList = new List<InteractiveButton>();
			remap = new RemapCoordinates();
			xmlParser = new UIXmlParser();
			xmlParser.ParseXml(sceneXml, "MainMenu");
			mouseIcons = new List<Sprite>();
			foreach (var uiObject in xmlParser.UIObjectList)
			{
				var imageSize = remap.RemapCoordinateSpaces(uiObject.ObjectSize);
				var centerPos = remap.RemapCoordinateSpaces(uiObject.Position);
				var drawArea = Rectangle.FromCenter(centerPos, imageSize);
				CreateScene(uiObject, drawArea);
			}
		}

		public List<InteractiveButton> InteractiveButtonList { get; private set; }
		private RemapCoordinates remap;
		private UIXmlParser xmlParser;
		private List<Sprite> mouseIcons;
		//private List<Sprite> specialAttackUiImages;

		private void CreateScene(UIObject uiObject, Rectangle drawArea)
		{
			var material = new Material(Shader.Position2DUv, uiObject.Name);
			if (Names.ComicStripImages.Contains(uiObject.Name))
			{
				var comicStripImage = new Sprite(material, drawArea);
				comicStripImage.RenderLayer = (int)CreepyTowersRenderLayer.Dialogues;
				comicStripImage.AddTag(uiObject.Name);

				if (uiObject.Name.Equals(Names.ComicStripBubble))
					AddTextToScene();

				Add(comicStripImage);
			}
			else if (Names.UiImages.Contains(uiObject.Name))
			{
				var uiImage = new Sprite(material, drawArea);
				uiImage.RenderLayer = (int)CreepyTowersRenderLayer.Dialogues;
				uiImage.AddTag(uiObject.Name);
				Add(uiImage);
			}
				//else if (uiObject.Name.Equals(Names.ComicStripBubble))
				//{
				//	speechBubble = new Sprite(material, drawArea)
				//	{
				//		RenderLayer = (int)CreepyTowersRenderLayer.Dialogues
				//	};
				//	speechBubble.AddTag(uiObject.Name);
				//	AddTextToScene();
				//	Add(speechBubble);
				//}
			else if (uiObject.Name.Equals(Names.IconMouseLeft) || uiObject.Name.Equals(Names.IconMouseRight))
			{
				var mouse = new Sprite(material, drawArea);
				mouse.RenderLayer = (int)CreepyTowersRenderLayer.Dialogues;
				mouse.AddTag(uiObject.Name);
				mouse.Visibility = Visibility.Hide;
				mouseIcons.Add(mouse);
				Add(mouse);
			}
			else
			{
				var button = new InteractiveButton(CreateTheme(uiObject.Name), drawArea)
				{
					RenderLayer = (int)CreepyTowersRenderLayer.Dialogues + 1
				};
				button.AddTag(uiObject.Name);

				if (button.ContainsTag(Names.ButtonBackLeft))
					button.Visibility = Visibility.Hide;

				if (Names.UiButtons.Contains(uiObject.Name))
					button.Visibility = Visibility.Hide;

				InteractiveButtonList.Add(button);
				Add(button);
			}
		}

		//private Sprite speechBubble;

		private void AddTextToScene()
		{
			var speechBubble =
				(Sprite)EntitiesRunner.Current.GetEntitiesWithTag(Names.ComicStripBubble)[0];
			messageText = new FontText(ContentLoader.Load<Font>(Names.FontChelseaMarket14),
				dialogMessages[MessageCount], speechBubble.DrawArea)
			{
				Color = Color.Black,
				RenderLayer = (int)CreepyTowersRenderLayer.Dialogues + 1
			};
			messageText.AddTag("Message");
			Add(messageText);
		}

		private FontText messageText;

		private static Theme CreateTheme(string buttonImageName)
		{
			var appearance = new Theme.Appearance(buttonImageName);
			return new Theme
			{
				Button = appearance,
				ButtonDisabled = new Theme.Appearance(buttonImageName, Color.Gray),
				ButtonMouseover = appearance,
				ButtonPressed = appearance,
				Font = ContentLoader.Load<Font>(Names.FontChelseaMarket14)
			};
		}

		public void NextDialogue()
		{
			if (MessageCount > 1)
				SetBackButtonState(Visibility.Show);

			if (MessageCount == dialogMessages.Length)
			{
				AddFadeoutEffects();
				return;
			}

			messageText.Text = dialogMessages[MessageCount];
			DisplayMouseIcons(MessageCount);
		}

		private void SetBackButtonState(Visibility state)
		{
			foreach (InteractiveButton control in
				Controls.Where(control => control.ContainsTag(Names.ButtonBackLeft)))
			{
				control.Visibility = state;
				break;
			}
		}

		private void AddFadeoutEffects()
		{
			//foreach (Entity2D control in
			//	Controls.Cast<Entity2D>().Where(control => control.IsActive))
			//{
			//	if (!Names.ComicStripImages.Contains(control.GetTags()[0]))
			//		continue;

			//	if (control.ContainsTag(Names.ButtonNext) || control.ContainsTag(Names.ButtonBackLeft))
			//		((InteractiveButton)control).IsEnabled = false;

			//	control.Add(new Transition.Duration(0.5f)).Add(new Transition.FadingColor(Color.DarkGray));
			//	control.Start<Transition>();
			//	control.Start<FinalTransition>();
			//}
		}

		private void DisplayMouseIcons(int count)
		{
			if (mouseIcons.Count < 1)
				return;

			switch (count)
			{
			case 2:
				mouseIcons[0].Visibility = Visibility.Show;
				mouseIcons[1].Visibility = Visibility.Hide;
				break;

			case 3:
				mouseIcons[0].Visibility = Visibility.Hide;
				mouseIcons[1].Visibility = Visibility.Show;
				break;

			default:
				foreach (Sprite icon in mouseIcons)
					icon.Visibility = Visibility.Hide;
				break;
			}
		}

		public void DialogueBack()
		{
			messageText.Text = dialogMessages[--MessageCount];
			DisplayMouseIcons(MessageCount);

			if (MessageCount <= 1)
				SetBackButtonState(Visibility.Hide);
		}

		public new void Dispose()
		{
			if (messageText != null)
				messageText.IsActive = false;

			Clear();
		}
	}
}