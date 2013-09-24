using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Models.Tests
{
	internal class BillboardTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateCamera()
		{
			camera = Camera.Use<LookAtCamera>();
			camera.Position = 4 * Vector3D.One;
			material = new Material(Shader.Position3DColorUv, "DeltaEngineLogo");
			new Grid3D(10);
			RegisterCameraCommands();
		}

		private LookAtCamera camera;
		private Material material;

		public void RegisterCameraCommands()
		{
			new Command(Command.MoveLeft, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				camera.Position -= right * Time.Delta * 4;
			});
			new Command(Command.MoveRight, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				camera.Position += right * Time.Delta * 4;
			});
			new Command(Command.Click, () => { camera.Zoom(0.5f); });
			new Command(Command.RightClick, () => { camera.Zoom(-0.5f); });
			new Command(Command.MoveUp, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				var up = Vector3D.Cross(right, front);
				camera.Position += up * Time.Delta * 4;
			});
			new Command(Command.MoveDown, () =>
			{
				var front = camera.Target - camera.Position;
				front.Normalize();
				var right = Vector3D.Cross(front, Vector3D.UnitZ);
				var up = Vector3D.Cross(right, front);
				camera.Position -= up * Time.Delta * 4;
			});
		}

		[Test]
		public void DrawBillboardCameraAligned()
		{
			new Billboard(Vector3D.Zero, new Size(1.0f, 1.0f), material);
		}

		[Test]
		public void DrawColoredBillboardCameraAligned()
		{
			material.DefaultColor = Color.Yellow;
			new Billboard(Vector3D.Zero, Size.One, material);
		}

		[Test]
		public void DrawBillboardUpAxisAligned()
		{
			new Billboard(Vector3D.Zero, Size.One, material,
				BillboardMode.CameraFacing | BillboardMode.UpAxis);
		}

		[Test]
		public void DrawBillboardFrontAxisAligned()
		{
			new Billboard(Vector3D.Zero, Size.One, material,
				BillboardMode.CameraFacing | BillboardMode.FrontAxis);
		}

		[Test]
		public void DrawBillboardGroundAxisAligned()
		{
			new Billboard(Vector3D.Zero, Size.One, material,
				BillboardMode.CameraFacing | BillboardMode.Ground);
		}

		[Test]
		public void DrawDifferentKindsOfBillboardsTogether()
		{
			material.DefaultColor = Color.Red;
			new Billboard(new Vector3D(-1.0f, -1.0f, 0.0f), Size.One, material);
			material.DefaultColor = Color.LightBlue;
			new Billboard(new Vector3D(-1.0f, 1.0f, 0.0f), Size.One, material,
				BillboardMode.CameraFacing | BillboardMode.UpAxis);
			material.DefaultColor = Color.PaleGreen;
			new Billboard(new Vector3D(1.0f, -1.0f, 0.0f), Size.One, material,
				BillboardMode.CameraFacing | BillboardMode.FrontAxis);
			material.DefaultColor = Color.Yellow;
			new Billboard(new Vector3D(1.0f, 1.0f, 0.0f), Size.One, material,
				BillboardMode.CameraFacing | BillboardMode.Ground);
		}
	}
}