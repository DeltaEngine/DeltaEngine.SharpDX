using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering.Sprites
{
	public class Billboard : Entity3D
	{
		public Billboard(Vector position, Size size, Material material,
			BillboardMode billboardMode = BillboardMode.CameraFacing)
			: base(position)
		{
			mode = billboardMode;
			planeQuad = new PlaneQuad(size, material);
			OnDraw<BillBoardRenderer>();
		}

		private readonly PlaneQuad planeQuad;
		private readonly BillboardMode mode;

		private class BillBoardRenderer : DrawBehavior
		{
			public BillBoardRenderer(Drawing drawing, Device device)
			{
				this.drawing = drawing;
				this.device = device;
			}
			
			private readonly Drawing drawing;
			private readonly Device device;

			public void Draw(IEnumerable<DrawableEntity> entities)
			{
				foreach (var entity in entities.OfType<Billboard>())
				{
					var inverseView = device.CameraInvertedViewMatrix;
					Vector look = inverseView.Translation - entity.Position;
					Vector cameraUp;
					if ((entity.mode & BillboardMode.FrontAxis) != 0)
					{
						cameraUp = Vector.UnitY;
						look.Y = 0;
					}
					else if ((entity.mode & BillboardMode.UpAxis) != 0)
					{
						cameraUp = Vector.UnitZ;
						look.Z = 0;
					}
					else if ((entity.mode & BillboardMode.Ground) != 0)
					{
						cameraUp = -Vector.UnitY;
						look = Vector.UnitZ;
					}
					else
					{
						cameraUp = inverseView.Up;
						cameraUp.Normalize();
					}
					look.Normalize();
					Vector right = Vector.Cross(cameraUp, look);
					Vector up = Vector.Cross(look, right);
					transform = Matrix.Identity;
					transform.Right = right;
					transform.Up = look;
					transform.Forward = up;
					transform.Translation = entity.Position;
					drawing.AddGeometry(entity.planeQuad.Geometry, entity.planeQuad.Material, transform);
				}
			}

			private Matrix transform;
		}
	}
}