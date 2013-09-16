using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DeltaEngine.Content;
using DeltaEngine.Content.Mocks;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Tests.Core
{
	public partial class BinaryDataLoadSaveTests
	{
		[Test]
		public void SaveAndLoadPrimitiveDataTypes()
		{
			SaveDataTypeAndLoadAgain((sbyte)-8);
			SaveDataTypeAndLoadAgain(-8);
			SaveDataTypeAndLoadAgain((Int16)8);
			SaveDataTypeAndLoadAgain((UInt16)8);
			SaveDataTypeAndLoadAgain((long)-8);
			SaveDataTypeAndLoadAgain((uint)8);
			SaveDataTypeAndLoadAgain((ulong)8);
			SaveDataTypeAndLoadAgain(3.4f);
			SaveDataTypeAndLoadAgain(8.4);
			SaveDataTypeAndLoadAgain(false);
		}

		[Test]
		public void SaveAndLoadOtherDatatypes()
		{
			SaveDataTypeAndLoadAgain("Hi");
			SaveDataTypeAndLoadAgain('x');
			SaveDataTypeAndLoadAgain((decimal)8.4);
			SaveDataTypeAndLoadAgain("asdf".ToCharArray());
			SaveDataTypeAndLoadAgain(StringExtensions.ToByteArray("asdf"));
			SaveDataTypeAndLoadAgain(TestEnum.SomeFlag);
		}

		[Test]
		public void SaveAndLoadLists()
		{
			SaveAndLoadList(new List<int> { 2, 4, 7, 15 });
			SaveAndLoadList(new List<Object> { 2, 0.5f, "Hello" });
		}

		[Test]
		public void SaveAndLoadDictionaries()
		{
			SaveAndLoadDictionary(new Dictionary<string, string>());
			SaveAndLoadDictionary(new Dictionary<string, string> { { "Key", "Value" } });
			SaveAndLoadDictionary(new Dictionary<string, int> { { "One", 1 }, { "Two", 2 } });
			SaveAndLoadDictionary(new Dictionary<int, float> { { 1, 1.1f }, { 2, 2.2f }, { 3, 3.3f } });
			SaveAndLoadDictionary(new Dictionary<int, object> { { 1, Point.One }, { 2, Color.Red } });
		}

		[Test]
		public void SaveAndLoadArrays()
		{
			SaveAndLoadArray(new[] { 2, 4, 7, 15 });
			SaveAndLoadArray(new object[] { 2, 0.5f, "Hello" });
			SaveAndLoadArray(new byte[] { 5, 6, 7 });
			SaveAndLoadArray(new byte[0]);
		}

		[Test]
		public void SaveAndLoadEnumArray()
		{
			SaveAndLoadArray(new[] { TestEnum.SomeFlag, TestEnum.SomeFlag });
		}

		[Test]
		public void SaveAndLoadArraysContainingNullValues()
		{
			BinaryDataExtensions.SaveDataIntoMemoryStream(new object[] { null });
			BinaryDataExtensions.SaveDataIntoMemoryStream(new object[] { 0, 'a', "hallo", null });
		}

		[Test]
		public void SaveAndLoadClassWithArrays()
		{
			var instance = new ClassWithArrays();
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(instance);
			var retrieved =
				BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<ClassWithArrays>(data);
			Assert.IsTrue(retrieved.byteData.Compare(new byte[] { 1, 2, 3, 4, 5 }),
				retrieved.byteData.ToText());
			Assert.IsTrue(retrieved.charData.Compare(new[] { 'a', 'b', 'c' }),
				retrieved.charData.ToText());
			Assert.IsTrue(retrieved.intData.Compare(new[] { 10, 20, 30 }), retrieved.intData.ToText());
			Assert.IsTrue(retrieved.stringData.Compare(new[] { "Hi", "there" }),
				retrieved.stringData.ToText());
			Assert.IsTrue(retrieved.enumData.Compare(new[] { DayOfWeek.Monday, DayOfWeek.Sunday }),
				retrieved.enumData.ToText());
			Assert.IsTrue(retrieved.byteEnumData.Compare(new[] { ByteEnum.Normal, ByteEnum.High }),
				retrieved.byteEnumData.ToText());
		}

		[Test]
		public void SaveAndLoadClassWithEmptyByteArray()
		{
			var instance = new ClassWithByteArray { data = new byte[] { 1, 2, 3 } };
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(instance);
			var retrieved =
				BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<ClassWithByteArray>(data);
			Assert.IsTrue(instance.data.Compare(retrieved.data));
		}

		[Test]
		public void SaveAndLoadArrayWithOnlyNullElements()
		{
			var instance = new object[] { null, null };
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(instance);
			var retrieved = BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<object[]>(data);
			Assert.IsTrue(instance.Compare(retrieved));
		}

		[Test]
		public void SaveAndLoadArrayWithMixedNumbersAndNullElements()
		{
			var instance = new object[] { 1, null };
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(instance);
			var retrieved = BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<object[]>(data);
			Assert.IsTrue(instance.Compare(retrieved));
		}

		[Test]
		public void SaveAndLoadExplicitLayoutStruct()
		{
			var explicitLayoutTest = new ExplicitLayoutTestClass
			{
				someValue = 8,
				anotherValue = 5,
				unionValue = 7
			};
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(explicitLayoutTest);
			var retrieved =
				BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<ExplicitLayoutTestClass>(data);
			Assert.AreEqual(8, retrieved.someValue);
			Assert.AreEqual(7, retrieved.anotherValue);
			Assert.AreEqual(7, retrieved.unionValue);
		}

		[Test]
		public void SaveAndLoadClassWithAnotherClassInside()
		{
			var instance = new ClassWithAnotherClassInside
			{
				Number = 17,
				Data =
					new ClassWithAnotherClassInside.InnerDerivedClass { Value = 1.5, additionalFlag = true },
				SecondInstanceNotSet = null
			};
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(instance);
			var retrieved =
				BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<ClassWithAnotherClassInside>(
					data);
			Assert.AreEqual(instance.Number, retrieved.Number);
			Assert.AreEqual(instance.Data.Value, retrieved.Data.Value);
			Assert.AreEqual(instance.Data.additionalFlag, retrieved.Data.additionalFlag);
			Assert.AreEqual(instance.SecondInstanceNotSet, retrieved.SecondInstanceNotSet);
		}

		[Test]
		public void ThrowExceptionTypeNameStartsWithXml()
		{
			Assert.Throws<BinaryDataSaver.UnableToSave>(
				() => BinaryDataExtensions.SaveToMemoryStream(new XmlBinaryData("Xml")));
			Assert.AreEqual("Xml", new XmlBinaryData("Xml").Text);
		}

		[Test]
		public void LoadAndSaveClassWithMemoryStream()
		{
			var instance = new ClassWithMemoryStream(new byte[] { 1, 2, 3, 4 });
			instance.Writer.Write(true);
			instance.Version = 3;
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(instance);
			// Only the internal data should be saved, 1 byte memory stream not null, 1 byte data length,
			// memory stream data: 4 bytes+1 bool byte, 4 byte for the int Version
			Assert.AreEqual(11, data.Length);
			var retrieved =
				BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<ClassWithMemoryStream>(data);
			Assert.IsNotNull(retrieved.reader);
			Assert.AreEqual(instance.Version, retrieved.Version);
			Assert.AreEqual(instance.Length, retrieved.Length);
			Assert.IsTrue(instance.Bytes.Compare(retrieved.Bytes), retrieved.Bytes.ToText());
		}

		[Test]
		public void LoadingAndSavingKnownTypeShouldNotCauseLoggerMessage()
		{
			using (var logger = new MockLogger())
			{
				var data = BinaryDataExtensions.SaveDataIntoMemoryStream(Point.One);
				var loaded = BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<Point>(data);
				Assert.AreEqual(Point.One, loaded);
				Assert.AreEqual(0, logger.NumberOfMessages);
			}
		}

		[Test]
		public void LoadUnknowTypeShouldThrowException()
		{
			Assert.Throws<Exception>(
				() =>
					BinaryDataLoader.TryCreateAndLoad(typeof(Point), new BinaryReader(new MemoryStream()),
						new Version(0, 0)));
		}

		[Test]
		public void CreateInstanceOfTypeWithCtorParamsShouldThrowException()
		{
			Assert.Throws<MissingMethodException>(
				() =>
					BinaryDataLoader.TryCreateAndLoad(typeof(ClassThatRequiresConstructorParameter),
						new BinaryReader(new MemoryStream()), new Version(0, 0)));
		}

		[Test]
		public void TestLoadContentType()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);
			const string ContentName = "SomeXml";
			writer.Write(ContentName);
			ContentLoader.Use<MockContentLoader>();
			stream.Position = 0;
			var reader = new BinaryReader(stream);
			object returnedContentType = BinaryDataLoader.TryCreateAndLoad(typeof(MockXmlContentType),
				reader, Assembly.GetExecutingAssembly().GetName().Version);
			var content = returnedContentType as MockXmlContentType;
			Assert.IsNotNull(content);
			Assert.AreEqual(ContentName, content.Name);
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void LoadContentWithoutNameShouldTrowUnableToLoadContentDataWithoutName()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);
			writer.Write(string.Empty);
			ContentLoader.Use<MockContentLoader>();
			stream.Position = 0;
			var reader = new BinaryReader(stream);
			Assert.That(
				() => BinaryDataLoader.TryCreateAndLoad(typeof(MockXmlContentType), reader,
					Assembly.GetExecutingAssembly().GetName().Version),
				Throws.Exception.With.InnerException.TypeOf
					<BinaryDataLoader.UnableToLoadContentDataWithoutName>());
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void WriteAndReadNumberMostlyBelow255ThatIsReallyBelow255()
		{
			var data = new MemoryStream();
			var writer = new BinaryWriter(data);
			const int NumberBelow255 = 123456;
			writer.WriteNumberMostlyBelow255(NumberBelow255);
			data.Position = 0;
			var reader = new BinaryReader(data);
			Assert.AreEqual(NumberBelow255, reader.ReadNumberMostlyBelow255());
		}

		[Test]
		public void WriteAndReadNumberMostlyBelow255WithANumberOver255()
		{
			var data = new MemoryStream();
			var writer = new BinaryWriter(data);
			const int NumberOver255 = 123456;
			writer.WriteNumberMostlyBelow255(NumberOver255);
			data.Position = 0;
			var reader = new BinaryReader(data);
			Assert.AreEqual(NumberOver255, reader.ReadNumberMostlyBelow255());
		}

		[Test]
		public void ThrowExceptionOnSavingAnInvalidObject()
		{
			Assert.Throws<NullReferenceException>(
				() => BinaryDataSaver.TrySaveData(null, typeof(object), null));
		}

		[Test]
		public void SaveContentData()
		{
			ContentLoader.Use<MockContentLoader>();
			var xmlContent = ContentLoader.Load<MockXmlContent>("XmlData");
			using (var dataWriter = new BinaryWriter(new MemoryStream()))
				BinaryDataSaver.TrySaveData(xmlContent, typeof(MockXmlContent), dataWriter);
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void ThrowExceptionOnSavingAnUnsupportedStream()
		{
			using (var otherStreamThanMemory = new BufferedStream(new MemoryStream()))
			using (var dataWriter = new BinaryWriter(otherStreamThanMemory))
				Assert.Throws<BinaryDataSaver.UnableToSave>(
					() => BinaryDataSaver.TrySaveData(otherStreamThanMemory, typeof(object), dataWriter));
		}
	}
}