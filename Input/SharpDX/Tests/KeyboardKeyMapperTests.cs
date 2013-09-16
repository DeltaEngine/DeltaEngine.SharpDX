using NUnit.Framework;
using SharpDXKey = SharpDX.DirectInput.Key;

namespace DeltaEngine.Input.SharpDX.Tests
{
	public class KeyboardKeyMapperTests
	{
		[Test]
		public void Translate()
		{
			Assert.AreEqual(SharpDXKey.Left, KeyboardKeyMapper.Translate(Key.CursorLeft));
			Assert.AreEqual(SharpDXKey.O, KeyboardKeyMapper.Translate(Key.O));
			Assert.AreEqual(SharpDXKey.RightBracket, KeyboardKeyMapper.Translate(Key.CloseBrackets));
		}
	}
}