using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace GhostWars.Tests
{
	public class EffectTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowArrow()
		{
			new Command(Command.Click, position => Effects.CreateArrow(Point.Half, position));
		}

		[Test]
		public void ShowDeathEffect()
		{
			new Command(Command.Click, position => Effects.CreateDeathEffect(position));
		}

		[Test]
		public void ShowHitEffect()
		{
			new Command(Command.Click, position => Effects.CreateHitEffect(position));
		}

		[Test]
		public void ShowSparkleEffect()
		{
			Effects.CreateSparkleEffect(Team.HumanYellow, Point.Half, 20);
		}
	}
}