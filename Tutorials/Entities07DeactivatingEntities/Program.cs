using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;

namespace DeltaEngine.Tutorials.Entities07DeactivatingEntities
{
	public class Program : App
	{
		public Program()
		{
			var earth = new Earth(Vector2D.Half);
			new Command(Command.Click, () => earth.IsActive = !earth.IsActive);
		}

		public class Earth : Sprite
		{
			public Earth(Vector2D position)
				: base(ContentLoader.Load<Material>("Earth"), position)
			{
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