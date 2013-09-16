using System.IO;
using NUnit.Framework;

namespace DeltaEngine.Content.Disk.Tests
{
	internal class MetaDataCreatorTests
	{
		[Test, Ignore]
		public void TryCreatingAnimationFromFiles()
		{
			File.Delete(Path.Combine("Content", "ContentMetaData.xml"));
			ContentLoader.Use<DiskContentLoader>();
			Assert.IsTrue(ContentLoader.Exists("ImageAnimation", ContentType.ImageAnimation));
		}
	}
}