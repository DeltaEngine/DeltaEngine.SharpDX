using DeltaEngine.Core;
using DeltaEngine.Platforms;

namespace DeltaEngine.Editor.Frameworks
{
	public sealed class BlockingViewportApp
	{
		public BlockingViewportApp(Window window)
		{
			resolver.RegisterInstance(window);
		}

		private readonly SharpDXResolver resolver = new SharpDXResolver();

		public void RunAndBlock()
		{
			resolver.Run();
			resolver.Dispose();
		}

		public T Resolve<T>() where T : class
		{
			return resolver.Resolve<T>();
		}
	}
}