using System.IO;

namespace DeltaEngine.Multimedia.MusicStreams
{
	public sealed class MusicStreamFactory
	{
		public BaseMusicStream Load(Stream stream, string filepath)
		{
			if (OggMusicStream.IsOggStream(stream))
				return new OggMusicStream(stream);
			return new Mp3MusicStream(stream);
		}
	}
}