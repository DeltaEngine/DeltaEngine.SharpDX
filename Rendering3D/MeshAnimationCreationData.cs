using DeltaEngine.Content;

namespace DeltaEngine.Rendering3D
{
	/// <summary>
	/// For creating MeshAnimations without loading through the content system
	/// </summary>
	public class MeshAnimationCreationData : ContentCreationData
	{
		public MeshAnimationCreationData(float startTime, float endTime, int framesCount)
		{
			StartTime = startTime;
			EndTime = endTime;
			FramesCount = framesCount;
		}

		public float StartTime { get; private set; }
		public float EndTime { get; private set; }
		public int FramesCount { get; private set; }
	}
}