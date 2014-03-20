using System;
using System.Diagnostics;
using System.IO;
using DeltaEngine.Extensions;
using DeltaEngine.Multimedia.MusicStreams;
using SlimDX.Multimedia;
using SlimDX.XAudio2;

namespace DeltaEngine.Multimedia.SlimDX
{
	/// <summary>
	/// Native XAudio implementation for music playback.
	/// </summary>
	public class XAudioMusic : Music
	{
		protected XAudioMusic(string contentName, XAudioDevice device)
			: base(contentName, device)
		{
			CreateBuffers();
		}

		protected override void LoadData(Stream fileData)
		{
			try
			{
				TryLoadData(fileData);
			}
			catch (Exception ex)
			{
				if (Debugger.IsAttached)
					throw new CouldNotLoadMusicFromFilestream(Name, ex);
			}
		}

		private void TryLoadData(Stream fileData)
		{
			if ((device as XAudioDevice).XAudio == null)
				return;
			var stream = new MemoryStream();
			fileData.CopyTo(stream);
			stream.Position = 0;
			musicStream = new MusicStreamFactory().Load(stream);
			source = new SourceVoice((device as XAudioDevice).XAudio,
				new WaveFormat
				{
					SamplesPerSecond = musicStream.Samplerate,
					BitsPerSample = 16,
					Channels = (short)musicStream.Channels,
					AverageBytesPerSecond = (musicStream.Samplerate / 16) / 8,
					BlockAlignment = (short)(2 * musicStream.Channels),
					FormatTag = WaveFormatTag.Pcm
				});
		}

		private BaseMusicStream musicStream;
		private SourceVoice source;

		private void CreateBuffers()
		{
			buffers = new StreamBuffer[NumberOfBuffers];
			for (int i = 0; i < NumberOfBuffers; i++)
				buffers[i] = new StreamBuffer();
		}

		private StreamBuffer[] buffers;

		protected override void PlayNativeMusic()
		{
			if (musicStream == null || source == null)
				return;
			musicStream.Rewind();
			source.Start();
			isPlaying = true;
			playStartTime = DateTime.Now;
		}

		private DateTime playStartTime;
		private bool isPlaying;

		protected override void SetPlayingVolume(float value)
		{
			if (source != null)
				source.Volume = value;
		}

		public override void Run()
		{
			while (source != null && source.State.BuffersQueued < NumberOfBuffers)
			{
				PutInStreamIfDataAvailable();
				if (isAbleToStream)
					continue;
				HandleStreamFinished();
				break;
			}
		}

		protected override void StopNativeMusic()
		{
			isPlaying = false;
			if (source == null)
				return;
			source.Stop();
			source.FlushSourceBuffers();
		}
		
		public override bool IsPlaying()
		{
			return isPlaying;
		}

		private void PutInStreamIfDataAvailable()
		{
			StreamBuffer currentBuffer = buffers[nextBufferIndex];
			isAbleToStream = currentBuffer.FillFromStream(musicStream);
			if (!isAbleToStream || source == null)
				return;
			source.SubmitSourceBuffer(currentBuffer.XAudioBuffer);
			nextBufferIndex = (nextBufferIndex + 1) % NumberOfBuffers;
		}

		private int nextBufferIndex;
		private bool isAbleToStream;

		protected override void DisposeData()
		{
			if (musicStream == null)
				return;
			base.DisposeData();
			musicStream = null;
			if (source != null)
				DisposeSource();
		}

		private void DisposeSource()
		{
			for (int i = 0; i < NumberOfBuffers; i++)
				buffers[i].Dispose();
			source.FlushSourceBuffers();
			source.Dispose();
			source = null;
		}

		public override float DurationInSeconds
		{
			get { return musicStream.LengthInSeconds; }
		}

		public override float PositionInSeconds
		{
			get
			{
				float seconds = (float)DateTime.Now.Subtract(playStartTime).TotalSeconds;
				return seconds.Clamp(0f, DurationInSeconds).Round(2);
			}
		}
	}
}