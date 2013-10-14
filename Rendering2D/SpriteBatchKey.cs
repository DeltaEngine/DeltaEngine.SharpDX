using DeltaEngine.Content;
using DeltaEngine.Core;

namespace DeltaEngine.Rendering2D
{
	internal struct SpriteBatchKey
	{
		public SpriteBatchKey(Sprite sprite)
			: this()
		{
			Material = sprite.Material;
			BlendMode = sprite.BlendMode;
		}

		public Material Material { get; private set; }
		public BlendMode BlendMode { get; private set; }

		public static bool operator ==(SpriteBatchKey key1, SpriteBatchKey key2)
		{
			return key1.Material == key2.Material && key1.BlendMode == key2.BlendMode;
		}

		public static bool operator !=(SpriteBatchKey key1, SpriteBatchKey key2)
		{
			return !(key1 == key2);
		}

		public bool Equals(SpriteBatchKey other)
		{
			return Material.Equals(other.Material) && BlendMode == other.BlendMode;
		}

		public override bool Equals(object obj)
		{
			return obj is SpriteBatchKey && Equals((SpriteBatchKey)obj);
		}

		public override int GetHashCode()
		{
			return Material.GetHashCode() * 397 ^ (int)BlendMode;
		}
	}
}