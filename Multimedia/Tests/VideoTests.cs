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
	/// Test video playback. Xna video loading won't work from ReSharper, use Program.cs instead.
	/// </summary>
	[Ignore]
	public class VideoTests : TestWithMocksOrVisually
	{
		[Test]
		public void PlayVideo()
		{
			ContentLoader.Load<Video>("DefaultVideo").Play();
		}

		[Test]
		public void PlayVideoOnClick()
		{
			new FontText(Font.Default, "Click to Play", Rectangle.One);
			var video = ContentLoader.Load<Video>("DefaultVideo");
			new Command(() => { video.Play(); }).Add(new MouseButtonTrigger());
		}

		[Test]
		public void PlayAndStop()
		{
			var video = ContentLoader.Load<Video>("DefaultVideo");
			video.Stop();
			Assert.IsFalse(video.IsPlaying());
			video.Play();
			Assert.IsTrue(video.IsPlaying());
		}

		[Test]
		public void PlayAndStopWithEntitiesRunner()
		{
			var video = ContentLoader.Load<Video>("DefaultVideo");
			Assert.AreEqual(0, EntitiesRunner.Current.NumberOfEntities);
			video.Stop();
			Assert.AreEqual(0, EntitiesRunner.Current.NumberOfEntities);
			video.Play();
			Assert.AreEqual(1, EntitiesRunner.Current.NumberOfEntities);
			video.Stop();
			Assert.AreEqual(0, EntitiesRunner.Current.NumberOfEntities);
		}

		[Test]
		public void CheckDurationAndPosition()
		{
			var video = ContentLoader.Load<Video>("DefaultVideo");
			video.Update();
			Assert.AreEqual(3.791f, video.DurationInSeconds, 0.5f);
			Assert.AreEqual(1.0f, video.PositionInSeconds);
		}

		[Test]
		public void StartAndStopVideo()
		{
			var video = ContentLoader.Load<Video>("DefaultVideo");
			Assert.AreEqual(3.791f, video.DurationInSeconds, 0.5f);
			new VideoPlayedOneSecondTester(video);
			video.Play();
		}

		private class VideoPlayedOneSecondTester : Entity, Updateable
		{
			public VideoPlayedOneSecondTester(Video video)
			{
				this.video = video;
			}

			private readonly Video video;

			public void Update()
			{
				if (Time.Total < 1)
					return;
				video.Stop();
				Assert.Less(0.99f, video.PositionInSeconds);
			}
		}
	}
}