using System.IO;

namespace DeltaEngine.Multimedia.MusicStreams
{
	public sealed class MusicStreamFactory
	{
		public BaseMusicStream Load(Stream stream, string filepath)
		{
			if (OggMusicStream.IsOggStream(stream))
				return new OggMusicStream(stream);
			if (WmaMusicStream.IsWmaStream(stream))
				return new WmaMusicStream(filepath + ".wma");
			return new Mp3MusicStream(stream);
		}
	}
}