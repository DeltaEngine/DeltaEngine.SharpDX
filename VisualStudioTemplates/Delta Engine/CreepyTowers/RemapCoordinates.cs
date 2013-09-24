using DeltaEngine.Datatypes;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class RemapCoordinates
	{
		public Size RemapCoordinateSpaces(Size objectSize)
		{
			CalculateAspect();
			return new Size(ScreenSpace.Current.FromPixelSpace(objectSize).Width * aspectX, 
				ScreenSpace.Current.FromPixelSpace(objectSize).Height * aspectY);
		}

		private float aspectX;
		private float aspectY;

		private void CalculateAspect()
		{
			aspectX = Game.window.ViewportPixelSize.Width / 1920.0f;
			aspectY = Game.window.ViewportPixelSize.Height / 1080.0f;
		}

		public Size RemapCoordinateSpaces(Vector2D position)
		{
			CalculateAspect();
			return new Size(ScreenSpace.Current.FromPixelSpace(position).X * aspectX, 
				ScreenSpace.Current.FromPixelSpace(position).Y * aspectY);
		}
	}
}