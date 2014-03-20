using DeltaEngine.Core;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Initializes the SlimDXResolver for Windows DirectX 9. To execute the app call Run.
	/// </summary>
	public abstract class App
	{
		protected App() { }

		protected App(Window windowToRegister)
		{
			resolver.RegisterInstance(windowToRegister);
		}

		private readonly SlimDXResolver resolver = new SlimDXResolver();

		protected void Run()
		{
			resolver.Run();
			resolver.Dispose();
		}

		protected T Resolve<T>() where T : class
		{
			return resolver.Resolve<T>();
		}
	}
}