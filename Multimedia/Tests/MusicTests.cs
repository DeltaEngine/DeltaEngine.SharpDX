using System.Diagnostics;
using System.Threading;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using NUnit.Framework;

namespace DeltaEngine.Multimedia.Tests
{
	/// <summary>
	/// Test music playback. Xna music loading won't work from ReSharper, use Program.cs instead.
	/// </summary>
	public class MusicTests : TestWithMocksOrVisually
	{
		[Test]
		public void PlayMusicSettingsVolume()
		{
			ContentLoader.Load<Music>("DefaultMusic").Play();
		}

		[Test]
		public void PlayMusicDifferingVolume()
		{
			ContentLoader.Load<Music>("DefaultMusic").Play(0.5f);			
		}

		[Test]
		public void PlayMusicLoop()
		{
			var music = ContentLoader.Load<Music>("DefaultMusic");
			music.Loop = true;
			music.Play();
		}

		[Test]
		public void TestIfPLayingMusic()
		{
			var music = ContentLoader.Load<Music>("DefaultMusic");
			music.Play();
			Assert.IsTrue(music.IsPlaying());
			AssertBetween(4.10f, 4.15f, music.DurationInSeconds);
		}

		private static void AssertBetween(float min, float max, float value)
		{
			Assert.GreaterOrEqual(value, min);
			Assert.LessOrEqual(value, max);
		}

		[Test, Ignore]
		public void PlayMusicOnClick()
		{
			new FontText(Font.Default, "Click to Play", Rectangle.One);
			var music = ContentLoader.Load<Music>("DefaultMusic");
			new Command(() => { music.Play(1); }).Add(new MouseButtonTrigger());
		}

		[Test, Ignore]
		public void PlayMusicWith5Fps()
		{
			var music = ContentLoader.Load<Music>("DefaultMusic");
			music.Play();
			new SleepEntity(5);
		}

		[Test, Ignore]
		public void PlayMusicWith10Fps()
		{
			var music = ContentLoader.Load<Music>("DefaultMusic");
			music.Play();
			new SleepEntity(10);
		}

		[Test, Ignore]
		public void PlayMusicWith30Fps()
		{
			var music = ContentLoader.Load<Music>("DefaultMusic");
			music.Play();
			new SleepEntity(30);
		}

		private class SleepEntity : Entity, Updateable
		{
			public SleepEntity(int fps)
			{
				timeout = 1000 / fps;
			}

			private readonly int timeout;

			public void Update()
			{
				Thread.Sleep(timeout);
			}

			public bool IsPauseable { get { return true; } }
		}

		[Test, Ignore]
		public void StartAndStopMusic()
		{
			var music = ContentLoader.Load<Music>("DefaultMusic");
			AssertBetween(4.10f, 4.15f, music.DurationInSeconds);
			new MusicPlayedOneSecondTester(music);
			music.Play();
			Assert.IsTrue(music.IsPlaying());
		}

		private class MusicPlayedOneSecondTester : Entity, Updateable
		{
			public MusicPlayedOneSecondTester(Music music)
			{
				this.music = music;
			}

			private readonly Music music;

			public void Update()
			{
				if (Time.Total < 1)
					return;
				music.Stop();
				Assert.IsFalse(music.IsPlaying());
				Assert.Less(0.99f, music.PositionInSeconds);
			}

			public bool IsPauseable { get { return true; } }
		}

		[Test, Ignore]
		public void ShouldThrowIfMusicNotLoadedInDebugModeOrWithDebuggerAttached()
		{
			if (!Debugger.IsAttached)
				return;
			//ncrunch: no coverage start
			Assert.Throws<ContentLoader.ContentNotFound>(
				() => ContentLoader.Load<Music>("UnavailableMusic"));
		}
	}
}