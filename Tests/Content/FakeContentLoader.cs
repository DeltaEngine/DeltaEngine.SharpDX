using System;
using DeltaEngine.Content;

namespace DeltaEngine.Tests.Content
{
	internal class FakeContentLoader : ContentLoader
	{
		private FakeContentLoader()
		{
			contentPath = "NoPath";
		}

		public override ContentMetaData GetMetaData(string contentName,
			Type contentClassType = null)
		{
			HasValidContentAndMakeSureItIsLoaded();
			var metaData = new ContentMetaData { Type = ContentType.Xml };
			if (contentName.Contains("WrongPath"))
				metaData.LocalFilePath = "No.xml";
			return metaData;
		}

		protected override bool HasValidContentAndMakeSureItIsLoaded()
		{
			return ContentMetaDataFilePath == null;
		}
	}
}