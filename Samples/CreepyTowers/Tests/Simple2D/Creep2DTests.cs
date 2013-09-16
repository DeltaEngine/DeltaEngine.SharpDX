using CreepyTowers.Simple2D;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering.Sprites;
using NUnit.Framework;

namespace CreepyTowers.Tests.Simple2D
{
	public class Creep2DTests : TestWithBasic2DDisplaySystem
	{
		[Test]
		public void CreateCreep()
		{
			var creep2D = new Creep2D(display, new Point(12, 12), Creep.CreepType.Cloth);
			Assert.AreEqual(new Point(12, 12), creep2D.Position);
			Assert.AreEqual(Creep.CreepType.Sand, creep2D.Type);
			Assert.AreEqual(Color.Yellow, creep2D.Color);
		}

		[Test]
		public void CreateAllCreepTypes()
		{
			int x = 0;
			foreach (var type in EnumExtensions.GetEnumValues<Creep.CreepType>())
				new Creep2D(display, new Point(4 + (x += 2), 10), type);
		}

		[Test]
		public void CreateMovingCreep()
		{
			display.AddCreep(new Point(0, 12), new Point(24, 12), Creep.CreepType.Cloth);
		}
	}
}