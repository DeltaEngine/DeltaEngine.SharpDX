using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;

namespace DeltaEngine.Tutorials.Entities01OwnEntity
{
	public class Program : App
	{
		public Program()
		{
			new Earth(new Vector2D(0.4f, 0.5f));
			var secondEarth = new Earth(new Vector2D(0.6f, 0.5f));
			new Command(Command.Click, () => secondEarth.IsPlaying = !secondEarth.IsPlaying);
		}

		public class Earth : Sprite
		{
			public Earth(Vector2D position)
				: base(ContentLoader.Load<Material>("Earth"), position) {}
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}