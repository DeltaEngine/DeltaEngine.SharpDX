using System.IO;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering.Models;

namespace DeltaEngine.Rendering.Mocks
{
	public class MockMeshAnimation : MeshAnimation
	{
		public MockMeshAnimation(string contentName)
			: base(contentName) {}

		public MockMeshAnimation(MeshAnimationCreationData creationData)
			: base(creationData) {}

		protected override void LoadData(Stream fileData)
		{
			Frames = new MeshAnimationFrame[MockFrameCount];
			Frames[0] = new MeshAnimationFrame();
			Frames[0].JointPoses = new Matrix[MockJointsPerFrame];
			Frames[0].JointPoses[0] = Matrix.Identity;
			Frames[0].JointPoses[1] = Matrix.CreateRotationY(45.0f);
		}

		private const int MockFrameCount = 1;
		private const int MockJointsPerFrame = 2;
	}
}