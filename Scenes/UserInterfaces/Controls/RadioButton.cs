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
			float aspectRatio = Theme.RadioButtonNotSelected.DiffuseMap != null
				? Theme.RadioButtonNotSelected.DiffuseMap.PixelSize.AspectRatio
				: DefaultRadioButtonAspectRatio;
			var size = new Size(aspectRatio * DrawArea.Height, DrawArea.Height);
			return new Rectangle(DrawArea.TopLeft, size);
		}

		private const float DefaultRadioButtonAspectRatio = 1.0f;

		public override void Update()
		{
			base.Update();
			SetAppearance(IsEnabled ? Theme.RadioButtonBackground : Theme.RadioButtonBackgroundDisabled);
			UpdateSelectorAppearance();
			selector.DrawArea = GetSelectorDrawArea();
		}

		private void UpdateSelectorAppearance()
		{
			if (!IsEnabled)
				selector.SetAppearance(Theme.RadioButtonDisabled);
			if (State.IsInside && State.IsSelected)
				selector.SetAppearance(Theme.RadioButtonSelectedMouseover);
			else if (State.IsInside)
				selector.SetAppearance(Theme.RadioButtonNotSelectedMouseover);
			else if (State.IsSelected)
				selector.SetAppearance(Theme.RadioButtonSelected);
			else
				selector.SetAppearance(Theme.RadioButtonNotSelected);
		}

		public override void Click()
		{
			State.IsSelected = true;
			base.Click();
		}
	}
}