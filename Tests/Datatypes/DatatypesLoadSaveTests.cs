using System.IO;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class DatatypesLoadSaveTests
	{
		[Test]
		public void SaveSingleColor()
		{
			byte[] savedBytes = BinaryDataExtensions.ToByteArray(Color.Red);
			Assert.AreEqual(4, savedBytes.Length);
			Assert.AreEqual(255, savedBytes[0]);
			Assert.AreEqual(0, savedBytes[1]);
			Assert.AreEqual(0, savedBytes[2]);
			Assert.AreEqual(255, savedBytes[3]);
		}

		[Test]
		public void SaveMultipleColors()
		{
			byte[] savedBytes = BinaryDataExtensions.ToByteArray(new[] { Color.Red, Color.Green });
			Assert.AreEqual(8, savedBytes.Length);
			Assert.AreEqual(255, savedBytes[0]);
			Assert.AreEqual(0, savedBytes[1]);
			Assert.AreEqual(0, savedBytes[2]);
			Assert.AreEqual(255, savedBytes[3]);
			Assert.AreEqual(0, savedBytes[4]);
			Assert.AreEqual(255, savedBytes[5]);
			Assert.AreEqual(0, savedBytes[6]);
			Assert.AreEqual(255, savedBytes[7]);
		}

		[Test]
		public void LoadColor()
		{
			var data = BinaryDataExtensions.SaveToMemoryStream(Color.Red);
			var reconstructedColor = data.CreateFromMemoryStream();
			Assert.AreEqual(Color.Red, reconstructedColor);
		}

		[Test]
		public void SaveAndLoadSize()
		{
			var data = BinaryDataExtensions.SaveToMemoryStream(Size.Half);
			byte[] savedBytes = data.ToArray();
			Assert.AreEqual(1 + 4 + "Size".Length + Size.SizeInBytes, savedBytes.Length);
			Assert.AreEqual("Size".Length, savedBytes[0]);
			var reconstructed = data.CreateFromMemoryStream();
			Assert.AreEqual(Size.Half, reconstructed);
		}

		[Test]
		public void SaveAndLoadPoint()
		{
			var data = BinaryDataExtensions.SaveToMemoryStream(Point.Half);
			byte[] savedBytes = data.ToArray();
			Assert.AreEqual(1 + 4 + "Point".Length + Point.SizeInBytes, savedBytes.Length);
			Assert.AreEqual("Point".Length, savedBytes[0]);
			var reconstructed = data.CreateFromMemoryStream();
			Assert.AreEqual(Point.Half, reconstructed);
		}

		[Test]
		public void SaveAndLoadRectangle()
		{
			var data = BinaryDataExtensions.SaveToMemoryStream(Rectangle.One);
			byte[] savedBytes = data.ToArray();
			Assert.AreEqual(1 + 4 + "Rectangle".Length + Rectangle.SizeInBytes, savedBytes.Length);
			Assert.AreEqual("Rectangle".Length, savedBytes[0]);
			var reconstructed = data.CreateFromMemoryStream();
			Assert.AreEqual(Rectangle.One, reconstructed);
		}

		[Test]
		public void SaveAndLoadRectangleManuallyWithBinaryWriterAndReader()
		{
			using (var dataStream = new MemoryStream())
			{
				var writer = new BinaryWriter(dataStream);
				var data = Rectangle.One;
				BinaryDataExtensions.Save(data, writer);
				dataStream.Seek(0, SeekOrigin.Begin);
				var reader = new BinaryReader(dataStream);
				data = (Rectangle)reader.Create();
				Assert.AreEqual(Rectangle.One, data);
			}
		}

		[Test]
		public void SaveAndLoadVector()
		{
			var data = BinaryDataExtensions.SaveToMemoryStream(Vector.UnitZ);
			byte[] savedBytes = data.ToArray();
			Assert.AreEqual(1 + 4 + "Vector".Length + Vector.SizeInBytes, savedBytes.Length);
			Assert.AreEqual("Vector".Length, savedBytes[0]);
			var reconstructed = data.CreateFromMemoryStream();
			Assert.AreEqual(Vector.UnitZ, reconstructed);
		}

		[Test]
		public void SaveAndLoadMatrix()
		{
			var data = BinaryDataExtensions.SaveToMemoryStream(Matrix.Identity);
			byte[] savedBytes = data.ToArray();
			Assert.AreEqual(1 + 4 + "Matrix".Length + Matrix.SizeInBytes, savedBytes.Length);
			Assert.AreEqual("Matrix".Length, savedBytes[0]);
			var reconstructed = (Matrix)data.CreateFromMemoryStream();
			Assert.AreEqual(Matrix.Identity, reconstructed);
		}
	}
}