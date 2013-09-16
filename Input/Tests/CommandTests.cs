using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class CommandTests : TestWithMocksOrVisually
	{
		[Test]
		public void CallingExitWillCloseTheProject()
		{
			new Command(Command.Exit, () => Resolve<Window>().CloseAfterFrame());
		}

		[Test]
		public void ClickingWillChangeBackgroundColor()
		{
			new Command(Command.Click, () => Resolve<Window>().BackgroundColor = Color.GetRandomColor());
		}

		[Test]
		public void MiddleClickingWillChangeBackgroundColor()
		{
			new Command(Command.MiddleClick,
				() => Resolve<Window>().BackgroundColor = Color.GetRandomColor());
		}

		[Test]
		public void RightClickingWillChangeBackgroundColor()
		{
			new Command(Command.RightClick,
				() => Resolve<Window>().BackgroundColor = Color.GetRandomColor());
		}

		[Test]
		public void ZoomWillChangeBackgroundColor()
		{
			new Command(Command.Zoom,
				() => Resolve<Window>().BackgroundColor = Color.GetRandomColor());
		}

		[Test]
		public void MovingARectangleWithTheArrows()
		{
			var rect = new FilledRect(new Rectangle(0.4f, 0.4f, 0.2f, 0.2f), Color.Green);
			new Command(Command.MoveDown,
				() =>
					rect.DrawArea =
						new Rectangle(rect.DrawArea.Left, rect.DrawArea.Top + 0.01f, rect.DrawArea.Width,
							rect.DrawArea.Height));
			new Command(Command.MoveUp,
				() =>
					rect.DrawArea =
						new Rectangle(rect.DrawArea.Left, rect.DrawArea.Top - 0.01f, rect.DrawArea.Width,
							rect.DrawArea.Height));
			new Command(Command.MoveLeft,
				() =>
					rect.DrawArea =
						new Rectangle(rect.DrawArea.Left - 0.01f, rect.DrawArea.Top, rect.DrawArea.Width,
							rect.DrawArea.Height));
			new Command(Command.MoveRight,
				() =>
					rect.DrawArea =
						new Rectangle(rect.DrawArea.Left + 0.01f, rect.DrawArea.Top, rect.DrawArea.Width,
							rect.DrawArea.Height));
		}

		[Test]
		public void MovingDirectlyWillChange()
		{
			new Command(Command.MoveDirectly,
				() => Resolve<Window>().BackgroundColor = Color.GetRandomColor());
		}

		[Test]
		public void RotateRectangleWithMouse()
		{
			var rect = new FilledRect(new Rectangle(0.4f, 0.4f, 0.2f, 0.2f), Color.Green);
			new Command(Command.RotateDirectly, delegate(Point point) { rect.Rotation += 1; });
		}

		[Test]
		public void BackWillRemoveRectangle()
		{
			var rect = new FilledRect(new Rectangle(0.4f, 0.4f, 0.2f, 0.2f), Color.Green);
			new Command(Command.Back, () => { rect.IsActive = false; });
		}

		[Test]
		public void TouchToMoveTheRectangleARound()
		{
			var rect = new FilledRect(new Rectangle(0.4f, 0.4f, 0.2f, 0.2f), Color.Green);
			new Command(Command.Drag, point => { rect.Center = point; });
		}

		[Test]
		public void FlickToChangeTheColor()
		{
			new Command(Command.Flick,
				delegate(Point point) { Resolve<Window>().BackgroundColor = Color.GetRandomColor(); });
		}

		[Test]
		public void PitchToChangeTheScale()
		{
			var rect = new FilledRect(new Rectangle(0.4f, 0.4f, 0.2f, 0.2f), Color.Green);
			new Command(Command.Pinch,
				delegate(Point point) {
					rect.DrawArea = new Rectangle(rect.DrawArea.Left, rect.DrawArea.Top,
						rect.DrawArea.Width - 0.01f, rect.DrawArea.Height);
				});
		}

		[Test]
		public void NotMovingWillCHangeTheColor()
		{
			new Command(Command.Hold,
				() => { Resolve<Window>().BackgroundColor = Color.GetRandomColor(); });
		}

		[Test]
		public void DoubleCLickingWillChangeTheColor()
		{
			new Command(Command.DoubleClick,
				() => { Resolve<Window>().BackgroundColor = Color.GetRandomColor(); });
		}

		[Test]
		public void UsingRotationOnATouchPadWillRotateTheRectangle()
		{
			var rect = new FilledRect(new Rectangle(0.4f, 0.4f, 0.2f, 0.2f), Color.Green);
			new Command(Command.Rotate, delegate(Point point) { rect.Rotation += 1; });
		}
	}
}