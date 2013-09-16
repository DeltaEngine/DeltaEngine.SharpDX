﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DeltaEngine.Content;
using DeltaEngine.Content.Online;
using DeltaEngine.Content.Xml;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Logging;
using DeltaEngine.Networking;
using DeltaEngine.Networking.Tcp;
using DeltaEngine.Rendering.Cameras;
using DeltaEngine.ScreenSpaces;
using Microsoft.Win32;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Starts an application on demand by registering, resolving and running it (via EntitiesRunner)
	/// </summary>
	public abstract class AppRunner : ApproveFirstFrameScreenshot
	{
		//ncrunch: no coverage start
		protected void RegisterCommonEngineSingletons()
		{
			CreateDefaultLoggers();
			CreateConsoleCommandResolver();
			LoadSettingsAndCommands();
			CreateContentLoader();
			RegisterInstance(new StopwatchTime());
			RegisterSingleton<Drawing>();
			CreateEntitySystem();
			RegisterMediaTypes();
			CreateNetworking();
			CreateScreenSpacesAndCameraResolvers();
		}

		private void CreateDefaultLoggers()
		{
			instancesToDispose.Add(new TextFileLogger());
			if (ExceptionExtensions.IsDebugMode)
				instancesToDispose.Add(new ConsoleLogger());
		}

		protected readonly List<IDisposable> instancesToDispose = new List<IDisposable>();

		private void CreateConsoleCommandResolver()
		{
			var consoleCommandManager = new ConsoleCommands(new ConsoleCommandResolver(this));
			AllRegistrationCompleted +=
				() => consoleCommandManager.RegisterCommandsFromTypes(alreadyRegisteredTypes);
			RegisterInstance(consoleCommandManager);
		}

		private void CreateContentLoader()
		{
			OnlineServiceConnection.RememberCreationDataForAppRunner(GetApiKey(),
				settings, OnTimeout, OnError, OnReady, OnContentReceived);
			if (ContentLoader.Type == null)
				ContentLoader.Use<DeveloperOnlineContentLoader>();
			ContentLoader.resolver = new AutofacContentLoaderResolver(this);
		}

		private static string GetApiKey()
		{
			string apiKey = "";
			using (var key = Registry.CurrentUser.OpenSubKey(@"Software\DeltaEngine\Editor", false))
				if (key != null)
					apiKey = (string)key.GetValue("ApiKey");
			return string.IsNullOrEmpty(apiKey) ? Guid.Empty.ToString() : apiKey;
		}

		internal enum ExitCode
		{
			InitializationError = -1,
			UpdateAndDrawTickFailed = -2,
			ContentMissingAndApiKeyNotSet = -3
		}

		private void OnConnectionError(string errorMessage)
		{
			Logger.Warning(errorMessage);
			connectionError = errorMessage;
		}

		private string connectionError;

		private void OnTimeout()
		{
			OnConnectionError("Content Service Connection " + settings.OnlineServiceIp + ":" +
				settings.OnlineServicePort + " timed out.");
		}

		private void OnError(string serverMessage)
		{
			OnConnectionError("Server Error: " + serverMessage);
		}

		private void OnReady()
		{
			onlineServiceReadyReceived = true;
		}

		private bool onlineServiceReadyReceived;

		public void OnContentReceived()
		{
			if (timeout < TimeoutTillNextMessage)
				timeout = TimeoutTillNextMessage;
		}

		private int timeout;
		private const int TimeoutTillNextMessage = 3000;

		private void LoadSettingsAndCommands()
		{
			instancesToDispose.Add(settings = new FileSettings());
			RegisterInstance(settings);
			ContentIsReady += () => ContentLoader.Load<InputCommands>("DefaultCommands");
		}

		protected Settings settings;
		protected internal static event Action ContentIsReady;

		private void CreateEntitySystem()
		{
			instancesToDispose.Add(
				entities = new EntitiesRunner(new AutofacHandlerResolver(this), settings));
			RegisterInstance(entities);
		}

		protected EntitiesRunner entities;

		protected virtual void RegisterMediaTypes()
		{
			Register<Material>();
			Register<ImageAnimation>();
			Register<SpriteSheetAnimation>();
		}

		internal override void MakeSureContentManagerIsReady()
		{
			if (alreadyCheckedContentManagerReady)
				return;
			alreadyCheckedContentManagerReady = true;
			if (ContentLoader.Type == typeof(DeveloperOnlineContentLoader) && !IsEditorContentLoader())
				WaitUntilContentFromOnlineServiceIsReady();
			if (!ContentLoader.HasValidContentForStartup())
			{
				if (StackTraceExtensions.IsStartedFromNunitConsole())
					throw new Exception("No local content available - Unable to continue: " + connectionError);
				Window.ShowMessageBox("No local content available", "Unable to continue: " +
					(connectionError ?? "No content found, please put content in the Content folder"),
					new[] { "OK" });
				Environment.Exit((int)ExitCode.ContentMissingAndApiKeyNotSet);
			}
			if (ContentIsReady != null)
				ContentIsReady();
		}

		private bool alreadyCheckedContentManagerReady;

		private static bool IsEditorContentLoader()
		{
			return ContentLoader.Type.FullName == "DeltaEngine.Editor.EditorContentLoader";
		}

		private void WaitUntilContentFromOnlineServiceIsReady()
		{
			if (!ContentLoader.HasValidContentForStartup())
				Logger.Info("No content available. Waiting until OnlineService sends it to us ...");
			timeout = TimeoutTillNextMessage;
			while (String.IsNullOrEmpty(connectionError) && !onlineServiceReadyReceived &&
				ContentLoader.Type == typeof(DeveloperOnlineContentLoader) && timeout > 0)
			{
				Thread.Sleep(10);
				timeout -= 10;
			}
			if (timeout <= 0)
				Logger.Warning(
					"Content download timeout reached, continuing app (content might be incomplete)");
		}

		private void CreateNetworking()
		{
			Register<TcpServer>();
			Register<TcpSocket>();
			instancesToDispose.Add(new Messaging(new AutofacNetworkResolver(this)));
		}

		protected void CreateScreenSpacesAndCameraResolvers()
		{
			Register<QuadraticScreenSpace>();
			Register<RelativeScreenSpace>();
			Register<PixelScreenSpace>();
			Register<Camera2DScreenSpace>();
			ScreenSpace.resolver = new AutofacScreenSpaceResolver(this);
			Camera.resolver = new AutofacCameraResolver(this);
		}

		public virtual void Run()
		{
			do
				RunTick();
			while (!Window.IsClosing);
			Dispose();
		}

		private Window Window
		{
			get { return cachedWindow ?? (cachedWindow = Resolve<Window>()); }
		}
		private Window cachedWindow;

		internal void RunTick()
		{
			Device.Clear();
			GlobalTime.Current.Update();
			UpdateAndDrawAllEntities();
			ExecuteTestCodeAndMakeScreenshotAfterFirstFrame();
			Device.Present();
			Window.Present();
		}

		private Device Device
		{
			get { return cachedDevice ?? (cachedDevice = Resolve<Device>()); }
		}
		private Device cachedDevice;

		/// <summary>
		/// When debugging or testing crash where the actual exception happens, not here.
		/// </summary>
		private void UpdateAndDrawAllEntities()
		{
			Drawing.NumberOfDynamicVerticesDrawnThisFrame = 0;
			Drawing.NumberOfDynamicDrawCallsThisFrame = 0;
			if (Debugger.IsAttached || StackTraceExtensions.StartedFromNCrunch)
				entities.UpdateAndDrawAllEntities(Drawing.DrawEverythingInCurrentLayer);
			else
				TryUpdateAndDrawAllEntities();
		}

		private Drawing Drawing
		{
			get { return cachedDrawing ?? (cachedDrawing = Resolve<Drawing>()); }
		}
		private Drawing cachedDrawing;

		private void TryUpdateAndDrawAllEntities()
		{
			try
			{
				entities.UpdateAndDrawAllEntities(Drawing.DrawEverythingInCurrentLayer);
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
				if (exception.IsWeak())
					return;
				if (StackTraceExtensions.IsStartedFromNunitConsole())
					throw;
				DisplayMessageBoxAndCloseApp("Fatal Runtime Error", exception);
			}
		}

		private void DisplayMessageBoxAndCloseApp(string title, Exception exception)
		{
			Window.CopyTextToClipboard(exception.ToString());
			if (IsShowingMessageBoxClosedWithIgnore(title, exception))
				return;
			Dispose();
			if (!StackTraceExtensions.StartedFromNCrunch)
				Environment.Exit((int)ExitCode.UpdateAndDrawTickFailed);
		}

		private bool IsShowingMessageBoxClosedWithIgnore(string title, Exception ex)
		{
			var closeOptions = new[] { "Abort", "Ignore" };
			return Window.ShowMessageBox(title, "Unable to continue: " + ex, closeOptions) == "Ignore";
		}

		public override void Dispose()
		{
			base.Dispose();
			foreach (var instance in instancesToDispose)
				instance.Dispose();
			instancesToDispose.Clear();
			ContentLoader.DisposeIfInitialized();
			if (ScreenSpace.IsInitialized)
				ScreenSpace.Current.Dispose();
			if (Camera.IsInitialized)
				Camera.Current.Dispose();
		}
	}
}