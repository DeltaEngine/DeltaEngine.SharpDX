using System;
using System.IO;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Content
{
	/// <summary>
	/// Displays images or shapes via shaders in 2D or 3D. Always need a shader and a diffuse map,
	/// which can be a single <see cref="Image"/>, an <see cref="ImageAnimation"/> or a
	/// <see cref="SpriteSheet"/>. 
	/// </summary>
	public sealed class Material : ContentData
	{
		/// <summary>
		/// Create material from content created via the Editor, contentName is NOT just the image name.
		/// </summary>
		private Material(string contentName)
			: base(contentName) {}

		/// <summary>
		/// As opposed to loading a material from content it can also be created with this constructor.
		/// If imageOrAnimationName is used you need to provide either a DiffuseMap or an Animation.
		/// </summary>
		public Material(string shaderName, string imageOrAnimationName)
			: base("<GeneratedMaterial:" + shaderName + ":" + imageOrAnimationName + ">")
		{
			Initialize(shaderName, imageOrAnimationName, Color.White);
		}

		private void Initialize(string shaderName, string imageOrAnimationName, Color defaultColor)
		{
			if (string.IsNullOrEmpty(shaderName))
				throw new UnableToCreateMaterialWithoutValidShaderName();
			Shader = ContentLoader.Load<Shader>(shaderName);
			DefaultColor = defaultColor;
			if (String.IsNullOrEmpty(imageOrAnimationName))
				return;
			if (ContentLoader.Exists(imageOrAnimationName, ContentType.ImageAnimation))
				Animation = ContentLoader.Load<ImageAnimation>(imageOrAnimationName);
			else if (ContentLoader.Exists(imageOrAnimationName, ContentType.SpriteSheetAnimation))
				SpriteSheet = ContentLoader.Load<SpriteSheetAnimation>(imageOrAnimationName);
			else
				DiffuseMap = ContentLoader.Load<Image>(imageOrAnimationName);
		}

		public class UnableToCreateMaterialWithoutValidShaderName : Exception {}

		public Shader Shader { get; private set; }
		public Image DiffuseMap { get; set; }
		public Image LightMap { get; set; }
		public Color DefaultColor { get; set; }

		/// <summary>
		/// Special constructor for creating custom shaders and images or reusing existing instances.
		/// </summary>
		public Material(Shader customShader, Image customDiffuseMap)
			: base("<GeneratedCustomMaterial:" + customShader + ":" + customDiffuseMap + ">")
		{
			Shader = customShader;
			DiffuseMap = customDiffuseMap;
			DefaultColor = Color.White;
		}

		public ImageAnimation Animation
		{
			get { return animation; }
			set
			{
				animation = value;
				DiffuseMap = animation.Frames[0];
				Duration = animation.DefaultDuration;
			}
		}

		private ImageAnimation animation;
		public float Duration { get; set; }

		public SpriteSheetAnimation SpriteSheet
		{
			get { return spriteSheet; }
			set
			{
				spriteSheet = value;
				DiffuseMap = spriteSheet.Image;
				Duration = spriteSheet.DefaultDuration;
			}
		}

		private SpriteSheetAnimation spriteSheet;

		/// <summary>
		/// When using the Sprite(Material, Point) constructor this size is used for the draw area.
		/// It is calculated from the DiffuseMap.PixelSize and the default content resolution, i.e.
		/// a 100x200 pixel image will be displayed aspect ratio correct relative to the window size.
		/// </summary>
		public Size MaterialRenderSize
		{
			get
			{
				var size = new Size(0.5f);
				if (spriteSheet != null)
					size = SetRenderSize(spriteSheet.SubImageSize);
				else if (DiffuseMap != null)
					size = SetRenderSize(DiffuseMap.PixelSize);
				return size;
			}
		}

		private Size SetRenderSize(Size pixelSize)
		{
			if (renderSize == RenderSize.PixelBased)
				pixelSize = ScreenSpace.Current.FromPixelSpace(pixelSize);
			else if (renderSize == RenderSize.Size800X480)
				pixelSize = ScreenSpace.Current.FromPixelSpace(pixelSize / new Size(800));
			else if (renderSize == RenderSize.Size1024X720)
				pixelSize = ScreenSpace.Current.FromPixelSpace(pixelSize / new Size(1024));
			else if (renderSize == RenderSize.Size1280X720)
				pixelSize = ScreenSpace.Current.FromPixelSpace(pixelSize / new Size(1280));
			else if (renderSize == RenderSize.Size1920X1080)
				pixelSize = ScreenSpace.Current.FromPixelSpace(pixelSize / new Size(1920));
			else if (renderSize == RenderSize.SettingsBased)
				pixelSize = GetRenderSizeBassedOnSettings(pixelSize);
			return pixelSize;
		}

		private static Size GetRenderSizeBassedOnSettings(Size pixelSize)
		{
			Settings settings = Settings.Current;
			var quadSizeSettings =
				new Size(settings.Resolution.Width > settings.Resolution.Height
					? settings.Resolution.Width : settings.Resolution.Height);
			return ScreenSpace.Current.FromPixelSpace(pixelSize / quadSizeSettings);
		}

		protected override void LoadData(Stream fileData)
		{
			var shaderName = MetaData.Get("ShaderName", "");
			if (string.IsNullOrEmpty(shaderName))
				throw new UnableToCreateMaterialWithoutValidShaderName();
			Shader = ContentLoader.Load<Shader>(shaderName);
			DefaultColor = MetaData.Get("Color", Color.White);
			string imageOrAnimationName = MetaData.Get("ImageOrAnimationName", "");
			if (string.IsNullOrEmpty(imageOrAnimationName))
				return;
			if (ContentLoader.Exists(imageOrAnimationName, ContentType.ImageAnimation))
				Animation = ContentLoader.Load<ImageAnimation>(imageOrAnimationName);
			else if (ContentLoader.Exists(imageOrAnimationName, ContentType.SpriteSheetAnimation))
				SpriteSheet = ContentLoader.Load<SpriteSheetAnimation>(imageOrAnimationName);
			else
				DiffuseMap = ContentLoader.Load<Image>(imageOrAnimationName);
			if (DiffuseMap != null)
				DiffuseMap.BlendMode = MetaData.Get("BlendMode", "Normal").TryParse(BlendMode.Normal);
			LoadLightMap();
		}

		private void LoadLightMap()
		{
			var lightMapName = MetaData.Get("LightMapName", "");
			if (!string.IsNullOrEmpty(lightMapName))
				LightMap = ContentLoader.Load<Image>(lightMapName);
		}

		protected override void DisposeData() {}

		public override string ToString()
		{
			return "Material: Shader=" + Shader + ", DiffuseMap=" + DiffuseMap + ", DefaultColor=" +
				DefaultColor + (Animation != null ? ", Animation=" + Animation : "") +
				(SpriteSheet != null ? ", SpriteSheet=" + SpriteSheet : "");
		}

		public void SetRenderSize(RenderSize size)
		{
			renderSize = size;
		}

		private RenderSize renderSize = RenderSize.PixelBased;
	}
}