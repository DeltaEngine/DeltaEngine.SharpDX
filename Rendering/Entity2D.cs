using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;

namespace DeltaEngine.Rendering
{
	/// <summary>
	/// 2D entities are the basis of all 2D renderables like lines, sprites etc.
	/// </summary>
	public class Entity2D : DrawableEntity
	{
		protected Entity2D(List<object> createFromComponents)
			: base(createFromComponents)
		{
			foreach (var component in createFromComponents)
				if (component is Rectangle)
					LastDrawArea = DrawArea = (Rectangle)component;
		}

		public Entity2D(Rectangle drawArea)
		{
			LastDrawArea = DrawArea = drawArea;
		}

		public Rectangle DrawArea { get; set; }
		public Rectangle LastDrawArea { get; set; }

		public Point TopLeft
		{
			get { return DrawArea.TopLeft; }
			set { DrawArea = new Rectangle(value, DrawArea.Size); }
		}

		public Point Center
		{
			get { return DrawArea.Center; }
			set { DrawArea = Rectangle.FromCenter(value, DrawArea.Size); }
		}

		public Size Size
		{
			get { return DrawArea.Size; }
			set { DrawArea = Rectangle.FromCenter(DrawArea.Center, value); }
		}

		public Color Color
		{
			get { return base.Contains<Color>() ? base.Get<Color>() : DefaultColor; }
			set { base.Set(value); }
		}

		public Color DefaultColor = Color.White;

		public float Alpha
		{
			get { return Color.AlphaValue; }
			set { Color = new Color(Color, value); }
		}

		public float Rotation
		{
			get { return base.Contains<float>() ? base.Get<float>() : DefaultRotation; }
			set { base.Set(value); }
		}

		private const float DefaultRotation = 0.0f;

		public Point RotationCenter
		{
			get { return base.Contains<Point>() ? base.Get<Point>() : Get<Rectangle>().Center; }
			set { base.Set(value); }
		}

		protected override void NextUpdateStarted()
		{
			DidFootprintChange = LastDrawArea != DrawArea || GetLastRotation() != Rotation ||
				GetLastRotationCenter() != RotationCenter;
			LastDrawArea = DrawArea;
			base.NextUpdateStarted();
		}

		protected bool DidFootprintChange { get; private set; }

		private float GetLastRotation()
		{
			object lastTickFloat = lastTickLerpComponents.Find(component => component is float);
			return lastTickFloat == null ? DefaultRotation : (float)lastTickFloat;
		}

		private Point GetLastRotationCenter()
		{
			object lastTickPoint = lastTickLerpComponents.Find(component => component is Point);
			return lastTickPoint == null ? LastDrawArea.Center : (Point)lastTickPoint;
		}

		public override sealed T Get<T>()
		{
			if (EntitiesRunner.Current.State == UpdateDrawState.Draw && typeof(T) == typeof(Rectangle))
				return (T)(object)LastDrawArea.Lerp(DrawArea, EntitiesRunner.CurrentDrawInterpolation);
			if (EntitiesRunner.Current.State == UpdateDrawState.Draw && typeof(T) == typeof(Color))
				return (T)(object)LastColor.Lerp(Color, EntitiesRunner.CurrentDrawInterpolation);
			if (EntitiesRunner.Current.State == UpdateDrawState.Draw && typeof(T) == typeof(float))
				return (T)(object)GetLastRotation().Lerp(Rotation, EntitiesRunner.CurrentDrawInterpolation);
			if (EntitiesRunner.Current.State == UpdateDrawState.Draw && typeof(T) == typeof(Point))
				return (T)(object)GetLerpedRotationCenter();
			if (typeof(T) == typeof(Rectangle))
				return (T)(object)DrawArea;
			if (typeof(T) == typeof(Color))
				return (T)(object)Color;
			if (typeof(T) == typeof(float))
				return (T)(object)Rotation;
			if (typeof(T) == typeof(Point))
				return (T)(object)RotationCenter;
			return base.Get<T>();
		}

		public Color LastColor
		{
			get
			{
				object lastTickColor = lastTickLerpComponents.Find(component => component is Color);
				return lastTickColor == null ? DefaultColor : (Color)lastTickColor;
			}
			set
			{
				for (int index = 0; index < lastTickLerpComponents.Count; index++)
					if (lastTickLerpComponents[index] is Color)
					{
						lastTickLerpComponents[index] = value;
						return;
					}
				lastTickLerpComponents.Add(value);
			}
		}

		private Point GetLerpedRotationCenter()
		{
			Point rotationCenter = base.Contains<Point>()
				? (Point)components.Find(c => c is Point) : DrawArea.Center;
			return GetLastRotationCenter().Lerp(rotationCenter, EntitiesRunner.CurrentDrawInterpolation);
		}

		public override sealed bool Contains<T>()
		{
			return typeof(T) == typeof(Rectangle) || typeof(T) == typeof(Color) ||
				typeof(T) == typeof(float) || typeof(T) == typeof(Point) || typeof(T) == typeof(int) ||
				base.Contains<T>();
		}

		public override sealed Entity Add<T>(T component)
		{
			if (typeof(T) == typeof(Rectangle) || typeof(T) == typeof(Color) ||
				typeof(T) == typeof(float) || typeof(T) == typeof(Point) || typeof(T) == typeof(int))
				throw new ComponentOfTheSameTypeAddedMoreThanOnce();
			return base.Add(component);
		}

		public override sealed void Set<T>(T component)
		{
			if (typeof(T) == typeof(Rectangle))
				DrawArea = (Rectangle)(object)component;
			base.Set(component);
		}

		public bool RotatedDrawAreaContains(Point point)
		{
			return DrawArea.Contains(Rotation == DefaultRotation
				? point : point.RotateAround(RotationCenter, -Rotation));
		}

		protected internal override List<object> GetComponentsForSaving()
		{
			var componentsToSave = new List<object>();
			componentsToSave.Add(DrawArea);
			componentsToSave.Add(Visibility);
			foreach (var component in base.GetComponentsForSaving())
				if (!component.GetType().Name.Contains("Theme") &&
					!component.GetType().Name.Contains("Appearance") &&
					!component.GetType().Name.Contains("InteractiveState") &&
					!component.GetType().Name.Contains("Font"))
					componentsToSave.Add(component);
			return componentsToSave;
		}
	}
}