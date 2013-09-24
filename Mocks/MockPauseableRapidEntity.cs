using DeltaEngine.Entities;

namespace DeltaEngine.Mocks
{
	/// <summary>
	/// RapidUpdateable, Pauseable Entity that does nothing. For unit testing.
	/// </summary>
	public class MockPauseableRapidEntity : Entity, RapidUpdateable, Pauseable, VerifiableUpdate
	{
		public MockPauseableRapidEntity(bool isPauseable)
		{
			IsPauseable = isPauseable;
		}

		public bool IsPauseable { get; set; }

		public void RapidUpdate()
		{
			WasUpdated = true;
		}

		public bool WasUpdated { get; set; }
	}
}