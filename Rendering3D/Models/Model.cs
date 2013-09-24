using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D.Models
{
	/// <summary>
	/// Models are collections of static meshes to be rendered.
	/// </summary>
	public class Model : Entity3D
	{
		public Model(string modelContentName, Vector3D position)
			: this(ContentLoader.Load<ModelData>(modelContentName), position) {}

		public Model(ModelData data, Vector3D position)
			: this(data, position, Quaternion.Identity) {}

		public Model(ModelData data, Vector3D position, Quaternion orientation)
			: base(position, orientation)
		{
			Add(data);
			OnDraw<ModelRenderer>();
		}

		private class ModelRenderer : DrawBehavior
		{
			public ModelRenderer(Drawing drawing)
			{
				this.drawing = drawing;
			}

			private readonly Drawing drawing;

			public void Draw(IEnumerable<DrawableEntity> entities)
			{
				foreach (var model in entities.OfType<Model>())
				{
					var modelTranform = Matrix.FromQuaternion(model.Get<Quaternion>());
					modelTranform.Translation = model.Get<Vector3D>();
					var data = model.Get<ModelData>();
					foreach (var mesh in data.Meshes)
						drawing.AddGeometry(mesh.Geometry, mesh.Material, modelTranform);
				}
			}
		}
	}
}