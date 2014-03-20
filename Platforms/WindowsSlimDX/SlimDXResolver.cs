using DeltaEngine.Content.Json;
using DeltaEngine.Content.Xml;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.SlimDX;
using DeltaEngine.Input.SlimDX;
using DeltaEngine.Multimedia.SlimDX;
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
	public class SlimDXResolver : AppRunner
	{
		public SlimDXResolver()
		{
#if DEBUG
			TryInitializeSlimDX();
#else
			// Some machines with missing frameworks initialization will crash, we need useful errors
			try
			{
				TryInitializeSlimDX();
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
				if (StackTraceExtensions.StartedFromNCrunchOrNunitConsole)
					throw;
				DisplayMessageBoxAndCloseApp("Fatal SlimDX Initialization Error", exception);
			}
#endif
		}

		private void TryInitializeSlimDX()
		{
			RegisterCommonEngineSingletons();
			RegisterSingleton<FormsWindow>();
			RegisterSingleton<WindowsSystemInformation>();
			RegisterSingleton<SlimDXDevice>();
			RegisterSingleton<Drawing>();
			RegisterSingleton<BatchRenderer2D>();
			RegisterSingleton<BatchRenderer3D>();
			RegisterSingleton<SlimDXScreenshotCapturer>();
			RegisterSingleton<XAudioDevice>();
			RegisterSingleton<SlimDXMouse>();
			RegisterSingleton<SlimDXKeyboard>();
			RegisterSingleton<SlimDXGamePad>();
			RegisterSingleton<SlimDXTouch>();
			Register<InputCommands>();
			if (IsAlreadyInitialized)
				throw new UnableToRegisterMoreTypesAppAlreadyStarted();
		}

		protected override void RegisterMediaTypes()
		{
			base.RegisterMediaTypes();
			Register<SlimDXImage>();
			Register<SlimDXShader>();
			Register<SlimDXGeometry>();
			Register<XAudioSound>();
			Register<XAudioMusic>();
			Register<SlimDXVideo>();
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