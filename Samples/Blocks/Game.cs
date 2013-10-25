using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.ScreenSpaces;

namespace Blocks
{
	/// <summary>
	/// Knits the main control classes together by feeding events raised by one to another
	/// </summary>
	public class Game
	{
		public Game(Window window, BlocksContent content)
		{
			this.window = window;
			screenSpace = new Camera2DScreenSpace(window);
			this.content = content;
			screenSpace.Zoom = (window.ViewportPixelSize.AspectRatio > 1)
				? 1 / window.ViewportPixelSize.AspectRatio : window.ViewportPixelSize.AspectRatio;
			var menu = new MainMenu(content);
			menu.InitGame += () =>
			{
				menu.Hide();
				StartGame();
			};
			menu.QuitGame += window.CloseAfterFrame;
			window.ViewportSizeChanged +=
				size =>
				{ 
					if(IsInGame)
						screenSpace.Zoom = (size.AspectRatio > 1) ? 1.8f / size.AspectRatio : 1.8f * size.AspectRatio;
					else
						screenSpace.Zoom = (size.AspectRatio > 1) ? 1 / size.AspectRatio : size.AspectRatio; };
		}

		public UserInterface UserInterface { get; private set; }
		public Controller Controller { get; private set; }
		public bool IsInGame { get; set; }
		private readonly Window window;
		private readonly BlocksContent content;
		private readonly Camera2DScreenSpace screenSpace;


		public void StartGame()
		{
			UserInterface = new UserInterface(content);
			Controller = new Controller(DisplayMode, content);
			window.ViewportSizeChanged += ShowCorrectSceneForAspect;
			IsInGame = true;
			screenSpace.Zoom *= 1.8f;
			Initialize();
		}

		private void Initialize()
		{
			SetDisplayMode();
			ShowCorrectSceneForAspect(window.ViewportPixelSize);
			SetControllerEvents();
			SetInputEvents();
		}

		private void SetDisplayMode()
		{
			window.Title = "Fruit Blocks";
			var aspectRatio = ScreenSpace.Current.Viewport.Aspect;
			DisplayMode = aspectRatio >= 1.0f ? Orientation.Landscape : Orientation.Portrait;
		}

		protected Orientation DisplayMode { get; set; }

		private void ShowCorrectSceneForAspect(Size size)
		{
			if (size.AspectRatio >= 1.0f)
				UserInterface.ShowUserInterfaceLandscape();
			else
				UserInterface.ShowUserInterfacePortrait();
		}

		private void SetControllerEvents()
		{
			Controller.AddToScore += UserInterface.AddToScore;
			Controller.Lose += UserInterface.Lose;
		}

		private void SetInputEvents()
		{
			CreateCommands();
			SetKeyboardEvents();
			SetMouseEvents();
			SetTouchEvents();
		}

		private void CreateCommands()
		{
			commands = new Command[9];
			commands[0] = new Command(() => StartMovingBlockLeft());
			commands[1] = new Command(() => { Controller.isBlockMovingLeft = false; });
			commands[2] = new Command(() => StartMovingBlockRight());
			commands[3] = new Command(() => { Controller.isBlockMovingRight = false; });
			commands[4] = new Command(() => Controller.RotateBlockAntiClockwiseIfPossible());
			commands[5] = new Command(() => { Controller.IsFallingFast = true; });
			commands[6] = new Command(() => { Controller.IsFallingFast = false; });
			commands[7] = new Command(position => { Pressing(position); });
			commands[8] = new Command(() => { Controller.IsFallingFast = false; });
		}

		private Command[] commands;

		private void SetKeyboardEvents()
		{
			commands[0].Add(new KeyTrigger(Key.CursorLeft));
			commands[1].Add(new KeyTrigger(Key.CursorLeft, State.Releasing));
			commands[2].Add(new KeyTrigger(Key.CursorRight));
			commands[3].Add(new KeyTrigger(Key.CursorRight, State.Releasing));
			commands[4].Add(new KeyTrigger(Key.CursorUp));
			commands[4].Add(new KeyTrigger(Key.Space));
			commands[5].Add(new KeyTrigger(Key.CursorDown));
			commands[6].Add(new KeyTrigger(Key.CursorDown, State.Releasing));
		}

		private void StartMovingBlockLeft()
		{
			Controller.MoveBlockLeftIfPossible();
			Controller.isBlockMovingLeft = true;
		}

		private void StartMovingBlockRight()
		{
			Controller.MoveBlockRightIfPossible();
			Controller.isBlockMovingRight = true;
		}

		private void SetMouseEvents()
		{
			commands[7].Add(new MouseButtonTrigger());
			commands[8].Add(new MouseButtonTrigger(MouseButton.Left, State.Releasing));
		}

		private void Pressing(Vector2D position)
		{
			if (position.X < 0.4f)
				Controller.MoveBlockLeftIfPossible();
			else if (position.X > 0.6f)
				Controller.MoveBlockRightIfPossible();
			else if (position.Y < 0.5f)
				Controller.RotateBlockAntiClockwiseIfPossible();
			else
				Controller.IsFallingFast = true;
		}

		private void SetTouchEvents()
		{
			commands[7].Add(new TouchPositionTrigger());
			commands[8].Add(new TouchPositionTrigger(State.Releasing));
		}
	}
}