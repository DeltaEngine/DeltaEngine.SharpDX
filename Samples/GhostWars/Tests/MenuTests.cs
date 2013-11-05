using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace GhostWars.Tests
{
	public class MenuTests : TestWithMocksOrVisually
	{
		//ncrunch: no coverage start
		[Test, Ignore]
		public void ShowMenu()
		{
			Resolve<Settings>().Resolution = new Size(1200, 750);
			new MainMenu(Resolve<Window>());
		}
		//ncrunch: no coverage end

		[Test]
		public void SetGameOverState()
		{
			var menu = new MainMenu(Resolve<Window>());
			menu.CurrentLevel = 1;
			menu.Clear();
			menu.SetGameOverState();
		}
	}
}