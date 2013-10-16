using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Tests.Content
{
	internal class FakeImageContentLoader : ContentLoader
	{
		private FakeImageContentLoader()
		{
			contentPath = "NoPath";
		}

		public override ContentMetaData GetMetaData(string contentName,
			Type contentClassType = null)
		{
			if (MetaData != null)
				return MetaData;
			MetaData = new ContentMetaData { Type = ContentType.Image };
			MetaData.Values.Add("ImageName", "DeltaEngineLogo");
			MetaData.Values.Add("UV", Rectangle.One.ToString());
			MetaData.Values.Add("PadLeft", "0.5");
			MetaData.Values.Add("PadRight", "0.5");
			MetaData.Values.Add("PadTop", "0.5");
			MetaData.Values.Add("PadBottom", "0.5");
			MetaData.Values.Add("Rotated", "false");
			return MetaData;
		}

		private ContentMetaData MetaData;

		protected override bool HasValidContentAndMakeSureItIsLoaded()
		{
			return true;
		}
	}
}