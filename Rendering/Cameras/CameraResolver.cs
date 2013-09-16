namespace DeltaEngine.Rendering.Cameras
{
	internal interface CameraResolver
	{
		Camera ResolveCamera<T>(object optionalParameter) where T : Camera;
	}
}