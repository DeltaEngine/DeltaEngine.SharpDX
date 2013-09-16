using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Input.SharpDX
{
	/// <summary>
	/// SharpDX does not support any touch input devices.
	/// </summary>
	public sealed class SharpDX : Touch
	{
		public SharpDX()
		{
			IsAvailable = false;
		}

		public override void Dispose() {}
		public override bool IsAvailable { get; protected set; }

		public override Point GetPosition(int touchIndex)
		{
			return new Point();
		}

		public override State GetState(int touchIndex)
		{
			return State.Released;
		}

		public override void Update(IEnumerable<Entity> entities) {}
	}
}