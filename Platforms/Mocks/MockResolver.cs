using System;
using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Input;
using DeltaEngine.Logging;
using DeltaEngine.Content.Mocks;
using DeltaEngine.Entities;
using DeltaEngine.Graphics.Mocks;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Mocks;
using DeltaEngine.Multimedia;
using DeltaEngine.Multimedia.Mocks;
using DeltaEngine.Networking;
using DeltaEngine.Networking.Mocks;
using DeltaEngine.Physics2D;
using DeltaEngine.Physics2D.Farseer;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Mocks;
using DeltaEngine.Rendering3D.Particles;
using DeltaEngine.Rendering3D.Shapes3D;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Platforms.Mocks
{
	/// <summary>
	/// Special resolver for unit tests to mocks all the integration classes (Window, Device, etc.)
	/// </summary>
	public class MockResolver : AppRunner
	{
		public MockResolver()
		{
			CreateConsoleCommandResolver();
			CreateSettingsContentEntitiesAndNetworking();
			RegisterMock(new MockGlobalTime());
			RegisterMock(new MockLogger());
			if (ExceptionExtensions.IsDebugMode)
				RegisterMock(new ConsoleLogger());
			Window = RegisterMock(new MockWindow());
			ContentIsReady += CreateDefaultInputCommands;
			RegisterMediaTypes();
			RegisterDeviceDrawingScreenSpaceAndCamera();
		}

		/// <summary>
		/// Does the same as InputCommands.CreateDefault, just much faster because types are known here.
		/// </summary>
		private static void CreateDefaultInputCommands()
		{
			Command.Register(Command.Exit, new KeyTrigger(Key.Escape));
			Command.Register(Command.Click,
				new Trigger[]
				{
					new KeyTrigger(Key.Space), new MouseButtonTrigger(), new TouchPressTrigger(),
					new GamePadButtonTrigger(GamePadButton.A)
				});
			Command.Register(Command.MiddleClick, new MouseButtonTrigger(MouseButton.Middle));
			Command.Register(Command.RightClick, new MouseButtonTrigger(MouseButton.Right));
			Command.Register(Command.MoveLeft, new KeyTrigger(Key.CursorLeft, State.Pressed));
			Command.Register(Command.MoveRight, new KeyTrigger(Key.CursorRight, State.Pressed));
			Command.Register(Command.MoveUp, new KeyTrigger(Key.CursorUp, State.Pressed));
			Command.Register(Command.MoveDown, new KeyTrigger(Key.CursorDown, State.Pressed));
			Command.Register(Command.MoveDirectly,
				new Trigger[]
				{
					new KeyTrigger(Key.CursorLeft, State.Pressed), 
					new KeyTrigger(Key.CursorRight, State.Pressed),
					new KeyTrigger(Key.CursorUp, State.Pressed), 
					new KeyTrigger(Key.CursorDown, State.Pressed)
				});
			Command.Register(Command.RotateDirectly, new MouseMovementTrigger());
			Command.Register(Command.Back, new KeyTrigger(Key.Backspace, State.Pressed));
			Command.Register(Command.Drag, new MouseDragTrigger());
			Command.Register(Command.Flick, new TouchFlickTrigger());
			Command.Register(Command.Pinch, new TouchPinchTrigger());
			Command.Register(Command.Hold, new TouchHoldTrigger());
			Command.Register(Command.DoubleClick, new MouseDoubleClickTrigger());
			Command.Register(Command.Rotate, new TouchRotateTrigger());
			Command.Register(Command.Zoom, new MouseZoomTrigger());
		}

		private void CreateConsoleCommandResolver()
		{
			ConsoleCommands.resolver = new ConsoleCommandResolver(this);
		}

		private void CreateSettingsContentEntitiesAndNetworking()
		{
			ContentLoader.resolver = new AutofacContentLoaderResolver(this);
			ContentLoader.Use<MockContentLoader>();
			instancesToDispose.Add(settings = RegisterMock(new MockSettings()));
			instancesToDispose.Add(
				entities = new EntitiesRunner(new AutofacHandlerResolver(this), settings));
			instancesToDispose.Add(new Messaging(new MockNetworkResolver()));
		}

		public Window Window { get; private set; }

		protected override sealed void RegisterMediaTypes()
		{
			base.RegisterMediaTypes();
			Register<MockImage>();
			Register<MockShader>();
			Register<MockGeometry>();
			Register<MockMeshAnimation>();
			Register<MockSound>();
			Register<MockMusic>();
			Register<MockVideo>();
			Register<Font>();
			Register<ParticleEmitterData>();
		}

		public T RegisterMock<T>(T instance) where T : class
		{
			Type instanceType = instance.GetType();
			foreach (object mock in registeredMocks)
				if (mock.GetType() == instanceType)
					throw new UnableToRegisterAlreadyRegisteredMockClass(instance, mock); //ncrunch: no coverage
			registeredMocks.Add(instance);
			if (instanceType.BaseType == typeof(object))
				alreadyRegisteredTypes.AddRange(instanceType.GetInterfaces());
			if (instance is IDisposable)
				instancesToDispose.Add(instance as IDisposable);
			RegisterInstance(instance);
			return instance;
		}

		internal class UnableToRegisterAlreadyRegisteredMockClass : Exception
		{
			//ncrunch: no coverage start
			public UnableToRegisterAlreadyRegisteredMockClass(object instance, object mock)
				: base("New instance: " + instance + ", already registered mock class: " + mock) { }
			//ncrunch: no coverage end
		}

		private readonly List<object> registeredMocks = new List<object>();

		private void RegisterDeviceDrawingScreenSpaceAndCamera()
		{
			RegisterSingleton<MockSoundDevice>();
			RegisterSingleton<MockKeyboard>();
			RegisterSingleton<MockMouse>();
			RegisterSingleton<MockTouch>();
			RegisterSingleton<MockGamePad>();
			device = RegisterMock(new MockDevice(Window));
			drawing = RegisterMock(new Drawing(device, Window));
			Camera.resolver = new AutofacCameraResolver(this);
			ScreenSpace.resolver = new AutofacScreenSpaceResolver(this);
			Register<RelativeScreenSpace>();
			Register<PixelScreenSpace>();
			Register<Camera2DScreenSpace>();
		}

		public bool IsInitialized
		{
			get { return IsAlreadyInitialized; }
		}

		public override BaseType Resolve<BaseType>()
		{
			foreach (object mock in registeredMocks)
				if (mock is BaseType)
					return mock as BaseType;
			var instance = GetRegisteredInstance(typeof(BaseType));
			if (instance != null)
				return instance as BaseType;
			return base.Resolve<BaseType>();
		}

		private object GetRegisteredInstance(Type baseType)
		{
			if (baseType == typeof(Device) || baseType == typeof(MockDevice))
				return device;
			if (baseType == typeof(Drawing))
				return drawing;
			if (baseType == typeof(SoundDevice) || baseType == typeof(MockSoundDevice))
				return GetRegisteredMock<MockSoundDevice>();
			if (baseType == typeof(Keyboard) || baseType == typeof(MockKeyboard))
				return GetRegisteredMock<MockKeyboard>();
			if (baseType == typeof(Mouse) || baseType == typeof(MockMouse))
				return GetRegisteredMock<MockMouse>();
			if (baseType == typeof(Touch) || baseType == typeof(MockTouch))
				return GetRegisteredMock<MockTouch>();
			if (baseType == typeof(GamePad) || baseType == typeof(MockGamePad))
				return GetRegisteredMock<MockGamePad>();
			if (baseType == typeof(InAppPurchase) || baseType == typeof(MockInAppPurchase))
				return GetRegisteredMock<MockInAppPurchase>();
			if (baseType == typeof(SystemInformation) || baseType == typeof(MockSystemInformation))
				return GetRegisteredMock<MockSystemInformation>();
			if (baseType == typeof(ScreenshotCapturer) || baseType == typeof(MockScreenshotCapturer))
				return GetRegisteredMock<MockScreenshotCapturer>();
			if (baseType.IsAssignableFrom(typeof(Camera)))
				return AddAndReturn(new LookAtCamera(device, Window));
			if (baseType == typeof(Physics) || baseType == typeof(FarseerPhysics))
				return GetRegisteredMock<FarseerPhysics>();
			if (baseType == typeof(QuadraticScreenSpace))
				return AddAndReturn(new QuadraticScreenSpace(Window));
			if (baseType == typeof(Line2DRenderer))
				return AddAndReturn(new Line2DRenderer(drawing));
			if (baseType == typeof(Line3DRenderer))
				return AddAndReturn(new Line3DRenderer(drawing));
			if (baseType == typeof(DrawPolygon2D))
				return AddAndReturn(new DrawPolygon2D(drawing, Window));
			if (baseType == typeof(SpriteBatchRenderer))
				return AddAndReturn(new SpriteBatchRenderer(drawing));
			if (baseType == typeof(VectorText.ProcessText))
				return AddAndReturn(new VectorText.ProcessText());
			if (baseType == typeof(VectorText.Render))
				return AddAndReturn(new VectorText.Render(drawing));
			if (baseType == typeof(Interact))
				return AddAndReturn(new Interact());
			return null;
		}

		private object AddAndReturn(object instance)
		{
			registeredMocks.Add(instance);
			return instance;
		}

		private Device device;
		private Drawing drawing;

		internal override object Resolve(Type baseType, object customParameter = null)
		{
			if (baseType == typeof(Shader))
			{
				var creationData = customParameter as ShaderCreationData;
				if (creationData != null)
					return new MockShader(creationData, device);
				return new MockShader(customParameter as string, device);
			}
			if (baseType == typeof(Image))
			{
				var creationData = customParameter as ImageCreationData;
				if (creationData != null)
					return new MockImage(creationData);
				return new MockImage(customParameter as string);
			}
			if (baseType == typeof(Font))
				return new MockFont(customParameter as string);
			if (baseType == typeof(Sound))
				return new MockSound(customParameter as string, settings);
			if (baseType == typeof(Music))
				return new MockMusic(customParameter as string, Resolve<SoundDevice>(), settings);
			if (baseType == typeof(Video))
				return new MockVideo(customParameter as string, Resolve<SoundDevice>());
			foreach (object mock in registeredMocks)
				if (baseType.IsInstanceOfType(mock))
					return mock;
			var instance = GetRegisteredInstance(baseType);
			if (instance != null)
				return instance;
			return base.Resolve(baseType, customParameter);
		}

		private object GetRegisteredMock<ConcreteType>()
			where ConcreteType : new()
		{
			foreach (object mock in registeredMocks)
				if (mock is ConcreteType)
					return mock;
			var newInstance = new ConcreteType();
			registeredMocks.Add(newInstance);
			return newInstance;
		}

		public override void Dispose()
		{
			base.Dispose();
			if (EntitiesRunner.Current != null)
				throw new EntitiesRunnerWasNotDisposed(); //ncrunch: no coverage
			if (ScreenSpace.IsInitialized)
				throw new ScreenSpaceWasNotDisposed(); //ncrunch: no coverage
			if (Messaging.Current != null)
				throw new MessagingWasNotDisposed(); //ncrunch: no coverage
			if (Camera.IsInitialized)
				throw new CameraWasNotDisposed(); //ncrunch: no coverage
		}

		public class EntitiesRunnerWasNotDisposed : Exception {}
		public class ScreenSpaceWasNotDisposed : Exception {}
		public class MessagingWasNotDisposed : Exception {}
		public class CameraWasNotDisposed : Exception {}
	}
}