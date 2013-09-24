using DeltaEngine.Content;
using DeltaEngine.Content.Disk;

namespace DeltaEngine.Rendering3D.Particles.Tests
{
	public class Program
	{
		public static void Main()
		{
			ContentLoader.Use<DiskContentLoader>();
			var test = new Particle3DEmitterTests();
			test.InitializeResolver();
			test.CreateCamera();
			test.SmokeAndWind();
			test.RunTestAndDisposeResolverWhenDone();
		}
	}
}
