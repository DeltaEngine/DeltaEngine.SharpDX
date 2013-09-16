using System.Collections.Generic;
using DeltaEngine.Entities;

namespace DeltaEngine.Mocks
{
	/// <summary>
	/// Entity that does nothing. For unit testing.
	/// </summary>
	public class MockEntity : Entity
	{
		private MockEntity(List<object> createFromComponents)
			: base(createFromComponents) {}

		public MockEntity() {}
	}
}