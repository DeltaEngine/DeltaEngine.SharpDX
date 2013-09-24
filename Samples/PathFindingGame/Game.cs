using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Shapes3D;

namespace PathfindingGame
{
	public class Game : Entity
	{
		public Game(Window window)
		{
			var camera = CreateLookAtCamera(Vector3D.One * 4.0f, Vector3D.Zero);
			grid = new PathFindingGrid(10, 20);
			new Line3D(new Vector3D(2.5f, 0.0f, 2.5f), new Vector3D(0.0f, 0.0f, 2.5f), Color.Red);
			new Line3D(new Vector3D(0.0f, 2.5f, 2.5f), new Vector3D(0.0f, 0.0f, 2.5f), Color.Green);
			new Line3D(new Vector3D(0.0f, 0.0f, 0.0f), new Vector3D(0.0f, 0.0f, 2.5f), Color.Blue);
			AddCommands(camera);
			new Command(Command.Exit, window.CloseAfterFrame);
			SetUnreachableNodes();
			grid.GetPathAndPaint(new Vector3D(3.0f, 2.0f, 0.0f), new Vector3D(-5.0f, -1.0f, 0.0f));
		}

		private readonly PathFindingGrid grid;

		private static LookAtCamera CreateLookAtCamera(Vector3D position, Vector3D target)
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = position;
			camera.Target = target;
			return camera;
		}

		private void AddCommands(LookAtCamera camera)
		{
			new Command(() => { MoveCameraToLeft(camera); }).Add(new KeyTrigger(Key.A, State.Pressed));
			new Command(() => { MoveCameraToRight(camera); }).Add(new KeyTrigger(Key.D, State.Pressed));
			new Command(() => { camera.Zoom(0.5f); }).Add(new KeyTrigger(Key.W, State.Pressed));
			new Command(() => { camera.Zoom(-0.5f); }).Add(new KeyTrigger(Key.S, State.Pressed));
			//new Command(Command.Click, () => { camera.Zoom(0.5f); });
			//new Command(Command.RightClick, () => { camera.Zoom(-0.5f); });
			new Command(Command.MoveUp, () => { MoveCameraUp(camera); });
			new Command(Command.MoveDown, () => { MoveCameraDown(camera); });
			new Command(point =>
			{
				var ray = camera.ScreenPointToRay(point);
				grid.AddCubeInTheGrid(ray);
				grid.GetPathAndPaint(new Vector3D(3.0f, 2.0f, 0.0f), new Vector3D(-5.0f, -1.0f, 0.0f));
			}).Add(new MouseButtonTrigger());
		}

		private static void MoveCameraToLeft(LookAtCamera camera)
		{
			var front = camera.Target - camera.Position;
			front.Normalize();
			var right = Vector3D.Cross(front, Vector3D.UnitZ);
			camera.Position -= right * Time.Delta * 2;
		}

		private static void MoveCameraToRight(LookAtCamera camera)
		{
			var front = camera.Target - camera.Position;
			front.Normalize();
			var right = Vector3D.Cross(front, Vector3D.UnitZ);
			camera.Position += right * Time.Delta * 2;
		}

		private static void MoveCameraUp(LookAtCamera camera)
		{
			var front = camera.Target - camera.Position;
			front.Normalize();
			var right = Vector3D.Cross(front, Vector3D.UnitZ);
			var up = Vector3D.Cross(right, front);
			camera.Position += up * Time.Delta * 2;
		}

		private static void MoveCameraDown(LookAtCamera camera)
		{
			var front = camera.Target - camera.Position;
			front.Normalize();
			var right = Vector3D.Cross(front, Vector3D.UnitZ);
			var up = Vector3D.Cross(right, front);
			camera.Position -= up * Time.Delta * 2;
		}

		private void SetUnreachableNodes()
		{
			grid.SetUnreachableNode(new Vector3D(0.25f, 2.25f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, 1.75f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, 1.25f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, 0.75f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, 0.25f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, -0.25f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, -0.75f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, -1.25f, 0.0f));
			grid.SetUnreachableNode(new Vector3D(0.25f, -1.75f, 0.0f));
		}
	}
}
