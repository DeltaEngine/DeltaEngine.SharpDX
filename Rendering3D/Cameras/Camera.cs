using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering3D.Cameras
{
	/// <summary>
	/// Provides some useful constants and properties that are commonly used 
	/// in most camera classes. The current camera can be assigned using Camera.Use.
	/// </summary>
	public abstract class Camera : Entity, IDisposable
	{
		public static T Use<T>(object optionalParameter = null) where T : Camera
		{
			if (Current != null)
				Current.Dispose();
			return (T)resolver.ResolveCamera<T>(optionalParameter);
		}

		public static Camera Current { get; private set; }
		internal static CameraResolver resolver;

		public static bool IsInitialized
		{
			get { return Current != null; }
		}

		protected Camera(Device device, Window window)
		{
			this.device = device;
			this.window = window;
			Current = this;
			FieldOfView = DefaultFieldOfView;
			FarPlane = DefaultFarPlane;
			NearPlane = DefaultNearPlane;
			ForceProjectionMatrixUpdate(window.ViewportPixelSize);
			window.ViewportSizeChanged += ForceProjectionMatrixUpdate;
			device.OnSet3DMode += SetMatricesToDevice;
		}

		private readonly Device device;
		protected readonly Window window;
		public float FieldOfView { get; set; }
		protected const float DefaultFieldOfView = 90.0f;
		public float FarPlane { get; set; }
		protected const float DefaultFarPlane = 100.0f;
		public float NearPlane { get; set; }
		protected const float DefaultNearPlane = 0.1f;

		protected void ForceProjectionMatrixUpdate(Size size)
		{
			updateProjection = true;
		}

		private bool updateProjection;

		private void SetMatricesToDevice()
		{
			if (updateProjection)
			{
				updateProjection = false;
				device.CameraProjectionMatrix = GetCurrentProjectionMatrix();
			}
			device.CameraViewMatrix = GetCurrentViewMatrix();
		}

		protected virtual Matrix GetCurrentProjectionMatrix()
		{
			return Matrix.CreatePerspective(FieldOfView, ScreenSpace.Current.AspectRatio, NearPlane,
				FarPlane);
		}

		protected internal virtual Matrix GetCurrentViewMatrix()
		{
			return Matrix.CreateLookAt(Position, Target, UpDirection);
		}

		public virtual Vector3D Position { get; set; }

		public virtual Vector3D Target
		{
			get { return GetFinalTargetPosition(); }
			set { targetPosition = value; }
		}

		protected static readonly Vector3D UpDirection = Vector3D.UnitZ;

		protected virtual Vector3D GetFinalTargetPosition()
		{
			return targetPosition;
		}

		private Vector3D targetPosition;

		public Ray ScreenPointToRay(Vector2D screenSpacePosition)
		{
			var pixelPos = ScreenSpace.Current.ToPixelSpace(screenSpacePosition);
			var viewportPixelSize = ScreenSpace.Current.ToPixelSpace(ScreenSpace.Current.Viewport);
			var normalizedPoint = new Vector2D(2.0f * (pixelPos.X / viewportPixelSize.Width) - 1.0f,
				1.0f - 2.0f * (pixelPos.Y / viewportPixelSize.Height));
			var clipSpaceNearPoint = new Vector3D(normalizedPoint.X, normalizedPoint.Y, NearPlane);
			var clipSpaceFarPoint = new Vector3D(normalizedPoint.X, normalizedPoint.Y, FarPlane);
			var viewProj = GetCurrentViewMatrix() * GetCurrentProjectionMatrix();
			var inverseViewProj = Matrix.Invert(viewProj);
			var worldSpaceNearPoint = Matrix.TransformHomogeneousCoordinate(clipSpaceNearPoint,
				inverseViewProj);
			var worldSpaceFarPoint = Matrix.TransformHomogeneousCoordinate(clipSpaceFarPoint,
				inverseViewProj);
			// Ray direction reversed because LookAt is RH
			return new Ray(worldSpaceNearPoint,
				Vector3D.Normalize(worldSpaceNearPoint - worldSpaceFarPoint));
		}

		public Vector2D WorldToScreenPoint(Vector3D point)
		{
			var viewProj = GetCurrentViewMatrix() * GetCurrentProjectionMatrix();
			var projectedPoint = Matrix.TransformHomogeneousCoordinate(point, viewProj);
			var screenSpacePoint = new Vector2D(0.5f * projectedPoint.X + 0.5f,
				1.0f - (0.5f * projectedPoint.Y + 0.5f));
			var viewportPixelSize = ScreenSpace.Current.ToPixelSpace(ScreenSpace.Current.Viewport);
			var pixelPos = new Vector2D(screenSpacePoint.X * viewportPixelSize.Width,
				screenSpacePoint.Y * viewportPixelSize.Height);
			return ScreenSpace.Current.FromPixelSpace(pixelPos);
		}

		public void Dispose()
		{
			window.ViewportSizeChanged -= ForceProjectionMatrixUpdate;
			device.OnSet3DMode -= SetMatricesToDevice;
			Current = null;
		}
	}
}