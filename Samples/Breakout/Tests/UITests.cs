using DeltaEngine.Core;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Breakout.Tests
{
	public class UITests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void UpdateWindowTitle()
		{
			var ui = Resolve<UI>();
			AdvanceTimeAndUpdateEntities(0.2f);
			Assert.True(ui.IsPauseable);
			Assert.IsTrue(Resolve<Window>().Title.Contains("Breakout "));
		}
	}
}