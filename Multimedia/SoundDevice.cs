using System;
using DeltaEngine.Entities;

namespace DeltaEngine.Multimedia
{
	/// <summary>
	/// Holds the audio device and automatically disposes all finished playing sound instances.
	/// </summary>
	public abstract class SoundDevice : Entity, RapidUpdateable, IDisposable
	{
		public virtual void RapidUpdate()
		{
			if (currentPlayingMusic != null)
				currentPlayingMusic.Run();
			if (currentPlayingVideo != null)
				currentPlayingVideo.Update();
		}

		private Music currentPlayingMusic;
		private Video currentPlayingVideo;

		public void RegisterCurrentMusic(Music music)
		{
			if (music != null && currentPlayingMusic != null)
				currentPlayingMusic.Stop();
			currentPlayingMusic = music;
		}

		public void RegisterCurrentVideo(Video video)
		{
			if (video != null && currentPlayingVideo != null)
				currentPlayingVideo.Stop();
			currentPlayingVideo = video;
		}

		public virtual void Dispose()
		{
			if (currentPlayingMusic != null)
				currentPlayingMusic.Dispose();
			currentPlayingMusic = null;
			if (currentPlayingVideo != null)
				currentPlayingVideo.Dispose();
			currentPlayingVideo = null;
		}
	}
}