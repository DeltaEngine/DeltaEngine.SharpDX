using System;
using DeltaEngine.Commands;
using DeltaEngine.Input;

namespace SideScroller
{
	public class GameControls
	{
		public GameControls()
		{
			commands = new Command[7];
			SetControlsToState();
		}

		private readonly Command[] commands;

		public void SetControlsToState()
		{
			CreateIngameControls();
		}

		private void CreateIngameControls()
		{
			commands[0] = new Command(() => Ascend());
			commands[1] = new Command(() => VerticalStop());
			commands[2] = new Command(()=> Sink());
			commands[3] = new Command(()=> Accelerate());
			commands[4] = new Command(()=>SlowDown());
			commands[5] = new Command(()=>Fire());
			commands[6] = new Command(()=>HoldFire());

			AddAscensionControls();
			AddSinkingControls();
			AddFireingControls();
			AddAccelerationControls();
			AddSlowingDownControls();
		}

		private void AddAscensionControls()
		{
			commands[0].Add(new KeyTrigger(Key.W, State.Pressed));
			commands[0].Add(new KeyTrigger(Key.W));
			commands[0].Add(new KeyTrigger(Key.CursorUp, State.Pressed));
			commands[0].Add(new KeyTrigger(Key.CursorUp));
			commands[1].Add(new KeyTrigger(Key.W, State.Releasing));
			commands[1].Add(new KeyTrigger(Key.CursorUp, State.Releasing));
		}

		private void AddSinkingControls()
		{
			commands[2].Add(new KeyTrigger(Key.S, State.Pressed));
			commands[2].Add(new KeyTrigger(Key.S));
			commands[2].Add(new KeyTrigger(Key.CursorDown, State.Pressed));
			commands[2].Add(new KeyTrigger(Key.CursorDown));
			commands[1].Add(new KeyTrigger(Key.S, State.Releasing));
			commands[1].Add(new KeyTrigger(Key.CursorDown, State.Releasing));
		}

		private void AddAccelerationControls()
		{
			commands[3].Add(new KeyTrigger(Key.D, State.Pressed));
			commands[3].Add(new KeyTrigger(Key.D));
			commands[3].Add(new KeyTrigger(Key.CursorRight, State.Pressed));
			commands[3].Add(new KeyTrigger(Key.CursorRight));
		}

		private void AddSlowingDownControls()
		{
			commands[4].Add(new KeyTrigger(Key.A, State.Pressed));
			commands[4].Add(new KeyTrigger(Key.A));
			commands[4].Add(new KeyTrigger(Key.CursorLeft, State.Pressed));
			commands[4].Add(new KeyTrigger(Key.CursorLeft));
		}

		private void AddFireingControls()
		{
			commands[5].Add(new KeyTrigger(Key.Space));
			commands[6].Add(new KeyTrigger(Key.Space, State.Releasing));
		}

		public event Action Ascend , Sink , VerticalStop , Accelerate , SlowDown , Fire , HoldFire;
	}
}