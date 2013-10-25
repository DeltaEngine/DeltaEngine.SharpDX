using DeltaEngine.Content;
using DeltaEngine.Content.Disk;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
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
	}
}