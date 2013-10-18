using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering2D
{
	internal class UpdateRenderingCalculations : UpdateBehavior
	{
		public UpdateRenderingCalculations()
			: base(Priority.Last) {}

		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (Entity entity in entities)
				UpdateSpriteRenderingCalculations((Sprite)entity);
		}

		private static void UpdateSpriteRenderingCalculations(Sprite sprite)
		{
			var data = sprite.Get<RenderingData>();
			if (data.RequestedDrawArea != sprite.DrawArea)
				sprite.Set(sprite.Material.RenderingCalculator.GetUVAndDrawArea(data.RequestedUserUV,
					sprite.DrawArea, data.FlipMode));
		}
	}
}