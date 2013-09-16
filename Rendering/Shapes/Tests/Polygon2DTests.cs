using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Shapes.Tests
{
	public class Polygon2DTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void NewPolygon()
		{
			var polygon = new Polygon2D(Rectangle.One, Color.White);
			polygon.Points.AddRange(new[] { Point.Zero, Point.One, Point.UnitY });
			Assert.AreEqual(Rectangle.One, polygon.DrawArea);
			Assert.AreEqual(Color.White, polygon.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeOutlineColor()
		{
			var polygon = new Polygon2D(Rectangle.One, Color.Red);
			polygon.Add(new OutlineColor(Color.Blue));
			Assert.AreEqual(Color.Blue, polygon.Get<OutlineColor>().Value);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderEllipseOutline()
		{
			var ellipse = new Ellipse(Point.Half, 0.4f, 0.2f, Color.Blue);
			ellipse.Add(new OutlineColor(Color.Red));
			ellipse.OnDraw<DrawPolygon2DOutlines>();
		}

		[Test, CloseAfterFirstFrame]
		public void RenderingPolygonWithNoPointsDoesNotError()
		{
			var ellipse = new Ellipse(Point.Half, 0.4f, 0.2f, Color.Blue);
			var points = ellipse.Get<List<Point>>();
			points.Clear();
			ellipse.Remove<Ellipse.UpdatePoints>();
			ellipse.Add(new OutlineColor(Color.Red));
			ellipse.OnDraw<DrawPolygon2DOutlines>();
		}

		[Test]
		public void NoErrorIfThereAreNoVertices()
		{
			var entity = new Entity2D(Rectangle.One);
			entity.Add(new OutlineColor(Color.Red));
			Assert.DoesNotThrow(entity.OnDraw<DrawPolygon2DOutlines>);
		}
	}
}