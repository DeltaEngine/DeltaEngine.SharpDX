using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Tests.Content
{
	public class MaterialRenderSizeTests : TestWithMocksOrVisually
	{
		[Test]
		public void TestPixelBasedRenderSize()
		{
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUv),
				ContentLoader.Create<Image>(new ImageCreationData(new Size(100, 100))));
			material.SetRenderSize(RenderSize.PixelBased);
			Assert.AreEqual(0.15625f,material.MaterialRenderSize.Width);
			Assert.AreEqual(0.15625f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test800X480RenderSize()
		{
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUv),
				ContentLoader.Create<Image>(new ImageCreationData(new Size(100, 100))));
			material.SetRenderSize(RenderSize.Size800X480);
			Assert.AreEqual(0.0001953125f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.0001953125f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test1024X720RenderSize()
		{
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUv),
				ContentLoader.Create<Image>(new ImageCreationData(new Size(100, 100))));
			material.SetRenderSize(RenderSize.Size1024X720);
			Assert.AreEqual(0.00015258789f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.00015258789f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test1280X720RenderSize()
		{
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUv),
				ContentLoader.Create<Image>(new ImageCreationData(new Size(100, 100))));
			material.SetRenderSize(RenderSize.Size1280X720);
			Assert.AreEqual(0.000122070313f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.000122070313f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void Test1920X1080RenderSize()
		{
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUv),
				ContentLoader.Create<Image>(new ImageCreationData(new Size(100, 100))));
			material.SetRenderSize(RenderSize.Size1920X1080);
			Assert.AreEqual(0.00008138021f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.00008138021f, material.MaterialRenderSize.Height);
		}

		[Test]
		public void TestSettingsBasedRenderSize()
		{
			Settings.Current = resolver.Resolve<Settings>();
			var material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUv),
				ContentLoader.Create<Image>(new ImageCreationData(new Size(100, 100))));
			material.SetRenderSize(RenderSize.SettingsBased);
			Assert.AreEqual(0.000244140625f, material.MaterialRenderSize.Width);
			Assert.AreEqual(0.000244140625f, material.MaterialRenderSize.Height);
		}
	}
}