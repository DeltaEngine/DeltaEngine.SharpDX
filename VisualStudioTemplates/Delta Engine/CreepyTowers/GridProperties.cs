using DeltaEngine.Datatypes;

namespace $safeprojectname$
{
	public struct GridProperties
	{
		public Vector MidPoint
		{
			get;
			set;
		}

		public Vector TopLeft
		{
			get;
			set;
		}

		public Vector TopRight
		{
			get;
			set;
		}

		public Vector BottomLeft
		{
			get;
			set;
		}

		public Vector BottomRight
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public bool IsOccupied
		{
			get;
			set;
		}
	}
}