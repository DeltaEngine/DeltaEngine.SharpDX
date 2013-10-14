using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	/// <summary>
	/// A simple tilemap with colored deltaengine logos
	/// </summary>
	public class ColoredLogoTilemap : Tilemap
	{
		public ColoredLogoTilemap(Size world, Size map)
			: base(world, map)
		{
			var data = Get<Data>();
			var logo = new Material(Shader.Position2DColorUV, "DeltaEngineLogo");
			CreateWorld(data, logo);
			CreateMap(data, logo);
		}

		private static void CreateWorld(Data data, Material logo)
		{
			for (int x = 0; x < data.WorldWidth; x++)
				for (int y = 0; y < data.WorldHeight; y++)
					data.World[x, y] = new RainbowTile(logo,
						new Color(Rainbow(x, data.WorldWidth), Rainbow(y, data.WorldHeight),
							Rainbow(x + y, data.WorldWidth)));
		}

		private static float Rainbow(int value, int max)
		{
			return ((8.0f * value) % max) / max;
		}

		private static void CreateMap(Data data, Material logo)
		{
			for (int x = 0; x < data.MapWidth; x++)
				for (int y = 0; y < data.MapHeight; y++)
					data.Map[x, y] = new Sprite(logo, Rectangle.Zero);
		}

		private class RainbowTile : Tile
		{
			public RainbowTile(Material material, Color color)
			{
				Material = material;
				Color = color;
			}

			public Material Material { get; set; }
			public Color Color { get; set; }
		}
	}
}