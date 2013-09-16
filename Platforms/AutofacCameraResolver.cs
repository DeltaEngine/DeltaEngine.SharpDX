using DeltaEngine.Rendering.Cameras;

namespace DeltaEngine.Platforms
{
	internal class AutofacCameraResolver : CameraResolver
	{
		public AutofacCameraResolver(Resolver resolver)
		{
			this.resolver = resolver;
		}

		private readonly Resolver resolver;

		public Camera ResolveCamera<T>(object optionalParameter) where T : Camera
		{
			return (T)resolver.Resolve(typeof(T), optionalParameter);
		}
	}
}