using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Sprites;

namespace $safeprojectname$
{
	public class Game
	{
		public Game(Window window)
		{
			mainMenu = new Menu();
			mainMenu.InitGame += StartGame;
			mainMenu.QuitGame += window.CloseAfterFrame;
		}

		private readonly Menu mainMenu;

		public void StartGame()
		{
			mainMenu.Hide();
			controls = new Controls(this);
			score = 0;
			SetUpBackground();
			GameState = GameState.Playing;
			InteractionLogics = new InteractionLogics();
			SetUpEvents();
			controls.SetControlsToState(GameState);
			hudInterface = new HudInterface();
		}

		private void SetUpEvents()
		{
			InteractionLogics.GameOver += () => 
			{
				GameOver();
			};
			InteractionLogics.IncreaseScore += increase => 
			{
				score += increase;
				hudInterface.SetScoreText(score);
			};
		}

		private Controls controls;
		private int score;

		public InteractionLogics InteractionLogics
		{
			get;
			private set;
		}

		public GameState GameState;
		private HudInterface hudInterface;

		private static void SetUpBackground()
		{
			var background = new Sprite(new Material(Shader.Position2DColorUv, "black-background"), new 
				Rectangle(Vector2D.Zero, new Size(1)));
			background.RenderLayer = (int)AsteroidsRenderLayer.Background;
		}

		public void GameOver()
		{
			if (GameState == GameState.GameOver)
				return;

			InteractionLogics.Player.IsActive = false;
			GameState = GameState.GameOver;
			controls.SetControlsToState(GameState);
			hudInterface.SetGameOverText();
		}

		public void RestartGame()
		{
			InteractionLogics.Restart();
			score = 0;
			hudInterface.SetScoreText(score);
			hudInterface.SetIngameMode();
			GameState = GameState.Playing;
			controls.SetControlsToState(GameState);
		}
	}
}