using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class KeyboardTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			keyboard = Resolve<Keyboard>();
			mockKeyboard = keyboard as MockKeyboard;
		}

		private Keyboard keyboard;
		private MockKeyboard mockKeyboard;

		[Test]
		public void PressKeyToShowCircle()
		{
			new FontText(Font.Default, "Press A on Keyboard to show red circle", Rectangle.One);
			var ellipse = new Ellipse(new Rectangle(0.1f, 0.1f, 0.1f, 0.1f), Color.Red);
			new Command(() => ellipse.Center = Point.Half).Add(new KeyTrigger(Key.A, State.Pressed));
			new Command(() => ellipse.Center = Point.Zero).Add(new KeyTrigger(Key.A, State.Released));
		}

		[Test, CloseAfterFirstFrame]
		public void TestSpaceKeyPress()
		{
			bool isSpacePressed = false;
			new Command(() => isSpacePressed = true).Add(new KeyTrigger(Key.Space, State.Pressed));
			Assert.IsFalse(isSpacePressed);
			if (mockKeyboard == null)
				return; //ncrunch: no coverage
			mockKeyboard.SetKeyboardState(Key.Space, State.Pressed);
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(isSpacePressed);
			Assert.IsTrue(mockKeyboard.IsAvailable);
		}

		[Test]
		public void CountKeyPressingAndReleasing()
		{
			int pressed = 0;
			int released = 0;
			var fontText = new FontText(Font.Default, "'A' pressed: 0 released: 0",
				Rectangle.One);
			new Command(() => fontText.Text = "'A' pressed: " + ++pressed + " released: " + released).
				Add(new KeyTrigger(Key.A));
			new Command(() => fontText.Text = "'A' pressed: " + pressed + " released: " + ++released).
				Add(new KeyTrigger(Key.A, State.Releasing));
		}
		
		[Test, CloseAfterFirstFrame]
		public void HandleInput()
		{
			Assert.AreEqual("", keyboard.HandleInput(""));
			mockKeyboard.SetKeyboardState(Key.A, State.Pressing);
			mockKeyboard.SetKeyboardState(Key.Z, State.Pressing);
			Assert.AreEqual("AZ", keyboard.HandleInput(""));
			mockKeyboard.SetKeyboardState(Key.A, State.Pressing);
			mockKeyboard.SetKeyboardState(Key.Backspace, State.Pressing);
			Assert.AreEqual("", keyboard.HandleInput(""));
			mockKeyboard.SetKeyboardState(Key.Escape, State.Pressing);
			Assert.AreEqual("", keyboard.HandleInput(""));
			mockKeyboard.SetKeyboardState(Key.Space, State.Pressing);
			mockKeyboard.SetKeyboardState(Key.D9, State.Pressing);
			Assert.AreEqual(" 9", keyboard.HandleInput(""));
		}

		[Test]
		public void HandleInputVisually()
		{
			var text = new FontText(Font.Default, "Type some text", Rectangle.One);
			new Command(() => text.Text = keyboard.HandleInput(text.Text)).Add(new KeyTrigger(Key.None,
				State.Released));
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyNumberKeyPress()
		{
			VerifyKeyPressIsHandled("xx1", "xx", Key.D1);
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyNumpadNumberKeyPress()
		{
			VerifyKeyPressIsHandled("xx2", "xx", Key.NumPad2);
		}

		private void VerifyKeyPressIsHandled(string expectedResult, string startText, Key key)
		{
			VerifyKeyPressesAreHandled(expectedResult, startText, new List<Key> { key });
		}

		private void VerifyKeyPressesAreHandled(string expectedResult, string startText,
			List<Key> keys)
		{
			if (mockKeyboard == null)
				return; //ncrunch: no coverage
			foreach (Key key in keys)
				mockKeyboard.SetKeyboardState(key, State.Pressing);
			Assert.AreEqual(expectedResult, mockKeyboard.HandleInput(startText));
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyLetterKeyPress()
		{
			VerifyKeyPressesAreHandled("12AZ", "12", new List<Key> { Key.A, Key.Z });
			VerifyKeyPressIsHandled("AB", "A", Key.B);
		}

		[Test, CloseAfterFirstFrame]
		public void VerifySpaceKeyPress()
		{
			VerifyKeyPressIsHandled("xx ", "xx", Key.Space);
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyBackspaceKeyPress()
		{
			VerifyKeyPressIsHandled("", "", Key.Backspace);
			VerifyKeyPressIsHandled("1234", "12345", Key.Backspace);
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyCommaKeyPress()
		{
			VerifyKeyPressIsHandled("1234,", "1234", Key.Comma);
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyDecimalPointKeyPress()
		{
			VerifyKeyPressIsHandled("1234.", "1234", Key.Decimal);
			VerifyKeyPressIsHandled("5678.", "5678", Key.Period);
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyUnknownKeyPressIsIgnored()
		{
			VerifyKeyPressIsHandled("1234", "1234", Key.CapsLock);
		}
	}
}