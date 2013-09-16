using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DeltaEngine.Extensions
{
	/// <summary>
	/// Used to start VisualTests from assemblies in an extra AppDomain for the SampleBrowser.
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

		private readonly string rememberedDirectory;
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

		public void Start(string className, string methodName, object[] parameters)
		{
			domain.SetData("EntryClass", className);
			domain.SetData("EntryMethod", methodName);
			domain.SetData("Parameters", parameters);
			domain.DoCallBack(StartEntryPoint);
		}

		private static void StartEntryPoint()
		{
			var assemblyFilePath = (string)AppDomain.CurrentDomain.GetData("EntryAssembly");
			var className = (string)AppDomain.CurrentDomain.GetData("EntryClass");
			var methodName = (string)AppDomain.CurrentDomain.GetData("EntryMethod");
			var parameters = (object[])AppDomain.CurrentDomain.GetData("Parameters");
			var assembly = Assembly.LoadFile(assemblyFilePath);
			foreach (var type in assembly.GetTypes().Where(type => type.Name == className))
				StartMethod(type, methodName, parameters);
		}

		private static void StartMethod(Type type, string methodName, object[] parameters)
		{
			var methods = type.GetMethods();
			var instance = Activator.CreateInstance(type);
			foreach (var method in methods.Where(method => method.Name == methodName))
				method.Invoke(instance, parameters);
		}

		public void Dispose()
		{
			Directory.SetCurrentDirectory(rememberedDirectory);
			AppDomain.Unload(domain);
		}
	}
}