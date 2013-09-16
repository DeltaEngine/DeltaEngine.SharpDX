using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Shapes3D
{
	public class Grid3D : Entity3D
	{
		public Grid3D(int dimension)
			: base(Vector.Zero)
		{
			Dimension = new Point(dimension, dimension);
			CreateHorizontalLines();
			CreateVerticalLines();
		}

		public Point Dimension { get; private set; }

		private void CreateHorizontalLines()
		{
			float halfDimension = Dimension.X / 2;
			for (float i = -halfDimension; i < halfDimension + 1; i++)
				new Line3D(new Vector(-halfDimension, i, 0), new Vector(halfDimension, i, 0), Color.White);
		}

		private void CreateVerticalLines()
		{
			float halfDimension = Dimension.Y / 2;
			for (float i = -halfDimension; i < halfDimension + 1; i++)
				new Line3D(new Vector(i, halfDimension, 0), new Vector(i, -halfDimension, 0), Color.White);
		}
	}
}