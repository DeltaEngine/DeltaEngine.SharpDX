using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Cameras;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Shapes3D.Tests
{
	public class Line3DTests : TestWithMocksOrVisually
	{
		[Test]
		public void RenderCoordinateSystemCross()
		{
			CreateLookAtCamera(Vector.One * 4.0f, Vector.Zero);
			new Line3D(-Vector.UnitX, Vector.UnitX * 3, Color.Red);
			new Line3D(-Vector.UnitY, Vector.UnitY * 3, Color.Green);
			new Line3D(-Vector.UnitZ, Vector.UnitZ * 3, Color.Blue);
		}
		
		private static void CreateLookAtCamera(Vector position, Vector target)
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = position;
			camera.Target = target;
		}

		[Test]
		public void RenderGrid()
		{
			const int GridSize = 10;
			const float GridScale = 0.5f;
			const float HalfGridSize = GridSize * 0.5f;
			var axisXz = new Point(-HalfGridSize, -HalfGridSize);
			CreateLookAtCamera(Vector.One * 4.0f, Vector.Zero);
			for (int i = 0; i <= GridSize; i++, axisXz.X += 1, axisXz.Y += 1)
			{
				new Line3D(new Vector(-HalfGridSize * GridScale, axisXz.Y * GridScale, 0.0f),
					new Vector(HalfGridSize * GridScale, axisXz.Y * GridScale, 0.0f), Color.White);
				new Line3D(new Vector(axisXz.X * GridScale, -HalfGridSize * GridScale, 0.0f),
					new Vector(axisXz.X * GridScale, HalfGridSize * GridScale, 0.0f), Color.White);
			}
		}

		[Test]
		public void RenderRedLine()
		{
			CreateLookAtCamera(Vector.UnitY, Vector.Zero);
			new Line3D(-Vector.UnitX, Vector.UnitX, Color.Red);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateLine3D()
		{
			CreateLookAtCamera(Vector.One * 4.0f, Vector.Zero);
			var entity = new Line3D(Vector.Zero, Vector.One, Color.Red);
			Assert.AreEqual(Vector.Zero, entity.StartPoint);
			Assert.AreEqual(Vector.One, entity.EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void SetLine3DPoints()
		{
			CreateLookAtCamera(Vector.One * 4.0f, Vector.Zero);
			var entity = new Line3D(Vector.Zero, Vector.Zero, Color.Red)
			{
				StartPoint = Vector.UnitX,
				EndPoint = Vector.UnitY
			};
			Assert.AreEqual(Vector.UnitX, entity.StartPoint);
			Assert.AreEqual(Vector.UnitY, entity.EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void SetLine3DPointList()
		{
			CreateLookAtCamera(Vector.One * 4.0f, Vector.Zero);
			var entity = new Line3D(Vector.Zero, Vector.Zero, Color.Red)
			{
				Points = new List<Vector> { Vector.UnitZ, Vector.UnitY }
			};
			Assert.AreEqual(Vector.UnitZ, entity.StartPoint);
			Assert.AreEqual(Vector.UnitY, entity.EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void RenderingHiddenLineDoesNotThrowException()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector.UnitZ;
			new Line3D(-Vector.UnitX, Vector.UnitX, Color.Red) { Visibility = Visibility.Hide };
			Assert.DoesNotThrow(() => AdvanceTimeAndUpdateEntities());
		}
	}
}