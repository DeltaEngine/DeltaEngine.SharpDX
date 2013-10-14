using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering2D.Tests
{
	internal class SpriteBatchKeyTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			logo = new Sprite("DeltaEngineLogo", Rectangle.Zero);
			key = new SpriteBatchKey(logo);
			keySameMaterialAndBlendMode = new SpriteBatchKey(new Sprite(logo.Material, Rectangle.One));
			keyDifferentMaterial = new SpriteBatchKey(new Sprite("DeltaEngineLogo", Rectangle.One));
			keyDifferentBlendMode =
				new SpriteBatchKey(new Sprite(logo.Material, Rectangle.One)
				{
					BlendMode = BlendMode.Opaque
				});
		}

		private Sprite logo;
		private SpriteBatchKey key;
		private SpriteBatchKey keySameMaterialAndBlendMode;
		private SpriteBatchKey keyDifferentMaterial;
		private SpriteBatchKey keyDifferentBlendMode;

		[Test]
		public void Constructor()
		{
			Assert.AreEqual(logo.Material, key.Material);
			Assert.AreEqual(logo.BlendMode, key.BlendMode);
		}

		[Test]
		public void Equals()
		{
			Assert.IsTrue(key == keySameMaterialAndBlendMode);
			Assert.IsFalse(key == keyDifferentMaterial);
			Assert.IsFalse(key == keyDifferentBlendMode);
		}

		[Test]
		public void NotEquals()
		{
			Assert.IsFalse(key != keySameMaterialAndBlendMode);
			Assert.IsTrue(key != keyDifferentMaterial);
			Assert.IsTrue(key != keyDifferentBlendMode);
		}

		[Test]
		public void ObjectEquals()
		{
			Assert.IsTrue(key.Equals(keySameMaterialAndBlendMode));
			Assert.IsFalse(key.Equals(keyDifferentMaterial));
			Assert.IsFalse(key.Equals(keyDifferentBlendMode));
			Assert.IsTrue(key.Equals((object)keySameMaterialAndBlendMode));
			// ReSharper disable SuspiciousTypeConversion.Global
			Assert.IsFalse(key.Equals(1));
			// ReSharper restore SuspiciousTypeConversion.Global
		}

		[Test]
		public void GetHashCodeViaDictionary()
		{
			var keyValues = new Dictionary<SpriteBatchKey, int>
			{
				{ key, 1 },
				{ keyDifferentMaterial, 2 }
			};
			Assert.IsTrue(keyValues.ContainsKey(key));
			Assert.IsTrue(keyValues.ContainsKey(keyDifferentMaterial));
			Assert.IsFalse(keyValues.ContainsKey(keyDifferentBlendMode));
		}
	}
}