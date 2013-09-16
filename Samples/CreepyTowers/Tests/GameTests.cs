using DeltaEngine.Core;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class GameTests : TestWithMocksOrVisually
	{
		[Test]
		public void CheckGameCreation()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
		}
	}
}