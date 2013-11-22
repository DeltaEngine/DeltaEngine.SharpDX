using System;
using System.Diagnostics;
using System.Threading;

namespace DeltaEngine.Core
{
	/// <summary>
	/// Starts a command line process with given argument and optional timeout. Supports events or
	/// can be used synchronously with checking Error or Output afterwards. Exceptions are thrown when
	/// things go bad (ExitCode not 0 or process times out).
	/// </summary>
	public class ProcessRunner
	{
		public ProcessRunner(string filePath, string argumentsLine = "", int timeoutInMs = NoTimeout)
		{
			FilePath = filePath;
			ArgumentsLine = argumentsLine;
			this.timeoutInMs = timeoutInMs;
			Output = "";
			Errors = "";
			WorkingDirectory = Environment.CurrentDirectory;
		}

		private const int NoTimeout = -1;

		public string FilePath { get; protected set; }
		public string ArgumentsLine { get; protected set; }
		protected readonly int timeoutInMs;

		public string WorkingDirectory { get; set; }
		public string Output { get; private set; }
		public string Errors { get; private set; }

		public void Start()
		{
			using (nativeProcess = new Process())
			{
				SetupStartInfo();
				SetupProcessAndRun();
			}
			nativeProcess = null;
		}

		private Process nativeProcess;

		protected void SetupStartInfo()
		{
			nativeProcess.StartInfo.FileName = FilePath;
			nativeProcess.StartInfo.Arguments = ArgumentsLine;
			nativeProcess.StartInfo.WorkingDirectory = WorkingDirectory;
			nativeProcess.StartInfo.CreateNoWindow = true;
			nativeProcess.StartInfo.UseShellExecute = false;
			nativeProcess.StartInfo.RedirectStandardOutput = true;
			nativeProcess.StartInfo.RedirectStandardError = true;
		}

		/// <summary>
		/// Helpful post how to avoid the possible deadlock of a process
		/// http://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
		/// </summary>
		private void SetupProcessAndRun()
		{
			using (outputWaitHandle = new AutoResetEvent(false))
			using (errorWaitHandle = new AutoResetEvent(false))
				AttachToOutputStreamAndRunNativeProcess();
			errorWaitHandle = null;
			outputWaitHandle = null;
		}

		private AutoResetEvent outputWaitHandle;
		private AutoResetEvent errorWaitHandle;

		private void AttachToOutputStreamAndRunNativeProcess()
		{
			nativeProcess.OutputDataReceived += OnStandardOutputDataReceived;
			nativeProcess.ErrorDataReceived += OnErrorOutputDataReceived;
			StartNativeProcessAndWaitForExit();
		}

		private void OnStandardOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (String.IsNullOrEmpty(e.Data))
			{
				outputWaitHandle.Set();
				return;
			}
			if (StandardOutputEvent != null)
				StandardOutputEvent(e.Data);
			Output += e.Data + "\n";
		}

		public event Action<string> StandardOutputEvent;

		private void OnErrorOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (String.IsNullOrEmpty(e.Data))
			{
				errorWaitHandle.Set();
				return;
			}
			//ncrunch: no coverage start
			if (ErrorOutputEvent != null)
				ErrorOutputEvent(e.Data);
			Errors += e.Data + "\n";
			//ncrunch: no coverage end
		}

		public event Action<string> ErrorOutputEvent;

		private void StartNativeProcessAndWaitForExit()
		{
			nativeProcess.Start();
			nativeProcess.BeginOutputReadLine();
			nativeProcess.BeginErrorReadLine();
			WaitForExit();
			if (!DontCheckExitCode)
				CheckExitCode();
		}

		public bool DontCheckExitCode { get; set; }

		private void WaitForExit()
		{
			if (!outputWaitHandle.WaitOne(timeoutInMs))
				throw new StandardOutputHasTimedOutException();
			if (!errorWaitHandle.WaitOne(timeoutInMs))
				throw new ErrorOutputHasTimedOutException(); //ncrunch: no coverage
			if (!nativeProcess.WaitForExit(timeoutInMs))
				throw new ProcessHasTimedOutException(); //ncrunch: no coverage
		}

		public class StandardOutputHasTimedOutException : Exception {}
		public class ErrorOutputHasTimedOutException : Exception {}
		public class ProcessHasTimedOutException : Exception {}

		private void CheckExitCode()
		{
			if (nativeProcess.ExitCode != 0)
				throw new ProcessTerminatedWithError(); //ncrunch: no coverage
		}

		public class ProcessTerminatedWithError : Exception {}
	}
}