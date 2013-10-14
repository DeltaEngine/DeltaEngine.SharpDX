using System;
using DeltaEngine.Content;
using DeltaEngine.Core;

namespace DeltaEngine.Multimedia
{
	/// <summary>
	/// Provides a way to load and play a music file.
	/// </summary>
	public abstract class Music : ContentData
	{
		protected Music(string contentName, SoundDevice device, Settings settings)
			: base(contentName)
		{
			this.device = device;
			this.settings = settings;
			Loop = false;
		}

		protected readonly SoundDevice device;
		private readonly Settings settings;

		public void Play()
		{
			device.RegisterCurrentMusic(this);
			PlayNativeMusic(settings.MusicVolume);
			cachedVolume = settings.MusicVolume;
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
		public class MusicNotFoundOrAccessible : Exception
		{
			public MusicNotFoundOrAccessible(string musicName, Exception innerException)
				: base(musicName, innerException) {}
		}
	}
}