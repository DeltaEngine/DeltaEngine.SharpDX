using CreepyTowers.Creeps;
using CreepyTowers.Simple2D;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using NUnit.Framework;

namespace CreepyTowers.Tests.Simple2D
{
	public class Creep2DTests : TestWithBasic2DDisplaySystem
	{
		[Test]
		public void CreateCreep()
		{
			var creep2D = new Creep2D(display, new Vector2D(12, 12), Creep.CreepType.Cloth);
			Assert.AreEqual(new Vector2D(12, 12), creep2D.Position);
			Assert.AreEqual(Creep.CreepType.Sand, creep2D.Type);
			Assert.AreEqual(Color.Yellow, creep2D.Color);
		}

		[Test]
		public void CreateAllCreepTypes()
		{
			int x = 0;
			foreach (var type in EnumExtensions.GetEnumValues<Creep.CreepType>())
				new Creep2D(display, new Vector2D(4 + (x += 2), 10), type);
		}

		[Test]
		public void CreateMovingCreep()
		{
			display.AddCreep(new Vector2D(0, 12), new Vector2D(24, 12), Creep.CreepType.Cloth);
		}
	}
}