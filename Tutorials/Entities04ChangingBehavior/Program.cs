using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;

namespace DeltaEngine.Tutorials.Entities04ChangingBehavior
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
				Add(new SimplePhysics.Data
				{
					Gravity = new Vector2D(0.0f, 0.1f),
					Bounced = () => Color = Color.GetRandomColor()
				});
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