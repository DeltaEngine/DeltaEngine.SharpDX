using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using DeltaEngine.Core;

namespace DeltaEngine.Content.Xml
{
	/// <summary>
	/// Loads Xml data via the Content system
	/// </summary>
	public class XmlContent : ContentData
	{
		protected XmlContent(string contentName)
			: base(contentName) {}

		public XmlData Data { get; set; }

		protected override void LoadData(Stream fileData)
		{
			try
			{
				Data = new XmlData(XDocument.Load(fileData).Root);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				if (Debugger.IsAttached)
					throw new XmlContentNotFound(Name, ex); //ncrunch: no coverage
				Data = new XmlData(Name);
			}
		}

		//ncrunch: no coverage start
		public class XmlContentNotFound : Exception
		{
			public XmlContentNotFound(string contentName, Exception innerException)
				: base(contentName, innerException) {}
		}
		//ncrunch: no coverage end

		protected override void DisposeData() {}
	}
}