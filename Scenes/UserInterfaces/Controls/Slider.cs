using System;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// Allows a range of values to be selected via a visible slider.
	/// </summary>
	public class Slider : BaseSlider
	{
		public Slider(Rectangle drawArea)
			: this(Theme.Default, drawArea) {}

		public Slider(Theme theme, Rectangle drawArea)
			: base(theme, theme.Slider, drawArea)
		{
			Add(values = new Values { MinValue = 0, Value = 100, MaxValue = 100 });
			Add(Pointer = new Picture(theme, theme.SliderPointer, Rectangle.Unused));
			AddChild(Pointer);
		}

		private readonly Values values;

		private class Values
		{
			public int MinValue { get; set; }
			public int Value { get; set; }
			public int MaxValue { get; set; }
		}

		protected override void UpdateSliderAppearance()
		{
			SetAppearance(IsEnabled ? theme.Slider : theme.SliderDisabled);
		}

		protected override void UpdatePointerAppearance()
		{
			if (!Pointer.IsEnabled)
				Pointer.SetAppearance(theme.SliderPointerDisabled);
			else if (Pointer.State.IsInside || Pointer.State.IsPressed)
				Pointer.SetAppearance(theme.SliderPointerMouseover);
			else
				Pointer.SetAppearance(theme.SliderPointer);
		}

		protected override void UpdatePointerValue()
		{
			if (!State.IsPressed)
				return;
			float percentage = State.RelativePointerPosition.X.Clamp(0.0f, 1.0f);
			float aspectRatio = Pointer.Material.MaterialRenderSize.AspectRatio;
			float unusable = aspectRatio / DrawArea.Aspect;
			float expandedPercentage = ((percentage - 0.5f) * (1.0f + unusable) + 0.5f).Clamp(0.0f, 1.0f);
			Value = (int)(MinValue + expandedPercentage * (MaxValue - MinValue));
			if (Value == lastPointerValue)
				return;
			lastPointerValue = Value;
			if (ValueChanged != null)
				ValueChanged(Value);
		}

		private int lastPointerValue = -999;

		public Action<int> ValueChanged;

		protected override void UpdatePointerDrawArea()
		{
			Rectangle drawArea = DrawArea;
			float aspectRatio = Pointer.Material.MaterialRenderSize.AspectRatio;
			var size = new Size(aspectRatio * drawArea.Height, drawArea.Height);
			float percentage = (Value - MinValue) / (float)(MaxValue - MinValue);
			var leftCenter = new Point(drawArea.Left + size.Width / 2, drawArea.Center.Y);
			var rightCenter = new Point(drawArea.Right - size.Width / 2, drawArea.Center.Y);
			Pointer.DrawArea = Rectangle.FromCenter(leftCenter.Lerp(rightCenter, percentage), size);
		}

		public int MinValue
		{
			get { return values.MinValue; }
			set { values.MinValue = value; }
		}

		public int Value
		{
			get { return values.Value; }
			set { values.Value = value; }
		}

		public int MaxValue
		{
			get { return values.MaxValue; }
			set { values.MaxValue = value; }
		}
	}
}