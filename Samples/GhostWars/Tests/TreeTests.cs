using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace GhostWars.Tests
{
	class TreeTests : TestWithMocksOrVisually
	{
		[Test]
		public void CreateTree()
		{
			new Tree(Vector2D.Half, Team.HumanYellow);
		}
	}
}
