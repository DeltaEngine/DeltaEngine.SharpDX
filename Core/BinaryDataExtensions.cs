using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using DeltaEngine.Extensions;

namespace DeltaEngine.Core
{
	/// <summary>
	/// Allows to easily save and recreate binary data objects with the full type names like other
	/// Serializers, but way faster (100x). Before reconstructing types load all needed assemblies.
	/// </summary>
	public static class BinaryDataExtensions
	{
		static BinaryDataExtensions()
		{
			RegisterAvailableBinaryDataImplementation();
			AppDomain.CurrentDomain.AssemblyLoad += (o, args) =>
			{
				if (ShouldLoadTypes(args.LoadedAssembly))
					AddAssemblyTypes(args.LoadedAssembly);
			};
		}

		private static void RegisterAvailableBinaryDataImplementation()
		{
			AddPrimitiveTypes();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies.Where(ShouldLoadTypes))
				AddAssemblyTypes(assembly);
		}

		private static bool ShouldLoadTypes(Assembly assembly)
		{
			return assembly.IsAllowed() &&
				!AssemblyExtensions.IsPlatformAssembly(assembly.GetName().Name) ||
				assembly.GetName().Name == "DeltaEngine";
		}

		private static void AddPrimitiveTypes()
		{
			AddType(typeof(object));
			AddType(typeof(bool));
			AddType(typeof(byte));
			AddType(typeof(char));
			AddType(typeof(decimal));
			AddType(typeof(double));
			AddType(typeof(float));
			AddType(typeof(short));
			AddType(typeof(int));
			AddType(typeof(long));
			AddType(typeof(string));
			AddType(typeof(sbyte));
			AddType(typeof(ushort));
			AddType(typeof(uint));
			AddType(typeof(ulong));
		}

