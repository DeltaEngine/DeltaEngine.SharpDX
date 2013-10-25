using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering2D.Fonts
{
	/// <summary>
	/// Entity used to render text on screen. Can be aligned horizontally and vertically within
	/// a draw area. Can also contain line breaks.
	/// </summary>
	public class FontText : Entity2D
	{
		protected FontText(){}

		public FontText(Font font, string text, Vector2D centerPosition)
			: this(font, text, Rectangle.FromCenter(centerPosition, new Size(0.3f, 0.1f))) {}

		public FontText(Font font, string text, Rectangle drawArea)
			: base(drawArea)
		{
			this.text = text;
			wasFontLoadedOk = font.WasLoadedOk;
			cachedMaterial = font.Material;
			if (wasFontLoadedOk)
				RenderAsFontText(font);
			else
				RenderAsVectorText();
		}

		private string text;
		private readonly bool wasFontLoadedOk;

		private void RenderAsFontText(Font font)
		{
			description = font.Description;
			description.Generate(text, HorizontalAlignment.Center);
			Add(font.Material);
			Add(description.Glyphs);
			Add(description.DrawSize);
			OnDraw<FontRenderer>();
		}

		private FontDescription description;

	  private void RenderAsVectorText()
		{
			Add(new VectorText.Data(text));
			Start<VectorText.ProcessText>();
			OnDraw<VectorText.Render>();
		}

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				if (wasFontLoadedOk)
					UpdateFontTextRendering();
				else
					Get<VectorText.Data>().Text = value;
			}
		}

		private void UpdateFontTextRendering()
		{
			description.Generate(text, HorizontalAlignment);
			Set(description.Glyphs);
			SetWithoutInterpolation(description.DrawSize);
		}

		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return Contains<HorizontalAlignment>()
					? Get<HorizontalAlignment>() : HorizontalAlignment.Center;
			}
			set
			{
				Set(value);
				if (wasFontLoadedOk)
					UpdateFontTextRendering();
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return Contains<VerticalAlignment>() ? Get<VerticalAlignment>() : VerticalAlignment.Center;
			}
			set
			{
				Set(value);
				if (wasFontLoadedOk)
					UpdateFontTextRendering();
			}
		}

		public override Entity Add<T>(T component)
		{
			if (component is Material)
				cachedMaterial = component as Material;
			return base.Add(component);
		}

		internal Material cachedMaterial;
	}
}