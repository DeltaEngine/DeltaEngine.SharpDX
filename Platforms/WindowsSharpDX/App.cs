using DeltaEngine.Core;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Initializes the SharpDXResolver for Windows DirectX 11. To execute the app call Run.
	/// </summary>
	public abstract class App
	{
		protected App() { }

		protected App(Window windowToRegister)
		{
			resolver.RegisterInstance(windowToRegister);
		}

		private readonly SharpDXResolver resolver = new SharpDXResolver();

		protected void Run()
		{
			resolver.Run();
		}

		protected T Resolve<T>() where T : class
		{
			return resolver.Resolve<T>();
		}
	}
}