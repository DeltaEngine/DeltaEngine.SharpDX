using System.IO;
using DeltaEngine.Core;

namespace DeltaEngine.Multimedia.Mocks
{
	/// <summary>
	/// Mocks music to speed up unit testing of multimedia.
	/// </summary>
	public class MockMusic : Music
	{
		protected MockMusic(string contentName, SoundDevice device, Settings settings)
			: base(contentName, device, settings) {}

		protected override void LoadData(Stream fileData) {}

		protected override void PlayNativeMusic(float volume)
		{
			MusicStopCalled = false;
		}

		public static bool MusicStopCalled { get; private set; }

		public override void Run() {}

		protected override void StopNativeMusic()
		{
			MusicStopCalled = true;
		}

		public override bool IsPlaying()
		{
			return !MusicStopCalled;
		}

		public override float DurationInSeconds
		{
			get { return 4.13f; }
		}

		public override float PositionInSeconds
		{
			get { return 1.0f; }
		}
	}
}