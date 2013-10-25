using DeltaEngine.Content;
using DeltaEngine.Content.Mocks;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Mocks;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Tests.Content
{
	public class MaterialTests
	{
		[SetUp]
		public void CreateContentLoader()
		{
			ContentLoader.Use<MockContentLoader>();
			new MockSettings();
			new QuadraticScreenSpace(new MockWindow());
			var shader = ContentLoader.Load<FakeShader>(Shader.Position2DColor);
			material = new Material(shader, null, new Size(100));
		}

		private Material material;

		[TearDown]
		public void DisposeContentLoader()
		{
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void TestPixelBasedRenderSize()
		{
			material.RenderSizeMode = RenderSizeMode.PixelBased;
			Assert.AreEqual(0.15625f,material.MaterialRenderSize.Width);
			Assert.AreEqual(0.15625f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test800X480RenderSize()
		{
			material.RenderSizeMode = RenderSizeMode.SizeFor800X480;
			Assert.AreEqual(0.125f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.125f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test1024X720RenderSize()
		{
			material.RenderSizeMode = RenderSizeMode.SizeFor1024X768;
			Assert.AreEqual(0.09765625f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.09765625f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test1280X720RenderSize()
		{
			material.RenderSizeMode = RenderSizeMode.SizeFor1280X720;
			Assert.AreEqual(0.078125f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.078125f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test1920X1080RenderSize()
		{
			material.RenderSizeMode = RenderSizeMode.SizeFor1920X1080;
			Assert.AreEqual(0.0520833321f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.0520833321f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void TestSettingsBasedRenderSize()
		{
			material.RenderSizeMode = RenderSizeMode.SizeForSettingsResolution;
			Assert.AreEqual(0.15625f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.15625f, material.MaterialRenderSize.Height);
		}
	}
}