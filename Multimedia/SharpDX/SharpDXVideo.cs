using System;
using System.Diagnostics;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Multimedia.VideoStreams;
using DeltaEngine.Rendering2D;
using DeltaEngine.ScreenSpaces;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace DeltaEngine.Multimedia.SharpDX
{
	/// <summary>
	/// SharpDX implementation of the Video content class.
	/// </summary>
	public class SharpDXVideo : Video
	{
		protected SharpDXVideo(string contentName, SoundDevice soundDevice)
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
				source = new SourceVoice((device as XAudioDevice).XAudio2,
					new WaveFormat(video.Samplerate, 16, video.Channels), false);
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
			source.DestroyVoice();
			source.Dispose();
			source = null;
		}

		protected override void PlayNativeVideo(float volume)
		{
			video.Rewind();
			source.Start();
			isPlaying = true;
			elapsedSeconds = 0f;
			if (image == null)
				image =
					ContentLoader.Create<Image>(new ImageCreationData(new Size(video.Width, video.Height)));
			surface = new Sprite(new Material(ContentLoader.Load<Shader>(Shader.Position2DUV), image),
				ScreenSpace.Current.Viewport);
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
			if (surface != null)
				surface.IsActive = false;
			surface = null;
			video.Stop();
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

		private unsafe void PutInStreamIfDataAvailable()
		{
			AudioBuffer currentBuffer = buffers[nextBufferIndex];
			try
			{
				byte[] bufferData = new byte[4096];
				video.ReadMusicBytes(bufferData, bufferData.Length);
				fixed (byte* ptr = &bufferData[0])
					currentBuffer.AudioDataPointer = (IntPtr)ptr;
				
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
			source.SubmitSourceBuffer(currentBuffer, null);
			nextBufferIndex = (nextBufferIndex + 1) % NumberOfBuffers;
		}

		private int nextBufferIndex;
		private bool isAbleToStream;

		private void UpdateVideoTexture()
		{
			byte[] bytes = video.ReadImageRgbaColors(Time.Delta);
			if (bytes != null)
				image.Fill(bytes);
			else
				Stop();
		}

		public override float DurationInSeconds
		{
			get { return video.LengthInSeconds; }
		}

		public override float PositionInSeconds
		{
			get { return MathExtensions.Round(elapsedSeconds.Clamp(0f, DurationInSeconds), 2); }
		}
	}
}