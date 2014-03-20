using DeltaEngine.Content.Json;
using DeltaEngine.Content.Xml;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.SharpDX;
using DeltaEngine.Input.SharpDX;
using DeltaEngine.Multimedia.SharpDX;
using DeltaEngine.Physics2D;
using DeltaEngine.Physics2D.Farseer;
using DeltaEngine.Physics3D;
using DeltaEngine.Physics3D.Jitter;
using DeltaEngine.Platforms.Windows;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering3D;
#if !DEBUG 
using System;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
#endif

namespace DeltaEngine.Platforms
{
	public class SharpDXResolver : AppRunner
	{
		public SharpDXResolver()
		{
#if DEBUG
			TryInitializeSharpDX();
#else
			// Some machines with missing frameworks initialization will crash, we need useful errors
			try
			{
				TryInitializeSharpDX();
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

		private void TryInitializeSharpDX()
		{
			RegisterCommonEngineSingletons();
			RegisterSingleton<FormsWindow>();
			RegisterSingleton<WindowsSystemInformation>();
			RegisterSingleton<SharpDXDevice>();
			RegisterSingleton<Drawing>();
			RegisterSingleton<BatchRenderer2D>();
			RegisterSingleton<BatchRenderer3D>();
			RegisterSingleton<SharpDXScreenshotCapturer>();
			RegisterSingleton<XAudioDevice>();
			RegisterSingleton<SharpDXMouse>();
			RegisterSingleton<SharpDXKeyboard>();
			RegisterSingleton<SharpDXGamePad>();
			RegisterSingleton<SharpDXTouch>();
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
			Register<SharpDXVideo>();
			Register<XmlContent>();
			Register<JsonContent>();
		}

		protected override void RegisterPhysics()
		{
			RegisterSingleton<FarseerPhysics>();
			RegisterSingleton<JitterPhysics>();
			Register<AffixToPhysics2D>();
			Register<AffixToPhysics3D>();
		}
	}
}