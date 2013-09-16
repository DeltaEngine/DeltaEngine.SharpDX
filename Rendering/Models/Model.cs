using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering.Models
{
	/// <summary>
	/// Models are collections of meshes to be rendered and in addition can have animation data.
	/// </summary>
	public class Model : Entity3D
	{
		public Model(string modelContentName, Vector position)
			: this(ContentLoader.Load<ModelData>(modelContentName), position) {}

		public Model(ModelData data, Vector position)
			: this(data, position, Quaternion.Identity) {}

		public Model(ModelData data, Vector position, Quaternion orientation)
			: base(position, orientation)
		{
			Add(data);
			Start<AnimationUpdater>();
			OnDraw<ModelRenderer>();
		}

		public class AnimationUpdater : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var model in entities.OfType<Model>())
				{
					var data = model.Get<ModelData>();
					foreach (var mesh in data.Meshes)
						if (mesh.HasAnimation)
							mesh.UpdateAnimationAndComputeBoneTransforms();
				}
			}
		}

		public class ModelRenderer : DrawBehavior
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
					modelTranform.Translation = model.Get<Vector>();
					var data = model.Get<ModelData>();
					foreach (var mesh in data.Meshes)
						drawing.AddGeometry(mesh.Geometry, mesh.Material, modelTranform);
				}
			}
		}
	}
}