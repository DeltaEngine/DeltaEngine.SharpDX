using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Tutorials.Entities10LogoRacer
{
	public class Program : App
	{
		public Program()
		{
			new Sprite(ContentLoader.Load<Material>("Road"), Rectangle.One).StartMovingUV(new Vector2D(
				0, -0.8f));
			new ScoreDisplay(new Player(), new EnemySpawner());
			new Command(Command.Exit, Resolve<Window>().CloseAfterFrame);
		}

		public class ScoreDisplay : FontText, Updateable
		{
			public ScoreDisplay(Player player, EnemySpawner spawner)
				: base(Font.Default, "Score: ", Rectangle.FromCenter(0.5f, 0.25f, 0.2f, 0.1f))
			{
				this.player = player;
				this.spawner = spawner;
				RenderLayer = 1;
			}

			private readonly Player player;
			private readonly EnemySpawner spawner;

			public void Update()
			{
				if (Text.StartsWith("Game Over"))
					return;
				if (player.Color == Color.White)
					Text = "Score: " + spawner.EnemiesSpawned;
				else
					Text = "Game Over! " + Text;
			}

			public bool IsPauseable { get { return false; } }
		}

		public class EnemySpawner : Entity, Updateable
		{
			public void Update()
			{
				if (!Time.CheckEvery(spawnTime))
					return;
				EnemiesSpawned++;
				if (spawnTime > 0.5f)
					spawnTime -= 0.15f;
				new Enemy();
			}

			private float spawnTime = 2.5f;
			public int EnemiesSpawned { get; private set; }
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

		public class Player : Sprite, Updateable
		{
			public Player()
				: base(ContentLoader.Load<Material>("Logo"), new Vector2D(0.5f, 0.7f))
			{
				commands.Add(new Command(Command.MoveLeft,
					() => Center -= new Vector2D(Time.Delta * 0.5f, 0)));
				commands.Add(new Command(Command.MoveRight,
					() => Center += new Vector2D(Time.Delta * 0.5f, 0)));
			}

			private readonly List<Command> commands = new List<Command>();

			public void Update()
			{
				var earths = EntitiesRunner.Current.GetEntitiesOfType<Enemy>();
				if (!earths.Any(e => Center.DistanceTo(e.Center) < Size.Width / 3 + e.Size.Width / 2))
					return;
				Set(Color.Red);
				foreach (var command in commands)
					command.IsActive = false;
			}

			public bool IsPauseable { get { return true; } }
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}