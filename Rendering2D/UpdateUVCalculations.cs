using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering2D
{
	internal class UpdateUVCalculations : UpdateBehavior
	{
		public UpdateUVCalculations()
			: base(Priority.Last) {}

		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (Entity entity in entities)
				UpdateSpriteUVCalculations((Sprite)entity);
		}

		private static void UpdateSpriteUVCalculations(Sprite sprite)
		{
			var results = sprite.Get<UVCalculator.Results>();
			if (results.RequestedDrawArea != sprite.DrawArea)
				sprite.Set(sprite.Material.UVCalculator.GetUVAndDrawArea(results.RequestedUserUV,
					sprite.DrawArea, results.FlipMode));
		}
	}
}