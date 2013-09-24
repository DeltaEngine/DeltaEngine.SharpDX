using System;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;

namespace ShadowShot
{
	public class GameInputControls
	{
		public GameInputControls(PlayerShip ship)
		{
			this.ship = ship;
			SetupInputControls();
		}
		private readonly PlayerShip ship;

		private void SetupInputControls()
		{
			AddKeyMoveCommands();
			AddFireCommand();
			AddMouseTouchMovement();
		}

		private void AddKeyMoveCommands()
		{
			var leftCommand = new Command(MoveLeft).Add(new KeyTrigger(Key.CursorLeft));
			leftCommand.Add(new KeyTrigger(Key.CursorLeft, State.Pressed));
			leftCommand.Add(new KeyTrigger(Key.A, State.Pressed)).Add(new KeyTrigger(Key.A));
			var rightCommand = new Command(MoveRight).Add(new KeyTrigger(Key.CursorRight));
			rightCommand.Add(new KeyTrigger(Key.CursorRight, State.Pressed));
			rightCommand.Add(new KeyTrigger(Key.D, State.Pressed)).Add(new KeyTrigger(Key.D));
			var stopCommand = new Command(StopMovement).Add(new KeyTrigger(Key.CursorDown));
			stopCommand.Add(new KeyTrigger(Key.CursorDown, State.Pressed));
			stopCommand.Add(new KeyTrigger(Key.S, State.Pressed)).Add(new KeyTrigger(Key.S));
		}

		private void MoveLeft()
		{
			ship.Accelerate(new Vector2D(-1.0f, 0.0f));
		}

		private void MoveRight()
		{
			ship.Accelerate(new Vector2D(1.0f, 0.0f));
		}

		private void StopMovement()
		{
			ship.Deccelerate();
		}

		private void AddFireCommand()
		{
			var fireCommand = new Command(() => ship.Fire());
			fireCommand.Add(new KeyTrigger(Key.Space, State.Pressed)).Add(new KeyTrigger(Key.Space));
			fireCommand.Add(new TouchPressTrigger());
			fireCommand.Add(new MouseButtonTrigger(MouseButton.Left, State.Pressed)).Add(
				new MouseButtonTrigger());
			fireCommand.Add(new TouchTapTrigger()).Add(new TouchPressTrigger(State.Pressed));
		}

		private void AddMouseTouchMovement()
		{
			var mouseMoveCommand = new Command(MouseControlledMovement);
			mouseMoveCommand.Add(new MouseButtonTrigger()).Add(new MouseButtonTrigger(MouseButton.Left,
				State.Pressed));
			mouseMoveCommand.Add(new TouchTapTrigger()).Add(new TouchPressTrigger(State.Pressed));
		}

		private void MouseControlledMovement(Vector2D position)
		{
			var distance = position.X - ship.Center.X;
			if(Math.Abs (distance) < 0.05f)
				StopMovement();
			else if (distance > 0)
				MoveRight();
			else
				MoveLeft();
		}
	}
}