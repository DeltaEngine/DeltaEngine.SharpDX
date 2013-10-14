using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class GameCamera
	{
		public GameCamera(float cameraSizeFactor)
		{
			FovSizeFactor = cameraSizeFactor;
		}

		public float FovSizeFactor
		{
			get
			{
				return sizeFactor;
			}
			set
			{
				sizeFactor = value;
				ChangeSize();
			}
		}

		private float sizeFactor;

		private void ChangeSize()
		{
			if (!(Camera.Current is OrthoCamera))
				CreateGameCamera();

			((OrthoCamera)Camera.Current).Size = new Size(ScreenSpace.Current.Viewport.Width * 
				FovSizeFactor, ScreenSpace.Current.Viewport.Height * FovSizeFactor);
		}

		private void CreateGameCamera()
		{
			gameCamera = Camera.Use<OrthoCamera>();
			gameCamera.Size = new Size(ScreenSpace.Current.Viewport.Width * FovSizeFactor, 
				ScreenSpace.Current.Viewport.Height * FovSizeFactor);
			gameCamera.Position = new Vector3D(6.0f, 5.9f, 6.3f);
			gameCamera.Start<CameraShakingUpdater>();
		}

		private OrthoCamera gameCamera;
		public class CameraShakingUpdater : UpdateBehavior
		{
			public CameraShakingUpdater()
			{
				normalPosition = Camera.Current.Position;
				if (Camera.Current is TargetedCamera)
					targetPosition = ((TargetedCamera)Camera.Current).Target;
			}

			private readonly Vector3D normalPosition;
			private readonly Vector3D targetPosition;

			public override void Update(IEnumerable<Entity> entities)
			{
				time += Time.Delta;
				if (time > 1.0f)
					ResetCamera();

				if (!isShaking)
					return;

				foreach (var entity in entities.OfType<OrthoCamera>())
					MoveCameraPosition(entity);

				goToLeft = !goToLeft;
			}

			private bool goToLeft;

			private void ResetCamera()
			{
				isShaking = false;
				time = 0.0f;
				Camera.Current.Position = normalPosition;
				if (Camera.Current is TargetedCamera)
					((TargetedCamera)Camera.Current).Target = targetPosition;
			}

			public static bool isShaking;
			private float time;

			private void MoveCameraPosition(OrthoCamera camera)
			{
				var front = Vector3D.Normalize(camera.Target - camera.Position);
				var right = Vector3D.Normalize(Vector3D.Cross(front, Vector3D.UnitZ));
				var delta = goToLeft ? -0.1f * right : 0.1f * right;
				camera.Position += delta;
				camera.Target += delta;
			}
		}
	}
}