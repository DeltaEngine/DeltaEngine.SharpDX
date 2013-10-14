using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Extensions;

namespace DeltaEngine.Mocks
{
	/// <summary>
	/// Mocks a settings file during unit tests instead just having a simple lookup.
	/// </summary>
	public class MockSettings : Settings
	{
		public override void Save() {}

		protected override T GetValue<T>(string key, T defaultValue)
		{
			string value;
			if (values.TryGetValue(key, out value))
				return value.Convert<T>();
			return defaultValue;
		}

		private readonly Dictionary<string, string> values = new Dictionary<string, string>();

		protected override void SetValue(string key, object value)
		{
			if (values.ContainsKey(key))
				values[key] = StringExtensions.ToInvariantString(value);
			else
				values.Add(key, StringExtensions.ToInvariantString(value));
		}

		public void SetAsCurrent()
		{
			Settings.Current = this;
		}

		public void Change()
		{
			wasChanged = true;
		}
	}
}