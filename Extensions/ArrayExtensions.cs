using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaEngine.Extensions
{
	/// <summary>
	/// Additional array manipulation and array to text methods.
	/// </summary>
	public static class ArrayExtensions
	{
		public static bool Contains<T>(this Array array, T value)
		{
			return array != null && array.Cast<T>().Contains(value);
		}

		public static bool Compare<T>(this IEnumerable<T> array1, IEnumerable<T> array2)
		{
			return array1 == null && array2 == null ||
				array1 != null && array2 != null && array1.SequenceEqual(array2);
		}

		public static string ToText<T>(this IEnumerable<T> texts, string separator = ", ")
		{
			return string.Join(separator, texts);
		}

		public static Value GetWithDefault<Key, Value>(Dictionary<Key, object> dict, Key key)
		{
			if (dict.ContainsKey(key) == false)
				return default(Value);
			Value result;
			try
			{
				result = (Value)dict[key];
			}
			catch
			{
				result = default(Value);
			}
			return result;
		}
	}
}