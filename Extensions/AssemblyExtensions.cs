using System;
using System.IO;
using System.Reflection;

namespace DeltaEngine.Extensions
{
	/// <summary>
	/// Additional methods for assembly related actions.
	/// </summary>
	public static class AssemblyExtensions
	{
		//ncrunch: no coverage start
		public static string GetMyDocumentsAppFolder()
		{
			var appPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DeltaEngine",
				GetEntryAssemblyForProjectName());
			if (!Directory.Exists(appPath))
				Directory.CreateDirectory(appPath);
			return appPath;
		}

		public static string GetTestNameOrProjectName()
		{
			return testOrProjectName ?? (testOrProjectName = StackTraceExtensions.GetEntryName());
		}

		private static string testOrProjectName;

		public static string GetEntryAssemblyForProjectName()
		{
			return projectName ?? (projectName = StackTraceExtensions.GetExecutingAssemblyName());
		}

		private static string projectName;

		public static bool IsAllowed(this AssemblyName assemblyName)
		{
			return IsAllowed(assemblyName.Name);
		}

		internal static bool IsAllowed(string name)
		{
			return !(IsMicrosoftAssembly(name) || IsIdeHelperTool(name) || IsThirdPartyLibrary(name));
		}

		private static bool IsMicrosoftAssembly(string name)
		{
			return name.StartsWith("System") || name.StartsWith("mscorlib") ||
				name.StartsWith("Microsoft.") || name.StartsWith("WindowsBase") ||
				name.StartsWith("PresentationFramework") || name.StartsWith("PresentationCore") ||
				name.StartsWith("WindowsFormsIntegration");
		}

		private static bool IsIdeHelperTool(string name)
		{
			return name.StartsWith("NUnit.") || name.StartsWith("nunit.") || name.StartsWith("JetBrains.") ||
				name.StartsWith("NCrunch.") || name.StartsWith("nCrunch.") || name.StartsWith("ReSharper.") ||
				name.StartsWith("vshost32");
		}

		private static bool IsThirdPartyLibrary(string name)
		{
			return name == "OpenAL32" || name == "wrap_oal" || name == "libEGL" || name == "libgles" ||
				name == "libGLESv2" || name == "csogg" || name == "csvorbis" || name == "Autofac" ||
				name == "Moq" || name == "OpenTK" || name == "Newtonsoft.Json" || name == "NVorbis" ||
				name.StartsWith("libvlc") || name.StartsWith("DynamicProxyGen") ||
				name.StartsWith("Anonymously Hosted") || name.StartsWith("Pencil.Gaming") ||
				name.StartsWith("AvalonDock") || name.StartsWith("Farseer") || name.StartsWith("MvvmLight") ||
				name.StartsWith("SharpDX") || name.StartsWith("SlimDX") || name.StartsWith("ToyMp3") ||
				name.StartsWith("EntityFramework") || name.StartsWith("NHibernate") ||
				name.StartsWith("Approval") || name.StartsWith("System.IO.Abstractions") ||
				name.StartsWith("AsfMojo") || name.StartsWith("SharpCompress") ||
				name.StartsWith("WPFLocalizeExtension") || name.StartsWith("XAMLMarkupExtensions") ||
				name.StartsWith("Glfw") || name.StartsWith("Glfw") || name.StartsWith("MonoGame");
		}

		public static bool IsAllowed(this Assembly assembly)
		{
			return IsAllowed(assembly.GetName().Name);
		}

		public static bool IsPlatformAssembly(string assemblyName)
		{
			if (assemblyName.EndsWith(".Tests") || assemblyName.EndsWith(".Remote") ||
				assemblyName == "DeltaEngine.Input")
				return false;
			return assemblyName == "DeltaEngine" || assemblyName == "DeltaEngine.Content.Disk" ||
				assemblyName == "DeltaEngine.Content.Online" ||
				assemblyName.StartsWith("DeltaEngine.Graphics.") ||
				assemblyName.StartsWith("DeltaEngine.Multimedia.") ||
				assemblyName.StartsWith("DeltaEngine.Input.") ||
				assemblyName.StartsWith("DeltaEngine.Platforms") ||
				assemblyName.StartsWith("DeltaEngine.Windows") ||
				assemblyName.StartsWith("DeltaEngine.TestWith");
		}

		/// <summary>
		/// See http://geekswithblogs.net/rupreet/archive/2005/11/02/58873.aspx
		/// </summary>
		public static bool IsManagedAssembly(string fileName)
		{
			using (Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				var reader = new BinaryReader(fs);
				GoToDataDictionaryOfPeOptionalHeaders(fs, reader);
				var dataDictionaryRva = new uint[16];
				var dataDictionarySize = new uint[16];
				for (int i = 0; i < 15; i++)
				{
					dataDictionaryRva[i] = reader.ReadUInt32();
					dataDictionarySize[i] = reader.ReadUInt32();
				}
				return dataDictionaryRva[14] != 0;
			}
		}

		/// <summary>
		/// See http://msdn.microsoft.com/en-us/library/windows/desktop/ms680313(v=vs.85).aspx
		/// </summary>
		private static void GoToDataDictionaryOfPeOptionalHeaders(Stream fs, BinaryReader reader)
		{
			fs.Position = 0x3C;
			fs.Position = reader.ReadUInt32();
			reader.ReadBytes(24);
			fs.Position = Convert.ToUInt16(Convert.ToUInt16(fs.Position) + 0x60);
		}
	}
}