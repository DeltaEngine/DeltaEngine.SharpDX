using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// Simple UI button which changes appearance based on mouse/touch interaction.
	/// </summary>
	public class Button : Label
	{
		protected Button(List<object> createFromComponents)
			: base(createFromComponents) {}

		public Button(Rectangle drawArea, string text = "")
			: this(Theme.Default, drawArea, text) {}

		public Button(Theme theme, Rectangle drawArea, string text = "")
			: base(theme, theme.Button, drawArea)
		{
			Text = text;
		}

		public override void Update()
		{
			if (!IsEnabled)
				SetAppearance(theme.ButtonDisabled);
			else if (State.IsInside && State.IsPressed)
				SetAppearance(theme.ButtonPressed);
			else if (State.IsInside)
				SetAppearance(theme.ButtonMouseover);
			else
				SetAppearance(theme.Button);
			base.Update();
		}
	}
}