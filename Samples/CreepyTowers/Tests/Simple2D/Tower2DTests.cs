using CreepyTowers.Simple2D;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using NUnit.Framework;

namespace CreepyTowers.Tests.Simple2D
{
	public class Tower2DTests : TestWithBasic2DDisplaySystem
	{
		[Test]
		public void CreateTower()
		{
			var tower = new Tower2D(display, new Point(12, 12), Tower.TowerType.Water);
			Assert.AreEqual(new Point(12, 12), tower.Position);
			Assert.AreEqual(Tower.TowerType.Water, tower.Type);
			Assert.AreEqual(Color.LightBlue, tower.Color);
		}

		[Test]
		public void CreateAllTowerTypes()
		{
			int x = 0;
			foreach (var type in EnumExtensions.GetEnumValues<Tower.TowerType>())
				display.AddTower(new Point(10 + (x++), 11), type);
		}
	}
}