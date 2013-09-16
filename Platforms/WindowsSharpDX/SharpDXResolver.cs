using DeltaEngine.Graphics.SharpDX;
using DeltaEngine.Input.SharpDX;
using DeltaEngine.Input.Windows;
using DeltaEngine.Multimedia.SharpDX;
using DeltaEngine.Platforms.Windows;

namespace DeltaEngine.Platforms
{
	internal class SharpDXResolver : AppRunner
	{
		public SharpDXResolver()
		{
			RegisterCommonEngineSingletons();
			RegisterSingleton<FormsWindow>();
			RegisterSingleton<WindowsSystemInformation>();
			RegisterSingleton<SharpDXDevice>();
			RegisterSingleton<SharpDXScreenshotCapturer>();
			RegisterSingleton<XAudioDevice>();
			RegisterSingleton<SharpDXMouse>();
			RegisterSingleton<SharpDXKeyboard>();
			RegisterSingleton<SharpDXGamePad>();
			RegisterSingleton<WindowsTouch>();
			RegisterSingleton<WindowsGamePad>();
			RegisterSingleton<CursorPositionTranslater>();
			if (IsAlreadyInitialized)
				throw new UnableToRegisterMoreTypesAppAlreadyStarted();
		}

		protected override void RegisterMediaTypes()
		{
			base.RegisterMediaTypes();
			Register<SharpDXImage>();
			Register<SharpDXShader>();
			Register<SharpDXGeometry>();
			Register<XAudioSound>();
			Register<XAudioMusic>();
			Register<SharpDXVideo>();
		}
	}
}