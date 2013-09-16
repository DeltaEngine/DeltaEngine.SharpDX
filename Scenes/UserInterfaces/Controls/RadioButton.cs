using DeltaEngine.Datatypes;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// A set of Radio Buttons form a Radio Dialog.
	/// </summary>
	public class RadioButton : Label
	{
		public RadioButton(Rectangle drawArea, string text = "")
			: this(Theme.Default, drawArea, text) {}

		public RadioButton(Theme theme, Rectangle drawArea, string text = "")
			: base(theme, theme.RadioButtonBackground, drawArea)
		{
			Text = text;
			AddChild(selector = new Picture(theme, theme.RadioButtonNotSelected, GetSelectorDrawArea()));
		}

		private readonly Picture selector;

		private Rectangle GetSelectorDrawArea()
		{
			float aspectRatio = theme.RadioButtonNotSelected.Material.MaterialRenderSize.AspectRatio;
			var size = new Size(aspectRatio * DrawArea.Height, DrawArea.Height);
			return new Rectangle(DrawArea.TopLeft, size);
		}

		public override void Update()
		{
			base.Update();
			SetAppearance(IsEnabled ? theme.RadioButtonBackground : theme.RadioButtonBackgroundDisabled);
			UpdateSelectorAppearance();
			selector.DrawArea = GetSelectorDrawArea();
		}

		private void UpdateSelectorAppearance()
		{
			if (!IsEnabled)
				selector.SetAppearance(theme.RadioButtonDisabled);
			if (State.IsInside && State.IsSelected)
				selector.SetAppearance(theme.RadioButtonSelectedMouseover);
			else if (State.IsInside)
				selector.SetAppearance(theme.RadioButtonNotSelectedMouseover);
			else if (State.IsSelected)
				selector.SetAppearance(theme.RadioButtonSelected);
			else
				selector.SetAppearance(theme.RadioButtonNotSelected);
		}

		public override void Click()
		{
			State.IsSelected = true;
			base.Click();
		}
	}
}