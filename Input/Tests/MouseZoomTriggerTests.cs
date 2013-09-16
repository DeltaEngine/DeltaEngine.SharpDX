using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class MouseZoomTriggerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			mouse = Resolve<Mouse>() as MockMouse;
		}

		private MockMouse mouse;

		[Test]
		public void ResizeEllipseByZoomTrigger()
		{
			var ellipse = new Ellipse(Point.Half, 0.1f, 0.1f, Color.Red);
			new Command(zoomAmount => { ellipse.Radius += zoomAmount * 0.02f; }).Add(new MouseZoomTrigger());
		}

		[Test]
		public void MouseWheelZoom()
		{
			bool zoom = false;
			new Command((float zoomAmount) => zoom = true).Add(new MouseZoomTrigger());
			MoveMouseWheelAndCheck(ref zoom);
		}

		private void MoveMouseWheelAndCheck(ref bool zoom)
		{
			SetMouseWheelValue(0);
			Assert.IsFalse(zoom);
			SetMouseWheelValue(1);
			Assert.IsTrue(zoom);
		}

		private void SetMouseWheelValue(int value)
		{
			mouse.SetScrollWheelValue(value);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void MouseWheelZoomUsingCommandName()
		{
			bool zoom = false;
			new Command(Command.Zoom, (float zoomAmount) => zoom = true);
			MoveMouseWheelAndCheck(ref zoom);
		}
	}
}