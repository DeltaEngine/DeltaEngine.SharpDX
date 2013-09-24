using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// Allows one value to be selected from a dropdown list of values
	/// </summary>
	public class SelectBox : Picture
	{
		public SelectBox(Rectangle firstLineDrawArea, List<object> values)
			: this(Theme.Default, firstLineDrawArea, values) {}

		public SelectBox(Theme theme, Rectangle firstLineDrawArea, List<object> values)
			: base(theme, theme.SelectBox, firstLineDrawArea)
		{
			AddChild(scrollbar = new Scrollbar(theme, Rectangle.Unused) { Rotation = 90 });
			Clicked += ClickLine;
			this.firstLineDrawArea = firstLineDrawArea;
			if (values == null || values.Count == 0)
				throw new MustBeAtLeastOneValue();
			maxDisplayCount = values.Count;
			DrawArea = new Rectangle(firstLineDrawArea.Left, firstLineDrawArea.Top,
				firstLineDrawArea.Width, firstLineDrawArea.Height * MaxDisplayCount);
			Values = values;
		}

		internal readonly List<FontText> texts = new List<FontText>();
		protected readonly Scrollbar scrollbar;
		private List<object> values;
		private Rectangle firstLineDrawArea;
		private int maxDisplayCount;

		public class MustBeAtLeastOneValue : Exception {}

		public List<object> Values
		{
			get { return values; }
			set
			{
				if (value == null || value.Count == 0)
					throw new MustBeAtLeastOneValue();
				values = value;
				UpdateGraphics();
			}
		}

		private void UpdateGraphics()
		{
			DrawArea = new Rectangle(firstLineDrawArea.Left, firstLineDrawArea.Top,
				firstLineDrawArea.Width, firstLineDrawArea.Height * DisplayCount);
			UpdateScrollbar();
			ClearOldSelectionBoxTexts();
			CreateNewSelectionBoxTexts();
		}

		public int DisplayCount
		{
			get { return values == null ? 0 : (int)MathExtensions.Min(MaxDisplayCount, values.Count); }
		}

		public int MaxDisplayCount
		{
			get { return maxDisplayCount; }
			set
			{
				maxDisplayCount = value;
				UpdateGraphics();
			}
		}

		private void UpdateScrollbar()
		{
			if (values == null)
				return;
			float width = DrawArea.Width * ScrollbarPercentageWidth;
			scrollbar.DrawArea = new Rectangle(DrawArea.Right - width, DrawArea.Top, DrawArea.Height,
				width);
			scrollbar.RotationCenter = new Vector2D(scrollbar.DrawArea.Left + width / 2,
				scrollbar.DrawArea.Top + width / 2);
			scrollbar.RenderLayer = RenderLayer + 1;
			scrollbar.MaxValue = values.Count - 1;
			scrollbar.ValueWidth = DisplayCount;
			scrollbar.Visibility = Visibility == Visibility.Hide || values.Count <= MaxDisplayCount
				? Visibility.Hide : Visibility.Show;
		}

		private const float ScrollbarPercentageWidth = 0.1f;

		private void ClearOldSelectionBoxTexts()
		{
			foreach (FontText text in texts)
			{
				RemoveChild(text);
				text.IsActive = false;
			}
			texts.Clear();
		}

		private void CreateNewSelectionBoxTexts()
		{
			int count = DisplayCount;
			for (int i = 0; i < count; i++)
			{
				var font = new FontText(theme.Font, "", Rectangle.Unused)
				{
					HorizontalAlignment = HorizontalAlignment.Left,
					Visibility = Visibility
				};
				AddChild(font);
				texts.Add(font);
			}
		}

		private void ClickLine()
		{
			int lineNumber = GetMouseoverLineNumber();
			if (lineNumber >= 0 && LineClicked != null)
				LineClicked(scrollbar.LeftValue + lineNumber);
		}

		public Action<int> LineClicked;

		private int GetMouseoverLineNumber()
		{
			float x = Get<InteractiveState>().RelativePointerPosition.X;
			if (scrollbar.Visibility == Visibility.Show && x >= 1.0f - ScrollbarPercentageWidth)
				return -1; //ncrunch: no coverage
			float y = Get<InteractiveState>().RelativePointerPosition.Y;
			return y < 0.0f || y > 1.0f ? -1 : (int)(y * DisplayCount);
		}

		//ncrunch: no coverage start
		public override void Update()
		{
			base.Update();
			firstLineDrawArea = new Rectangle(DrawArea.Left, DrawArea.Top, DrawArea.Width,
				DrawArea.Height / DisplayCount);
			SetAppearance(IsEnabled ? theme.SelectBox : theme.SelectBoxDisabled);
			UpdateTexts();
			UpdateScrollbar();
		}

		private void UpdateTexts()
		{
			int count = DisplayCount;
			int mouseoverValue = GetMouseoverLineNumber();
			for (int i = 0; i < count; i++)
			{
				texts[i].DrawArea = Rectangle.FromCenter(DrawArea.Center.X,
					DrawArea.Top + (i + 0.5f) * firstLineDrawArea.Height,
					DrawArea.Width * ReductionDueToBorder, firstLineDrawArea.Height);
				texts[i].Color = i == mouseoverValue ? Color.White : Color.VeryLightGray;
				texts[i].Text = values[i + scrollbar.LeftValue].ToString();
			}
		}

		//ncrunch: no coverage end

		private const float ReductionDueToBorder = 0.9f;
	}
}