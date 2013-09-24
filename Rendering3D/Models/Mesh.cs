using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D.Models
{
	/// <summary>
	/// A mesh is some 3D geometry with material information ready to be rendered in a 3D scene.
	/// </summary>
	public class Mesh : ContentData
	{
		public Mesh(string contentName)
			: base(contentName) {}

		public Mesh(Geometry geometry, Material material)
			: base("<GeneratedMesh Geometry=" + geometry + ", material=" + material + ">")
		{
			Geometry = geometry;
			Material = material;
		}

		public Geometry Geometry { get; set; }
		public Material Material { get; set; }
		public MeshAnimation Animation { get; internal set; }
		public bool HasAnimation { get { return Animation != null; } }

		protected override void LoadData(Stream fileData)
		{
			Geometry = ContentLoader.Load<Geometry>(MetaData.Get("GeometryName", ""));
			Material = ContentLoader.Load<Material>(MetaData.Get("MaterialName", ""));
			var animationName = MetaData.Get("AnimationName", "");
			if (!string.IsNullOrEmpty(animationName))
				Animation = ContentLoader.Load<MeshAnimation>(animationName);
		}

		protected override void DisposeData() {}
	}
}