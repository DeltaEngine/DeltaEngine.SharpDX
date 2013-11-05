using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;

namespace DeltaEngine.Tutorials.Entities06Triggers
{
	public class Program : App
	{
		public Program()
		{
			var random = Randomizer.Current;
			for (int num = 0; num < 3; num++)
				new Earth(new Vector2D(random.Get(0.2f, 0.8f), random.Get(0.3f, 0.7f)),
					new Vector2D(random.Get(-0.4f, 0.4f), random.Get(-0.3f, 0.3f)));
		}

		public class Earth : Sprite, Updateable
		{
			public Earth(Vector2D position, Vector2D velocity)
				: base(ContentLoader.Load<Material>("Earth"), position)
			{
				Add(new SimplePhysics.Data { Gravity = new Vector2D(0.0f, 0.1f), Velocity = velocity });
				Start<SimplePhysics.BounceIfAtScreenEdge>();
				Start<SimplePhysics.Move>();
			}

			public void Update()
			{
				var allEarths = EntitiesRunner.Current.GetEntitiesOfType<Earth>();
				Set(allEarths.Any(other => this != other && Center.DistanceTo(other.Center) < Size.Width)
					? Color.Yellow : Color.White);
			}

			public bool IsPauseable { get { return true; } }
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}