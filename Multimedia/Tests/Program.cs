namespace DeltaEngine.Multimedia.Tests
{
	internal static class Program
	{
		//ncrunch: no coverage start
		public static void Main()
		{
			//var sounds = new SoundTests();
			//sounds.InitializeResolver();
			//sounds.PlaySoundOnClick();
			//sounds.RunTestAndDisposeResolverWhenDone();

			//var music = new MusicTests();
			//music.InitializeResolver();
			//music.PlayMusic();
			//music.PlayMusicOnClick();
			//music.RunTestAndDisposeResolverWhenDone();

			var video = new VideoTests();
			video.InitializeResolver();
			//video.PlayVideo();
			video.PlayVideoOnClick();
			video.RunTestAndDisposeResolverWhenDone();
		}
	}
}