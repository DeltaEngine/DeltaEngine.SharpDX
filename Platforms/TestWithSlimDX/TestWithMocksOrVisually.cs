using System;
using DeltaEngine.Core;
using DeltaEngine.Extensions;
using DeltaEngine.Platforms.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Automatically tests with MockResolver when NCrunch is used, otherwise SlimDXResolver is used.
	/// </summary>
	[TestFixture]
	public class TestWithMocksOrVisually
	{
		[SetUp]
		public void InitializeResolver()
		{
			if (StackTraceExtensions.ForceUseOfMockResolver())
			{
				resolver = new MockResolver();
				return;
			}
			//ncrunch: no coverage start
			if (!StackTraceExtensions.StartedFromProgramMain)
				StackTraceExtensions.SetUnitTestName(TestContext.CurrentContext.Test.FullName);
			resolver = new SlimDXResolver();
			if (StackTraceExtensions.IsCloseAfterFirstFrameAttributeUsed() ||
				StackTraceExtensions.StartedFromNCrunchOrNunitConsole)
				Resolve<Window>().CloseAfterFrame();
			//ncrunch: no coverage end
		}

		private AppRunner resolver;

		protected bool IsMockResolver
		{
			get { return resolver is MockResolver; }
		}

		protected void RegisterMock<T>(T instance) where T : class
		{
			if (IsMockResolver)
				(resolver as MockResolver).RegisterMock(instance);
		}

		[TearDown]
		public void RunTestAndDisposeResolverWhenDone()
		{
			try
			{
				if (StackTraceExtensions.StartedFromProgramMain ||
					TestContext.CurrentContext.Result.Status == TestStatus.Passed)
					resolver.Run();
			}
			finally
			{
				resolver.Dispose();
			}
		}

		protected T Resolve<T>() where T : class
		{
			return resolver.Resolve<T>();
		}

		protected void RunAfterFirstFrame(Action executeOnce)
		{
			resolver.CodeAfterFirstFrame = executeOnce;
		}

		protected void AdvanceTimeAndUpdateEntities(
			float timeToAddInSeconds = 1.0f / Settings.DefaultUpdatesPerSecond)
		{
			if (CheckIfWeNeedToRunTickToAvoidInitializationDelay())
				resolver.RunTick();
			var startTimeMs = GlobalTime.Current.Milliseconds;
			do
				resolver.RunTick();
			while (GlobalTime.Current.Milliseconds - startTimeMs +
				MathExtensions.Epsilon < timeToAddInSeconds * 1000);
		}

		private bool CheckIfWeNeedToRunTickToAvoidInitializationDelay()
		{
			return !(resolver is MockResolver) && GlobalTime.Current.Milliseconds == 0;
		}
	}
}