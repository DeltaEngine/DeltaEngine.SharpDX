using DeltaEngine.Datatypes;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// A button which changes size depending on its state 
	/// - eg. grows on mouseover and shrinks on being clicked.
	/// </summary>
	public class InteractiveButton : Button
	{
		public InteractiveButton(Rectangle drawArea, string text = "")
			: this(Theme.Default, drawArea, text) {}

		public InteractiveButton(Theme theme, Rectangle drawArea, string text = "")
			: base(theme, drawArea, text)
		{
			Add(drawArea.Size);
		}

		public override void Update()
		{
			base.Update();
			if (!IsEnabled)
				DrawArea = Rectangle.FromCenter(DrawArea.Center, BaseSize);
			else if (State.IsInside && !State.IsPressed)
				DrawArea = Rectangle.FromCenter(DrawArea.Center, BaseSize * Growth);
			else if (State.IsPressed)
				DrawArea = Rectangle.FromCenter(DrawArea.Center, BaseSize / Growth);
			else
				DrawArea = Rectangle.FromCenter(DrawArea.Center, BaseSize);
		}

		private const float Growth = 1.05f;

		public Size BaseSize
		{
			get { return Get<Size>(); }
			set { Set(value); }
		}
	}
}