using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering.Particles;
using DeltaEngine.Rendering.Sprites;

namespace $safeprojectname$
{
	public class Level : IDisposable
	{
		public Level(Score score)
		{
			brickMaterial = new Material(Shader.Position2DColorUv, "Brick");
			var explosionMaterial = new Material(Shader.Position2DColorUv, "Explosion");
			explosionSound = ContentLoader.Load<Sound>("BrickExplosion");
			lostBallSound = ContentLoader.Load<Sound>("LostBall");
			explosionData = new ParticleEmitterData {
				Color = new RangeGraph<Color>(Color.White, Color.TransparentWhite),
				Size = new RangeGraph<Size>(ExplosionSize, ExplosionSize * 2),
				ParticleMaterial = explosionMaterial,
				MaximumNumberOfParticles = 1,
				LifeTime = 0.6f,
				SpawnInterval = -1
			};
			this.score = score;
			Initialize();
		}

		private readonly Material brickMaterial;
		private readonly ParticleEmitterData explosionData;
		private readonly Sound explosionSound;
		private readonly Sound lostBallSound;
		private readonly Score score;

		private void Initialize()
		{
			rows = score.Level + 1;
			columns = score.Level + 1;
			bricks = new Sprite[rows, columns];
			brickWidth = 1.0f / rows;
			brickHeight = 0.5f / columns;
			CreateBricks();
		}

		private float brickWidth;
		private float brickHeight;
		protected int rows;
		protected int columns;
		protected Sprite[,] bricks;

		private void CreateBricks()
		{
			for (int x = 0; x < rows; x++)
				for (int y = 0; y < columns; y++)
				{
					bricks [x, y] = new Sprite(brickMaterial, GetBounds(x, y));
					bricks [x, y].Set(GetBrickColor(x, y));
					bricks [x, y].RenderLayer = 5;
				}
		}

		private Rectangle GetBounds(int x, int y)
		{
			return new Rectangle(x * brickWidth, y * brickHeight, brickWidth, brickHeight);
		}

		private Color GetBrickColor(int x, int y)
		{
			if (score.Level <= 1)
				return GetLevelOneBrickColor(x, y);

			if (score.Level == 2)
				return GetLevelTwoBrickColor(y);

			if (score.Level == 3)
				return GetLevelThreeBrickColor(x, y);

			if (score.Level == 4)
				return GetLevelFourBrickColor(x, y);

			return GetLevelFiveOrAboveBrickColor(x, y);
		}

		private static Color GetLevelOneBrickColor(int x, int y)
		{
			return x + y == 1 ? Color.Green : Color.Orange;
		}

		private static Color GetLevelTwoBrickColor(int y)
		{
			if (y == 0)
				return new Color(0.25f, 0.25f, 0.25f);

			return y == 1 ? Color.Red : Color.Gold;
		}

		private static Color GetLevelThreeBrickColor(int x, int y)
		{
			return LevelThreeColors [(x * 4 + y) % LevelThreeColors.Length];
		}

		private static readonly Color[] LevelThreeColors = new[] {
			Color.Yellow,
			Color.Teal,
			Color.Green,
			Color.LightBlue,
			Color.Teal
		};

		private static Color GetLevelFourBrickColor(int x, int y)
		{
			return new Color(x * 0.2f + 0.1f, 0.2f, (x + y / 2) * 0.15f + 0.2f);
		}

		private static Color GetLevelFiveOrAboveBrickColor(int x, int y)
		{
			return new Color(0.9f - x * 0.15f, 0.5f, (x + y / 2) * 0.1f + 0.2f);
		}

		public void InitializeNextLevel()
		{
			Dispose();
			score.NextLevel();
			Initialize();
		}

		public void Dispose()
		{
			for (int x = 0; x < rows; x++)
				for (int y = 0; y < columns; y++)
					bricks [x, y].Visibility = Visibility.Hide;
		}

		public int BricksLeft
		{
			get
			{
				var bricksAlive = 0;
				for (int x = 0; x < rows; x++)
					for (int y = 0; y < columns; y++)
						if (bricks [x, y].Visibility == Visibility.Show)
							bricksAlive++;

				return bricksAlive;
			}
		}

		public Sprite GetBrickAt(float x, float y)
		{
			var brickIndexX = (int)(x / brickWidth);
			var brickIndexY = (int)(y / brickHeight);
			if (brickIndexX < 0 || brickIndexX >= rows || brickIndexY < 0 || brickIndexY >= columns || 
				bricks [brickIndexX, brickIndexY].Visibility != Visibility.Show)
				return null;

			return bricks [brickIndexX, brickIndexY];
		}

		public void Explode(Sprite brick, Point collision)
		{
			score.IncreasePoints();
			brick.Visibility = Visibility.Hide;
			CreateExplosion(collision);
			explosionSound.Play();
		}

		private void CreateExplosion(Point collision)
		{
			var explosion = new Particle2DEmitter(explosionData, collision);
			explosion.RenderLayer = 16;
			explosion.SpawnBurst(1, true);
		}

		private static readonly Size ExplosionSize = new Size(0.1f, 0.1f);

		public void LifeLost()
		{
			score.LifeLost();
			lostBallSound.Play();
		}
	}
}