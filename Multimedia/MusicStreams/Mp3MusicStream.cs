using System.IO;
using ToyMp3;

namespace DeltaEngine.Multimedia.MusicStreams
{
	public class Mp3MusicStream : BaseMusicStream
	{
		public Mp3MusicStream(Stream stream)
		{
			baseStream = stream;
			mp3 = new Mp3Stream(stream);
		}

		private Stream baseStream;
		private Mp3Stream mp3;

		public void Dispose()
		{
			baseStream = null;
			mp3 = null;
		}

		public int Channels
		{
			get { return mp3.Channels; }
		}

		public int Samplerate
		{
			get { return mp3.Samplerate; }
		}

		public float LengthInSeconds
		{
			get { return mp3.LengthInSeconds; }
		}

		public int Read(byte[] buffer, int length)
		{
			return mp3.Read(buffer, length);
		}

		public void Rewind()
		{
			baseStream.Position = 0;
			mp3 = new Mp3Stream(baseStream);
		}
	}
}