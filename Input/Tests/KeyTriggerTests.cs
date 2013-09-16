using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class KeyTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void PressEscapeToCloseWindow()
		{
			new FontText(Font.Default, "Press ESC to close the window", Rectangle.One);
			new Command(() => Resolve<Window>().CloseAfterFrame()).Add(new KeyTrigger(Key.Escape,
				State.Pressed));
		}

		[Test]
		public void PressEscapeToCloseWindowViaRegisteredCommands()
		{
			new FontText(Font.Default, "Press ESC to close the window", Rectangle.One);
			Command.Register("Exit", new KeyTrigger(Key.Escape, State.Pressed));
			new Command("Exit", () => Resolve<Window>().CloseAfterFrame());
		}

		[Test]
		public void PressCursorKeysToShowCircles()
		{
			var centers = new[]
			{
				new Point(0.5f, 0.4f), new Point(0.5f, 0.6f), new Point(0.3f, 0.6f), new Point(0.7f, 0.6f)
			};
			var size = new Size(0.1f, 0.1f);
			CreateFontTexts(centers, size);
			AddCirclesAndInputCommands(centers, size);
		}

		private static void CreateFontTexts(Point[] centers, Size size)
		{
			new FontText(Font.Default, "Up", Rectangle.FromCenter(centers[0], size));
			new FontText(Font.Default, "Down", Rectangle.FromCenter(centers[1], size));
			new FontText(Font.Default, "Left", Rectangle.FromCenter(centers[2], size));
			new FontText(Font.Default, "Right", Rectangle.FromCenter(centers[3], size));
		}

		private static void AddCirclesAndInputCommands(Point[] centers, Size size)
		{
			var up = new Ellipse(centers[0], size.Width, size.Height, Color.Orange);
			var down = new Ellipse(centers[1], size.Width, size.Height, Color.Orange);
			var left = new Ellipse(centers[2], size.Width, size.Height, Color.Orange);
			var right = new Ellipse(centers[3], size.Width, size.Height, Color.Orange);
			new Command(() => up.Visibility = Visibility.Show).Add(new KeyTrigger(Key.CursorUp,
				State.Pressed));
			new Command(() => up.Visibility = Visibility.Hide).Add(new KeyTrigger(Key.CursorUp,
				State.Released));
			new Command(() => down.Visibility = Visibility.Show).Add(new KeyTrigger(Key.CursorDown,
				State.Pressed));
			new Command(() => down.Visibility = Visibility.Hide).Add(new KeyTrigger(Key.CursorDown,
				State.Released));
			new Command(() => left.Visibility = Visibility.Show).Add(new KeyTrigger(Key.CursorLeft,
				State.Pressed));
			new Command(() => left.Visibility = Visibility.Hide).Add(new KeyTrigger(Key.CursorLeft,
				State.Released));
			new Command(() => right.Visibility = Visibility.Show).Add(new KeyTrigger(Key.CursorRight,
				State.Pressed));
			new Command(() => right.Visibility = Visibility.Hide).Add(new KeyTrigger(Key.CursorRight,
				State.Released));
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			var trigger = new KeyTrigger(Key.Z, State.Pressed);
			Assert.AreEqual(Key.Z, trigger.Key);
			Assert.AreEqual(State.Pressed, trigger.State);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateFromString()
		{
			var trigger = new KeyTrigger("Z Pressed");
			Assert.AreEqual(Key.Z, trigger.Key);
			Assert.AreEqual(State.Pressed, trigger.State);
			Assert.Throws<KeyTrigger.CannotCreateKeyTriggerWithoutKey>(() => new KeyTrigger(""));
		}
	}
}