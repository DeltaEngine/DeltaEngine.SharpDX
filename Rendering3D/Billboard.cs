using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D
{
	/// <summary>
	/// A quad mesh that always has a fixed orientation with respect to the camera
	/// </summary>
	public class Billboard : Entity3D
	{
		public Billboard(Vector3D position, Size size, Material material,
			BillboardMode billboardMode = BillboardMode.CameraFacing)
			: base(position)
		{
			mode = billboardMode;
			planeQuad = new PlaneQuad(size, material);
			OnDraw<BillBoardRenderer>();
		}

		private readonly BillboardMode mode;
		public readonly PlaneQuad planeQuad;

		private class BillBoardRenderer : DrawBehavior
		{
			public BillBoardRenderer(Drawing drawing, Device device)
			{
				this.drawing = drawing;
				this.device = device;
			}
			
			private readonly Drawing drawing;
			private readonly Device device;

			public void Draw(List<DrawableEntity> visibleEntities)
			{
				foreach (var entity in visibleEntities.OfType<Billboard>())
					DrawBillboard(entity);
			}

			private void DrawBillboard(Billboard entity)
			{
				inverseView = device.CameraInvertedViewMatrix;
				look = inverseView.Translation - entity.Position;
				AlignBillboard(entity);
				DrawGeometry(entity);
			}

			private Matrix inverseView;
			private Vector3D look;

			private void AlignBillboard(Billboard entity)
			{
				if ((entity.mode & BillboardMode.FrontAxis) != 0)
					AlignBillboardFrontAxis();
				else if ((entity.mode & BillboardMode.UpAxis) != 0)
					AlignBillboardUpAxis();
				else if ((entity.mode & BillboardMode.Ground) != 0)
					AlignBillboardGroundAxis();
				else
					DrawBillboardUnaligned();
			}

			private void AlignBillboardFrontAxis()
			{
				cameraUp = Vector3D.UnitY;
				look.Y = 0;
			}

			private Vector3D cameraUp;

			private void AlignBillboardUpAxis()
			{
				cameraUp = Vector3D.UnitZ;
				look.Z = 0;
			}

			private void AlignBillboardGroundAxis()
			{
				cameraUp = -Vector3D.UnitY;
				look = Vector3D.UnitZ;
			}

			private void DrawBillboardUnaligned()
			{
				cameraUp = inverseView.Up;
				cameraUp.Normalize();
			}

			private void DrawGeometry(Billboard entity)
			{
				look.Normalize();
				Vector3D right = Vector3D.Cross(cameraUp, look);
				Vector3D up = Vector3D.Cross(look, right);
				Matrix transform = Matrix.Identity;
				transform.Right = right;
				transform.Up = look;
				transform.Forward = up;
				transform.Translation = entity.Position;
				drawing.AddGeometry(entity.planeQuad.Geometry, entity.planeQuad.Material, transform);
			}
		}
	}
}