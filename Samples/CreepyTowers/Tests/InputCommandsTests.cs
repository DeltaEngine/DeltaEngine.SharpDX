using System.Collections.Generic;
using DeltaEngine.Core;

using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
//TODO: should not be done manually, load from content file
	public class InputCommandsTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>());
			manager = new Manager(6.0f);
			inactiveButtonsTagList = new List<string> { Content.GUI.ButtonFireTower.ToString(),
				Content.GUI.ButtonIceTower.ToString() };
			input = new InGameCommands(manager, inactiveButtonsTagList);
		}

		private Manager manager;
		private List<string> inactiveButtonsTagList;
		private InGameCommands input;

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