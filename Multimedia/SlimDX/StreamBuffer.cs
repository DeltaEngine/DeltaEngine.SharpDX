using System;
using System.IO;
using DeltaEngine.Multimedia.MusicStreams;
using SlimDX.XAudio2;

namespace DeltaEngine.Multimedia.SlimDX
{
	/// <summary>
	/// Helper class wrapping an XAudio buffer object and streaming the next data into the buffer.
	/// </summary>
	internal class StreamBuffer : IDisposable
	{
		public StreamBuffer()
		{
			XAudioBuffer = new AudioBuffer();
			byteBuffer = new byte[BufferSize];
			bufferStream = new MemoryStream();
		}

		public AudioBuffer XAudioBuffer { get; private set; }
		private byte[] byteBuffer;
		private const int BufferSize = 4096 * 8;
		private readonly MemoryStream bufferStream;

		public bool FillFromStream(BaseMusicStream stream)
		{
			if (stream == null)
				return false;
			try
			{
				return TryFillFromStream(stream);
			}
			catch (Exception)
			{
				return false;
			}
		}

		private bool TryFillFromStream(BaseMusicStream stream)
		{
			int size = stream.Read(byteBuffer, BufferSize);
			if (size <= 0)
				return false;
			bufferStream.Position = 0;
			bufferStream.Write(byteBuffer, 0, size);
			bufferStream.Position = 0;
			XAudioBuffer.AudioData = bufferStream;
			XAudioBuffer.AudioBytes = size;
			int blockAlign = stream.Channels * 2;
			XAudioBuffer.PlayLength = size / blockAlign;
			return true;
		}

		public void Dispose()
		{
			XAudioBuffer = null;
			byteBuffer = null;
		}
	}
}