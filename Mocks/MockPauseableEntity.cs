using DeltaEngine.Entities;

namespace DeltaEngine.Mocks
{
	/// <summary>
	/// Updateable, Pauseable Entity that does nothing. For unit testing.
	/// </summary>
	public class MockPauseableEntity : Entity, Updateable, Pauseable, VerifiableUpdate
	{
		public MockPauseableEntity(bool isPauseable)
		{
			IsPauseable = isPauseable;
		}

		public bool IsPauseable { get; set; }

		public void Update()
		{
			WasUpdated = true;
		}

		public bool WasUpdated { get; set; }
	}
}