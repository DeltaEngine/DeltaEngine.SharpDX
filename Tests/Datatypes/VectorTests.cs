using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class VectorTests
	{
		[Test]
		public void Create()
		{
			const float X = 3.51f;
			const float Y = 0.23f;
			const float Z = 0.95f;
			var v = new Vector(X, Y, Z);
			Assert.AreEqual(v.X, X);
			Assert.AreEqual(v.Y, Y);
			Assert.AreEqual(v.Z, Z);
		}

		[Test]
		public void CreateFromPoint()
		{
			var v = new Vector(new Point(1, 2));
			Assert.AreEqual(1, v.X);
			Assert.AreEqual(2, v.Y);
			Assert.AreEqual(0, v.Z);
		}

		[Test]
		public void CreateFromString()
		{
			var v = new Vector("2.3, 1.5, 0.9");
			Assert.AreEqual(v.X, 2.3f);
			Assert.AreEqual(v.Y, 1.5f);
			Assert.AreEqual(v.Z, 0.9f);
			Assert.Throws<Vector.InvalidNumberOfComponents>(() => new Vector("2.3"));
			Assert.Throws<FormatException>(() => new Vector("a, b, c"));
		}

		[Test]
		public void Statics()
		{
			Assert.AreEqual(new Vector(0, 0, 0), Vector.Zero);
			Assert.AreEqual(new Vector(1, 1, 1), Vector.One);
			Assert.AreEqual(new Vector(1, 0, 0), Vector.UnitX);
			Assert.AreEqual(new Vector(0, 1, 0), Vector.UnitY);
			Assert.AreEqual(new Vector(0, 0, 1), Vector.UnitZ);
			Assert.AreEqual(12, Vector.SizeInBytes);
		}

		[Test]
		public void Dot()
		{
			Assert.AreEqual(0.0f, Vector.Dot(Vector.UnitX, Vector.UnitY));
			Assert.AreEqual(1.0f, Vector.Dot(Vector.UnitX, Vector.UnitX));
		}

		[Test]
		public void Cross()
		{
			Assert.AreEqual(Vector.UnitZ, Vector.Cross(Vector.UnitX, Vector.UnitY));
			Assert.AreEqual(Vector.UnitX, Vector.Cross(Vector.UnitY, Vector.UnitZ));
		}

		
		[Test]
		public void GetAngleBetweenTwoVectors()
		{
			Assert.AreEqual(90.0f, Vector.AngleBetweenVectors(Vector.UnitX, Vector.UnitY));
		}

		[Test]
		public void StaticNormalize()
		{
			var vector = new Vector(2.0f, 4.0f, -3.0f);
			var normalized = Vector.Normalize(vector);
			Assert.AreEqual(1.0f, normalized.LengthSquared, MathExtensions.Epsilon);
			Assert.AreEqual(Vector.Zero, Vector.Normalize(Vector.Zero));
		}

		[Test]
		public void NormalizeOfAnyVector()
		{
			var vector = new Vector(1.0f, 2.0f, -3.0f);
			Assert.AreNotEqual(1.0f, vector.Length);
			vector.Normalize();
			Assert.AreEqual(1.0f, vector.LengthSquared, MathExtensions.Epsilon);
			vector = Vector.UnitX;
			vector.Normalize();
			Assert.AreEqual(1.0f, vector.Length, MathExtensions.Epsilon);
		}

		[Test]
		public void NormalizeOfUnitVectorIsNotNecessary()
		{
			var vector = Vector.UnitX;
			Assert.AreEqual(1.0f, vector.Length, MathExtensions.Epsilon);
			vector.Normalize();
			Assert.AreEqual(1.0f, vector.Length, MathExtensions.Epsilon);
		}

		[Test]
		public void ChangeVector()
		{
			var v = new Vector { X = 2.1f, Y = 2.1f, Z = 0.1f };
			Assert.AreEqual(2.1f, v.X);
			Assert.AreEqual(2.1f, v.Y);
			Assert.AreEqual(0.1f, v.Z);
		}
		
		[Test]
		public void Addition()
		{
			var v1 = new Vector(1, 2, 3);
			var v2 = new Vector(3, 4, 5);
			Assert.AreEqual(new Vector(4, 6, 8), v1 + v2);
		}

		[Test]
		public void Subtraction()
		{
			var v1 = new Vector(1, 2, 3);
			var v2 = new Vector(3, 4, 5);
			Assert.AreEqual(new Vector(-2, -2, -2), v1 - v2);
		}

		[Test]
		public void Negation()
		{
			var v = new Vector(1, 2, 3);
			Assert.AreEqual(-v, new Vector(-1, -2, -3));
		}

		[Test]
		public void Multiplication()
		{
			var v = new Vector(2, 4, -1);
			const float Factor = 1.5f;
			Assert.AreEqual(new Vector(3, 6, -1.5f), v * Factor);
			Assert.AreEqual(new Vector(3, 6, -1.5f), Factor * v);
			var v2 = new Vector(2, 2, 2);
			Assert.AreEqual(new Vector(4, 8, -2), v * v2);
		}

		[Test]
		public void Division()
		{
			var v = new Vector(2, 4, -1);
			const float F = 2f;
			Assert.AreEqual(new Vector(1, 2, -0.5f), v / F);
			var dividedByOutValue = Vector.Zero;
			Vector.Divide(ref v, F, ref dividedByOutValue);
			Assert.AreEqual(dividedByOutValue, v / F);
		}

		[Test]
		public void Lerp()
		{
			var u = new Vector(10, 20, 30);
			var v = new Vector(30, 20, 10);
			Assert.AreEqual(Vector.Zero, Vector.Zero.Lerp(Vector.One, 0));
			Assert.AreEqual(Vector.Zero, Vector.One.Lerp(Vector.Zero, 1));
			Assert.AreEqual(new Vector(0.5f, 0.5f, 0.5f), Vector.Zero.Lerp(Vector.One, 0.5f));
			Assert.AreEqual(new Vector(16, 20, 24), u.Lerp(v, 0.3f));
		}

		[Test]
		public void Equals()
		{
			var v1 = new Vector(1, 2, 3);
			var v2 = new Vector(3, 4, 5);
			Assert.AreNotEqual(v1, v2);
			Assert.AreEqual(v1, new Vector(1, 2, 3));
			Assert.IsTrue(v1 == new Vector(1, 2, 3));
			Assert.IsTrue(v1 != v2);
			Assert.IsTrue(v1.Equals((object)new Vector(1, 2, 3)));
		}

		[Test]
		public void ImplicitCastFromPoint()
		{
			var p = new Point(1, 2);
			var v = new Vector(1, 2, 3);
			Vector addition = p + v;
			Assert.AreEqual(new Vector(2, 4, 3), addition);
		}
		
		[Test]
		public void GetHashCodeViaDictionary()
		{
			var first = new Vector(1, 2, 3);
			var second = new Vector(3, 4, 5);
			var pointValues = new Dictionary<Vector, int> { { first, 1 }, { second, 2 } };
			Assert.IsTrue(pointValues.ContainsKey(first));
			Assert.IsTrue(pointValues.ContainsKey(second));
			Assert.IsFalse(pointValues.ContainsKey(new Vector(5, 6, 7)));
		}

		[Test]
		public void VectorToString()
		{
			Assert.AreEqual("3, 4, 5", new Vector(3, 4, 5).ToString());
		}

		[Test]
		public void ToStringAndFromString()
		{
			var v = new Vector(2.23f, 3.45f, 0.59f);
			string vectorAsString = v.ToString();
			Assert.AreEqual(v, new Vector(vectorAsString));
		}

		[Test]
		public void TransformNormal()
		{
			var matrix = new Matrix(2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1);
			Assert.AreEqual(new Vector(2, 4, 0), new Vector(1, 2, 3).TransformNormal(matrix));
		}
	}
}