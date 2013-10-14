using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DeltaEngine.Extensions
{
	/// <summary>
	/// Used to start VisualTests from assemblies in an extra AppDomain for the SampleBrowser. Also
	/// used in the ContinuousUpdater to get all tests and start them safely in the Editor.
	/// </summary>
	public class AssemblyStarter : IDisposable
	{
		//ncrunch: no coverage start
		public AssemblyStarter(string assemblyFilePath)
		{
			rememberedDirectory = Directory.GetCurrentDirectory();
			string assemblyDirectory = Path.GetDirectoryName(assemblyFilePath);
			if (assemblyDirectory.Length == 0)
				assemblyDirectory = rememberedDirectory;
			Directory.SetCurrentDirectory(assemblyDirectory);
			domain = AppDomain.CreateDomain(DomainName, null, CreateDomainSetup(assemblyDirectory));
			domain.SetData("EntryAssembly", Path.GetFullPath(assemblyFilePath));
		}

		[NonSerialized]
		private readonly string rememberedDirectory;
		[NonSerialized]
		private readonly AppDomain domain;
		private const string DomainName = "Delta Engine Assembly Starter";

		private static AppDomainSetup CreateDomainSetup(string assemblyFilePath)
		{
			return new AppDomainSetup
			{
				ApplicationBase = Path.GetFullPath(assemblyFilePath),
				ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
			};
		}

		public void Start(string className, string methodName, object[] parameters = null)
		{
			domain.SetData("EntryClass", className);
			domain.SetData("EntryMethod", methodName);
			domain.SetData("Parameters", parameters);
			domain.DoCallBack(StartEntryPoint);
		}

		private static void StartEntryPoint()
		{
			var assembly = LoadAssembly();
			var className = (string)AppDomain.CurrentDomain.GetData("EntryClass");
			var methodName = (string)AppDomain.CurrentDomain.GetData("EntryMethod");
			var parameters = (object[])AppDomain.CurrentDomain.GetData("Parameters");
			foreach (var type in assembly.GetTypes())
				if (type.Name == className)
					StartMethod(type, methodName, parameters);
		}

		private static Assembly LoadAssembly()
		{
			var assemblyFilePath = (string)AppDomain.CurrentDomain.GetData("EntryAssembly");
			return Assembly.LoadFile(assemblyFilePath);
		}

		private static void StartMethod(Type type, string methodName, object[] parameters)
		{
			var methods = type.GetMethods();
			var instance = Activator.CreateInstance(type);
			//TODO: also call SetUp and TearDown if needed with base class too
			foreach (var method in methods.Where(method => method.Name == methodName))
				method.Invoke(instance, parameters);
		}

		public string[] GetTestNames()
		{
			domain.DoCallBack(FindAllTestNames);
			return (string[])domain.GetData("TestClassAndMethodNames");
		}

		private static void FindAllTestNames()
		{
			var assembly = LoadAssembly();
			var tests = new List<string>();
			foreach (var type in assembly.GetTypes())
				if (type.Name == "Program" || type.Name.EndsWith("Tests"))
					foreach (var method in type.GetMethods())
						if (method.Name != "ToString" && method.Name != "Equals" &&
							method.Name != "GetHashCode" && method.Name != "GetType")
							//TODO: exclude SetUp, TearDown, etc.
							tests.Add(type.Name + "." + method.Name);
			AppDomain.CurrentDomain.SetData("TestClassAndMethodNames", tests.ToArray());
		}

		public void Dispose()
		{
			Directory.SetCurrentDirectory(rememberedDirectory);
			AppDomain.Unload(domain);
		}
	}
}