		private static void AddAssemblyTypes(Assembly assembly)
		{
			try
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types.Where(type => IsValidBinaryDataType(type)))
					AddType(type);
			}
			//ncrunch: no coverage start
			catch (ReflectionTypeLoadException ex)
			{
				foreach (var failedLoader in ex.LoaderExceptions)
					Logger.Error(failedLoader);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
			//ncrunch: no coverage end
		}

		private static bool IsValidBinaryDataType(Type type)
		{
			if (type.IsAbstract || typeof(Exception).IsAssignableFrom(type))
				return false;
			string name = type.FullName;
			return !name.StartsWith("NUnit.") && !name.EndsWith("Tests") && !name.Contains("<") &&
				!name.StartsWith("Microsoft.");
		}

		private static void AddType(Type type)
		{
			string shortName = type.Name;
			if (TypeMap.ContainsKey(shortName))
			{
				shortName = type.FullName;
				if (TypeMap.ContainsKey(shortName))
					return; //ncrunch: no coverage
			}
			ShortNames.Add(type, shortName);
			TypeMap.Add(shortName, type);
		}

		private static readonly Dictionary<string, Type> TypeMap = new Dictionary<string, Type>();
		private static readonly Dictionary<Type, string> ShortNames = new Dictionary<Type, string>();

		internal static string GetShortName(this object data)
		{
			if (ShortNames.ContainsKey(data.GetType()))
				return ShortNames[data.GetType()];
			throw new NoShortNameStoredFor(data);
		}

		internal class NoShortNameStoredFor : Exception
		{
			public NoShortNameStoredFor(object data)
				: base(data.ToString()) {}
		}

		internal static string GetShortNameOrFullNameIfNotFound(this object data)
		{
			var type = GetTypeOrObjectType(data);
			return ShortNames.ContainsKey(type) ? ShortNames[type] : type.AssemblyQualifiedName;
		}

		internal static Type GetTypeOrObjectType(Object element)
		{
			return element == null ? typeof(object) : element.GetType();
		}

		internal static Type GetTypeFromShortNameOrFullNameIfNotFound(this string typeName)
		{
			return TypeMap.ContainsKey(typeName) ? TypeMap[typeName] : Type.GetType(typeName, true);
		}

		/// <summary>
		/// Saves any object type information and the actual data contained in in, use Create to load.
		/// </summary>
		public static void Save(object data, BinaryWriter writer)
		{
			if (data == null)
				throw new ArgumentNullException("data");
			writer.Write(data.GetShortName());
			WriteDataVersionNumber(data, writer);
			BinaryDataSaver.TrySaveData(data, data.GetType(), writer);
		}

		private static void WriteDataVersionNumber(object data, BinaryWriter writer)
		{
			var dataVersion = data.GetType().Assembly.GetName().Version;
			writer.Write((byte)dataVersion.Major);
			writer.Write((byte)dataVersion.Minor);
			writer.Write((byte)dataVersion.Build);
			writer.Write((byte)dataVersion.Revision);
		}

		/// <summary>
		/// Loads a binary data object and reconstructs the object based on the saved type information.
		/// </summary>
		public static object Create(this BinaryReader reader)
		{
			if (reader.BaseStream.Position + 6 > reader.BaseStream.Length)
				throw new NotEnoughDataLeftInStream(reader.BaseStream.Length);
			string shortName = reader.ReadString();
			var dataVersion = ReadDataVersionNumber(reader);
			if (TypeMap.ContainsKey(shortName))
				return BinaryDataLoader.TryCreateAndLoad(TypeMap[shortName], reader, dataVersion);
			throw new UnknownMessageTypeReceived(shortName); //ncrunch: no coverage
		}

		public class NotEnoughDataLeftInStream : Exception
		{
			public NotEnoughDataLeftInStream(long length)
				: base("Length=" + length) {}
		}

		private static Version ReadDataVersionNumber(BinaryReader r)
		{
			return new Version(r.ReadByte(), r.ReadByte(), r.ReadByte(), r.ReadByte());
		}

		public class UnknownMessageTypeReceived : Exception
		{
			public UnknownMessageTypeReceived(string message)
				: base(message) {}
		}

		public static MemoryStream SaveToMemoryStream(object binaryData)
		{
			var data = new MemoryStream();
			var writer = new BinaryWriter(data);
			Save(binaryData, writer);
			return data;
		}

		public static object CreateFromMemoryStream(this MemoryStream data)
		{
			data.Seek(0, SeekOrigin.Begin);
			return Create(new BinaryReader(data));
		}

		public static byte[] ToByteArrayWithLengthHeader(object message)
		{
			byte[] data = ToByteArrayWithTypeInformation(message);
			using (var total = new MemoryStream())
			using (var writer = new BinaryWriter(total))
			{
				writer.WriteNumberMostlyBelow255(data.Length);
				writer.Write(data);
				return total.ToArray();
			}
		}

		public static byte[] ToByteArrayWithTypeInformation(object data)
		{
			using (var messageStream = new MemoryStream())
			using (var messageWriter = new BinaryWriter(messageStream))
			{
				Save(data, messageWriter);
				return messageStream.ToArray();
			}
		}

		public static byte[] ToByteArray(object data)
		{
			using (var messageStream = new MemoryStream())
			using (var messageWriter = new BinaryWriter(messageStream))
			{
				if (data is IList)
					foreach (object value in data as IList)
						BinaryDataSaver.TrySaveData(value, value.GetType(), messageWriter);
				else
					BinaryDataSaver.TrySaveData(data, data.GetType(), messageWriter);
				return messageStream.ToArray();
			}
		}

		public static object ToBinaryData(this byte[] data)
		{
			using (var messageStream = new MemoryStream(data))
			using (var messageReader = new BinaryReader(messageStream))
				return Create(messageReader);
		}

		public static MemoryStream SaveDataIntoMemoryStream<DataType>(DataType input)
		{
			var data = new MemoryStream();
			var writer = new BinaryWriter(data);
			BinaryDataSaver.TrySaveData(input, typeof(DataType), writer);
			return data;
		}

		public static DataType LoadKnownTypeWithoutVersionCheck<DataType>(this BinaryReader reader)
		{
			return (DataType)BinaryDataLoader.TryCreateAndLoad(typeof(DataType), reader, null);
		}

		public static DataType LoadDataWithKnownTypeFromMemoryStream<DataType>(MemoryStream data)
		{
			data.Seek(0, SeekOrigin.Begin);
			var reader = new BinaryReader(data);
			var version = typeof(DataType).Assembly.GetName().Version;
			return (DataType)BinaryDataLoader.TryCreateAndLoad(typeof(DataType), reader, version);
		}

		internal static bool DoNotNeedToSaveType(this Type fieldType)
		{
			return fieldType == typeof(Action) || fieldType == typeof(Action<>) ||
				fieldType.BaseType == typeof(MulticastDelegate) || fieldType == typeof(BinaryWriter) ||
				fieldType == typeof(BinaryReader) || fieldType == typeof(Pointer) ||
				fieldType == typeof(IntPtr) || fieldType == typeof(ISerializable);
		}

		internal static bool NeedToSaveTypeName(this Type fieldType)
		{
			return fieldType.AssemblyQualifiedName != null && fieldType != typeof(string) &&
				!fieldType.IsArray && !typeof(IList).IsAssignableFrom(fieldType) &&
				!typeof(IDictionary).IsAssignableFrom(fieldType) && fieldType != typeof(MemoryStream);
		}
	}
}