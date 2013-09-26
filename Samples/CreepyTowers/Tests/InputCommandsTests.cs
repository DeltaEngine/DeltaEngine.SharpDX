using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class InputCommandsTests : TestWithMocksOrVisually
	{

		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			manager = new Manager(6.0f);
			inactiveButtonsTagList = new List<string> { Names.ButtonFireTower, Names.ButtonIceTower };
			input = new InputCommands(manager, inactiveButtonsTagList);
		}

		private Manager manager;
		private List<string> inactiveButtonsTagList;
		private InputCommands input;

		[Test]
		public void DiposingInputCommandsHidesTowerPanel()
		{
			input.Dispose();
			Assert.IsFalse(input.CommandsList[0].IsActive);
			Assert.IsFalse(input.CommandsList[1].IsActive);
			Assert.IsFalse(input.CommandsList[2].IsActive);
		}
	}
}
