using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// A background sprite and foreground text.
	/// </summary>
	public class Label : Picture
	{
		protected Label() {}

		public Label(Rectangle drawArea, string text = "")
			: this(Theme.Default, drawArea, text) {}

		public Label(Theme theme, Rectangle drawArea, string text = "")
			: this(theme, theme.Label, drawArea)
		{
			Text = text;
			PreviousText = Text;
		}

		public string Text
		{
			get { return FontText.Text; }
			set
			{
				if (FontText.Text == value)
					return;
				PreviousText = FontText.Text;
				FontText.Text = value;
			}
		}

		protected FontText FontText
		{
			get { return Get<FontText>(); }
		}

		public string PreviousText { get; protected set; }

		internal Label(Theme theme, Material material, Rectangle drawArea)
			: base(theme, material, drawArea)
		{
			var fontText = new FontText(theme.Font, "", GetFontTextDrawArea());
			Add(fontText);
			AddChild(fontText);
		}

		private Rectangle GetFontTextDrawArea()
		{
			return Rectangle.FromCenter(Center, Size * ReductionDueToBorder);
		}

		protected const float ReductionDueToBorder = 0.9f;

		public override void Set(object component)
		{
			if (component is FontText)
				ReplaceChild((FontText)component);
			base.Set(component);
		}

		private void ReplaceChild(FontText text)
		{
			if (Contains<FontText>())
				RemoveChild(Get<FontText>());
			AddChild(text);
		}

		public override void Update()
		{
			base.Update();
			FontText.DrawArea = GetFontTextDrawArea();
		}
	}
}