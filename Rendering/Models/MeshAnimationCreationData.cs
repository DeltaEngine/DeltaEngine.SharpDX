using DeltaEngine.Content;

namespace DeltaEngine.Rendering.Models
{
	public class MeshAnimationCreationData : ContentCreationData
	{
		public MeshAnimationCreationData(float startTime, float endTime, int framesCount)
		{
			StartTime = startTime;
			EndTime = endTime;
			FramesCount = framesCount;
		}

		public int FramesCount { get; private set; }
		public float EndTime { get; private set; }
		public float StartTime { get; private set; }
	}
}