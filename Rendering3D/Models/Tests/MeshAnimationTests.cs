using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Models.Tests
{
	internal class MeshAnimationTests : TestWithMocksOrVisually
	{
		private static MeshAnimation CreateOneSecondTenFramesAnimation()
		{
			return ContentLoader.Create<MeshAnimation>(new MeshAnimationCreationData(0.0f, 1.0f, 10));
		}

		[Test, CloseAfterFirstFrame]
		public void CreateMeshAnimation()
		{
			var animation = CreateOneSecondTenFramesAnimation();
			Assert.AreEqual(0.0f, animation.StartTime);
			Assert.AreEqual(1.0f, animation.EndTime);
			Assert.AreEqual(1.0f, animation.Duration);
			Assert.AreEqual(10, animation.FramesCount);
		}

		[Test]
		public void LoadMeshAnimation()
		{
			var animation = ContentLoader.Load<MeshAnimation>("TwoBonesAnimation");
			Assert.AreEqual(1, animation.FramesCount);
			Assert.AreEqual(2, animation.Frames[0].JointPoses.Length);
			Assert.AreEqual(Matrix.Identity, animation.Frames[0].JointPoses[0]);
			Assert.AreEqual(Matrix.CreateRotationY(45.0f), animation.Frames[0].JointPoses[1]);
		}

		[Test]
		public void ShowWavingSkinnedRectangle()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector3D.One * 1.5f;
			var geometry = CreateSkinnedGeometry();
			var skinnedMesh = new Mesh(geometry,
				new Material(Shader.Position3DUvSkinned, "DeltaEngineLogoOpaque"));
			skinnedMesh.Animation = CreateSkinnedMeshAnimation();
			new SkinnedModel(new ModelData(skinnedMesh), Vector3D.Zero);
		}

		private static Geometry CreateSkinnedGeometry()
		{
			var geometry =
				ContentLoader.Create<Geometry>(new GeometryCreationData(VertexFormat.Position3DUvSkinned,
				SkinnedVertexCount, SkinnedIndexCount));
			geometry.SetData(GetVerticesFromBottomToTop(),
				new short[] { 1, 0, 3, 3, 0, 2, 3, 2, 5, 5, 2, 4 });
			return geometry;
		}

		private const int SkinnedVertexCount = 6;
		private const int SkinnedIndexCount = 12;

		private static Vertex[] GetVerticesFromBottomToTop()
		{
			return new Vertex[]
			{
				new VertexPosition3DUvSkinned(new Vector3D(-0.5f, 0.0f, -1.0f), Vector2D.One,
					new SkinningData(0, 0, 1.0f, 0.0f)),
				new VertexPosition3DUvSkinned(new Vector3D(0.5f, 0.0f, -1.0f), Vector2D.UnitY,
					new SkinningData(0, 0, 1.0f, 0.0f)),
				new VertexPosition3DUvSkinned(new Vector3D(-0.5f, 0.0f, 0.0f), Vector2D.UnitX,
					new SkinningData(0, 1, 0.5f, 0.5f)),
				new VertexPosition3DUvSkinned(new Vector3D(0.5f, 0.0f, 0.0f), Vector2D.Zero,
					new SkinningData(0, 1, 0.5f, 0.5f)),
				new VertexPosition3DUvSkinned(new Vector3D(-0.5f, 0.0f, 1.0f), Vector2D.One,
					new SkinningData(1, 1, 1.0f, 0.0f)),
				new VertexPosition3DUvSkinned(new Vector3D(0.5f, 0.0f, 1.0f), Vector2D.UnitY,
					new SkinningData(1, 1, 1.0f, 0.0f))
			};
		}

		private static MeshAnimation CreateSkinnedMeshAnimation()
		{
			var animation =
				ContentLoader.Create<MeshAnimation>(new MeshAnimationCreationData(0.0f, 1.0f,
					AnimationFrameCount));
			CreateIdentityKeyFrames(animation);
			SetAnimatedKeyFrames(animation);
			return animation;
		}

		private static void CreateIdentityKeyFrames(MeshAnimation animation)
		{
			for (int frameIndex = 0; frameIndex < AnimationFrameCount; ++frameIndex)
			{
				animation.Frames[frameIndex] = new MeshAnimationFrame();
				animation.Frames[frameIndex].JointPoses = new Matrix[AnimationJointCount];
				for (int jointIndex = 0; jointIndex < AnimationJointCount; ++jointIndex)
					animation.Frames[frameIndex].JointPoses[jointIndex] = Matrix.Identity;
			}
		}

		private const int AnimationFrameCount = 30;
		private const int AnimationJointCount = 2;

		private static void SetAnimatedKeyFrames(MeshAnimation animation)
		{
			for (int frameIndex = 0; frameIndex < AnimationFrameCount; ++frameIndex)
			{
				float interpolation = ((float)frameIndex / AnimationFrameCount) * 360.0f;
				animation.Frames[frameIndex].JointPoses[1] =
					Matrix.CreateRotationZyx(MathExtensions.Sin(interpolation) * AnimationMaximumBendAngle,
						MathExtensions.Sin(interpolation) * AnimationMaximumBendAngle,
						MathExtensions.Cos(interpolation) * AnimationMaximumBendAngle);
			}
		}

		private const float AnimationMaximumBendAngle = 45.0f;
	}
}