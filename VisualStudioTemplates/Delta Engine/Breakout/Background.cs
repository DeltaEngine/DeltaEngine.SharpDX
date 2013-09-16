using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering.Sprites;

namespace $safeprojectname$
{
	public class Background : Sprite
	{
		public Background() : base(new Material(Shader.Position2DColorUv, "Background"), Rectangle.One)
		{
			RenderLayer = DefaultRenderLayer;
		}
	}
}