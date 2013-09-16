namespace $safeprojectname$
{
	public class Constants
	{
		public const int PlayerMaxHp = 100;
		public const float PlayerAcceleration = 1;
		public const float MaximumObjectVelocity = 0.3f;
		public const float ProjectileVelocity = 0.5f;
		public const float PlayerCadance = 0.003f;
		public const float PlayerDecelFactor = 0.7f;
		public const int MaximumAsteroids = 5;
		public const float AsteroidSpawnProbability = 7.0f;
		public enum RenderLayer
		{
			Background,
			Rockets,
			PlayerShip,
			Asteroids,
			UserInterface
		}

		public enum GameState
		{
			Menu,
			Playing,
			GameOver,
			Pause
		}
	}
}