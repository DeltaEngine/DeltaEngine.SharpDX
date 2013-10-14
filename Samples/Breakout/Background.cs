using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;

namespace Breakout
{
	/// <summary>
	/// Just renders the background graphic
	/// </summary>
	public class Background : Sprite
	{
		public Background()
			: base(new Material(Shader.Position2DColorUV, "Background"), Rectangle.One)
		{
			RenderLayer = DefaultRenderLayer;
		}
	}
}