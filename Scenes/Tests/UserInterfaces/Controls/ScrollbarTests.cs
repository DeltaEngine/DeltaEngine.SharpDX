using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class ScrollbarTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			scrollbar = new Scrollbar(Center);
			scrollbar.Add(new FontText(Font.Default, "", new Rectangle(0.5f, 0.7f, 0.2f, 0.1f)));
			scrollbar.Start<DisplayScrollbarValue>();
			InitializeMouse();
		}

		private Scrollbar scrollbar;
		private static readonly Rectangle Center = Rectangle.FromCenter(0.5f, 0.5f, 0.5f, 0.1f);

		private class DisplayScrollbarValue : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Scrollbar scrollbar in entities)
					scrollbar.Get<FontText>().Text = scrollbar.LeftValue + " - " + scrollbar.RightValue;
			}
		}

		private void InitializeMouse()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse == null)
				return; //ncrunch: no coverage

			mouse.SetPosition(Point.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		private MockMouse mouse;

		[Test, ApproveFirstFrameScreenshot]
		public void RenderScrollbarZeroToOneHundredWithPointerWidthTwenty()
		{
			scrollbar.ValueWidth = 20;
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderScrollbarWithValueWidthEquallingValues()
		{
			scrollbar.MaxValue = 2;
			scrollbar.ValueWidth = 3;
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderScrollbarWithOneMoreValueThanValueWidth()
		{
			scrollbar.MaxValue = 3;
			scrollbar.ValueWidth = 3;
		}

		[Test, CloseAfterFirstFrame]
		public void DefaultsToEnabled()
		{
			Assert.IsTrue(scrollbar.IsEnabled);
			Assert.AreEqual(Color.White, scrollbar.Color);
			Assert.AreEqual(Color.LightGray, scrollbar.Pointer.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void DisablingScrollbarDisablesPointer()
		{
			scrollbar.IsEnabled = false;
			Assert.IsFalse(scrollbar.IsEnabled);
			Assert.IsFalse(scrollbar.Pointer.IsEnabled);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderDisabledScrollbar()
		{
			scrollbar.IsEnabled = false;
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(Color.Gray, scrollbar.Color);
			Assert.AreEqual(Color.Gray, scrollbar.Pointer.Color);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderScrollbarZeroToOneThousandWithPointerWidthFiveHundred()
		{
			scrollbar.MaxValue = 1000;
			scrollbar.ValueWidth = 500;
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderGrowingScrollbar()
		{
			scrollbar.Start<Grow>();
		}

		//ncrunch: no coverage start
		private class Grow : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Scrollbar scrollbar in entities)
				{
					var center = scrollbar.DrawArea.Center + new Point(0.01f, 0.01f) * Time.Delta;
					var size = scrollbar.DrawArea.Size * (1.0f + Time.Delta / 10);
					scrollbar.DrawArea = Rectangle.FromCenter(center, size);
				}
			}
		}

		//ncrunch: no coverage end

		[Test, CloseAfterFirstFrame]
		public void DefaultValues()
		{
			Assert.AreEqual(0, scrollbar.MinValue);
			Assert.AreEqual(99, scrollbar.MaxValue);
			Assert.AreEqual(10, scrollbar.ValueWidth);
			Assert.AreEqual(90, scrollbar.LeftValue);
			Assert.AreEqual(95, scrollbar.CenterValue);
			Assert.AreEqual(99, scrollbar.RightValue);
		}

		[Test, CloseAfterFirstFrame]
		public void UpdateValues()
		{
			scrollbar.MinValue = 1;
			scrollbar.MaxValue = 10;
			scrollbar.ValueWidth = 2;
			scrollbar.CenterValue = 4;
			Assert.AreEqual(1, scrollbar.MinValue);
			Assert.AreEqual(10, scrollbar.MaxValue);
			Assert.AreEqual(4, scrollbar.CenterValue);
			Assert.AreEqual(2, scrollbar.ValueWidth);
		}

		[Test, CloseAfterFirstFrame]
		public void ValidatePointerSize()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			Assert.AreEqual(new Size(0.05f, 0.1f), scrollbar.Pointer.DrawArea.Size);
		}

		[Test, CloseAfterFirstFrame]
		public void ValidatePointerCenter()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			var position = new Point(0.3f, 0.52f);
			DragMouse(position);
			Assert.AreEqual(new Point(0.3f, 0.5f), scrollbar.Pointer.DrawArea.Center);
		}

		private void DragMouse(Point position)
		{
			SetMouseState(State.Pressing, position + new Point(0.1f, 0.1f));
			SetMouseState(State.Pressing, position);
		}

		private void SetMouseState(State state, Point position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage

			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void RenderVerticalScrollbar()
		{
			scrollbar.MaxValue = 9;
			scrollbar.ValueWidth = 3;
			scrollbar.Rotation = 90;
		}

		[Test]
		public void Render45DegreeScrollbar()
		{
			scrollbar.Rotation = 45;
		}

		[Test]
		public void RenderSpinningScrollbar()
		{
			scrollbar.Start<Spin>();
		}

		//ncrunch: no coverage start
		private class Spin : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Scrollbar scrollbar in entities)
					scrollbar.Rotation += 20 * Time.Delta;
			}
		}

		//ncrunch: no coverage end

		[Test]
		public void RenderSpinningScrollbarAttachedToMouse()
		{
			scrollbar.Start<Spin>();
			new Command(
				point => scrollbar.DrawArea = Rectangle.FromCenter(point, scrollbar.DrawArea.Size)).Add(
					new MouseMovementTrigger());
		}
	}
}