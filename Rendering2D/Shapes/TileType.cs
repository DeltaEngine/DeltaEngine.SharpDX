namespace DeltaEngine.Rendering2D.Shapes
{
	/// <summary>
	/// Basic tile types for the TileMap, can be extended or replaced by each game.
	/// </summary>
	public enum TileType : byte
	{
		Nothing,
		Blocked,
		Placeable,
		BlockedPlaceable,
		Red,
		Green,
		Blue,
		Yellow,
		Brown,
		Gray,
		SpawnPoint,
		ExitPoint
	}
}