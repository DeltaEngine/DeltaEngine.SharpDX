using System;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering.Models
{
	public class MeshAnimation : ContentData
	{
		protected MeshAnimation(string contentName)
			: base(contentName) {}

		protected MeshAnimation(MeshAnimationCreationData creationData)
			: base("<GeneratedMeshAnimationCreationData>")
		{
			StartTime = creationData.StartTime;
			EndTime = creationData.EndTime;
			FrameRate = 30.0f;
			Frames = new MeshAnimationFrame[creationData.FramesCount];
		}

		public float StartTime { get; private set; }
		public float EndTime { get; private set; }
		public float Duration { get { return EndTime - StartTime; } }
		public float FrameRate { get; private set; }

		public MeshAnimationFrame[] Frames;
		public int FramesCount { get { return Frames.Length; } }

		protected override void LoadData(Stream fileData)
		{
			if (fileData.Length == 0)
				throw new InvalidMeshAnimationFile();
			Frames = new BinaryReader(fileData).Create() as MeshAnimationFrame[];
		}

		public class InvalidMeshAnimationFile : Exception {}

		public void UpdateFrameTransoforms()
		{
			var currentFrameIndex = (int)(Time.Total * FrameRate) % FramesCount;
			var currentFrame = Frames[currentFrameIndex];
			CurrentFrameTransforms = currentFrame.JointPoses;
		}

		public Matrix[] CurrentFrameTransforms { get; private set; }

		protected override void DisposeData() {}
	}
}