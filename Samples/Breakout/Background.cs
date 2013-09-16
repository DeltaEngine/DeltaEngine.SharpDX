using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering.Sprites;

namespace Breakout
{
	/// <summary>
	/// Just renders the background graphic
	/// </summary>
	public class Background : Sprite
	{
		public Background()
			: base(new Material(Shader.Position2DColorUv, "Background"), Rectangle.One)
		{
			RenderLayer = DefaultRenderLayer;
		}
	}
}