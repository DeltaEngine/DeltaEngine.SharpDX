using CreepyTowers.Towers;
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
			var tower = new Tower2D(display, new Vector2D(12, 12), TowerType.Water);
			Assert.AreEqual(new Vector2D(12, 12), tower.Position);
			Assert.AreEqual(TowerType.Water, tower.Type);
			Assert.AreEqual(Color.LightBlue, tower.Color);
		}

		[Test]
		public void CreateAllTowerTypes()
		{
			int x = 0;
			foreach (var type in EnumExtensions.GetEnumValues<TowerType>())
				display.AddTower(new Vector2D(10 + (x++), 11), type);
		}
	}
}