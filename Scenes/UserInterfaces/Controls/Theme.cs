using System;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// Holds a set of materials and colors for Scenes UI controls, as well as the font to be used.
	/// </summary>
	public class Theme : ContentData
	{
		public static Theme Default
		{
			get { return defaultTheme ?? (defaultTheme = new Theme()); }
		}

		private static Theme defaultTheme;

		public Theme()
			: base("<GeneratedDefaultTheme>")
		{
			DefaultButtonAppearance();
			DefaultDropdownListAppearance();
			Font = Font.Default;
			Label = new Material(new Size(4, 1), Color.Gray);
			DefaultRadioButtonAppearance();
			DefaultScrollbarAppearance();
			DefaultSelectBoxAppearance();
			DefaultSliderAppearance();
			DefaultTextBoxAppearance();
		}

		public Font Font
		{
			get { return font; }
			set { font = value; }
		}

		[NonSerialized]
		private Font font;

		public Material Label { get; set; }

		private void DefaultButtonAppearance()
		{
			Button = new Material(new Size(4, 1), Color.Gray);
			ButtonDisabled = new Material(new Size(4, 1), Color.DarkGray);
			ButtonMouseover = new Material(new Size(4, 1), Color.LightGray);
			ButtonPressed = new Material(new Size(4, 1), Color.LightBlue);
		}

		public Material Button { get; set; }
		public Material ButtonDisabled { get; set; }
		public Material ButtonMouseover { get; set; }
		public Material ButtonPressed { get; set; }

		private void DefaultDropdownListAppearance()
		{
			DropdownListBox = new Material(new Size(4, 1), Color.Gray);
			DropdownListBoxDisabled = new Material(new Size(4, 1), Color.DarkGray);
		}

		public Material DropdownListBox { get; set; }
		public Material DropdownListBoxDisabled { get; set; }

		private void DefaultRadioButtonAppearance()
		{
			RadioButtonBackground = new Material(new Size(4, 1), Color.Gray);
			RadioButtonBackgroundDisabled = new Material(new Size(4, 1), Color.DarkGray);
			RadioButtonDisabled = new Material(new Size(1, 1), Color.Gray);
			RadioButtonNotSelected = new Material(new Size(1, 1), Color.LightGray);
			RadioButtonNotSelectedMouseover = new Material(new Size(1, 1), Color.VeryLightGray);
			RadioButtonSelected = new Material(new Size(1, 1), Color.White);
			RadioButtonSelectedMouseover = new Material(new Size(1, 1), Color.LightBlue);
		}

		public Material RadioButtonBackground { get; set; }
		public Material RadioButtonBackgroundDisabled { get; set; }
		public Material RadioButtonDisabled { get; set; }
		public Material RadioButtonNotSelected { get; set; }
		public Material RadioButtonNotSelectedMouseover { get; set; }
		public Material RadioButtonSelected { get; set; }
		public Material RadioButtonSelectedMouseover { get; set; }

		private void DefaultScrollbarAppearance()
		{
			Scrollbar = new Material(new Size(4, 1), Color.Gray);
			ScrollbarDisabled = new Material(new Size(4, 1), Color.DarkGray);
			ScrollbarPointerMouseover = new Material(new Size(1, 1), Color.LightBlue);
			ScrollbarPointerDisabled = new Material(new Size(1, 1), Color.Gray);
			ScrollbarPointer = new Material(new Size(1, 1), Color.LightGray);
		}

		public Material Scrollbar { get; set; }
		public Material ScrollbarDisabled { get; set; }
		public Material ScrollbarPointer { get; set; }
		public Material ScrollbarPointerDisabled { get; set; }
		public Material ScrollbarPointerMouseover { get; set; }

		private void DefaultSelectBoxAppearance()
		{
			SelectBox = new Material(new Size(4, 1), Color.Gray);
			SelectBoxDisabled = new Material(new Size(4, 1), Color.DarkGray);
		}

		public Material SelectBox { get; set; }
		public Material SelectBoxDisabled { get; set; }

		private void DefaultSliderAppearance()
		{
			Slider = new Material(new Size(4, 1), Color.Gray);
			SliderDisabled = new Material(new Size(4, 1), Color.DarkGray);
			SliderPointer = new Material(new Size(1, 2), Color.LightGray);
			SliderPointerDisabled = new Material(new Size(1, 2), Color.Gray);
			SliderPointerMouseover = new Material(new Size(1, 2), Color.LightBlue);
		}

		public Material Slider { get; set; }
		public Material SliderDisabled { get; set; }
		public Material SliderPointer { get; set; }
		public Material SliderPointerDisabled { get; set; }
		public Material SliderPointerMouseover { get; set; }

		private void DefaultTextBoxAppearance()
		{
			TextBox = new Material(new Size(4, 1), Color.Gray);
			TextBoxFocused = new Material(new Size(4, 1), Color.LightGray);
			TextBoxDisabled = new Material(new Size(4, 1), Color.DarkGray);
		}

		public Material TextBox { get; set; }
		public Material TextBoxDisabled { get; set; }
		public Material TextBoxFocused { get; set; }

		protected override void DisposeData() {}

		protected override void LoadData(Stream fileData)
		{
			var theme = (Theme)new BinaryReader(fileData).Create();
			Button = theme.Button;
			ButtonDisabled = theme.ButtonDisabled;
			ButtonMouseover = theme.ButtonMouseover;
			ButtonPressed = theme.ButtonPressed;
		}
	}
}