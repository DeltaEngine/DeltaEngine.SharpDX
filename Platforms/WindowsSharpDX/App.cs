using System;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Initializes the SharpDXResolver for Windows DirectX 11. To execute the app call Run.
	/// </summary>
	public abstract class App
	{
		protected void Run()
		{
			resolver.Run();
		}

		private readonly SharpDXResolver resolver = new SharpDXResolver();

		protected T Resolve<T>() where T : class
		{
			return resolver.Resolve<T>();
		}
	}
}