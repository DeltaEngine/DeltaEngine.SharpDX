namespace DeltaEngine.Rendering2D.Shapes.Levels
{
	/// <summary>
	/// Basic tile types for <see cref="Level"/>, can be extended or replaced by each game.
	/// </summary>
	public enum LevelTileType : byte
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