using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace LogoApp.Tests
{
	public class BouncingLogoTests : TestWithMocksOrVisually
	{
		[Test]
		public void Create()
		{
			var logo = new BouncingLogo();
			Assert.IsTrue(logo.Center.X > 0);
			Assert.IsTrue(logo.Center.Y > 0);
			Assert.AreNotEqual(Color.Black, logo.Color);
		}

		[Test]
		public void RunAFewTimesAndCloseGame()
		{
			new BouncingLogo();
		}

		[Test]
		public void ShowManyLogos()
		{
			for (int i = 0; i < 100; i++)
				new BouncingLogo();
		}

		[Test, CloseAfterFirstFrame]
		public void Drawing100LogosOnlyCauseOneDrawCall()
		{
			for (int i = 0; i < 100; i++)
				new BouncingLogo();
			RunAfterFirstFrame(() =>
			{
				Assert.AreEqual(1, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame);
				Assert.AreEqual(100 * 4, Resolve<Drawing>().NumberOfDynamicVerticesDrawnThisFrame);
			});
		}

		[Test]
		public void Draw1000LogosToTestPerformance()
		{
			for (int i = 0; i < 1000; i++)
				new BouncingLogo();
		}

		[Test]
		public void PressingSpacePausesLogoApp()
		{
			for (int i = 0; i < 10; i++)
				new BouncingLogo();
			new Command(() => Time.IsPaused = !Time.IsPaused).Add(new KeyTrigger(Key.Space));
		}
	}
}