using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Shapes;
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
			var ellipse = new Ellipse(Vector2D.Half, 0.1f, 0.1f, Color.Red);
			new Command(zoomAmount => { ellipse.Radius += zoomAmount * 0.02f; }).Add(
				new MouseZoomTrigger());
		}

		[Test]
		public void MouseWheelZoom()
		{
			bool isZoomed = false;
			new Command((float zoomAmount) => isZoomed = true).Add(new MouseZoomTrigger());
			mouse.ScrollUp();
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(isZoomed);
		}

		[Test]
		public void MouseWheelZoomUsingCommandName()
		{
			bool isZoomed = false;
			new Command(Command.Zoom, (float zoomAmount) => isZoomed = true);
			mouse.ScrollUp();
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(isZoomed);
		}
	}
}