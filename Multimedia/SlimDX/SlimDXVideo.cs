using System;
using System.Diagnostics;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics.SlimDX;
using DeltaEngine.Multimedia.VideoStreams;
using DeltaEngine.Rendering2D;
using DeltaEngine.ScreenSpaces;
using SlimDX.Multimedia;
using SlimDX.XAudio2;

namespace DeltaEngine.Multimedia.SlimDX
{
	public class SlimDXVideo : Video
	{
		protected SlimDXVideo(string contentName, SoundDevice soundDevice,
			SlimDXDevice graphicsDevice)
			: base(contentName, soundDevice)
		{
			CreateBuffers();
		}

		private SourceVoice source;

		private void CreateBuffers()
		{
			buffers = new AudioBuffer[NumberOfBuffers];
			for (int i = 0; i < NumberOfBuffers; i++)
				buffers[i] = new AudioBuffer();
		}

		private AudioBuffer[] buffers;
		private const int NumberOfBuffers = 2;

		protected override void LoadData(Stream fileData)
		{
			try
			{
				video = new VideoStreamFactory().Load(fileData, "Content/" + Name);
				source = new SourceVoice((device as XAudioDevice).XAudio,
					new WaveFormat
					{
						SamplesPerSecond = video.Samplerate,
						BitsPerSample = 16,
						Channels = (short)video.Channels,
						BlockAlignment = (short)(2 * video.Channels)
					});
			}
			catch (Exception ex)
			{
				if (Debugger.IsAttached)
					throw new VideoNotFoundOrAccessible(Name, ex);
			}
		}

		private BaseVideoStream video;

		protected override void DisposeData()
		{
			base.DisposeData();
			video.Dispose();
			video = null;
			if (source != null)
				DisposeSource();
		}

		private void DisposeSource()
		{
			for (int i = 0; i < NumberOfBuffers; i++)
				buffers[i] = null;

			source.FlushSourceBuffers();
			source.Dispose();
			source = null;
		}

		protected override void PlayNativeVideo(float volume)
		{
			video.Rewind();
			source.Start();
			isPlaying = true;
			if (image == null)
				image =
					ContentLoader.Create<Image>(new ImageCreationData(new Size(video.Width, video.Height)));
			var shader =
				ContentLoader.Create<Shader>(new ShaderCreationData(ShaderFlags.Position2DTextured));
			surface = new Sprite(new Material(shader, image), ScreenSpace.Current.Viewport);
		}

		private Image image;
		private bool isPlaying;
		private Sprite surface;
		private float elapsedSeconds;

		protected override void StopNativeVideo()
		{
			isPlaying = false;
			source.Stop();
			source.FlushSourceBuffers();
			elapsedSeconds = 0f;
			if (surface != null)
				surface.IsActive = false;
			video.Stop();
			surface = null;
		}

		public override bool IsPlaying()
		{
			return isPlaying;
		}

		public override void Update()
		{
			if (isPlaying)
				RunIfPlaying();
		}

		private void RunIfPlaying()
		{
			while (source.State.BuffersQueued < NumberOfBuffers)
			{
				PutInStreamIfDataAvailable();
				if (isAbleToStream)
					continue;
				Stop();
				return;
			}
			elapsedSeconds += Time.Delta;
			UpdateVideoTexture();
		}

		private void PutInStreamIfDataAvailable()
		{
			AudioBuffer currentBuffer = buffers[nextBufferIndex];
			try
			{
				var bufferData = new byte[4096];
				video.ReadMusicBytes(bufferData, bufferData.Length);
				var bufferStream = new MemoryStream(bufferData);
				currentBuffer.AudioData = bufferStream;
				currentBuffer.AudioBytes = bufferData.Length;
				int blockAlign = video.Channels * 2;
				currentBuffer.PlayLength = bufferData.Length / blockAlign;
			}
			catch
			{
				isAbleToStream = false;
				return;
			}

			isAbleToStream = true;
			source.SubmitSourceBuffer(currentBuffer);
			nextBufferIndex = (nextBufferIndex + 1) % NumberOfBuffers;
		}

		private int nextBufferIndex;
		private bool isAbleToStream;

		private void UpdateVideoTexture()
		{
			byte[] rgbaColors = video.ReadImageRgbaColors(Time.Delta);
			if (rgbaColors != null)
				image.FillRgbaData(rgbaColors);
			else
				Stop();
		}

		public override float DurationInSeconds
		{
			get { return video.LengthInSeconds; }
		}

		public override float PositionInSeconds
		{
			get { return elapsedSeconds.Clamp(0.0f, DurationInSeconds).Round(2); }
		}
	}
}