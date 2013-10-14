using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.GameLogic
{
	/// <summary>
	/// Any game object you want to handle in a game should use this interface. It can be an Entity2D,
	/// Entity3D or any other class deriving from Entity. You can access all actors in the world via
	/// <see cref="EntitiesRunner.GetEntitiesOfType{Actor}" />
	/// </summary>
	public interface Actor
	{
		Vector3D Position { get; set; }
		Quaternion Orientation { get; set; }
		Vector3D Scale { get; set; }
	}
}