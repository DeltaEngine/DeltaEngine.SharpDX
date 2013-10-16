using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeltaEngine.Core;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace SideScroller.Tests
{
	public class GameTests : TestWithMocksOrVisually
	{
		[Test]
		public void CreateNewGame()
		{
			var game = new Game(Resolve<Window>());
		}
	}
}
