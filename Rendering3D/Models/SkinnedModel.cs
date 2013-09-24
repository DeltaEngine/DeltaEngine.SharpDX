using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering3D.Models
{
	/// <summary>
	/// SkinnedModel contains meshes with animation data to be animated and rendered.
	/// </summary>
	public class SkinnedModel : Model
	{
		public SkinnedModel(string modelContentName, Vector3D position)
			: this(ContentLoader.Load<ModelData>(modelContentName), position) {}

		public SkinnedModel(ModelData data, Vector3D position)
			: this(data, position, Quaternion.Identity) {}

		public SkinnedModel(ModelData data, Vector3D position, Quaternion orientation)
			: base(data, position, orientation)
		{
			Start<AnimationUpdater>();
		}

		private class AnimationUpdater : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var model in entities.OfType<SkinnedModel>())
				{
					var data = model.Get<ModelData>();
					foreach (var mesh in data.Meshes)
					{
						mesh.Animation.UpdateFrameTransoforms();
						mesh.Geometry.JointTranforms = mesh.Animation.CurrentFrameTransforms;
					}
				}
			}
		}
	}
}