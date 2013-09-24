using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Logging;
using DeltaEngine.Content.Mocks;
using DeltaEngine.Entities;
using DeltaEngine.Graphics.Mocks;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Mocks;
using DeltaEngine.Multimedia.Mocks;
using DeltaEngine.Networking;
using DeltaEngine.Networking.Mocks;
using DeltaEngine.Physics2D.Farseer;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Mocks;
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
			CreateInstancesToDispose();
			RegisterMock(new MockGlobalTime());
			RegisterMock(new MockLogger());
			if (ExceptionExtensions.IsDebugMode)
				RegisterMock(new ConsoleLogger());
			Window = RegisterMock(new MockWindow());
			ContentIsReady += () => ContentLoader.Load<InputCommands>("DefaultCommands");
			RegisterMockSingletons();
			RegisterMediaTypes();
			CreateScreenSpacesAndCameraResolvers();
		}

		private void CreateInstancesToDispose()
		{
			ContentLoader.resolver = new AutofacContentLoaderResolver(this);
			ContentLoader.Use<MockContentLoader>();
			Register<MockContentLoader>();
			instancesToDispose.Add(settings = RegisterMock(new MockSettings()));
			instancesToDispose.Add(
				entities = new EntitiesRunner(new AutofacHandlerResolver(this), settings));
			instancesToDispose.Add(new Messaging(new MockNetworkResolver()));
		}

		private void CreateConsoleCommandResolver()
		{
			var consoleCommandResolver = new ConsoleCommands(new ConsoleCommandResolver(this));
			AllRegistrationCompleted +=
				() => consoleCommandResolver.RegisterCommandsFromTypes(alreadyRegisteredTypes);
		}

		private void RegisterMockSingletons()
		{
			RegisterSingleton<MockInAppPurchase>();
			RegisterSingleton<MockDevice>();
			RegisterSingleton<Drawing>();
			RegisterSingleton<MockScreenshotCapturer>();
			RegisterSingleton<LookAtCamera>();
			RegisterSingleton<MockSoundDevice>();
			RegisterSingleton<MockKeyboard>();
			RegisterSingleton<MockMouse>();
			RegisterSingleton<MockTouch>();
			RegisterSingleton<MockGamePad>();
			RegisterSingleton<MockSystemInformation>();
			RegisterSingleton<FarseerPhysics>();
		}

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
		}

		public Window Window { get; private set; }

		public T RegisterMock<T>(T instance) where T : class
		{
			Type instanceType = instance.GetType();
			foreach (object mock in registeredMocks.Where(mock => mock.GetType() == instanceType))
				throw new UnableToRegisterAlreadyRegisteredMockClass(instance, mock); //ncrunch: no coverage
			registeredMocks.Add(instance);
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

		public bool IsInitialized
		{
			get { return IsAlreadyInitialized; }
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