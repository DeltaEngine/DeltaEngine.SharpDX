//using System;
//using System.Collections.Generic;
//using System.Linq;
//using CreepyTowers.Content;
//using DeltaEngine.Content;
//using DeltaEngine.Datatypes;
//using DeltaEngine.Entities;
//using DeltaEngine.Rendering2D.Fonts;
//using DeltaEngine.Rendering2D.Sprites;
//using DeltaEngine.Scenes;
//using DeltaEngine.Scenes.UserInterfaces.Controls;

//namespace CreepyTowers.GUI
//{
//	//TODO: remove!
//	public class CreateSceneFromXml : Scene
//	{
//		public CreateSceneFromXml(string xmlName, string[] messages)
//		{
//			sceneXml = xmlName;
//			dialogMessages = messages;
//			MessageCount = 0;
//			ReadXml();
//		}

//		private readonly string sceneXml;
//		private readonly string[] dialogMessages;
//		public int MessageCount { get; set; }

//		private void ReadXml()
//		{
//			InteractiveButtonList = new List<InteractiveButton>();
//			remap = new RemapCoordinates();
//			xmlParser = new UIXmlParser();
//			xmlParser.ParseXml(sceneXml, "MainMenu");
//			mouseIcons = new List<Sprite>();
//			foreach (var uiObject in xmlParser.UiObjectList)
//			{
//				var imageSize = remap.RemapCoordinateSpaces(uiObject.ObjectSize);
//				var centerPos = remap.RemapCoordinateSpaces(uiObject.Position);
//				var drawArea = Rectangle.FromCenter(centerPos, imageSize);
//				CreateUiElement(uiObject, drawArea);
//			}
//		}

//		public List<InteractiveButton> InteractiveButtonList { get; private set; }
//		private RemapCoordinates remap;
//		private UIXmlParser xmlParser;
//		private List<Sprite> mouseIcons;

//		private void CreateUiElement(UIObject uiObject, Rectangle drawArea)
//		{
//			var material = new Material(Shader.Position2DUV, uiObject.Name);
//			if (Enum.IsDefined(typeof(ComicStripImages), uiObject.Name))
//			{
//				var comicStripImage = new Sprite(material, drawArea);
//				comicStripImage.RenderLayer = (int)CreepyTowersRenderLayer.Dialogues;
//				comicStripImage.AddTag(uiObject.Name);
//				if (uiObject.Name.Equals(ComicStrips.ComicStripBubble.ToString()))
//					AddTextToScene();
//				Add(comicStripImage);
//			}
//			else if (uiObject.Name.Equals(ComicStrips.IconMouseLeft.ToString()) ||
//				uiObject.Name.Equals(ComicStrips.IconMouseRight.ToString()))
//			{
//				var mouse = new Sprite(material, drawArea);
//				mouse.RenderLayer = (int)CreepyTowersRenderLayer.Dialogues;
//				mouse.AddTag(uiObject.Name);
//				mouse.IsVisible = false;
//				mouseIcons.Add(mouse);
//				Add(mouse);
//			}
//			else
//			{
//				var button = new InteractiveButton(CreateTheme(uiObject.Name), drawArea)
//				{
//					RenderLayer = (int)CreepyTowersRenderLayer.Dialogues + 1
//				};
//				button.AddTag(uiObject.Name);
//				if (button.ContainsTag(Content.GUI.ButtonMessageBack.ToString()))
//					button.IsVisible = false;
//				if (Enum.IsDefined(typeof(Content.GUI), uiObject.Name))
//					button.IsVisible = false;
//				InteractiveButtonList.Add(button);
//				Add(button);
//			}
//		}

//		private void AddTextToScene()
//		{
//			var speechBubble =
//				(Sprite)EntitiesRunner.Current.GetEntitiesWithTag(ComicStrips.ComicStripBubble.ToString())[0];
//			messageText = new FontText(ContentLoader.Load<Font>(Fonts.ChelseaMarket14.ToString()),
//				dialogMessages[MessageCount], speechBubble.DrawArea)
//			{
//				Color = Color.Black,
//				RenderLayer = (int)CreepyTowersRenderLayer.Dialogues + 1
//			};
//			messageText.AddTag("Message");
//			Add(messageText);
//		}

//		private FontText messageText;


//		private void AddFadeoutEffects()
//		{
//		}

//		public void NextDialogue()
//		{
//			if (MessageCount > 1)
//				SetBackButtonState(true);
//			if (MessageCount == dialogMessages.Length)
//			{
//				AddFadeoutEffects();
//				return;
//			}
//			messageText.Text = dialogMessages[MessageCount];
//			DisplayMouseIcons(MessageCount);
//		}

//		private void SetBackButtonState(bool visibility)
//		{
//			foreach (InteractiveButton control in
//				Controls.Where(control => control.ContainsTag(Content.GUI.ButtonMessageBack.ToString())))
//			{
//				control.IsVisible = visibility;
//				break;
//			}
//		}

//		private void DisplayMouseIcons(int count)
//		{
//			if (mouseIcons.Count < 1)
//				return;
//			switch (count)
//			{
//			case 2:
//				mouseIcons[0].IsVisible = true;
//				mouseIcons[1].IsVisible = false;
//				break;
//			case 3:
//				mouseIcons[0].IsVisible = false;
//				mouseIcons[1].IsVisible = true;
//				break;
//			default:
//				foreach (Sprite icon in mouseIcons)
//					icon.IsVisible = false;
//				break;
//			}
//		}

//		public void DialogueBack()
//		{
//			messageText.Text = dialogMessages[--MessageCount];
//			DisplayMouseIcons(MessageCount);
//			if (MessageCount <= 1)
//				SetBackButtonState(false);
//		}

//		protected override void DisposeData()
//		{
//			if (messageText != null)
//				messageText.IsActive = false;
//			Clear();
//		}
//	}
//}