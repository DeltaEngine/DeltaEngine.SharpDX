using $safeprojectname$.Levels;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class CreateCameraAndGrid
	{
		public CreateCameraAndGrid(float cameraSizeFactor)
		{
			FovSizeFactor = cameraSizeFactor;
			CreateGrid();
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
			if (DeltaEngine.Rendering3D.Cameras.Camera.Current == null)
				CreateGameCamera();

			((OrthoCamera)DeltaEngine.Rendering3D.Cameras.Camera.Current).Size = new 
				Size(ScreenSpace.Current.Viewport.Width * FovSizeFactor, 
					ScreenSpace.Current.Viewport.Height * FovSizeFactor);
		}

		private void CreateGameCamera()
		{
			GameCamera = Camera.Use<OrthoCamera>();
			GameCamera.Size = new Size(ScreenSpace.Current.Viewport.Width * FovSizeFactor, 
				ScreenSpace.Current.Viewport.Height * FovSizeFactor);
			GameCamera.Position = new Vector3D(6.0f, 5.9f, 6.3f);
		}

		public OrthoCamera GameCamera
		{
			get;
			private set;
		}

		private void CreateGrid()
		{
			Grid = new LevelGrid(GridSize, GridScale);
			Grid.AddTag("Grid");
		}

		public LevelGrid Grid
		{
			get;
			private set;
		}

		private const int GridSize = 24;
		private const float GridScale = 0.20f;
	}
}