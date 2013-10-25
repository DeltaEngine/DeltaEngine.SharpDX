using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// Allows one value to be selected from a dropdown list of values
	/// </summary>
	public class DropdownList : Label
	{
		public DropdownList(Rectangle drawArea, List<object> values)
			: this(Theme.Default, drawArea, values) {}

		public DropdownList(Theme theme, Rectangle drawArea, List<object> values)
			: base(theme, theme.DropdownListBox, drawArea)
		{
			selectBox = new SelectBox(theme,
				new Rectangle(drawArea.Left, drawArea.Top + drawArea.Height, drawArea.Width,
					drawArea.Height), values) { IsVisible = false };
			selectBox.LineClicked += SelectNewValue;
			AddChild(selectBox);
			Values = values;
			Clicked += ToggleSelectionBoxVisibility;
			fontText.HorizontalAlignment = HorizontalAlignment.Left;
		}

		internal readonly SelectBox selectBox;

		public List<object> Values
		{
			get { return selectBox.Values; }
			set
			{
				selectBox.Values = value;
				if (SelectedValue == null || !value.Contains(SelectedValue))
					SelectedValue = value[0];
			}
		}

		public object SelectedValue
		{
			get { return selectedValue; }
			set
			{
				if (!selectBox.Values.Contains(value))
					throw new SelectedValueMustBeOneOfTheListOfValues();
				selectedValue = value;
				Text = value.ToString();
			}
		}

		private object selectedValue;

		public class SelectedValueMustBeOneOfTheListOfValues : Exception {}

		private void SelectNewValue(int lineNumber)
		{
			if (!selectBox.IsVisible)
				return;
			SelectedValue = Values[lineNumber];
			selectBox.IsVisible = false;
		}

		private void ToggleSelectionBoxVisibility()
		{
			selectBox.IsVisible = !selectBox.IsVisible;
		}

		public override void Update()
		{
			base.Update();
			SetAppearance(IsEnabled ? Theme.DropdownListBox : Theme.DropdownListBoxDisabled);
			if (selectBox == null)
				return;
			selectBox.DrawArea = new Rectangle(DrawArea.Left, DrawArea.Top + DrawArea.Height,
				DrawArea.Width, DrawArea.Height * selectBox.DisplayCount);
		}

		public override bool IsEnabled
		{
			get { return base.IsEnabled; }
			set
			{
				base.IsEnabled = value;
				if (!value)
					selectBox.IsVisible = false;
			}
		}

		public int MaxDisplayCount
		{
			get { return selectBox.MaxDisplayCount; }
			set { selectBox.MaxDisplayCount = value; }
		}
	}
}