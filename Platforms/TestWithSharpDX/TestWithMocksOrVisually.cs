using System;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms.Mocks;
using DeltaEngine.Rendering2D;
using NUnit.Framework;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Automatically tests with MockResolver when NCrunch is used, otherwise SharpDXResolver is used.
	/// </summary>
	[TestFixture]
	public class TestWithMocksOrVisually
	{
		[SetUp]
		public void InitializeResolver()
		{
			if (StackTraceExtensions.StartedFromNCrunch)
			{
				resolver = new MockResolver();
				return;
			}
			//ncrunch: no coverage start
			if (!StackTraceExtensions.StartedFromProgramMain)
				StackTraceExtensions.SetUnitTestName(TestContext.CurrentContext.Test.FullName);
			resolver = new SharpDXResolver();
			if (StackTraceExtensions.IsCloseAfterFirstFrameAttributeUsed() ||
				StackTraceExtensions.IsStartedFromNunitConsole())
				Resolve<Window>().CloseAfterFrame();
			//ncrunch: no coverage end
		}

		protected AppRunner resolver;

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
			var renderer = resolver.Resolve<BatchRenderer>();
			var drawing = resolver.Resolve<Drawing>();
			if (CheckIfWeNeedToRunTickToAvoidInitializationDelay())
				RunTickOnce(renderer, drawing);
			var startTimeMs = GlobalTime.Current.Milliseconds;
			do
				RunTickOnce(renderer, drawing);
			while (GlobalTime.Current.Milliseconds - startTimeMs +
				MathExtensions.Epsilon < timeToAddInSeconds * 1000);
		}

		private bool CheckIfWeNeedToRunTickToAvoidInitializationDelay()
		{
			return !(resolver is MockResolver) && GlobalTime.Current.Milliseconds == 0;
		}

		private static void RunTickOnce(BatchRenderer batchRenderer, Drawing drawing)
		{
			GlobalTime.Current.Update();
			EntitiesRunner.Current.UpdateAndDrawAllEntities(() =>
			{
				batchRenderer.DrawAndResetBatches();
				drawing.DrawEverythingInCurrentLayer();
			});
		}
	}
}