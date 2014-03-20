using System.Collections.Generic;
using System.IO;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System.Diagnostics;
using System;

namespace DeltaEngine.Multimedia.SharpDX
{
	/// <summary>
	/// Native SharpDX implementation of Sound.
	/// </summary>
	public class XAudioSound : Sound
	{
		protected XAudioSound(string contentName, XAudioDevice device)
			: base(contentName)
		{
			xAudio = device.XAudio;
		}

		private readonly XAudio2 xAudio;

		protected override void LoadData(Stream fileData)
		{
			try
			{
				TryLoadData(fileData);
			}
			catch (Exception ex)
			{
				if (Debugger.IsAttached)
					throw new SoundNotFoundOrAccessible(Name, ex);
			}
		}

		private void TryLoadData(Stream fileData)
		{
			var soundStream = new SoundStream(fileData);
			format = soundStream.Format;
			length = CalculateLengthInSeconds(format, (int)soundStream.Length);
			buffer = CreateAudioBuffer(soundStream.ToDataStream());
			decodedInfo = soundStream.DecodedPacketsInfo;
		}

		private WaveFormat format;
		private float length;
		private AudioBuffer buffer;
		private uint[] decodedInfo;

		private static AudioBuffer CreateAudioBuffer(DataStream dataStream)
		{
			return new AudioBuffer
			{
				Stream = dataStream,
				AudioBytes = (int)dataStream.Length,
				Flags = BufferFlags.EndOfStream
			};
		}

		private static float CalculateLengthInSeconds(WaveFormat format, int dataLength)
		{
			return (float)dataLength / format.BlockAlign / format.SampleRate;
		}

		public override float LengthInSeconds
		{
			get { return length; }
		}

		protected override void DisposeData()
		{
			base.DisposeData();
			if (buffer != null)
				buffer.Stream.Dispose();
			buffer = null;
		}

		public override void PlayInstance(SoundInstance instanceToPlay)
		{
			var soundInstance = instanceToPlay.Handle as SourceVoice;
			if (soundInstance == null)
				return;
			soundInstance.SubmitSourceBuffer(buffer, decodedInfo);
			soundInstance.SetVolume(instanceToPlay.Volume);
			float left = 0.5f - instanceToPlay.Panning / 2;
			float right = 0.5f + instanceToPlay.Panning / 2;
			soundInstance.SetOutputMatrix(1, 2, new[] { left, right });
			soundInstance.SetFrequencyRatio(instanceToPlay.Pitch);
			soundInstance.Start();
			instancesPlaying.Add(instanceToPlay);
		}

		private readonly List<SoundInstance> instancesPlaying = new List<SoundInstance>();

		public override void StopInstance(SoundInstance instanceToStop)
		{
			var soundInstance = instanceToStop.Handle as SourceVoice;
			if (soundInstance != null)
				soundInstance.Stop();
			if (instancesPlaying.Contains(instanceToStop))
				instancesPlaying.Remove(instanceToStop);
		}

		protected override void CreateChannel(SoundInstance instanceToFill)
		{
			if (buffer == null || xAudio == null)
				return;
			var source = new SourceVoice(xAudio, format, true);
			source.StreamEnd += () => instancesPlaying.Remove(instanceToFill);
			instanceToFill.Handle = source;
		}

		protected override void RemoveChannel(SoundInstance instanceToRemove)
		{
			var soundInstance = instanceToRemove.Handle as SourceVoice;
			if (soundInstance != null)
			{
				soundInstance.Stop();
				soundInstance.Dispose();
			}
			instanceToRemove.Handle = null;
		}

		public override bool IsPlaying(SoundInstance instance)
		{
			return instancesPlaying.Contains(instance);
		}
	}
}