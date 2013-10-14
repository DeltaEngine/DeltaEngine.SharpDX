using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Core;
using DeltaEngine.Extensions;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// App settings loaded from and saved to file.
	/// </summary>
	public class FileSettings : Settings
	{
		public FileSettings()
		{
			filePath = Path.Combine(AssemblyExtensions.GetMyDocumentsAppFolder(), SettingsFilename);
			if (File.Exists(filePath))
				data = new XmlFile(filePath).Root; //ncrunch: no coverage
			else
			{
				data = new XmlData("Settings");
				AppRunner.ContentIsReady += LoadDefaultSettings;
			}
		}

		private readonly string filePath;
		private XmlData data;

		private void LoadDefaultSettings()
		{
			var dataChangedBeforeLoading = data;
			data = ContentLoader.Load<XmlContent>("DefaultSettings").Data;
			if (dataChangedBeforeLoading != null)
				foreach (var child in dataChangedBeforeLoading.Children)
					SetValue(child.Name, child.Value); //ncrunch: no coverage
			wasChanged = true;
		}

		public override void Save()
		{
			new XmlFile(data).Save(filePath);
		}

		protected override T GetValue<T>(string key, T defaultValue)
		{
			return data == null ? defaultValue : data.GetChildValue(key, defaultValue);
		}

		protected override void SetValue(string key, object value)
		{
			if (data.GetChild(key) == null)
				data.AddChild(key, StringExtensions.ToInvariantString(value)); //ncrunch: no coverage
			else
				data.GetChild(key).Value = StringExtensions.ToInvariantString(value);
			wasChanged = true;
		}
	}
}