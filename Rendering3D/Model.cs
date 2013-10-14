using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D
{
	/// <summary>
	/// Collection of static meshes to be rendered, makes it easier rendering complex things like
	/// Levels, 3D Models with animation and collections of meshes. Should be combined with Actor.
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
	}
}