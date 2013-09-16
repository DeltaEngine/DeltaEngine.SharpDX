namespace DeltaEngine.Commands
{
	public abstract class InputTrigger : Trigger
	{
		protected InputTrigger()
		{
			StartInputDevice();
		}

		protected abstract void StartInputDevice();
	}
}