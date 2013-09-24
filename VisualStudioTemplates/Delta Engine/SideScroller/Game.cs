using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	internal class Game : Entity2D
	{
		public Game(Window window) : base(Rectangle.Zero)
		{
			mainMenu = new Menu();
			mainMenu.InitGame += StartGame;
			mainMenu.QuitGame += window.CloseAfterFrame;
		}

		public void StartGame()
		{
			mainMenu.Hide();
			interact = new InteractionLogics();
			playerTexture = new Material(Shader.Position2DColorUv, "PlayerPlane");
			enemyTexture = new Material(Shader.Position2DColorUv, "EnemyPlane");
			player = new PlayerPlane(playerTexture, new Vector2D(0.15f, 0.5f));
			controls = new GameControls();
			background = new ParallaxBackground(4, layerImageNames, layerScrollFactors);
			background.BaseSpeed = 1.2f;
			BindPlayerToControls();
			BindPlayerAndInteraction();
			Start<EnemySpawner>();
		}

		internal PlayerPlane player;
		internal GameControls controls;
		internal InteractionLogics interact;
		internal Material playerTexture;
		internal Material enemyTexture;
		private readonly Menu mainMenu;
		private ParallaxBackground background;

		public void CreateEnemyAtPosition(Vector2D position)
		{
			var enemy = new EnemyPlane(enemyTexture, position);
			player.PlayerFiredShot += point => enemy.CheckIfHitAndReact(point);
			enemy.EnemyFiredShot += point => interact.FireShotByEnemy(point);
		}

		private void BindPlayerToControls()
		{
			controls.Ascend += () => player.AccelerateVertically(-Time.Delta);
			controls.Sink += () => player.AccelerateVertically(Time.Delta);
			controls.VerticalStop += () => player.StopVertically();
			controls.Fire += () => 
			{
				player.IsFireing = true;
			};
			controls.HoldFire += () => 
			{
				player.IsFireing = false;
			};
			controls.SlowDown += () => 
			{
			};
			controls.Accelerate += () => 
			{
			};
		}

		private void BindPlayerAndInteraction()
		{
			player.PlayerFiredShot += point => 
			{
				interact.FireShotByPlayer(point);
			};
		}

		private readonly string[] layerImageNames = new[] {
			"SkyBackground",
			"Mountains_Back",
			"Mountains_Middle",
			"Mountains_Front"
		};
		private readonly float[] layerScrollFactors = new[] {
			0.4f,
			0.6f,
			1.0f,
			1.4f
		};
		private class EnemySpawner : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					if (!(GlobalTime.Current.Milliseconds - timeLastOneSpawned > 2000))
						continue;

					var game = entity as Game;
					game.CreateEnemyAtPosition(new Vector2D(ScreenSpace.Current.Viewport.Right, 
						ScreenSpace.Current.Viewport.Center.Y + alternating * 0.1f));
					timeLastOneSpawned = GlobalTime.Current.Milliseconds;
					alternating *= -1;
				}
			}

			private float timeLastOneSpawned;
			private int alternating = 1;
		}
	}
}