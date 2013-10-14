using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering2D
{
	/// <summary>
	/// Updates current frame for a sprite sheet animation
	/// </summary>
	public class UpdateSpriteSheetAnimation : UpdateBehavior
	{
		public UpdateSpriteSheetAnimation()
			: base(Priority.First) {}

		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (var sprite in entities.OfType<Sprite>().Where(sprite => sprite.IsPlaying))
				UpdateSpriteSheet(sprite);
		}

		private static void UpdateSpriteSheet(Sprite animation)
		{
			var animationData = animation.Material.SpriteSheet;
			animation.Elapsed += Time.Delta;
			animation.CurrentFrame =
				(int)(animationData.UVs.Count * animation.Elapsed / animation.Material.Duration);
			if (animation.CurrentFrame >= animationData.UVs.Count)
				animation.InvokeAnimationEndedAndReset();
			animation.SetUVWithoutInterpolation(animationData.UVs[animation.CurrentFrame]);
		}
	}
}