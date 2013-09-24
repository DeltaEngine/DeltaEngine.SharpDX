using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Sprites;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class LevelsBathroomTests : TestWithMocksOrVisually
	{
		/*fix
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>()).GameMainMenu.Dispose();
			//level = new LevelBathRoom();
			level = null;
		}

		private LevelBathRoom level;

		[Test]
		public void ShowBathroomLevelWithGrid()
		{
			new LevelBathRoom();
		}

		[Test]
		public void DiposingLevelRemovesLevel()
		{
			level = new LevelBathRoom();
			level.Dispose();
		}

		[Test]
		public void TransitionImage()
		{
			var attackImage =
				new Sprite(
					new Material (Shader.Position2DUv, Names.DragonAttackMockup ),
					Rectangle.One) { RenderLayer = int.MinValue };
			//attackImage.Add(new Transition.Duration(2.0f)).Add(new Transition.FadingColor());
			//attackImage.Start<Transition>();
			//attackImage.Start<FinalTransition>();
		}
		 */
	}
}