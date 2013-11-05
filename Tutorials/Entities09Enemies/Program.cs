using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;

namespace DeltaEngine.Tutorials.Entities09Enemies
{
	public class Program : App
	{
		public Program()
		{
			new Sprite(ContentLoader.Load<Material>("Road"), Rectangle.One).StartMovingUV(new Vector2D(
				0, -0.8f));
			new EnemySpawner();
		}

		public class EnemySpawner : Entity, Updateable
		{
			public void Update()
			{
				if (Time.CheckEvery(2.5f))
					new Enemy();
			}

			public bool IsPauseable { get { return true; } }
		}

		public class Enemy : Sprite
		{
			public Enemy()
				: base(ContentLoader.Load<Material>("Earth"), Rectangle.FromCenter(
				new Vector2D(Randomizer.Current.Get(), 0.1f), new Size(0.1f * 1.35f, 0.1f)))
			{
				Add(new SimplePhysics.Data { Gravity = new Vector2D(0.0f, 0.1f), Duration = 10 });
				Start<SimplePhysics.Move>();
			}
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}