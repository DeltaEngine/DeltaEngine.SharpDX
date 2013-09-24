using CreepyTowers.Levels;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.ScreenSpaces;

namespace CreepyTowers
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
			get { return sizeFactor; }
			set
			{
				sizeFactor = value;
				ChangeSize();
			}
		}

		private float sizeFactor;

		private void ChangeSize()
		{
			if (Camera.Current == null)
				CreateGameCamera();
			((OrthoCamera)Camera.Current).Size =
				new Size(ScreenSpace.Current.Viewport.Width * FovSizeFactor,
					ScreenSpace.Current.Viewport.Height * FovSizeFactor);
		}

		private void CreateGameCamera()
		{
			var camera = Camera.Use<OrthoCamera>();
			camera.Size = new Size(ScreenSpace.Current.Viewport.Width * FovSizeFactor,
				ScreenSpace.Current.Viewport.Height * FovSizeFactor);
			camera.Position = new Vector3D(6.0f, 5.9f, 6.3f);
			//Camera.Target = new Vector3D(0.6f, 0.0f, 0.07f);
		}

		private void CreateGrid()
		{
			Grid = new LevelGrid(GridSize, GridScale);
			Grid.AddTag("Grid");
		}

		public LevelGrid Grid { get; private set; }
		private const int GridSize = 24;
		private const float GridScale = 0.20f;
	}
}