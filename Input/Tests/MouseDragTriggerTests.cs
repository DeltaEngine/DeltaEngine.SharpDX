using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class MouseDragTriggerTests : TestWithMocksOrVisually
	{
		[Test]
		public void DragMouseToCreateRectangles()
		{
			var rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			new Command((start, end, done) =>
			{
				rectangle.DrawArea = Rectangle.FromCorners(start, end);
				if (done)
					rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			}).Add(new MouseDragTrigger());
		}

		[Test, CloseAfterFirstFrame]
		public void Create()
		{
			Assert.AreEqual(MouseButton.Left, new MouseDragTrigger().Button);
			Assert.AreEqual(MouseButton.Right, new MouseDragTrigger(MouseButton.Right).Button);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateFromString()
		{
			Assert.AreEqual(MouseButton.Left, new MouseDragTrigger("").Button);
			Assert.AreEqual(MouseButton.Right, new MouseDragTrigger("Right").Button);
		}

		[Test]
		public void DragMouseHorizontalToCreateRectangles()
		{
			var rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			new Command((start, end, done) =>
			{
				rectangle.DrawArea = new Rectangle(start.X, start.Y - 0.01f, end.X - start.X, 0.02f);
				if (done)
					rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			}).Add(new MouseDragTrigger(MouseButton.Left, DragDirection.Horizontal));
		}

		[Test]
		public void DragMouseVerticalToCreateRectangles()
		{
			var rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			new Command((start, end, done) =>
			{
				rectangle.DrawArea = new Rectangle(start.X - 0.01f, start.Y, 0.02f, end.Y - start.Y);
				if (done)
					rectangle = new FilledRect(Rectangle.Unused, Color.GetRandomColor());
			}).Add(new MouseDragTrigger(MouseButton.Left, DragDirection.Vertical));
		}
	}
}