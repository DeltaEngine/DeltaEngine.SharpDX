using System;
using DeltaEngine.Datatypes;

namespace DeltaEngine.GameLogic
{
	public static class ActorExtensions
	{
		public static bool IsColliding(this Actor actor, Actor other)
		{
			if (Is2D(actor))
				return GetDrawArea(actor).IsColliding(GetRotationZ(actor), GetDrawArea(other),
					GetRotationZ(other));
			return GetBoundingBox(actor).IsColliding(GetBoundingBox(other));
			//return GetBoundingSphere(actor).IsColliding(GetBoundingSphere(other));
		}

		public static bool Is2D(this Actor actor)
		{
			return actor.Scale.Z == 0.0f;
		}

		public static Rectangle GetDrawArea(this Actor actor)
		{
			return Rectangle.FromCenter(actor.Position.X, actor.Position.Y, actor.Scale.X, actor.Scale.Y);
		}

		public static float GetRotationZ(this Actor actor)
		{
			return actor.Orientation.Z;
		}

		public static BoundingBox GetBoundingBox(this Actor actor)
		{
			return BoundingBox.FromCenter(actor.Position, actor.Scale);
		}

		public static BoundingSphere GetBoundingSphere(this Actor actor)
		{
			return new BoundingSphere(actor.Position,
				Math.Max(actor.Scale.X, Math.Max(actor.Scale.Y, actor.Scale.Z)));
		}
	}
}