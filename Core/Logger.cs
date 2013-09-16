using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Extensions;

namespace DeltaEngine.Core
{
	/// <summary>
	/// Allows any problem or issue to be logged via messages or exceptions, usually used by the
	/// MockLogger for tests and ConsoleLogger, TextFileLogger and NetworkLogger in apps.
	/// </summary>
	public abstract class Logger : IDisposable
	{
		private static readonly ThreadStatic<List<Logger>> RegisteredLoggers =
			new ThreadStatic<List<Logger>>();

		protected Logger(bool registerToAllThreads = false)
		{
			if (registerToAllThreads)
				RegisterToAllThreads();
			else
				RegisterToCurrentThread();
		}

		private void RegisterToAllThreads()
		{
			if (CurrentLoggersInAllThreads.Any(logger => logger.GetType() == GetType()))
				if (GetType().Name.StartsWith("Console"))
					RemoveConsoleLoggerFromPreviousFailingTest();
				else
					throw new LoggerWasAlreadyAttached();
			CurrentLoggersInAllThreads.Add(this);
		}

		private void RemoveConsoleLoggerFromPreviousFailingTest()
		{
			foreach (Logger logger in CurrentLoggersInAllThreads)
				if (logger.GetType() == GetType())
				{
					RegisteredLoggers.Current.Remove(logger);
					break;
				}
		}

		private void RegisterToCurrentThread()
		{
			if (!RegisteredLoggers.HasCurrent)
				RegisteredLoggers.Use(new List<Logger>());
			if (RegisteredLoggers.Current.Any(logger => logger.GetType() == GetType()))
				if (GetType().Name.StartsWith("Mock"))
					RemoveMockLoggerFromPreviousFailingTest();
				else
					throw new LoggerWasAlreadyAttached();
			RegisteredLoggers.Current.Add(this);
		}

		private void RemoveMockLoggerFromPreviousFailingTest()
		{
			foreach (Logger logger in RegisteredLoggers.Current)
				if (logger.GetType() == GetType())
				{
					RegisteredLoggers.Current.Remove(logger);
					break;
				}
		}

		private static readonly List<Logger> CurrentLoggersInAllThreads = new List<Logger>();

		internal static int TotalNumberOfAttachedLoggers
		{
			get
			{
				return CurrentLoggersInAllThreads.Count +
					(RegisteredLoggers.HasCurrent ? RegisteredLoggers.Current.Count : 0);
			}
		}

		public class LoggerWasAlreadyAttached : Exception {}

		/// <summary>
		/// Lowest available log level for notifications like a successful operation or debug output.
		/// </summary>
		public static void Info(string message)
		{
			WarnIfNoLoggersAreAttached(message);
			foreach (var logger in CurrentLoggers)
				logger.WriteMessage(MessageType.Info, message);
		}

		private static List<Logger> CurrentLoggers
		{
			get
			{
				var total = new List<Logger>(CurrentLoggersInAllThreads);
				if (RegisteredLoggers.HasCurrent)
					total.AddRange(RegisteredLoggers.Current);
				return total;
			}
		}

		private void WriteMessage(MessageType type, string message)
		{
			if (message != LastMessage)
				Write(type, message);
			else
				NumberOfRepeatedMessagesIgnored++;
			LastMessage = message;
		}

		public string LastMessage { get; private set; }
		public int NumberOfRepeatedMessagesIgnored { get; private set; }

		private static void WarnIfNoLoggersAreAttached(string message)
		{
			if (CurrentLoggers.Count == 0)
				Console.WriteLine("No loggers have been created for this message: " + message);
		}

		public abstract void Write(MessageType messageType, string message);

		public enum MessageType
		{
			Info,
			Warning,
			Error
		}

		/// <summary>
		/// If something bad happened but we can continue this type of message is logged.
		/// </summary>
		public static void Warning(string message)
		{
			WarnIfNoLoggersAreAttached(message);
			foreach (var logger in CurrentLoggers)
				logger.WriteMessage(MessageType.Warning, message);
		}

		/// <summary>
		/// If something bad happened and we caught a non fatal exception this message is logged.
		/// </summary>
		public static void Warning(Exception exception)
		{
			WarnIfNoLoggersAreAttached(exception.ToString());
			foreach (var logger in CurrentLoggers)
				logger.WriteMessage(MessageType.Warning, exception.ToString());
		}

		/// <summary>
		/// If a fatal exception happened this message is logged. Often the App has to quit afterwards.
		/// </summary>
		public static void Error(Exception exception)
		{
			var exceptionText = StackTraceExtensions.FormatExceptionIntoClickableMultilineText(exception);
			WarnIfNoLoggersAreAttached(exceptionText);
			foreach (var logger in CurrentLoggers)
				logger.WriteMessage(MessageType.Error, exceptionText);
		}

		protected string CreateMessageTypePrefix(MessageType messageType)
		{
			return DateTime.Now.ToString("T") + " " +
				(messageType == MessageType.Info ? "" : messageType + ": ");
		}

		public virtual void Dispose()
		{
			if (RegisteredLoggers.HasCurrent)
				RegisteredLoggers.Current.Remove(this);
			CurrentLoggersInAllThreads.Remove(this);
		}
	}
}
