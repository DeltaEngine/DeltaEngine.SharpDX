using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Graphics
{
	/// <summary>
	/// The graphics device clears everything via <see cref="Clear"/> at the beginning of each frame
	/// and shows the render buffer on screen at the end of each frame via <see cref="Present"/>.
	/// </summary>
	public abstract class Device : IDisposable
	{
		protected Device(Window window)
		{
			this.window = window;
			CameraViewMatrix = Matrix.CreateLookAt(Vector3D.One * 3, Vector3D.Zero, Vector3D.UnitZ);
			CameraProjectionMatrix = Matrix.CreatePerspective(90, window.ViewportPixelSize.AspectRatio,
				1, 100);
			window.ViewportSizeChanged += SetViewport;
			window.FullscreenChanged += OnFullscreenChanged;
			ModelViewProjectionMatrix = Matrix.CreateOrthoProjection(window.ViewportPixelSize);
		}

		protected readonly Window window;

		public Matrix CameraViewMatrix
		{
			get { return cameraViewMatrix; }
			set
			{
				cameraViewMatrix = value;
				CameraInvertedViewMatrix = Matrix.Invert(cameraViewMatrix);
			}
		}

		private Matrix cameraViewMatrix;

		public Matrix CameraProjectionMatrix { get; set; }

		public abstract void SetViewport(Size viewportSize);

		public Matrix CameraInvertedViewMatrix { get; private set; }

		protected virtual void OnFullscreenChanged(Size displaySize, bool wantFullscreen)
		{
			SetViewport(displaySize);
		}

		public virtual void SetModelViewProjectionMatrixFor2D()
		{
			ModelViewProjectionMatrix = Matrix.CreateOrthoProjection(window.ViewportPixelSize);
			SetProjectionMatrix(ModelViewProjectionMatrix);
			SetModelViewMatrix(Matrix.Identity);
		}

		public void Set2DMode()
		{
			SetModelViewProjectionMatrixFor2D();
			DisableDepthTest();
		}

		public Matrix ModelViewProjectionMatrix { get; private set; }
		public virtual void SetProjectionMatrix(Matrix matrix) {}
		public virtual void SetModelViewMatrix(Matrix matrix) {}
		public abstract void DisableDepthTest();

		public void Set3DMode()
		{
			if (OnSet3DMode != null)
				OnSet3DMode();
			ModelViewProjectionMatrix = CameraViewMatrix * CameraProjectionMatrix;
			SetProjectionMatrix(CameraProjectionMatrix);
			SetModelViewMatrix(CameraViewMatrix);
			EnableDepthTest();
		}

		public class Set3DModeHasNoDelegatesRegistered : Exception {}

		public event Action OnSet3DMode;

		public abstract void EnableDepthTest();
		public abstract void Dispose();
		public abstract void Clear();
		public abstract void Present();
		public abstract void SetBlendMode(BlendMode blendMode);

		public ShaderWithFormat Shader { get; set; }

		public abstract CircularBuffer CreateCircularBuffer(ShaderWithFormat shader,
			BlendMode blendMode, VerticesMode drawMode = VerticesMode.Triangles);
	}
}