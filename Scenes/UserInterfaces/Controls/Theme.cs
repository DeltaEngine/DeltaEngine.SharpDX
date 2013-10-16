using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// Holds a set of materials and colors for Scenes UI controls, as well as the font to be used.
	/// </summary>
	public class Theme
		//TODO: needs to be ContentData, otherwise we cannot reload it for each control!
	{
		public static Theme Default
		{
			get { return defaultTheme ?? (defaultTheme = new Theme()); }
		}

		private static Theme defaultTheme;

		public Theme()
		{
			DefaultButtonAppearance();
			DefaultDropdownListAppearance();
			Font = Font.Default;
			Label = new Appearance(CreateDefaultMaterial(new Size(128, 32), Color.White));
			DefaultRadioButtonAppearance();
			DefaultScrollbarAppearance();
			DefaultSelectBoxAppearance();
			DefaultSliderAppearance();
			DefaultTextBoxAppearance();
		}

		public Font Font { get; set; }
		public Appearance Label { get; set; }

		public struct Appearance
		{
			public Appearance(string materialName)
				: this(new Material(Shader.Position2DColorUV, materialName)) {}

			public Appearance(Material material)
				: this()
			{
				Material = material;
				Color = Material.DefaultColor;
			}

			public Material Material { get; set; }
			public Color Color { get; set; }

			public Appearance(string materialName, Color color)
				: this()
			{
				Material = new Material(Shader.Position2DColorUV, materialName);
				Color = color;
			}
		}

		private static Material CreateDefaultMaterial(Size imageSize, Color defaultColor)
		{
			var imageData = new ImageCreationData(imageSize);
			var createdImage = ContentLoader.Create<Image>(imageData);
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUV),
				createdImage);
			material.DefaultColor = defaultColor;
			material.SetRenderSize(RenderSize.PixelBased);
			return material;
		}

		private void DefaultButtonAppearance()
		{
			var material = CreateDefaultMaterial(new Size(256, 64), Color.LightGray);
			Button = new Appearance(material);
			material.DefaultColor = Color.Gray;
			ButtonDisabled = new Appearance(material);
			material.DefaultColor = Color.White;
			ButtonMouseover = new Appearance(material);
			material.DefaultColor = Color.LightBlue;
			ButtonPressed = new Appearance(material);
		}

		public Appearance Button { get; set; }
		public Appearance ButtonDisabled { get; set; }
		public Appearance ButtonMouseover { get; set; }
		public Appearance ButtonPressed { get; set; }

		private void DefaultDropdownListAppearance()
		{
			var material = CreateDefaultMaterial(new Size(128, 32), Color.White);
			DropdownListBox = new Appearance(material);
			material.DefaultColor = Color.Gray;
			DropdownListBoxDisabled = new Appearance(material);
		}

		public Appearance DropdownListBox { get; set; }
		public Appearance DropdownListBoxDisabled { get; set; }

		private void DefaultRadioButtonAppearance()
		{
			RadioButtonBackground =
				new Appearance(CreateDefaultMaterial(new Size(128, 32), Color.White));
			RadioButtonBackgroundDisabled =
				new Appearance(CreateDefaultMaterial(new Size(128, 32), Color.Gray));
			RadioButtonDisabled = new Appearance(CreateDefaultMaterial(new Size(32), Color.Gray));
			RadioButtonNotSelected =
				new Appearance(CreateDefaultMaterial(new Size(32), Color.LightGray));
			RadioButtonNotSelectedMouseover =
				new Appearance(CreateDefaultMaterial(new Size(32), Color.VeryLightGray));
			RadioButtonSelected = new Appearance(CreateDefaultMaterial(new Size(32), Color.White));
			RadioButtonSelectedMouseover =
				new Appearance(CreateDefaultMaterial(new Size(32), Color.LightBlue));
		}

		public Appearance RadioButtonBackground { get; set; }
		public Appearance RadioButtonBackgroundDisabled { get; set; }
		public Appearance RadioButtonDisabled { get; set; }
		public Appearance RadioButtonNotSelected { get; set; }
		public Appearance RadioButtonNotSelectedMouseover { get; set; }
		public Appearance RadioButtonSelected { get; set; }
		public Appearance RadioButtonSelectedMouseover { get; set; }

		private void DefaultScrollbarAppearance()
		{
			var material = CreateDefaultMaterial(new Size(256, 64), Color.White);
			Scrollbar = new Appearance(material);
			ScrollbarPointerMouseover = new Appearance(material);
			material.DefaultColor = Color.Gray;
			ScrollbarPointerDisabled = new Appearance(material);
			ScrollbarDisabled = new Appearance(material);
			material.DefaultColor = Color.LightGray;
			ScrollbarPointer = new Appearance(material);
		}

		public Appearance Scrollbar { get; set; }
		public Appearance ScrollbarDisabled { get; set; }
		public Appearance ScrollbarPointer { get; set; }
		public Appearance ScrollbarPointerDisabled { get; set; }
		public Appearance ScrollbarPointerMouseover { get; set; }

		private void DefaultSelectBoxAppearance()
		{
			var material = CreateDefaultMaterial(new Size(128, 32), Color.White);
			SelectBox = new Appearance(material);
			material.DefaultColor = Color.Gray;
			SelectBoxDisabled = new Appearance(material);
		}

		public Appearance SelectBox { get; set; }
		public Appearance SelectBoxDisabled { get; set; }

		private void DefaultSliderAppearance()
		{
			Slider = new Appearance(CreateDefaultMaterial(new Size(256, 64), Color.White));
			SliderDisabled = new Appearance(CreateDefaultMaterial(new Size(256, 64), Color.Gray));
			SliderPointer = new Appearance(CreateDefaultMaterial(new Size(16, 32), Color.White));
			SliderPointerDisabled =
				new Appearance(CreateDefaultMaterial(new Size(16, 32), Color.Gray));
			SliderPointerMouseover =
				new Appearance(CreateDefaultMaterial(new Size(16, 32), Color.White));
		}

		public Appearance Slider { get; set; }
		public Appearance SliderDisabled { get; set; }
		public Appearance SliderPointer { get; set; }
		public Appearance SliderPointerDisabled { get; set; }
		public Appearance SliderPointerMouseover { get; set; }

		private void DefaultTextBoxAppearance()
		{
			var material = CreateDefaultMaterial(new Size(128, 32), Color.White);
			TextBoxFocused = new Appearance(material);
			material.DefaultColor = Color.LightGray;
			TextBox = new Appearance(material);
			material.DefaultColor = Color.Gray;
			TextBoxDisabled = new Appearance(material);
		}

		public Appearance TextBox { get; set; }
		public Appearance TextBoxDisabled { get; set; }
		public Appearance TextBoxFocused { get; set; }
	}
}