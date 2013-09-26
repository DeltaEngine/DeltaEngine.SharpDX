using DeltaEngine.Content;
using DeltaEngine.Multimedia.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Multimedia.Tests
{
	public class SoundDeviceTests : TestWithMocksOrVisually
	{
		[Test]
		public void PlayMusicWhileOtherIsPlaying()
		{
			var music1 = ContentLoader.Load<Music>("DefaultMusic");
			var music2 = ContentLoader.Load<Music>("DefaultMusic");
			music1.Play();
			Assert.False(MockMusic.MusicStopCalled);
			music2.Play();
			Assert.False(MockMusic.MusicStopCalled);
		}

		[Test]
		public void RunWithVideoAndMusic()
		{
			var video = ContentLoader.Load<Video>("DefaultVideo");
			var music = ContentLoader.Load<Music>("DefaultMusic");
			video.Play();
			music.Play();
		}

		[Test]
		public void TestIfPLayingMusic()
		{
			var video = ContentLoader.Load<Video>("DefaultVideo");
			video.Play();
			Assert.IsTrue(video.IsPlaying());
			Assert.AreEqual(3.33333325f, video.DurationInSeconds);
			Assert.AreEqual(1.0f, video.PositionInSeconds);
		}

		[Test]
		public void PlayMusicAndVideo()
		{
			MockSoundDevice device = new MockSoundDevice();
			Assert.IsTrue(device.IsInitialized);
			var video1 = ContentLoader.Load<Video>("DefaultVideo");
			var music1 = ContentLoader.Load<Music>("DefaultMusic");
			music1.Play();
			video1.Play();
			Assert.False(MockVideo.VideoStopCalled);
			Assert.False(MockMusic.MusicStopCalled);
			device.RegisterCurrentVideo(video1);
			device.RegisterCurrentMusic(music1);
			Assert.IsTrue(device.IsActive);
			Assert.IsTrue(device.IsInitialized);
			device.RapidUpdate();
			device.Dispose();
		}
	}
}