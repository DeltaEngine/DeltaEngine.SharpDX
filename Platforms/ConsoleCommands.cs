using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DeltaEngine.Core;
using DeltaEngine.Extensions;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Evaluates command strings and executes delegates.
	/// </summary>
	public class ConsoleCommands
	{
		internal ConsoleCommands(ConsoleCommandResolver resolver)
		{
			this.resolver = resolver;
			ThreadStaticConsoleCommands.Use(this);
		}

		private readonly ConsoleCommandResolver resolver;
		private static readonly ThreadStatic<ConsoleCommands> ThreadStaticConsoleCommands =
			new ThreadStatic<ConsoleCommands>();

		public static ConsoleCommands Current
		{
			get { return ThreadStaticConsoleCommands.Current; }	
		}

		public void RegisterCommandsFromTypes(IEnumerable<Type> types)
		{
			foreach (Type type in types)
			{
				var allMethods = 
					type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
				var consoleMethods = allMethods.Where(method => 
					method.GetCustomAttributes(typeof(ConsoleCommandAttribute), true).Any()).ToList();
				RegisterCommandsForType(type, consoleMethods);
			}
		}

		private void RegisterCommandsForType(Type type, List<MethodInfo> methods)
		{
			if (methods.Count == 0)
				return;
			var instance = resolver.Resolve(type);
			foreach (MethodInfo method in methods)
				AddCommand(method, instance);
		}

		private void AddCommand(MethodInfo method, object target)
		{
			string delegateKey = method + " + " + target;
			if (delegatesAlreadyCreated.Contains(delegateKey))
				return;
			delegates.Add(CreateDelegate(method, target));
			delegatesAlreadyCreated.Add(delegateKey);
		}

		private readonly List<string> delegatesAlreadyCreated = new List<string>();
		private readonly List<Delegate> delegates = new List<Delegate>();

		private static string GetMethodName(MethodInfo method)
		{
			object[] attributes = method.GetCustomAttributes(typeof(ConsoleCommandAttribute), true);
			var attribute = attributes.FirstOrDefault() as ConsoleCommandAttribute;
			return attribute.Name;
		}

		private static Delegate CreateDelegate(MethodInfo method, object target)
		{
			Type delegateType = Expression.GetDelegateType(
					method.GetParameters().Select(p => p.ParameterType).Concat(new[] { method.ReturnType }).
								ToArray());
			return Delegate.CreateDelegate(delegateType, target, method);
		}

		public string ExecuteCommand(string command)
		{
			var commandAndParameters = new List<string>(command.SplitAndTrim(' '));
			if (commandAndParameters.Count == 0)
				return "";
			var method = delegates.FirstOrDefault(d =>
						GetMethodName(d.Method).Equals(commandAndParameters[0],
						StringComparison.OrdinalIgnoreCase));
			if (method == null)
				return "Error: Unknown console command '" + commandAndParameters[0] + "'";
			commandAndParameters.RemoveAt(0);
			return ExecuteMethod(commandAndParameters, method);
		}

		private static string ExecuteMethod(List<string> commandParameters, Delegate method)
		{
			var methodParameters = method.Method.GetParameters();
			if (methodParameters.Length == commandParameters.Count)
				return methodParameters.Length == 0
					? InvokeDelegate(method, null)
					: ExecuteMethodWithParameters(commandParameters, method, methodParameters);
			string plural = methodParameters.Length == 1 ? "" : "s";
			return "Error: The command has " + methodParameters.Length + " parameter" + plural +
				", but you entered " + commandParameters.Count;
		}

		private static string InvokeDelegate(Delegate method, params object[] parameters)
		{
			try
			{
				return TryInvokeDelegate(method, parameters);
			}
			catch (Exception ex)
			{
				return "Error: Exception while invoking the command: '" + ex.Message + "'";
			}
		}

		private static string TryInvokeDelegate(Delegate method, params object[] parameters)
		{
			var result = Convert.ToString(method.DynamicInvoke(parameters));
			return string.IsNullOrWhiteSpace(result) ? "Command executed" : "Result: '" + result + "'";
		}

		private static string ExecuteMethodWithParameters(List<string> commandParameters,
			Delegate method, ParameterInfo[] parameter)
		{
			var paramObjLst = new List<object>();
			for (int i = 0; i < parameter.Length; i++)
				try
				{
					paramObjLst.Add(Convert.ChangeType(commandParameters[i], parameter[i].ParameterType));
				}
				catch (Exception ex)
				{
					return "Error: Can't process parameter no. " + (i + 1) + ": '" + ex.Message + "'";
				}
			return InvokeDelegate(method, paramObjLst.ToArray());
		}

		public List<string> GetAutoCompletionList(string input)
		{
			return GetMatchingDelegates(input).Select(GetDescription).ToList().OrderBy(x => x).ToList();
		}

		private IEnumerable<MethodInfo> GetMatchingDelegates(string input)
		{
			return 
				delegates.Select(x => x.Method).Where(m => GetMethodName(m).StartsWith(input, true, null)).
									ToList();
		}

		private static string GetDescription(MethodInfo info)
		{
			string description = GetMethodName(info);
			IEnumerable<string> parameters = info.GetParameters().Select(x => x.ParameterType.Name);
			// ReSharper disable PossibleMultipleEnumeration
			if (parameters.Any())
				description += " " + parameters.Aggregate((x, y) => x + " " + y);
			// ReSharper restore PossibleMultipleEnumeration
			return description;
		}

		public string AutoCompleteString(string input)
		{
			IEnumerable<MethodInfo> matchingDelegates = GetMatchingDelegates(input);
			IEnumerable<string> names = matchingDelegates.Select(x => GetMethodName(x));
			// ReSharper disable PossibleMultipleEnumeration
			return !names.Any() ? input : GetShortestString(names);
			// ReSharper restore PossibleMultipleEnumeration
		}

		private static string GetShortestString(IEnumerable<string> names)
		{
			// ReSharper disable PossibleMultipleEnumeration
			string shortestString = names.First(x => x.Length == names.Select(y => y.Length).Min());
			while (!names.All(s => s.StartsWith(shortestString)))
				shortestString = shortestString.Substring(0, shortestString.Length - 1);
			// ReSharper restore PossibleMultipleEnumeration
			return shortestString;
		}
	}
}