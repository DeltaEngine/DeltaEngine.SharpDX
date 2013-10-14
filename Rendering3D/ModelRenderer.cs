using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D
{
	public class ModelRenderer : DrawBehavior
	{
		public ModelRenderer(Drawing drawing)
		{
			this.drawing = drawing;
		}

		private readonly Drawing drawing;

		public void Draw(List<DrawableEntity> visibleEntities)
		{
			foreach (var model in visibleEntities)
			{
				var entity = model as Entity3D;
				if (entity == null)
					continue;
				var modelTransform = Matrix.FromQuaternion(entity.Orientation);
				modelTransform.Translation = entity.Position;
				var data = model.Get<ModelData>();
				modelTransform *= Matrix.CreateScale(data.Scaling, data.Scaling, data.Scaling);
				foreach (var mesh in data.Meshes)
					drawing.AddGeometry(mesh.Geometry, mesh.Material, mesh.LocalTransform * modelTransform);
			}
		}
	}
}