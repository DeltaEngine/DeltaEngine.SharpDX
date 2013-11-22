using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Platforms;
using DeltaEngine.Scenes.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class ThemeTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void SaveAndLoadDefaultTheme()
		{
			var theme = new Theme();
			var stream = BinaryDataExtensions.SaveToMemoryStream(theme);
			var loadedTheme = (Theme)stream.CreateFromMemoryStream();
			Assert.IsTrue(AreMaterialsEqual(theme.ButtonMouseover, loadedTheme.ButtonMouseover));
			Assert.IsTrue(AreMaterialsEqual(theme.ScrollbarPointer, loadedTheme.ScrollbarPointer));
			Assert.IsTrue(AreMaterialsEqual(theme.TextBoxFocused, loadedTheme.TextBoxFocused));
		}

		private static bool AreMaterialsEqual(Material material1, Material material2)
		{
			return material1.DefaultColor == material2.DefaultColor &&
				material1.DiffuseMap.Name == material2.DiffuseMap.Name &&
				material1.Shader.Name == material2.Shader.Name;
		}

		[Test, CloseAfterFirstFrame]
		public void SaveAndLoadModifiedTheme()
		{
			var theme = new Theme();
			theme.Slider = new Material(Shader.Position2DColorUV, "DeltaEngineLogo")
			{
				DefaultColor = Color.Red
			};
			var stream = BinaryDataExtensions.SaveToMemoryStream(theme);
			var loadedTheme = (Theme)stream.CreateFromMemoryStream();
			Assert.IsTrue(AreMaterialsEqual(theme.Slider, loadedTheme.Slider));
		}

		[Test, CloseAfterFirstFrame]
		public void SaveAndLoadMockThemeViaContentLoader()
		{
			if (!StackTraceExtensions.StartedFromNCrunch)
				return; // ncrunch: no coverage
			var theme = ContentLoader.Load<Theme>("TestTheme");
			Assert.AreEqual(Color.Blue, theme.SelectBox.DefaultColor);
			Assert.AreEqual("DeltaEngineLogo", theme.SelectBox.DiffuseMap.Name);
		}
	}
}