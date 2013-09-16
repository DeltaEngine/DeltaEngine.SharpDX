using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Sprites.Tests
{
	public class SpriteSaveAndLoadTests : TestWithMocksOrVisually
	{
		[Test]
		public void CreateSpriteFromComponents()
		{
			var components = new List<object>();
			components.Add(Rectangle.One);
			components.Add(Visibility.Show);
			var material = new Material(Shader.Position2DUv, "DeltaEngineLogo");
			components.Add(material);
			components.Add(material.DiffuseMap.BlendMode);
			components.Add(new Sprite.SpriteCoordinates(Rectangle.One));
			var sprite = Activator.CreateInstance(typeof(Sprite),
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
				new[] { (object)components }, CultureInfo.CurrentCulture) as Sprite;
			Assert.AreEqual(material, sprite.Material);
			Assert.AreEqual(Rectangle.One, sprite.DrawArea);
			Assert.AreEqual(BlendMode.Normal, sprite.BlendMode);
		}

		[Test, Ignore]
		public void SaveAndLoadSprite()
		{
			var sprite = new Sprite("DeltaEngineLogo", Rectangle.One);
			var data = BinaryDataExtensions.SaveToMemoryStream(sprite);
			Assert.Greater(data.Length, 0);
			var loadedSprite = data.CreateFromMemoryStream() as Sprite;
			Assert.AreEqual(sprite.Material, loadedSprite.Material);
			Assert.AreEqual(sprite.DrawArea, loadedSprite.DrawArea);
			Assert.AreEqual(sprite.BlendMode, loadedSprite.BlendMode);
			Assert.AreEqual(sprite.Rotation, loadedSprite.Rotation);
		}
	}
}