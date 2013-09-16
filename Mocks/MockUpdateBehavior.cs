using System.Collections.Generic;
using DeltaEngine.Entities;

namespace DeltaEngine.Mocks
{
	/// <summary>
	/// An update behavior that does nothing during a unit test.
	/// </summary>
	public class MockUpdateBehavior : UpdateBehavior
	{
		public override void Update(IEnumerable<Entity> entities) {}
	}
}