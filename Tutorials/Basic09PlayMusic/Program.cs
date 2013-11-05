using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Multimedia;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Tutorials.Basic09PlayMusic
{
	public class Program : App
	{
		public Program()
		{
			var music = ContentLoader.Load<Music>("DefaultMusic");
			new FontText(Font.Default, "Click to play music", Rectangle.HalfCentered);
			new Command(Command.Click, () => music.Play());
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}