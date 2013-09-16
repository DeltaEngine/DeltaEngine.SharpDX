using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Mocks
{
	internal class MockImage : Image
	{
		protected MockImage(string contentName)
			: base(contentName) {}

		public MockImage(ImageCreationData creationData)
			: base(creationData) { }

		protected override void LoadImage(Stream fileData)
		{
			if (!AllowCreationIfContentNotFound)
				return; //ncrunch: no coverage
			DisposeData();
			CreateDefault();
			Fill(new byte[0]);
		}

		public override void Fill(Color[] colors) {}

		public override void Fill(byte[] colors) {}

		protected override void SetSamplerState() {}

		protected override void DisposeData() { }

		public void CheckAlphaIsCorrect(bool hasAlpha)
		{
			WarnAboutWrongAlphaFormat(hasAlpha);
		}
	}
}