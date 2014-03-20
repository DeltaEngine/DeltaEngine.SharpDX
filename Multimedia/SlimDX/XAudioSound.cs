using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SlimDX.Multimedia;
using SlimDX.XAudio2;

namespace DeltaEngine.Multimedia.SlimDX
{
	/// <summary>
	/// Native SlimDX implementation of Sound.
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
			var waveStream = new WaveStream(fileData);
			format = waveStream.Format;
			length = CalculateLengthInSeconds(format, (int)waveStream.Length);
			buffer = CreateAudioBuffer(waveStream);
		}

		private WaveFormat format;
		private float length;
		private AudioBuffer buffer;

		private static AudioBuffer CreateAudioBuffer(Stream waveStream)
		{
			return new AudioBuffer
			{
				AudioData = waveStream,
				AudioBytes = (int)waveStream.Length,
				Flags = BufferFlags.EndOfStream
			};
		}

		private static float CalculateLengthInSeconds(WaveFormat format, int dataLength)
		{
			return (float)dataLength / format.BlockAlignment / format.SamplesPerSecond;
		}

		public override float LengthInSeconds
		{
			get { return length; }
		}

		protected override void DisposeData()
		{
			base.DisposeData();
			if (buffer != null)
				buffer.AudioData.Dispose();
			buffer = null;
		}

		public override void PlayInstance(SoundInstance instanceToPlay)
		{
			var soundInstance = instanceToPlay.Handle as SourceVoice;
			if (soundInstance == null)
				return;
			soundInstance.SubmitSourceBuffer(buffer);
			soundInstance.Volume = instanceToPlay.Volume;
			float left = 0.5f - instanceToPlay.Panning / 2;
			float right = 0.5f + instanceToPlay.Panning / 2;
			soundInstance.SetOutputMatrix(1, 2, new[] { left, right });
			soundInstance.FrequencyRatio = instanceToPlay.Pitch;
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
			var source = new SourceVoice(xAudio, format);
			source.StreamEnd += (sender, args) => instancesPlaying.Remove(instanceToFill);
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