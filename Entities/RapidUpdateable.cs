namespace DeltaEngine.Entities
{
	/// <summary>
	/// Certain entities need to be updated more often than normally. By default only physics
	/// collision checking needs to runs much quicker to provide events more accurately. When
	/// used RapidUpdate is called 60 times per second by default (no matter how fast the game runs).
	/// </summary>
	public interface RapidUpdateable
	{
		void RapidUpdate();
	}
}