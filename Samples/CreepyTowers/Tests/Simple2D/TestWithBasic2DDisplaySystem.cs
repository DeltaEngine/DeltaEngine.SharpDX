using CreepyTowers.Creeps;
using CreepyTowers.Simple2D;
using CreepyTowers.Towers;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace CreepyTowers.Tests.Simple2D
{
	public class TestWithBasic2DDisplaySystem : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateGrid()
		{
			display = new Basic2DDisplaySystem("LevelsChildrensRoom");
			SelectTower(Tower.TowerType.Acid);
			Assert.AreEqual(new Size(24, 24), display.Size);
			SetCommands();
		}

		protected Basic2DDisplaySystem display;
		private Tower.TowerType selectedTower;

		private void SetCommands()
		{
			new Command(Command.Click,
				pos => { display.AddTower(display.GetGridPosition(pos), selectedTower); });
			new Command(pos => { display.AddWall(display.GetGridPosition(pos)); }).Add(
				new MouseButtonTrigger(MouseButton.Right));
			new Command(pos => { display.RemoveElement(display.GetGridPosition(pos)); }).Add(
				new MouseButtonTrigger(MouseButton.Middle));
			new Command(() => SelectTower(Tower.TowerType.Acid)).Add(new KeyTrigger(Key.D1,
				State.Pressed));
			new Command(() => SelectTower(Tower.TowerType.Blade)).Add(new KeyTrigger(Key.D2,
				State.Pressed));
			new Command(() => SelectTower(Tower.TowerType.Fire)).Add(new KeyTrigger(Key.D3,
				State.Pressed));
			new Command(() => SelectTower(Tower.TowerType.Ice)).Add(new KeyTrigger(Key.D4,
				State.Pressed));
			new Command(() => SelectTower(Tower.TowerType.Impact)).Add(new KeyTrigger(Key.D5,
				State.Pressed));
			new Command(() => SelectTower(Tower.TowerType.Water)).Add(new KeyTrigger(Key.D6,
				State.Pressed));
		}

		private void SelectTower(Tower.TowerType towerType)
		{
			selectedTower = towerType;
			if (selectedTowerRect != null)
				selectedTowerRect.IsActive = false;
			selectedTowerRect = new FilledRect(position, Tower2D.GetColor(towerType));
		}

		private readonly Rectangle position = new Rectangle(0.0f, 0.75f, 0.1f, 0.1f);
		private FilledRect selectedTowerRect;

		[Test]
		public void AddFireTower()
		{
			display.AddTower(new Vector2D(12, 12), Tower.TowerType.Water);
		}

		[Test]
		public void AddClothCreep()
		{
			display.AddCreep(new Vector2D(0, 10), new Vector2D(24, 10), Creep.CreepType.Cloth);
		}

		[Test]
		public void TestPerformanceAStar()
		{
			for (int i = 0; i < 50; i++)
				display.GetPath(new Vector2D(0, 10), new Vector2D(24, 10));
		}
	}
}
