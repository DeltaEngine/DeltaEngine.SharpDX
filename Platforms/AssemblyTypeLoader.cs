using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DeltaEngine.Core;
using DeltaEngine.Extensions;

namespace DeltaEngine.Platforms
{
	internal class AssemblyTypeLoader
	{
		public AssemblyTypeLoader(Resolver resolver)
		{
			this.resolver = resolver;
		}

		private readonly Resolver resolver;

		public void RegisterAllTypesFromAllAssemblies<ContentDataType, UpdateType, DrawType>()
		{
			var assemblies = TryLoadAllUnloadedAssemblies(AppDomain.CurrentDomain.GetAssemblies());
			foreach (Assembly assembly in assemblies)
				if (!AssemblyExtensions.IsPlatformAssembly(assembly.GetName().Name))
				{
					Type[] assemblyTypes = TryToGetAssemblyTypes(assembly);
					if (assemblyTypes == null)
						continue; //ncrunch: no coverage
					RegisterAllTypesInAssembly<ContentDataType>(assemblyTypes, false);
					RegisterAllTypesInAssembly<UpdateType>(assemblyTypes, true);
					RegisterAllTypesInAssembly<DrawType>(assemblyTypes, true);
					resolver.RegisterAllTypesInAssembly(assemblyTypes);
				}
		}

		private static IEnumerable<Assembly> TryLoadAllUnloadedAssemblies(Assembly[] loadedAssemblies)
		{
			var assemblies = new List<Assembly>(loadedAssemblies);
			var dllFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
			foreach (var filePath in dllFiles)
				try
				{
					string name = Path.GetFileNameWithoutExtension(filePath);
					if (AssemblyExtensions.IsManagedAssembly(filePath) && new AssemblyName(name).IsAllowed() &&
						!AssemblyExtensions.IsPlatformAssembly(name) && !name.EndsWith(".Mocks") &&
						!name.EndsWith(".Tests") && assemblies.All(a => a.GetName().Name != name))
						assemblies.Add(Assembly.LoadFrom(filePath));
				}
				catch (Exception ex)
				{
					Logger.Warning("Failed to load assembly " + filePath + ": " + ex.Message);
				}
			foreach (var assembly in loadedAssemblies)
				if (assembly.IsAllowed() && !AssemblyExtensions.IsPlatformAssembly(assembly.GetName().Name))
					LoadDependentAssemblies(assembly, assemblies);
			return assemblies;
		}

		private static void LoadDependentAssemblies(Assembly assembly, List<Assembly> assemblies)
		{
			foreach (var dependency in assembly.GetReferencedAssemblies())
				if (!IsConflictiveDependency(dependency) && dependency.IsAllowed() && !dependency.Name.EndsWith(".Mocks") &&
					assemblies.All(loaded => dependency.Name != loaded.GetName().Name))
					assemblies.Add(Assembly.Load(dependency));
		}

		//Needed in Windows 8.1 Pro Preview since Windows.Storage (referenced by Windows.Foundation) cannot be loaded
		private static bool IsConflictiveDependency(AssemblyName dependency)
		{
			var conflictiveDependencies = new[] { "Windows.Storage" };
			return conflictiveDependencies.Any(conflictiveDependency => conflictiveDependency == dependency.Name);
		}

		private static Type[] TryToGetAssemblyTypes(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			//ncrunch: no coverage start
			catch (Exception ex)
			{
				string errorText = ex.ToString();
				var loaderError = ex as ReflectionTypeLoadException;
				if (loaderError != null)
					foreach (var error in loaderError.LoaderExceptions)
						errorText += "\n\n" + error;
				Logger.Warning("Failed to load types from " + assembly.GetName().Name + ": " + errorText);
				return null;
			}
			//ncrunch: no coverage end
		}

		private void RegisterAllTypesInAssembly<T>(Type[] assemblyTypes, bool registerAsSingleton)
		{
			foreach (Type type in assemblyTypes)
				if (typeof(T).IsAssignableFrom(type) && IsTypeResolveable(type))
					if (registerAsSingleton)
						resolver.RegisterSingleton(type);
					else
						resolver.Register(type);
		}

		/// <summary>
		/// Allows to ignore most types. IsAbstract will also check if the class is static
		/// </summary>
		public static bool IsTypeResolveable(Type type)
		{
			if (type.IsEnum || type.IsAbstract || type.IsInterface || type.IsValueType ||
				typeof(Exception).IsAssignableFrom(type) || type == typeof(Action) ||
				type == typeof(Action<>) || typeof(MulticastDelegate).IsAssignableFrom(type))
				return false;
			if (IsGeneratedType(type) || IsGenericType(type) || type.Name.StartsWith("Mock") ||
				type.Name == "Program")
				return false;
			return !IgnoreForResolverAttribute.IsTypeIgnored(type);
		}

		private static bool IsGeneratedType(Type type)
		{
			return type.FullName.StartsWith("<") || type.Name.StartsWith("<>");
		}

		private static bool IsGenericType(Type type)
		{
			return type.FullName.Contains("`1");
		}
	}
}