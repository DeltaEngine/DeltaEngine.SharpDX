using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// A background sprite and foreground text.
	/// </summary>
	public class Label : Picture
	{
		protected Label()
		{
			fontText = new FontText(theme.Font, "", GetFontTextDrawArea());
			Add(fontText);
			// Text needs to be extracted
		}

		public Label(Rectangle drawArea, string text = "")
			: this(Theme.Default, drawArea, text) {}

		public Label(Theme theme, Rectangle drawArea, string text = "")
			: this(theme, theme.Label, drawArea)
		{
			Text = text;
		}

		public string Text
		{
			get { return fontText.Text; }
			set { fontText.Text = value; }
		}

		protected readonly FontText fontText;

		internal Label(Theme theme, Theme.Appearance appearance, Rectangle drawArea)
			: base(theme, appearance, drawArea)
		{
			fontText = new FontText(theme.Font, "", GetFontTextDrawArea());
			Add(fontText);
			AddChild(fontText);
		}

		private Rectangle GetFontTextDrawArea()
		{
			return Rectangle.FromCenter(Center, Size * ReductionDueToBorder);
		}

		protected const float ReductionDueToBorder = 0.9f;

		public override void Update()
		{
			base.Update();
			fontText.DrawArea = GetFontTextDrawArea();
		}
	}
}