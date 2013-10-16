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
			var music = ContentLoader.Load<Music>("DefaultMusic");
			music.Play();
		}

		[Test]
		public void PlayMusicAndVideo()
		{
			MockSoundDevice device = new MockSoundDevice();
			Assert.IsTrue(device.IsInitialized);
			var music1 = ContentLoader.Load<Music>("DefaultMusic");
			music1.Play();
			Assert.False(MockMusic.MusicStopCalled);
			device.RegisterCurrentMusic(music1);
			Assert.IsTrue(device.IsActive);
			Assert.IsTrue(device.IsInitialized);
			device.RapidUpdate();
			device.Dispose();
		}

		[Test]
		public void PlayAndStopMusic()
		{
			MockSoundDevice device = new MockSoundDevice();
			Assert.IsTrue(device.IsPauseable);
			Assert.IsTrue(device.IsInitialized);
			var musicTime = ContentLoader.Load<Music>("DefaultMusic");
			musicTime.StreamFinished = () => { };
			musicTime.Play();
			device.RegisterCurrentMusic(musicTime);
			musicTime.Stop();
			device.RapidUpdate();
			musicTime.Loop = true;
			musicTime.Stop();
			device.RapidUpdate();
			device.Dispose();
		}
	}
}