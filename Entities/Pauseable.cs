namespace DeltaEngine.Entities
{
	/// <summary>
	/// Only relevant to Updateable and RapidUpdateable Entities. 
	/// By default they will stop updating when the app is paused. Add this interface and set 
	/// IsPausable to false to cause the update to run even when the app is paused.
	/// </summary>
	public interface Pauseable
	{
		bool IsPauseable { get; }
	}
}