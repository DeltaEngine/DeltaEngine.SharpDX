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
		}

		private Music currentPlayingMusic;

		public bool IsPauseable { get { return true; } }

		public void RegisterCurrentMusic(Music music)
		{
			if (IsStopNeededForRegister(music, currentPlayingMusic))
				currentPlayingMusic.Stop();
			currentPlayingMusic = music;
		}

		private static bool IsStopNeededForRegister<T>(T newInstance, T currentInstance)
			where T : class
		{
			return newInstance != null && currentInstance != null;
		}

		public override void Dispose()
		{
			if (currentPlayingMusic != null)
				currentPlayingMusic.Dispose();
			currentPlayingMusic = null;
		}
	}
}