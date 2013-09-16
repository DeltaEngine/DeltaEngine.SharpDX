using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class RectangleTests
	{
		[Test]
		public void Create()
		{
			var point = new Point(2f, 2f);
			var size = new Size(1f, 1f);
			var rect = new Rectangle(point, size);
			Assert.AreEqual(point.X, rect.Left);
			Assert.AreEqual(point.Y, rect.Top);
			Assert.AreEqual(size.Width, rect.Width);
			Assert.AreEqual(size.Height, rect.Height);
			Assert.AreEqual(point, rect.TopLeft);
			Assert.AreEqual(size, rect.Size);
		}

		[Test]
		public void StaticRectangles()
		{
			Assert.AreEqual(new Rectangle(0, 0, 0, 0), Rectangle.Zero);
			Assert.AreEqual(new Rectangle(0, 0, 1, 1), Rectangle.One);
			Assert.AreEqual(new Rectangle(Point.Unused, Size.Unused), Rectangle.Unused);
		}

		[Test]
		public void SizeOfRectangle()
		{
			Assert.AreEqual(16, Rectangle.SizeInBytes);
		}

		[Test]
		public void ChangeValues()
		{
			var rect = Rectangle.One;
			rect.Left = 2;
			rect.Top = 1;
			rect.Width = 2;
			rect.Height = 3;
			Assert.AreEqual(new Rectangle(2, 1, 2, 3), rect);
		}

		[Test]
		public void Equals()
		{
			var rect1 = new Rectangle(3, 4, 1, 2);
			var rect2 = new Rectangle(5, 6, 1, 2);
			Assert.AreNotEqual(rect1, rect2);
			Assert.AreEqual(rect1, new Rectangle(3, 4, 1, 2));
			Assert.IsTrue(rect1 == new Rectangle(3, 4, 1, 2));
			Assert.IsTrue(rect1 != rect2);
			Assert.IsFalse(rect1.Equals(rect2));
			Assert.IsTrue(rect1.Equals(rect1));
			Assert.False(rect1.Equals((object)Rectangle.One));
		}

		[Test]
		public void GetHashCodeViaDictionary()
		{
			var rect1 = new Rectangle(3, 4, 1, 2);
			var rect2 = new Rectangle(5, 6, 1, 2);
			var rectValues = new Dictionary<Rectangle, int> { { rect1, 1 }, { rect2, 2 } };
			Assert.IsTrue(rectValues.ContainsKey(rect1));
			Assert.IsTrue(rectValues.ContainsKey(rect2));
			Assert.IsFalse(rectValues.ContainsKey(new Rectangle(3, 9, 1, 2)));
		}

		[Test]
		public void RectangleToString()
		{
			var p = new Point(2f, 2f);
			var s = new Size(1f, 1f);
			var rect = new Rectangle(p, s);
			Assert.AreEqual("2 2 1 1", rect.ToString());
		}

		[Test]
		public void RectangleToStringAndFromString()
		{
			var rect = new Rectangle(2.12f, 2.12f, 1.12f, 1.12f);
			string rectString = rect.ToString();
			Assert.AreEqual(rect, new Rectangle(rectString));
			Assert.AreEqual(Rectangle.One, new Rectangle("0 0 1 1"));
			Assert.Throws<Rectangle.InvalidNumberOfComponents>(() => new Rectangle("abc"));
		}
		
		[Test]
		public static void FromInvariantStringWithDatatypes()
		{
			Assert.AreEqual(Size.Zero, "0,0".Convert<Size>());
			Assert.AreEqual(Point.UnitX, "1,0".Convert<Point>());
			Assert.AreEqual(new Rectangle(1, 1, 2, 2), "1 1 2 2".Convert<Rectangle>());
		}

		[Test]
		public void Right()
		{
			var rect = new Rectangle(1, 2, 10, 20) { Right = 13 };
			Assert.AreEqual(3, rect.Left);
			Assert.AreEqual(13, rect.Right);
			Assert.AreEqual(10, rect.Width);
		}

		[Test]
		public void Bottom()
		{
			var rect = new Rectangle(1, 2, 10, 20) { Bottom = 23 };
			Assert.AreEqual(3, rect.Top);
			Assert.AreEqual(23, rect.Bottom);
			Assert.AreEqual(20, rect.Height);
		}

		[Test]
		public void TopRight()
		{
			var rect = new Rectangle(1, 2, 10, 20);
			Assert.AreEqual(new Point(11, 2), rect.TopRight);
		}

		[Test]
		public void BottomLeft()
		{
			var rect = new Rectangle(1, 2, 10, 20);
			Assert.AreEqual(new Point(1, 22), rect.BottomLeft);
		}

		[Test]
		public void BottomRight()
		{
			var rect = new Rectangle(1, 2, 10, 20);
			Assert.AreEqual(new Point(11, 22), rect.BottomRight);
		}

		[Test]
		public void GetCenter()
		{
			var rect = new Rectangle(4, 4, 4, 4);
			Assert.AreEqual(new Point(4, 4), rect.TopLeft);
			Assert.AreEqual(new Point(8, 8), rect.BottomRight);
			Assert.AreEqual(new Point(6, 6), rect.Center);
		}

		[Test]
		public void SetCenter()
		{
			var rect = new Rectangle(8, 10, 2, 2) { Center = Point.One };
			Assert.AreEqual(new Point(0, 0), rect.TopLeft);
			Assert.AreEqual(new Point(2, 2), rect.BottomRight);
			Assert.AreEqual(new Point(1, 1), rect.Center);
		}

		[Test]
		public void Contains()
		{
			var rect = new Rectangle(1, 2, 10, 20);
			Assert.IsTrue(rect.Contains(new Point(1, 2)));
			Assert.IsTrue(rect.Contains(new Point(5, 5)));
			Assert.IsFalse(rect.Contains(new Point(11, 5)));
			Assert.IsFalse(rect.Contains(new Point(5, 22)));
		}

		[Test]
		public void Lerp()
		{
			Assert.AreEqual(Rectangle.One, Rectangle.Zero.Lerp(Rectangle.One, 1.0f));
			Assert.AreEqual(new Rectangle(0.5f, 0.5f, 1, 1),
				Rectangle.Zero.Lerp(new Rectangle(1, 1, 2, 2), 0.5f));
		}

		[Test]
		public void FromCenter()
		{
			Rectangle rect = Rectangle.FromCenter(new Point(11, 12), new Size(4, 6));
			Assert.AreEqual(new Rectangle(9, 9, 4, 6), rect);
			Rectangle anotherRect = Rectangle.FromCenter(0.5f, 0.5f, 1.0f, 1.0f);
			Assert.AreEqual(new Rectangle(0, 0, 1, 1), anotherRect);
		}

		[Test]
		public void FromCorners()
		{
			Rectangle rect = Rectangle.FromCorners(new Point(1, 2), new Point(3, 5));
			Assert.AreEqual(new Rectangle(new Point(1, 2), new Size(2, 3)), rect);
		}

		[Test]
		public void Aspect()
		{
			Assert.AreEqual(0.5f, new Rectangle(0, 0, 1, 2).Aspect);
			Assert.AreEqual(2.0f, new Rectangle(0, 0, 4, 2).Aspect);
		}

		[Test]
		public void Increase()
		{
			var rect = new Rectangle(1, 1, 2, 2);
			Assert.AreEqual(new Rectangle(0.9f, 0.9f, 2.2f, 2.2f), rect.Increase(new Size(0.2f)));
		}

		[Test]
		public void Reduce()
		{
			var rect = new Rectangle(1, 1, 2, 2);
			Assert.AreEqual(new Rectangle(1.5f, 1.5f, 1, 1), rect.Reduce(Size.One));
		}

		[Test]
		public void GetInnerRectangle()
		{
			var rect = new Rectangle(1, 1, 2, 2);
			Assert.AreEqual(rect, rect.GetInnerRectangle(Rectangle.One));
			Assert.AreEqual(new Rectangle(1.0f, 1.0f, 1.0f, 1.0f),
				rect.GetInnerRectangle(new Rectangle(0.0f, 0.0f, 0.5f, 0.5f)));
			Assert.AreEqual(new Rectangle(2.0f, 2.0f, 1.0f, 1.0f),
				rect.GetInnerRectangle(new Rectangle(0.5f, 0.5f, 0.5f, 0.5f)));
		}

		[Test]
		public void GetRelativePoint()
		{
			var rect = new Rectangle(1, 2, 3, 4);
			Assert.AreEqual(Point.Zero, rect.GetRelativePoint(new Point(1, 2)));
			Assert.AreEqual(Point.One, rect.GetRelativePoint(new Point(4, 6)));
			Assert.AreEqual(new Point(-1, -2), rect.GetRelativePoint(new Point(-2, -6)));
		}

		[Test]
		public void Move()
		{
			var rect = new Rectangle(1, 1, 1, 1);
			Assert.AreEqual(rect, rect.Move(Point.Zero));
			Assert.AreEqual(new Rectangle(2.0f, 2.0f, 1.0f, 1.0f), rect.Move(Point.One));
			Assert.AreEqual(new Rectangle(-1.0f, -2.0f, 1.0f, 1.0f), rect.Move(new Point(-2, -3)));
		}

		[Test]
		public void GetRotatedRectangleCornersWithoutRotation()
		{
			var points = new Rectangle(1, 1, 1, 1).GetRotatedRectangleCorners(Point.Zero, 0);
			Assert.AreEqual(4, points.Length);
			Assert.AreEqual(Point.One, points[0]);
			Assert.AreEqual(new Point(2, 1), points[1]);
			Assert.AreEqual(new Point(2, 2), points[2]);
		}

		[Test]
		public void GetRotatedRectangleCornersWith180DegreesRotation()
		{
			var points = new Rectangle(1, 1, 1, 1).GetRotatedRectangleCorners(Point.Zero, 180);
			Assert.AreEqual(-Point.One, points[0]);
			Assert.AreEqual(-new Point(2, 1), points[1]);
			Assert.AreEqual(-new Point(2, 2), points[2]);
		}

		[Test]
		public void IsColliding()
		{
			var screenRect = Rectangle.One;
			var insideRect = new Rectangle(0.1f, 0.1f, 2.9f, 0.3f);
			var outsideRect = new Rectangle(2.4f, 0.35f, 0.1f, 0.1f);
			Assert.IsTrue(insideRect.IsColliding(0, screenRect, 0));
			Assert.IsFalse(outsideRect.IsColliding(0, screenRect, 0));
			Assert.IsTrue(outsideRect.IsColliding(0, insideRect, 0));
			Assert.IsFalse(outsideRect.IsColliding(0, insideRect, 70));
		}

		[Test]
		public void IsCollidingTopBottom()
		{
			var topRect = new Rectangle(0.44f, 0.4f, 0.05f, 0.03f);
			var bottomRect = new Rectangle(0.44f, 0.44f, 0.04f, 0.03f);
			Assert.IsFalse(topRect.IsColliding(0, bottomRect, 0));
			Assert.IsFalse(bottomRect.IsColliding(0, topRect, 0));
		}

		[Test]
		public void IsOneRectangleCollidingWhenInsideAnother()
		{
			var insideRect = new Rectangle(0.3f, 0.3f, 0.1f, 0.1f);
			var outsideRect = new Rectangle(0.2f, 0.2f, 0.3f, 0.3f);
			Assert.IsTrue(outsideRect.IsColliding(0, insideRect, 0));
			Assert.IsTrue(outsideRect.IsColliding(0, insideRect, 70));
		}
	}
}