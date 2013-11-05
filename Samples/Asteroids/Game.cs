using System;
using System.Globalization;
using System.IO;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
using DeltaEngine.Scenes;
using DeltaEngine.ScreenSpaces;

namespace Asteroids
{
	/// <summary>
	/// Game Logics and initialization for Asteroids
	/// </summary>
	public class Game : Scene
	{
		public Game(Window window)
		{
			this.window = window;
			window.ViewportPixelSize = Settings.Current.Resolution;
			screenSpace = new Camera2DScreenSpace(window);
			screenSpace.Zoom = (window.ViewportPixelSize.AspectRatio > 1)
				? 1 / window.ViewportPixelSize.AspectRatio : window.ViewportPixelSize.AspectRatio;
			highScores = new int[10];
			TryLoadingHighscores();
			SetUpBackground();
			mainMenu = new Menu();
			mainMenu.InitGame += StartGame;
			mainMenu.QuitGame += window.CloseAfterFrame;
			InteractionLogics = new InteractionLogics();
			mainMenu.UpdateHighscoreDisplay(highScores);
			//ncrunch: no coverage start
			window.ViewportSizeChanged += size =>
			{
				Settings.Current.Resolution = size;
				if (GameState == GameState.MainMenu)
					screenSpace.Zoom = (size.AspectRatio > 1) ? 1 / size.AspectRatio : size.AspectRatio;
			};
			//ncrunch: no coverage end
		}

		private int[] highScores;
		private readonly Menu mainMenu;
		private readonly Camera2DScreenSpace screenSpace;
		private readonly Window window;

		private void TryLoadingHighscores()
		{
			var highscorePath = GetHighscorePath();
			if (!File.Exists(highscorePath))
				return; //ncrunch: no coverage, can't use files in mocks
			using (var stream = File.OpenRead(highscorePath))
			{
				var reader = new StreamReader(stream);
				GetHighscoresFromString(reader.ReadToEnd());
			}
		}

		private static string GetHighscorePath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				"DeltaEngine", "Asteroids", "Highscores");
		}

		public void GetHighscoresFromString(string highscoreString)
		{
			if (string.IsNullOrEmpty(highscoreString))
				return;
			var partitions = highscoreString.SplitAndTrim(new[] { ',', ' ' });
			highScores = new int[10];
			for (int i = 0; i < highScores.Length; i++)
				try
				{
					highScores[i] = int.Parse(partitions[i]);
				}
				catch
				{
					highScores[i] = 0;
				}
		}

		public void StartGame()
		{
			mainMenu.Hide();
			Show();
			screenSpace.Zoom = 1.0f;
			controls = new Controls(this);
			score = 0;
			GameState = GameState.Playing;
			InteractionLogics.BeginGame();
			SetUpEvents();
			controls.SetControlsToState(GameState);
			HudInterface = new HudInterface();
		}

		private void SetUpEvents()
		{
			InteractionLogics.GameOver += () => { GameOver(); };
			InteractionLogics.IncreaseScore += increase =>
			{
				score += increase;
				HudInterface.SetScoreText(score);
			};
		}

		private Controls controls;
		private int score;
		public InteractionLogics InteractionLogics { get; private set; }
		public GameState GameState;
		public HudInterface HudInterface { get; private set; }

		private void SetUpBackground()
		{
			SetQuadraticBackground("AsteroidsBackground");
		}

		public void GameOver()
		{
			if (GameState == GameState.GameOver)
				return;
			RefreshHighScores();
			InteractionLogics.PauseUpdate();
			InteractionLogics.Player.IsActive = false;
			GameState = GameState.GameOver;
			controls.SetControlsToState(GameState);
			HudInterface.SetGameOverText();
		}

		public void RestartGame()
		{
			InteractionLogics.Restart();
			score = 0;
			HudInterface.SetScoreText(score);
			HudInterface.SetIngameMode();
			GameState = GameState.Playing;
			controls.SetControlsToState(GameState);
		}

		//ncrunch: no coverage start
		public void BackToMenu()
		{
			Hide();
			InteractionLogics.DisposeObjects();
			controls.SetControlsToState(GameState.MainMenu);
			HudInterface.Dispose();
			screenSpace.Zoom = (window.ViewportPixelSize.AspectRatio > 1)
				? 1 / window.ViewportPixelSize.AspectRatio : window.ViewportPixelSize.AspectRatio;
			mainMenu.Show();
		}
		//ncrunch: no coverage end

		private void RefreshHighScores()
		{
			AddLastScoreToHighscoreIfQualified();
			mainMenu.UpdateHighscoreDisplay(highScores);
			SaveHighScore();
		}

		//ncrunch: no coverage start
		private void AddLastScoreToHighscoreIfQualified()
		{
			if (score <= highScores[highScores.Length - 1])
				return;
			if (score > highScores[0])
			{
				highScores[0] = score;
				return;
			}
			for (int i = 0; i < highScores.Length - 2; i++)
				if (highScores[i] > score && score > highScores[i + 1])
					InsertNewScoreAt(i + 1);
		}

		private void InsertNewScoreAt(int index)
		{
			var scoreBuffer = highScores;
			highScores = new int[10];
			for (int i = 0; i < 10; i++)
				if (i == index)
					highScores[i] = score;
				else if (i > index)
					highScores[i] = scoreBuffer[i - 1];
				else
					highScores[i] = scoreBuffer[i];
		}
		//ncrunch: no coverage end

		private void SaveHighScore()
		{
			var highscoreFilePath = GetHighscorePath();
			using (FileStream highscoreFile = File.Create(highscoreFilePath))
			{
				var writer = new StreamWriter(highscoreFile);
				writer.Write(CreateHighscoreString());
				writer.Flush();
			}
		}

		private string CreateHighscoreString()
		{
			var stringOfScores = highScores[0].ToString(CultureInfo.InvariantCulture);
			for (int i = 1; i < highScores.Length; i++)
				stringOfScores += ", " + highScores[i].ToString(CultureInfo.InvariantCulture);
			return stringOfScores;
		}
	}
}