using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D.Cameras
{
	/// <summary>
	/// Follows a path as if on rails. Can be started and stopped. Updates at 60fps
	/// </summary>
	public sealed class PathCamera : Camera, RapidUpdateable
	{
		public PathCamera(Device device, Window window, Matrix[] viewMatrices)
			: base(device, window)
		{
			if (viewMatrices == null || viewMatrices.Length < 2)
				throw new NoTrackSpecified();
			this.viewMatrices = viewMatrices;
		}

		public class NoTrackSpecified : Exception { }

		private readonly Matrix[] viewMatrices;

		public void RapidUpdate()
		{
			if (IsMoving && currentFrame < viewMatrices.Length - 1)
				currentFrame += 1;
		}

		public bool IsMoving { get; set; }	
		private int currentFrame;

		public int CurrentFrame
		{
			get { return currentFrame; }
			set
			{
				currentFrame = value;
				if (currentFrame >= viewMatrices.Length)
					currentFrame = viewMatrices.Length - 1;
			}
		}

		protected internal override Matrix GetCurrentViewMatrix()
		{
			return viewMatrices[currentFrame];
		}
	}
}