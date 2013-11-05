using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;

namespace DeltaEngine.Tutorials.Entities05Tags
{
	public class Program : App
	{
		public Program()
		{
			new Earth(new Vector2D(0.3f, 0.5f)).AddTag("EarthToggle");
			new Earth(new Vector2D(0.5f, 0.5f)).AddTag("EarthToggle");
			new Earth(new Vector2D(0.7f, 0.5f)).AddTag("EarthNormal");
			new Command(Command.Click, () =>
			{
				foreach (Earth entity in EntitiesRunner.Current.GetEntitiesWithTag("EarthToggle"))
					entity.IsPlaying = !entity.IsPlaying;
			});
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