using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Scenes.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class PictureTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void SaveAndLoad()
		{
			var material = new Material(Shader.Position2DColorUV, "DeltaEngineLogo");
			var theme = new Theme();
			theme.SliderPointer = material;
			var picture = new Picture(theme, material, Rectangle.HalfCentered);
			var stream = BinaryDataExtensions.SaveToMemoryStream(picture);
			var loadedPicture = (Picture)stream.CreateFromMemoryStream();
			Assert.AreEqual("DeltaEngineLogo", loadedPicture.Material.DiffuseMap.Name);
			Assert.AreEqual("DeltaEngineLogo", loadedPicture.Get<Theme>().SliderPointer.DiffuseMap.Name);
			Assert.AreEqual(Rectangle.HalfCentered, loadedPicture.DrawArea);
		}

		[Test]
		public void DrawLoadedPicture()
		{
			var material = new Material(Shader.Position2DColorUV, "DeltaEngineLogo");
			var picture = new Picture(Theme.Default, material, Rectangle.HalfCentered);
			var stream = BinaryDataExtensions.SaveToMemoryStream(picture);
			picture.IsActive = false;
			stream.CreateFromMemoryStream();
		}
	}
}