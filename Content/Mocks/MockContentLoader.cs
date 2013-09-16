using System;
using System.Collections.Generic;
using System.IO;
using DeltaEngine.Commands;
using DeltaEngine.Content.Xml;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Rendering.Particles;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace DeltaEngine.Content.Mocks
{
	/// <summary>
	/// Loads mock content used in unit tests.
	/// </summary>
	public class MockContentLoader : ContentLoader
	{
		protected override Stream GetContentDataStream(ContentData content)
		{
			var stream = Stream.Null;
			if (content.MetaData.Type == ContentType.Xml && content.Name.Equals("Test"))
				stream = new XmlFile(new XmlData("Root").AddChild(new XmlData("Hi"))).ToMemoryStream();
			else if (content.MetaData.Type == ContentType.Xml && content.Name.Equals("Texts"))
				stream = new XmlFile(new XmlData("Texts").AddChild(GoLocalizationNode)).ToMemoryStream();
			else if (content.MetaData.Type == ContentType.InputCommand &&
				content.Name.Equals("DefaultCommands"))
				stream = CreateInputCommandXml().ToMemoryStream();
			else if (content.Name.Equals("Verdana12") || content.Name.Equals("Tahoma30"))
				stream = CreateFontXml().ToMemoryStream();
			else if (content.Name.Equals("TestParticle"))
				stream = SaveTestParticle();
			else if (content.MetaData.Type == ContentType.Shader)
				stream = SaveShader(content.Name);
			else if (content.Name.Equals("EmptyScene"))
				stream = SaveEmptyScene();
			else if (content.Name.Equals("SceneWithAButton"))
				stream = SaveSceneWithAButton();
			else if (content.Name.Equals("TestMenuXml"))
				stream = SaveTestMenu();
			return stream;
		}

		protected override bool HasValidContentAndMakeSureItIsLoaded()
		{
			return true;
		}

		private static XmlData GoLocalizationNode
		{
			get
			{
				var keyNode = new XmlData("Go");
				keyNode.AddAttribute("en", "Go");
				keyNode.AddAttribute("de", "Los");
				keyNode.AddAttribute("es", "¡vamos!");
				return keyNode;
			}
		}

		private static XmlFile CreateInputCommandXml()
		{
			var inputSettings = new XmlData("InputSettings");
			AddToInputCommandXml(inputSettings, Command.Exit, new MockCommand("KeyTrigger", "Escape"));
			AddToInputCommandsXml(inputSettings, Command.Click,
				new List<MockCommand>
				{
					new MockCommand("KeyTrigger", "Space"),
					new MockCommand("MouseButtonTrigger", "Left"),
					new MockCommand("TouchPressTrigger", ""),
					new MockCommand("GamePadButtonTrigger", "A")
				});
			AddToInputCommandXml(inputSettings, Command.MiddleClick,
				new MockCommand("MouseButtonTrigger", "Middle"));
			AddToInputCommandXml(inputSettings, Command.RightClick,
				new MockCommand("MouseButtonTrigger", "Right"));
			AddToInputCommandsXml(inputSettings, Command.MoveLeft,
				new List<MockCommand>
				{
					new MockCommand("KeyTrigger", "CursorLeft Pressed"),
					new MockCommand("KeyTrigger", "A"),
					new MockCommand("GamePadAnalogTrigger", "LeftThumbStick")
				});
			AddToInputCommandsXml(inputSettings, Command.MoveRight,
				new List<MockCommand>
				{
					new MockCommand("KeyTrigger", "CursorRight Pressed"),
					new MockCommand("KeyTrigger", "D"),
					new MockCommand("GamePadAnalogTrigger", "RightThumbStick")
				});
			AddToInputCommandXml(inputSettings, Command.MoveUp,
				new MockCommand("KeyTrigger", "CursorUp Pressed"));
			AddToInputCommandXml(inputSettings, Command.MoveDown,
				new MockCommand("KeyTrigger", "CursorDown Pressed"));
			AddToInputCommandsXml(inputSettings, Command.MoveDirectly,
				new List<MockCommand>
				{
					new MockCommand("MousePositionTrigger", "Left"),
					new MockCommand("TouchPressTrigger", "")
				});
			AddToInputCommandXml(inputSettings, Command.RotateDirectly,
				new MockCommand("GamePadAnalogTrigger", "RightThumbStick"));
			AddToInputCommandXml(inputSettings, Command.Back,
				new MockCommand("KeyTrigger", "Backspace Pressed"));
			AddToInputCommandXml(inputSettings, Command.Drag,
				new MockCommand("MouseDragTrigger", "Left Pressed"));
			AddToInputCommandXml(inputSettings, Command.Flick, new MockCommand("TouchFlickTrigger", ""));
			AddToInputCommandXml(inputSettings, Command.Pinch, new MockCommand("TouchPinchTrigger", ""));
			AddToInputCommandXml(inputSettings, Command.Hold, new MockCommand("TouchHoldTrigger", ""));
			AddToInputCommandXml(inputSettings, Command.DoubleClick,
				new MockCommand("MouseDoubleClickTrigger", "Left"));
			AddToInputCommandXml(inputSettings, Command.Rotate,
				new MockCommand("TouchRotateTrigger", ""));
			AddToInputCommandXml(inputSettings, Command.Zoom, new MockCommand("MouseZoomTrigger", ""));
			return new XmlFile(inputSettings);
		}

		private struct MockCommand
		{
			public MockCommand(string trigger, string command)
			{
				Trigger = trigger;
				Command = command;
			}

			public readonly string Trigger;
			public readonly string Command;
		}

		private static void AddToInputCommandXml(XmlData inputSettings, string commandName,
			MockCommand command)
		{
			var entry = new XmlData("Command").AddAttribute("Name", commandName);
			entry.AddChild(command.Trigger, command.Command);
			inputSettings.AddChild(entry);
		}

		private static void AddToInputCommandsXml(XmlData inputSettings, string commandName,
			IEnumerable<MockCommand> commands)
		{
			var entry = new XmlData("Command").AddAttribute("Name", commandName);
			foreach (var command in commands)
				entry.AddChild(command.Trigger, command.Command);
			inputSettings.AddChild(entry);
		}

		private static XmlFile CreateFontXml()
		{
			var glyph1 = new XmlData("Glyph");
			glyph1.AddAttribute("Character", ' ');
			glyph1.AddAttribute("UV", "0 0 1 16");
			glyph1.AddAttribute("AdvanceWidth", "7.34875");
			glyph1.AddAttribute("LeftBearing", "0");
			glyph1.AddAttribute("RightBearing", "4.21875");
			var glyph2 = new XmlData("Glyph");
			glyph2.AddAttribute("Character", 'a');
			glyph2.AddAttribute("UV", "0 0 1 16");
			glyph2.AddAttribute("AdvanceWidth", "7.34875");
			glyph2.AddAttribute("LeftBearing", "0");
			glyph2.AddAttribute("RightBearing", "4.21875");
			var glyphs = new XmlData("Glyphs").AddChild(glyph1).AddChild(glyph2);
			var kerningPair = new XmlData("Kerning");
			kerningPair.AddAttribute("First", " ");
			kerningPair.AddAttribute("Second", "a");
			kerningPair.AddAttribute("Distance", "1");
			var kernings = new XmlData("Kernings");
			kernings.AddChild(kerningPair);
			var bitmap = new XmlData("Bitmap");
			bitmap.AddAttribute("Name", "Verdana12Font");
			bitmap.AddAttribute("Width", "128");
			bitmap.AddAttribute("Height", "128");
			var font = new XmlData("Font");
			font.AddAttribute("Family", "Verdana");
			font.AddAttribute("Size", "12");
			font.AddAttribute("Style", "AddOutline");
			font.AddAttribute("LineHeight", "16");
			font.AddChild(bitmap).AddChild(glyphs).AddChild(kernings);
			return new XmlFile(font);
		}

		private static MemoryStream SaveTestParticle()
		{
			var emptyParticleEffect = new ParticleEmitterData();
			var shader = Load<Shader>(Shader.Position2DColorUv);
			emptyParticleEffect.ParticleMaterial = new Material(shader,
				Create<Image>(new ImageCreationData(new Size(8, 8))));
			emptyParticleEffect.Size = new RangeGraph<Size>(new Size(0.1f, 0.1f), new Size(0.1f, 0.1f));
			var data = BinaryDataExtensions.SaveToMemoryStream(emptyParticleEffect);
			data.Seek(0, SeekOrigin.Begin);
			return data;
		}

		private static MemoryStream SaveShader(string contentName)
		{
			var shader = new ShaderCreationData(ShaderWithFormat.UvVertexCode,
				ShaderWithFormat.UvFragmentCode, ShaderWithFormat.UvHlslCode,
				ShaderWithFormat.Dx9Position2DTexture, GetVertexFormatFromDefaultNames(contentName));
			var data = BinaryDataExtensions.SaveToMemoryStream(shader);
			data.Seek(0, SeekOrigin.Begin);
			return data;
		}

		private static VertexFormat GetVertexFormatFromDefaultNames(string contentName)
		{
			var format = VertexFormat.Position2DColorUv;
			if (contentName == Shader.Position2DUv)
				format = VertexFormat.Position2DUv;
			if (contentName == Shader.Position2DColor)
				format = VertexFormat.Position2DColor;
			if (contentName == Shader.Position3DColor)
				format = VertexFormat.Position3DColor;
			if (contentName == Shader.Position3DUv)
				format = VertexFormat.Position3DUv;
			if (contentName == Shader.Position3DColorUv)
				format = VertexFormat.Position3DColorUv;
			return format;
		}

		private static MemoryStream SaveEmptyScene()
		{
			var emptyScene = new Scene();
			var data = BinaryDataExtensions.SaveToMemoryStream(emptyScene);
			data.Seek(0, SeekOrigin.Begin);
			return data;
		}

		private static Stream SaveSceneWithAButton()
		{
			var scene = new Scene();
			scene.Controls.Add(new Button(Theme.Default, Rectangle.One, "Hello"));
			var data = BinaryDataExtensions.SaveToMemoryStream(scene);
			data.Seek(0, SeekOrigin.Begin);
			return data;
		}

		private static MemoryStream SaveTestMenu()
		{
			var emptyScene = new AutoArrangingMenu(Size.One);
			var data = BinaryDataExtensions.SaveToMemoryStream(emptyScene);
			data.Seek(0, SeekOrigin.Begin);
			return data;
		}

		protected override ContentMetaData GetMetaData(string contentName,
			Type contentClassType = null)
		{
			if (contentName.StartsWith("Unavailable"))
				return null;
			ContentType contentType = ConvertClassTypeToContentType(contentClassType);
			if (contentType == ContentType.Material)
				return CreateMaterialMetaData(contentName);
			if (contentName.Contains("SpriteSheet") || contentType == ContentType.SpriteSheetAnimation)
				return CreateSpriteSheetAnimationMetaData(contentName);
			if (contentName == "ImageAnimationNoImages")
				return CreateImageAnimationNoImagesMetaData(contentName);
			if (contentName == "ImageAnimation" || contentType == ContentType.ImageAnimation)
				return CreateImageAnimationMetaData(contentName);
			if (contentType == ContentType.Image)
				return CreateImageMetaData(contentName);
			if (contentType == ContentType.Model)
				return CreateModelMetaData(contentName);
			if (contentType == ContentType.Mesh)
				return CreateMeshMetaData(contentName);
			if (contentType == ContentType.Shader)
				return CreateShaderData(contentName);
			return new ContentMetaData { Name = contentName, Type = contentType };
		}

		private static ContentType ConvertClassTypeToContentType(Type contentClassType)
		{
			if (contentClassType == null)
				return ContentType.Xml;
			var typeName = contentClassType.Name;
			foreach (var contentType in EnumExtensions.GetEnumValues<ContentType>())
				if (contentType != ContentType.Image && contentType != ContentType.Mesh &&
					typeName.Contains(contentType.ToString()))
					return contentType;
			if (typeName.Contains("Image") || typeName.Contains("Texture"))
				return ContentType.Image;
			if (typeName.Contains("ModelData"))
				return ContentType.Model;
			if (typeName.Contains("Mesh"))
				return ContentType.Mesh;
			if (typeName.Contains("Geometry"))
				return ContentType.Geometry;
			return ContentType.Xml;
		}

		private static ContentMetaData CreateMaterialMetaData(string name)
		{
			var metaData = new ContentMetaData { Name = name, Type = ContentType.Material };
			if (!name.Contains("NoShader"))
				metaData.Values.Add("ShaderName", "Position2DUv");
			if (!name.Contains("NoImage"))
				if (name.Contains("ImageAnimation"))
					metaData.Values.Add("ImageOrAnimationName", "ImageAnimation");
				else if (name.Contains("SpriteSheet"))
					metaData.Values.Add("ImageOrAnimationName", "SpriteSheet");
				else
					metaData.Values.Add("ImageOrAnimationName", "DeltaEngineLogo");
			metaData.Values.Add("LightMapName", "lightMap");
			return metaData;
		}

		private static ContentMetaData CreateSpriteSheetAnimationMetaData(string name)
		{
			var metaData = new ContentMetaData { Name = name, Type = ContentType.SpriteSheetAnimation };
			metaData.Values.Add("ImageName", "EarthImages");
			metaData.Values.Add("DefaultDuration", "5.0");
			metaData.Values.Add("SubImageSize", "32,32");
			return metaData;
		}

		private static ContentMetaData CreateImageAnimationMetaData(string name)
		{
			var metaData = new ContentMetaData { Name = name, Type = ContentType.ImageAnimation };
			metaData.Values.Add("ImageNames", "ImageAnimation01,ImageAnimation02,ImageAnimation03");
			metaData.Values.Add("DefaultDuration", "3");
			return metaData;
		}

		private static ContentMetaData CreateImageAnimationNoImagesMetaData(string name)
		{
			var metaData = new ContentMetaData { Name = name, Type = ContentType.ImageAnimation };
			metaData.Values.Add("ImageNames", "");
			metaData.Values.Add("DefaultDuration", "3");
			return metaData;
		}

		private static ContentMetaData CreateImageMetaData(string contentName)
		{
			var metaData = new ContentMetaData { Name = contentName, Type = ContentType.Image };
			metaData.Values.Add("PixelSize", "128,128");
			return metaData;
		}

		private static ContentMetaData CreateModelMetaData(string contentName)
		{
			var metaData = new ContentMetaData { Name = contentName, Type = ContentType.Model };
			if (contentName == "InvalidModel")
				return metaData;
			metaData.Values.Add("MeshNames", "MockModel");
			return metaData;
		}

		private static ContentMetaData CreateMeshMetaData(string contentName)
		{
			var metaData = new ContentMetaData { Name = contentName, Type = ContentType.Mesh };
			metaData.Values.Add("GeometryName", "MockGeometry");
			metaData.Values.Add("MaterialName", "MockMaterial");
			if (contentName.Contains("Animation"))
				metaData.Values.Add("AnimationName", "MockAnimation");
			return metaData;
		}

		private static ContentMetaData CreateShaderData(string contentName)
		{
			var metaData = new ContentMetaData { Name = contentName, Type = ContentType.Shader };
			return metaData;
		}
	}
}