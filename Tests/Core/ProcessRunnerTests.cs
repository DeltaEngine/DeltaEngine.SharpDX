using System;
using DeltaEngine.Core;
using DeltaEngine.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Tests.Core
{
	public class ProcessRunnerTests
	{
		[Test]
		public void DefaultWorkingDirectory()
		{
			var processRunner = new ProcessRunner("cmd.exe", "/c dir");
			Assert.AreEqual(Environment.CurrentDirectory, processRunner.WorkingDirectory);
		}

		[Test]
		public void ChangingWorkingDirectory()
		{
			var processRunner = new ProcessRunner("cmd.exe", "/c dir");
			processRunner.Start();
			var outputWithDefaultWorkingDirectory = processRunner.Output;
			processRunner.WorkingDirectory = @"C:\";
			processRunner.Start();
			var outputWithDefinedWorkingDirectory = processRunner.Output;
			Assert.AreNotEqual(outputWithDefaultWorkingDirectory, outputWithDefinedWorkingDirectory);
		}

		[Test]
		public void StandardOutputEvent()
		{
			var logger = new MockLogger();
			var processRunner = new ProcessRunner("cmd.exe", "/c dir");
			processRunner.StandardOutputEvent +=
				outputMessage => logger.Write(Logger.MessageType.Info, outputMessage);
			processRunner.Start();
			Assert.IsTrue(logger.LastMessage.Contains("Dir(s)"), logger.LastMessage);
		}

		[Test]
		public void TestTimeout()
		{
			var processRunner = new ProcessRunner("cmd.exe", "/c dir", 1);
			Assert.Throws<ProcessRunner.StandardOutputHasTimedOutException>(processRunner.Start);
		}
	}
}