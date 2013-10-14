using System;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering3D
{
	/// <summary>
	/// Consists of a number of frames of animation. Defaults to 30fps
	/// </summary>
	public class MeshAnimation : ContentData
	{
		protected MeshAnimation(string contentName)
			: base(contentName) {}

		public MeshAnimation(MeshAnimationCreationData creationData)
			: base("<GeneratedMeshAnimationCreationData>")
		{
			StartTime = creationData.StartTime;
			EndTime = creationData.EndTime;
			FrameRate = DefaultFrameRate;
			Frames = new MeshAnimationFrame[creationData.FramesCount];
		}

		public float StartTime { get; private set; }
		public float EndTime { get; private set; }
		public float FrameRate { get; set; }
		private const float DefaultFrameRate = 30.0f;
		public MeshAnimationFrame[] Frames;

		public int FramesCount { get { return Frames.Length; } }
		public float Duration { get { return EndTime - StartTime; } }

		protected override void LoadData(Stream fileData)
		{
			if (fileData.Length == 0)
				throw new InvalidMeshAnimationFile();
			Frames = new BinaryReader(fileData).Create() as MeshAnimationFrame[];
		}

		public class InvalidMeshAnimationFile : Exception {}

		public void UpdateFrameTransforms()
		{
			var currentFrameIndex = (int)(Time.Total * FrameRate) % FramesCount;
			var currentFrame = Frames[currentFrameIndex];
			CurrentFrameTransforms = currentFrame.JointPoses;
		}

		public Matrix[] CurrentFrameTransforms { get; private set; }

		protected override void DisposeData() {}
	}
}