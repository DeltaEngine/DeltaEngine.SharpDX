namespace DeltaEngine.Entities
{
	/// <summary>
	/// Most entities do not need to be updated, they use default behaviors. This interface adds a
	/// Update method, which is automatically called (by default 20 times per second) at the given
	/// UpdatePriority for each active entity. See <see cref="RapidUpdateable"/> for faster updates.
	/// </summary>
	public interface Updateable
	{
		void Update();
	}
}