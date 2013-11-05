using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Shapes;

namespace DeltaEngine.Tutorials.Entities03AddingComponents
{
	public class Program : App
	{
		public Program()
		{
			new Earth(Vector2D.Half);
		}

		public class Earth : Sprite
		{
			public Earth(Vector2D position)
				: base(ContentLoader.Load<Material>("Earth"), position)
			{
				Add(new OutlineColor(Color.Red));
				OnDraw<DrawPolygon2DOutlines>();
				Add(new SimplePhysics.Data { Gravity = new Vector2D(0.0f, 0.1f) });
				Start<SimplePhysics.BounceIfAtScreenEdge>();
				Start<SimplePhysics.Move>();
			}
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}