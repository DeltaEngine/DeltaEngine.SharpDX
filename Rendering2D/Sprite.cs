using System;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;

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
			Add(Material.UVCalculator.GetUVAndDrawArea(Rectangle.One, drawArea, FlipMode.None));
			Start<UpdateUVCalculations>();
			IsPlaying = true;
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