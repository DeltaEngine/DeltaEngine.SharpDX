using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace EmptyGame
{
	/// <summary>
	/// Just creates a window and slowly changes the background color.
	/// </summary>
	public class Game : Entity
	{
		public Game(Window window)
		{
			this.window = window;
			Start<ColorUpdate>();
		}

		public readonly Window window;
		public float FadePercentage { get; private set; }
		public Color CurrentColor { get; private set; }
		public Color NextColor { get; private set; }

		private void SwitchToNextRandomColor()
		{
			CurrentColor = NextColor;
			NextColor = Color.GetRandomColor();
			FadePercentage = 0;
		}

		public class ColorUpdate : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var gameEntity = entity as Game;
					gameEntity.FadePercentage += Time.Delta;
					if (gameEntity.FadePercentage >= 1.0f)
						gameEntity.SwitchToNextRandomColor();

					gameEntity.window.BackgroundColor = gameEntity.CurrentColor.Lerp(
						gameEntity.NextColor, gameEntity.FadePercentage);
				}
			}
		}
	}
}