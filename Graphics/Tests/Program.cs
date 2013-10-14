namespace DeltaEngine.Graphics.Tests
{
	public class Program
	{
		public static void Main()
		{
			var tests = new DeviceTests();
			tests.InitializeResolver();
			//ContentLoader.Use<DiskContentLoader>();
			tests.DrawRedBackground();
			//new DrawingTests().DrawRedLine();
			//new DrawingTests().DrawVertices();
			//new ImageTests().DrawImage();
			//new MeshTests().DrawRotatingIceTower();
			tests.RunTestAndDisposeResolverWhenDone();
		}
	}
}