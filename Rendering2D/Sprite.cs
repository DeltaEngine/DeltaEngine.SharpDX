using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering2D
{
	/// <summary>
	/// 2D sprite to be rendered, which is an image or animation or sprite sheet used as an Entity2D.
	/// </summary>
	public class Sprite : Entity2D
	{
		protected Sprite()
			: base(Rectangle.Zero) {}

		public Sprite(Material material, Vector2D position)
			: this(material, Rectangle.FromCenter(position, material.MaterialRenderSize)) {}

		public Sprite(string imageName, Rectangle drawArea)
			: this(new Material(Shader.Position2DColorUV, imageName), drawArea) {}

		public Sprite(Material material, Rectangle drawArea)
			: base(drawArea)
		{
			Add(material);
			Add(material.DiffuseMap.BlendMode);
			if (material.DefaultColor != DefaultColor)
				Color = material.DefaultColor;
			OnDraw<SpriteBatchRenderer>();
			if (material.Animation != null)
				Start<UpdateImageAnimation>();
			if (material.SpriteSheet != null)
				Start<UpdateSpriteSheetAnimation>();
			uVCalculatorResults = Material.UVCalculator.GetUVAndDrawArea(Rectangle.One, drawArea,
				FlipMode.None);
			lastUVCalculatorResults = uVCalculatorResults;
			Start<UpdateUVCalculations>();
			IsPlaying = true;
		}

		private UVCalculator.Results uVCalculatorResults;
		private UVCalculator.Results lastUVCalculatorResults;

		public override sealed void Set(object component)
		{
			if (component is UVCalculator.Results)
			{
				EntitiesRunner.Current.CheckIfInUpdateState();
				uVCalculatorResults = (UVCalculator.Results)component;
			}
			else
				base.Set(component);
		}

		public override sealed void SetWithoutInterpolation<T>(T component)
		{
			if (typeof(T) == typeof(UVCalculator.Results))
			{
				EntitiesRunner.Current.CheckIfInUpdateState();
				uVCalculatorResults = (UVCalculator.Results)(object)component;
				lastUVCalculatorResults = uVCalculatorResults;
			}
			else
				base.SetWithoutInterpolation(component);
		}

		public override sealed Entity Add<T>(T component)
		{
			if (typeof(T) == typeof(UVCalculator.Results))
				throw new ComponentOfTheSameTypeAddedMoreThanOnce();
			return base.Add(component);
		}

		public override sealed T Get<T>()
		{
			if (EntitiesRunner.Current.State == UpdateDrawState.Draw && typeof(T) == typeof(UVCalculator.Results))
				return (T)(object)lastUVCalculatorResults.Lerp(uVCalculatorResults, EntitiesRunner.CurrentDrawInterpolation);
			if (typeof(T) == typeof(UVCalculator.Results))
				return (T)(object)uVCalculatorResults;
			return base.Get<T>();
		}

		protected internal override sealed List<object> GetComponentsForSaving()
		{
			List<object> componentsForSaving = base.GetComponentsForSaving(); 
			componentsForSaving.Add(uVCalculatorResults);
			return componentsForSaving;
		}

		protected internal override sealed List<object> GetComponentsForViewing()
		{
			return GetComponentsForSaving();
		}

		protected override sealed void NextUpdateStarted()
		{
			lastUVCalculatorResults = uVCalculatorResults;
			base.NextUpdateStarted();
		}

		public Rectangle UV
		{
			get { return Get<UVCalculator.Results>().RequestedUserUV; }
			set { Set(Material.UVCalculator.GetUVAndDrawArea(value, DrawArea, FlipMode)); }
		}

		public FlipMode FlipMode
		{
			get { return Get<UVCalculator.Results>().FlipMode; }
			set { Set(Material.UVCalculator.GetUVAndDrawArea(UV, DrawArea, value)); }
		}

		public Material Material
		{
			get { return Get<Material>(); }
			set { Set(value); }
		}

		public BlendMode BlendMode
		{
			get { return Get<BlendMode>(); }
			set { Set(value); }
		}

		public void SetUVWithoutInterpolation(Rectangle uv)
		{
			SetWithoutInterpolation(Material.UVCalculator.GetUVAndDrawArea(uv, DrawArea, FlipMode));
		}

		public float Elapsed { get; set; }
		public int CurrentFrame { get; set; }
		public bool IsPlaying { get; set; }

		public void Reset()
		{
			CurrentFrame = 0;
			if (Material.Animation != null || Material.SpriteSheet != null)
				while (Elapsed >= Material.Duration)
					Elapsed -= Material.Duration;
			else
				Elapsed = 0.0f;
		}

		internal void InvokeAnimationEndedAndReset()
		{
			if (AnimationEnded != null)
				AnimationEnded();
			Reset();
		}

		public event Action AnimationEnded;
	}
}