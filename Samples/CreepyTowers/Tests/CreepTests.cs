using System;
using System.Collections.Generic;
using DeltaEngine;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Cameras;
using DeltaEngine.Rendering.Sprites;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class CreepTests : TestWithMocksOrVisually
	{
		/*fix
		[SetUp]
		public void Initialize()
		{
			//window.ViewportPixelSize = new Size(1920, 1080);
			
			//camera =
			//	new OrthoCamera(
			//		new Size(ScreenSpace.Current.Viewport.Width * 6.0f,
			//			ScreenSpace.Current.Viewport.Height * 6.0f), Vector.One * 6.0f);

			//camera.Update();
			//grid = new LevelGrid(20, 0.20f);
			//new Game(Resolve<Window>(), Resolve<Device>()).GameMainMenu.Dispose();
			//camera = Game.CameraAndGrid.Camera;
			//grid = Game.CameraAndGrid.Grid;
		}

		private LevelGrid grid;
		private Camera camera;

		[Test]
		public void CreateClothCreep()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid>();
		}

		private Creep CreateDefaultCreep()
		{
			var position = grid.PropertyMatrix[2, 3].MidPoint;
			var creep = new Creep(position, Creep.CreepType.Cloth, Names.CreepCottonMummy);
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(4, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
			});
			return creep;
		}

		[Test]
		public void DisposingCreepRemovesCreepEntity()
		{
			var creep = CreateDefaultCreep();
			creep.Dispose();
			Assert.IsFalse(creep.IsActive);
		}

		[Test]
		public void DisplayCreepWalkingAlongXAxisForward()
		{
			var creep = CreateDefaultCreep();
			creep.Position = grid.PropertyMatrix[6, 0].MidPoint;
			creep.Remove<MovementInGrid.MovementData>();

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(1.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(0, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(14, 0) }
			});

			Assert.LessOrEqual(grid.PropertyMatrix[14, 0].MidPoint.X, creep.Position.X);
		}

		[Test]
		public void DisplayCreepWalkingAlongXAxisBackward()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[4, 0].MidPoint;
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(1.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(4, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
			});
		}

		[Test]
		public void DisplayCreepWalkingAlongZAxisForward()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[0, 10].MidPoint;
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.0f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(0, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(0, 16) }
			});

			Assert.LessOrEqual(grid.PropertyMatrix[0, 16].MidPoint.Z, creep.Position.Z);
		}

		[Test]
		public void DisplayCreepWalkingAlongZAxisBackwards()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[0, 8].MidPoint;

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.0f, 0.0f, 1.0f),
				StartGridPos = new Tuple<int, int>(0, 10),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(0, 4) }
			});

			Assert.LessOrEqual(grid.PropertyMatrix[0, 16].MidPoint.Z, creep.Position.Z);
		}

		[Test]
		public void MoveCreepAlongWaypoints()
		{
			var position = grid.PropertyMatrix[15, 2].MidPoint;
			var creep = new Creep(position, Creep.CreepType.Cloth, Names.CreepCottonMummy);

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.5f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(15, 2),
				FinalGridPos = new Tuple<int, int>(4, 19),
				Waypoints =
					new List<Tuple<int, int>>
					{
						new Tuple<int, int>(11, 2),
						new Tuple<int, int>(11, 6),
						new Tuple<int, int>(7, 6),
						new Tuple<int, int>(7, 10),
						new Tuple<int, int>(12, 10),
						new Tuple<int, int>(12, 15),
						new Tuple<int, int>(8, 15),
						new Tuple<int, int>(8, 18),
						new Tuple<int, int>(4, 18),
						new Tuple<int, int>(4, 19),
					}
			});
		}

		[Test]
		public void CreepDisappearsUponReachingDestination()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[15, 2].MidPoint;

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.5f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(15, 2),
				FinalGridPos = new Tuple<int, int>(11, 2),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 2) }
			});

			Assert.AreEqual(grid.PropertyMatrix[15, 2].MidPoint, creep.Position);
			AdvanceTimeAndUpdateEntities(3.0f);
			var data = creep.Get<MovementInGrid.MovementData>();
			Assert.AreEqual(grid.PropertyMatrix[data.FinalGridPos.Item1, data.FinalGridPos.Item2].MidPoint,
				creep.Position);
			Assert.IsFalse(creep.IsActive);
		}

		[Test]
		public void CreepDisappearsWhenDead()
		{
			var creep = CreateDefaultCreep();
			creep.Remove<MovementInGrid.MovementData>();
			creep.Position = grid.PropertyMatrix[15, 2].MidPoint;

			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.5f, 0.0f, 0.5f),
				StartGridPos = new Tuple<int, int>(15, 2),
				FinalGridPos = new Tuple<int, int>(11, 2),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 2) }
			});

			creep.ReceiveAttack(Tower.TowerType.Acid, creep.Get<CreepProperties>().MaxHp);
			Assert.LessOrEqual(creep.Get<CreepProperties>().CurrentHp, 0.0f);
			Assert.IsFalse(creep.IsActive);
			Assert.IsFalse(creep.HealthBar.IsActive);
		}

		[Test]
		public void DisplayCreepDyingEffect()
		{
			var creep = CreateDefaultCreep();
			//creep.ReceiveAttack(Tower.TowerType.Acid, creep.Get<CreepProperties>().MaxHp);
			var creep2DPoint = camera.WorldToScreenPoint(creep.Position);
			var drawArea = Rectangle.FromCenter(creep2DPoint, new Size(0.07f));
			//new SpriteSheetAnimation("SpriteDyingCloud", drawArea);
		}

		//[Test]
		//public void RenderFadingLogo()
		//{
		//	var image = ContentLoader.Load<Image>(Names.ComicStripDragon);
		//	var button = new InteractiveButton(CreateTheme(image), new Rectangle(Point.Half, Size.Half));
		//	//var sprite = new Sprite(new Material(image), Rectangle.One);
		//	button.Add(new Transition.Duration(2));
		//	button.Start<Transition>().Add(new Transition.FadingColor(Color.Green));
		//}

		//private static Theme CreateTheme(Image buttonImage)
		//{
		//	var appearance = new Theme.Appearance(buttonImage);
		//	return new Theme
		//	{
		//		Button = appearance,
		//		ButtonDisabled = new Theme.Appearance(buttonImage, Color.Gray),
		//		ButtonMouseover = appearance,
		//		ButtonPressed = appearance,
		//		Font = new Font(Names.FontChelseaMarket14)
		//	};
		//}

		//	[Test, Ignore]
		//	public void ShowHitEffectOnCreep()
		//	{
		//		var creep = new Creep(Point.Half, Creep.CreepType.Sand);
		//		var window = Resolve<ScreenSpace>().Window;
		//		window.ViewportPixelSize = new Size(900.0f);
		//		if (resolver.GetType() == typeof(MockResolver))
		//		{
		//			creep.ReceiveAttack(Tower.TowerType.Water, 0);
		//			AdvanceTimeAndUpdateEntities();
		//			Assert.AreEqual(Color.Red, creep.WalkAnimation.Color);
		//			return;
		//		}

		//		new Command(() => { creep.ReceiveAttack(Tower.TowerType.Water, 0); }).Add(
		//			new MouseButtonTrigger(MouseButton.Left, State.Releasing));
		//	}
		 */
	}
}