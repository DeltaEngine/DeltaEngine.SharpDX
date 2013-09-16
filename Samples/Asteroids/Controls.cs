using DeltaEngine.Commands;
using DeltaEngine.Input;

namespace Asteroids
{
	/// <summary>
	/// Controls, can be set according to a gameState
	/// </summary>
	public class Controls
	{
		public Controls(Game game)
		{
			this.game = game;
			controlCommands = new Command[5];
			commandsInUse = 0;
		}

		private readonly Game game;

		private readonly Command[] controlCommands;
		private int commandsInUse;

		public void SetControlsToState(GameState state)
		{
			for (int i = 0; i < commandsInUse; i++)
				controlCommands[i].IsActive = false;
			commandsInUse = 0;
			switch (state)
			{
			case GameState.Playing:
				AddPlayerAccelerationControl();
				AddPlayerSteerLeft();
				AddPlayerSteerRight();
				AddPlayerShootingControls();
				break;
			case GameState.GameOver:
				AddRestartControl();
				break;
			default:
				return;
			}
		}

		private void AddPlayerAccelerationControl()
		{
			controlCommands[0] = new Command(PlayerForward);
			controlCommands[0].Add(new KeyTrigger(Key.W, State.Pressed));
			controlCommands[0].Add(new KeyTrigger(Key.W));
			controlCommands[0].Add(new KeyTrigger(Key.CursorUp, State.Pressed));
			controlCommands[0].Add(new KeyTrigger(Key.CursorUp));
			commandsInUse++;
		}

		private void AddPlayerSteerLeft()
		{
			controlCommands[1] = new Command(PlayerSteerLeft);
			controlCommands[1].Add(new KeyTrigger(Key.A, State.Pressed));
			controlCommands[1].Add(new KeyTrigger(Key.A));
			controlCommands[1].Add(new KeyTrigger(Key.CursorLeft, State.Pressed));
			controlCommands[1].Add(new KeyTrigger(Key.CursorLeft));
			commandsInUse++;
		}

		private void AddPlayerSteerRight()
		{
			controlCommands[2] = new Command(PlayerSteerRight);
			controlCommands[2].Add(new KeyTrigger(Key.D, State.Pressed));
			controlCommands[2].Add(new KeyTrigger(Key.D));
			controlCommands[2].Add(new KeyTrigger(Key.CursorRight, State.Pressed));
			controlCommands[2].Add(new KeyTrigger(Key.CursorRight));
			commandsInUse++;
		}

		private void AddPlayerShootingControls()
		{
			controlCommands[3] = new Command(PlayerBeginFireing).Add(new KeyTrigger(Key.Space));
			controlCommands[4] = new Command(PlayerHoldFire).Add(new KeyTrigger(Key.Space, State.Releasing));
			commandsInUse += 2;
		}

		private void AddRestartControl()
		{
			controlCommands[0] = new Command(() => game.RestartGame());
			controlCommands[0].Add(new KeyTrigger(Key.Space, State.Releasing));
			commandsInUse++;
		}

		private void PlayerForward()
		{
			game.InteractionLogics.Player.ShipAccelerate();
		}

		private void PlayerSteerLeft()
		{
			game.InteractionLogics.Player.SteerLeft();
		}

		private void PlayerSteerRight()
		{
			game.InteractionLogics.Player.SteerRight();
		}

		private void PlayerBeginFireing()
		{
			game.InteractionLogics.Player.IsFiring = true;
		}

		private void PlayerHoldFire()
		{
			game.InteractionLogics.Player.IsFiring = false;
		}
	}
}