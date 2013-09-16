using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Models.Tests
{
	class MeshAnimationTests : TestWithMocksOrVisually
	{
		private static MeshAnimation CreateOneSecondTenFramesAnimation()
		{
			return ContentLoader.Create<MeshAnimation>(new MeshAnimationCreationData(0.0f, 1.0f, 10));
		}

		[Test, CloseAfterFirstFrame]
		public void CreateAnimation()
		{
			var animation = CreateOneSecondTenFramesAnimation();
			Assert.AreEqual(0.0f, animation.StartTime);
			Assert.AreEqual(1.0f, animation.EndTime);
			Assert.AreEqual(1.0f, animation.Duration);
			Assert.AreEqual(10, animation.FramesCount);
		}

		[Test]
		public void LoadAnimation()
		{
			var animation = ContentLoader.Load<MeshAnimation>("TwoBonesAnimation");
			Assert.AreEqual(1, animation.FramesCount);
			Assert.AreEqual(2, animation.Frames[0].JointPoses.Length);
			Assert.AreEqual(Matrix.Identity, animation.Frames[0].JointPoses[0]);
			Assert.AreEqual(Matrix.CreateRotationY(45.0f), animation.Frames[0].JointPoses[1]);
		}
	}
}