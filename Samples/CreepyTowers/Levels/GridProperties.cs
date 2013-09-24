using DeltaEngine.Datatypes;

namespace CreepyTowers.Levels
{
	public struct GridProperties
	{
		public Vector3D MidPoint { get; set; }
		public Vector3D TopLeft { get; set; }
		public Vector3D TopRight { get; set; }
		public Vector3D BottomLeft { get; set; }
		public Vector3D BottomRight { get; set; }
		public bool IsActive { get; set; }
		public bool IsOccupied { get; set; }
	}
}