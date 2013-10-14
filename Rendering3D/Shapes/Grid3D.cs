using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Shapes3D
{
	public class Grid3D : Entity3D
	{
		public Grid3D(int dimension)
			: base(Vector3D.Zero)
		{
			Dimension = new Vector2D(dimension, dimension);
			CreateHorizontalLines();
			CreateVerticalLines();
		}

		public Vector2D Dimension { get; private set; }

		private void CreateHorizontalLines()
		{
			float halfDimension = Dimension.X / 2;
			for (float i = -halfDimension; i < halfDimension + 1; i++)
				new Line3D(new Vector3D(-halfDimension, i, 0), new Vector3D(halfDimension, i, 0),
					Color.White);
		}

		private void CreateVerticalLines()
		{
			float halfDimension = Dimension.Y / 2;
			for (float i = -halfDimension; i < halfDimension + 1; i++)
				new Line3D(new Vector3D(i, halfDimension, 0), new Vector3D(i, -halfDimension, 0),
					Color.White);
		}
	}
}