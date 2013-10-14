using System;
using System.IO;
using System.Text;
using DeltaEngine.Content.Xml;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering3D.Particles;
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
			else if (content.Name.Contains("Verdana12") || content.Name.Contains("Tahoma30"))
				stream = CreateFontXml().ToMemoryStream();
			else if (content.MetaData.Type == ContentType.ParticleEmitter)
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
			var particleEmitterData = new ParticleEmitterData();
			var shader = Load<Shader>(Shader.Position2DColorUV);
			particleEmitterData.ParticleMaterial = new Material(shader,
				Create<Image>(new ImageCreationData(new Size(8, 8))));
			particleEmitterData.Size = new RangeGraph<Size>(new Size(0.1f, 0.1f));
			particleEmitterData.MaximumNumberOfParticles = 256;
			particleEmitterData.LifeTime = 5.0f;
			particleEmitterData.SpawnInterval = 0.1f;
			particleEmitterData.Color = new RangeGraph<Color>(Color.Red, Color.Green);
			var data = BinaryDataExtensions.SaveToMemoryStream(particleEmitterData);
			data.Seek(0, SeekOrigin.Begin);
			return data;
		}

		private static MemoryStream SaveShader(string contentName)
		{
			var shader = new ShaderCreationData(ShaderCodeOpenGL.PositionUVOpenGLVertexCode,
				ShaderCodeOpenGL.PositionUVOpenGLFragmentCode, ShaderCodeDX11.PositionUVDX11,
				ShaderCodeDX9.Position2DUVDX9, GetVertexFormatFromDefaultNames(contentName));
			var data = BinaryDataExtensions.SaveToMemoryStream(shader);
			data.Seek(0, SeekOrigin.Begin);
			return data;
		}

		private static VertexFormat GetVertexFormatFromDefaultNames(string contentName)
		{
			var format = VertexFormat.Position2DColorUV;
			if (contentName == Shader.Position2DUV)
				format = VertexFormat.Position2DUV;
			if (contentName == Shader.Position2DColor)
				format = VertexFormat.Position2DColor;
			if (contentName == Shader.Position3DColor)
				format = VertexFormat.Position3DColor;
			if (contentName == Shader.Position3DUV)
				format = VertexFormat.Position3DUV;
			if (contentName == Shader.Position3DColorUV)
				format = VertexFormat.Position3DColorUV;
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

		public override ContentMetaData GetMetaData(string contentName, Type contentClassType = null)
		{
			if (IsNoMetaDataAllowed(contentName, contentClassType))
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
			if (contentType == ContentType.Video)
				return CreateVideoData(contentName);
			return new ContentMetaData { Name = contentName, Type = contentType };
		}

		private static bool IsNoMetaDataAllowed(string contentName, Type classOfContent)
		{
			return contentName == "DefaultCommands" || contentName.StartsWith("Unavailable") ||
				contentName.StartsWith("NoData") ||
				(classOfContent != null && classOfContent.Name.StartsWith("NoData"));
		}

		private static ContentType ConvertClassTypeToContentType(Type contentClassType)
		{
			if (contentClassType == null)
				return ContentType.Xml;
			var typeName = contentClassType.Name;
			if (typeName.Contains("ImageAnimation"))
				return ContentType.ImageAnimation;
			if (typeName.Contains("Image") || typeName.Contains("Texture"))
				return ContentType.Image;
			if (typeName.Contains("Sound"))
				return ContentType.Sound;
			if (typeName.Contains("Font"))
				return ContentType.Font;
			if (typeName.Contains("Xml"))
				return ContentType.Xml;
			if (typeName.Contains("InputCommands"))
				return ContentType.InputCommand;
			if (typeName.Contains("Material"))
				return ContentType.Material;
			if (typeName.Contains("Music"))
				return ContentType.Music;
			if (typeName.Contains("Video"))
				return ContentType.Video;
			if (typeName.Contains("MeshAnimation"))
				return ContentType.MeshAnimation; // ncrunch: no coverage (slow test)
			if (typeName.Contains("Mesh"))
				return ContentType.Mesh;
			if (typeName.Contains("Geometry"))
				return ContentType.Geometry;
			if (typeName.Contains("ModelData"))
				return ContentType.Model;
			foreach (var contentType in EnumExtensions.GetEnumValues<ContentType>())
				if (contentType != ContentType.Image && contentType != ContentType.Mesh &&
					typeName.Contains(contentType.ToString()))
					return contentType;
			return ContentType.Xml;
		}

		private static ContentMetaData CreateMaterialMetaData(string name)
		{
			var metaData = new ContentMetaData { Name = name, Type = ContentType.Material };
			if (!name.Contains("NoShader"))
				AddShaderNameToMetaData(metaData);
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

		private static void AddShaderNameToMetaData(ContentMetaData metaData)
		{
			metaData.Values.Add("ShaderName",
				metaData.Name.EndsWith("2D") ? "Position2DUV" : "Position3DUV");
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
			if (contentName.Contains("ParticleFire"))
				metaData.Values.Add("BlendMode", "Additive");
			return metaData;
		}

		private static ContentMetaData CreateModelMetaData(string contentName)
		{
			var metaData = new ContentMetaData { Name = contentName, Type = ContentType.Model };
			if (contentName == "InvalidModel")
				return metaData;
			metaData.Values.Add("MeshNames", "Mock" + contentName);
			return metaData;
		}

		private static ContentMetaData CreateMeshMetaData(string contentName)
		{
			var metaData = new ContentMetaData { Name = contentName, Type = ContentType.Mesh };
			metaData.Values.Add("GeometryName", "MockGeometry");
			metaData.Values.Add("MaterialName", "MockMaterial");
			if (contentName.Contains("Animation"))
				metaData.Values.Add("AnimationName", "MockAnimation"); // ncrunch: no coverage (slow test)
			if (contentName.Contains("CustomTransform"))
				metaData.Values.Add("LocalTransform", "2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 1, 3, 5, 1");
			return metaData;
		}

		private static ContentMetaData CreateShaderData(string contentName)
		{
			return new ContentMetaData { Name = contentName, Type = ContentType.Shader };
		}

		private static ContentMetaData CreateVideoData(string contentName)
		{
			if (contentName.Contains("No"))
				throw new Video.VideoNotFoundOrAccessible(contentName, null);
			return new ContentMetaData { Name = contentName, Type = ContentType.Video };
		}

		public string GetContentMetaDataFilePath()
		{
			return ContentMetaDataFilePath;
		}
	}
}