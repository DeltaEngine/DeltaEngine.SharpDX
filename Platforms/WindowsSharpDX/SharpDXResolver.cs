using DeltaEngine.Content.Xml;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.SharpDX;
using DeltaEngine.Input.SharpDX;
using DeltaEngine.Input.Windows;
using DeltaEngine.Multimedia.SharpDX;
using DeltaEngine.Platforms.Windows;
using DeltaEngine.Rendering2D;
#if !DEBUG 
using System;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
#endif

namespace DeltaEngine.Platforms
{
	internal class SharpDXResolver : AppRunner
	{
		public SharpDXResolver()
		{
#if DEBUG
			InitializeSharpDX();
#else
			// Some machines with missing frameworks initialization will crash, we need useful errors
			try
			{
				InitializeSharpDX();
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
				if (StackTraceExtensions.StartedFromNCrunchOrNunitConsole)
					throw;
				DisplayMessageBoxAndCloseApp("Fatal SharpDX Initialization Error", exception);
			}
#endif
		}

		private void InitializeSharpDX()
		{
			RegisterCommonEngineSingletons();
			RegisterSingleton<FormsWindow>();
			RegisterSingleton<WindowsSystemInformation>();
			RegisterSingleton<SharpDXDevice>();
			RegisterSingleton<Drawing>();
			RegisterSingleton<BatchRenderer>();
			RegisterSingleton<SharpDXScreenshotCapturer>();
			RegisterSingleton<XAudioDevice>();
			RegisterSingleton<SharpDXMouse>();
			RegisterSingleton<SharpDXKeyboard>();
			RegisterSingleton<SharpDXGamePad>();
			RegisterSingleton<WindowsTouch>();
			RegisterSingleton<WindowsGamePad>();
			RegisterSingleton<CursorPositionTranslater>();
			Register<InputCommands>();
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
			Register<XmlContent>();
		}
	}
}