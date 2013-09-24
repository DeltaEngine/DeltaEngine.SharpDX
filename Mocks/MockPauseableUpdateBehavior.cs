using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Entities;

namespace DeltaEngine.Mocks
{
	/// <summary>
	/// A pauseable update behavior that does nothing during a unit test.
	/// </summary>
	public class MockPauseableUpdateBehavior : UpdateBehavior, Pauseable
	{
		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (var verifable in entities.OfType<VerifiableUpdate>())
				verifable.WasUpdated = true;
		}

		public bool IsPauseable
		{
			get { return true; }
		}
	}
}