using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Sprites
{
	/// <summary>
	/// 2D sprite to be rendered, which is an image or animation or sprite sheet used as an Entity2D.
	/// </summary>
	public class Sprite : Entity2D
	{
		protected Sprite(List<object> createFromComponents)
			: base(createFromComponents) {}

		public Sprite(Material material, Point position)
			: this(material, Rectangle.FromCenter(position, material.MaterialRenderSize)) {}

		public Sprite(string imageName, Rectangle drawArea)
			: this(new Material(Shader.Position2DUv, imageName), drawArea) {}

		public Sprite(Material material, Rectangle drawArea)
			: base(drawArea)
		{
			Add(material);
			Add(material.DiffuseMap.BlendMode);
			Add(new SpriteCoordinates(Rectangle.One));
			if (material.DefaultColor != DefaultColor)
				Color = material.DefaultColor;
			OnDraw<SpriteBatchRenderer>();
			if (material.Animation != null)
				Start<UpdateImageAnimation>();
			if (material.SpriteSheet != null)
				Start<UpdateSpriteSheetAnimation>();
			IsPlaying = true;
		}

		public struct SpriteCoordinates : Lerp<SpriteCoordinates>
		{
			public SpriteCoordinates(Rectangle uv, FlipMode flipMode = FlipMode.None)
				: this()
			{
				if (flipMode == FlipMode.Horizontal)
					uv = new Rectangle(uv.Right, uv.Top, -uv.Width, uv.Height);
				else if (flipMode == FlipMode.Vertical)
					uv = new Rectangle(uv.Left, uv.Bottom, uv.Width, -uv.Height);
				UV = uv;
			}

			public Rectangle UV { get; private set; }

			public SpriteCoordinates Lerp(SpriteCoordinates other, float interpolation)
			{
				return new SpriteCoordinates(other.UV.Lerp(UV, interpolation));
			}
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

		public SpriteCoordinates Coordinates
		{
			get { return Get<SpriteCoordinates>(); }
			set { Set(value); }
		}

		/// <summary>
		/// Force a new uv for SpriteSheets without interpolation between last uv and current uv data.
		/// </summary>
		public void SetNewUV(Rectangle uv, FlipMode flipMode = FlipMode.None)
		{
			Coordinates = new SpriteCoordinates(uv, flipMode);
			for (int index = 0; index < lastTickLerpComponents.Count; index++)
			{
				object component = lastTickLerpComponents[index];
				if (component is SpriteCoordinates)
					lastTickLerpComponents[index] = new SpriteCoordinates(uv, flipMode);
			}
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