using DeltaEngine.Datatypes;
using DeltaEngine.Input;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// A Label into which text can be typed.
	/// </summary>
	public class TextBox : Label, KeyboardControllable
	{
		public TextBox(Rectangle drawArea, string text = "")
			: this(Theme.Default, drawArea, text) {}

		public TextBox(Theme theme, Rectangle drawArea, string text = "")
			: base(theme, theme.TextBox, drawArea)
		{
			Text = text;
			State.CanHaveFocus = true;
			Start<Keyboard>();
		}

		public override void Update()
		{
			base.Update();
			if (!IsEnabled)
				SetAppearance(theme.TextBoxDisabled);
			else if (HasFocus)
				SetAppearance(theme.TextBoxFocused);
			else
				SetAppearance(theme.TextBox);
		}

		public bool HasFocus
		{
			get { return State.HasFocus; }
		}
	}
}