using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// For example, the horizontal and vertical scrollbars on a browser, document etc.
	/// </summary>
	public class Scrollbar : BaseSlider
	{
		public Scrollbar(Rectangle drawArea)
			: this(Theme.Default, drawArea) {}

		public Scrollbar(Theme theme, Rectangle drawArea)
			: base(theme, theme.Scrollbar, drawArea)
		{
			Add(values = new Values { MinValue = 0, MaxValue = 99, LeftValue = 90, ValueWidth = 10 });
			Add(Pointer = new Picture(theme, theme.ScrollbarPointer, Rectangle.Unused));
			AddChild(Pointer);
		}

		private readonly Values values;

		private class Values
		{
			public int MinValue { get; set; }
			public int MaxValue { get; set; }
			public int LeftValue { get; set; }
			public int ValueWidth { get; set; }
		}

		protected override void UpdateSliderAppearance()
		{
			SetAppearance(IsEnabled ? theme.Scrollbar : theme.ScrollbarDisabled);
		}

		protected override void UpdatePointerAppearance()
		{
			if (!Pointer.IsEnabled)
				Pointer.SetAppearance(theme.ScrollbarPointerDisabled);
			else if (Pointer.State.IsInside || Pointer.State.IsPressed)
				Pointer.SetAppearance(theme.ScrollbarPointerMouseover);
			else
				Pointer.SetAppearance(theme.ScrollbarPointer);
		}

		protected override void UpdatePointerValue()
		{
			if (!State.IsPressed)
				return;
			float percentage = State.RelativePointerPosition.X.Clamp(0.0f, 1.0f);
			CenterValue = (int)(MinValue + percentage * (MaxValue - MinValue + 1));
		}

		private float GetPointerPercentageWidth()
		{
			var percentageWidth = (float)ValueWidth / (MaxValue - MinValue + 1);
			return percentageWidth.Clamp(MinimumPointerPercentageWidth, MaximumPointerPercentageWidth);
		}

		private const float MinimumPointerPercentageWidth = 0.05f;
		private const float MaximumPointerPercentageWidth = 1.0f;

		protected override void UpdatePointerDrawArea()
		{
			Rectangle drawArea = DrawArea;
			var size = new Size(drawArea.Width * GetPointerPercentageWidth(), drawArea.Height);
			float percentage = (LeftValue - MinValue) / (float)(MaxValue - MinValue - ValueWidth + 1);
			var min = new Point(drawArea.Left, drawArea.Top);
			var max = new Point(drawArea.Right - size.Width, drawArea.Top);
			Pointer.DrawArea = new Rectangle(min.Lerp(max, percentage), size);
		}

		public int CenterValue
		{
			get { return LeftValue + ValueWidth / 2; }
			set
			{
				int leftValue = value - ValueWidth / 2;
				if (leftValue + ValueWidth - 1 > MaxValue)
					leftValue = MaxValue - ValueWidth + 1;
				if (leftValue < MinValue)
					leftValue = MinValue;
				Get<Values>().LeftValue = leftValue;
			}
		}

		public int LeftValue
		{
			get { return values.LeftValue; }
		}

		public int ValueWidth
		{
			get { return values.ValueWidth; }
			set
			{
				values.ValueWidth = value;
				CenterValue = CenterValue;
			}
		}

		public int MinValue
		{
			get { return values.MinValue; }
			set
			{
				values.MinValue = value;
				CenterValue = CenterValue;
			}
		}

		public int MaxValue
		{
			get { return values.MaxValue; }
			set
			{
				values.MaxValue = value;
				CenterValue = CenterValue;
			}
		}

		public int RightValue
		{
			get { return LeftValue + ValueWidth - 1; }
		}
	}
}