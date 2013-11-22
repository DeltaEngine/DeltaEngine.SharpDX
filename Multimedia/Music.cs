using System;
using DeltaEngine.Content;
using DeltaEngine.Core;

namespace DeltaEngine.Multimedia
{
	/// <summary>
	/// Provides a way to load and play music files. Usually setup in Scenes.
	/// </summary>
	public abstract class Music : ContentData
	{
		protected Music(string contentName, SoundDevice device)
			: base(contentName)
		{
			this.device = device;
			Loop = false;
		}

		protected readonly SoundDevice device;

		public void Play()
		{
			device.RegisterCurrentMusic(this);
			cachedVolume = Settings.Current.MusicVolume;
			PlayNativeMusic(cachedVolume);
		}

		protected float cachedVolume;

		public void Play(float volume)
		{
			device.RegisterCurrentMusic(this);
			PlayNativeMusic(volume);
			cachedVolume = volume;
		}

		protected abstract void PlayNativeMusic(float volume);

		public void Stop()
		{
			device.RegisterCurrentMusic(null);
			StopNativeMusic();
		}

		protected abstract void StopNativeMusic();
		public abstract bool IsPlaying();
		public abstract void Run();

		public abstract float DurationInSeconds { get; }
		public abstract float PositionInSeconds { get; }
		protected const int NumberOfBuffers = 2;

		protected override void DisposeData()
		{
			Stop();
		}

		protected void HandleStreamFinished()
		{
			if (StreamFinished != null)
				StreamFinished();
			if (Loop)
				Play(cachedVolume);
			else
				Stop();
		}

		public Action StreamFinished;

		public bool Loop { protected get; set; }

		//ncrunch: no coverage start
		public class CouldNotLoadMusicFromFilestream : Exception
		{
			public CouldNotLoadMusicFromFilestream(string musicName, Exception innerException)
				: base(musicName, innerException) {}
		}
	}
}