using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;
using Randomizer = DeltaEngine.Core.Randomizer;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class LabelTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			label = new Label(Center, "Hello World");
		}

		private Label label;

		[Test, ApproveFirstFrameScreenshot]
		public void RenderGrowingLabel()
		{
			label.Start<Grow>();
		}

		//ncrunch: no coverage start
		private class Grow : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Label label in entities)
				{
					var center = label.DrawArea.Center + new Vector2D(0.01f, 0.01f) * Time.Delta;
					var size = label.DrawArea.Size * (1.0f + Time.Delta / 10);
					label.DrawArea = Rectangle.FromCenter(center, size);
				}
			}
		}

		//ncrunch: no coverage end

		private static readonly Rectangle Center = Rectangle.FromCenter(0.5f, 0.5f, 0.3f, 0.1f);

		[Test, CloseAfterFirstFrame]
		public void InitialText()
		{
			Assert.AreEqual("Hello World", label.Text);
			Assert.AreEqual("Hello World", label.PreviousText);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeTextOnce()
		{
			label.Text = "Change 1";
			Assert.AreEqual("Change 1", label.Text);
			Assert.AreEqual("Hello World", label.PreviousText);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeTextTwice()
		{
			label.Text = "Change 1";
			label.Text = "Change 2";
			Assert.AreEqual("Change 2", label.Text);
			Assert.AreEqual("Change 1", label.PreviousText);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangingLabelVisibilityChangesFontTextVisibility()
		{
			Assert.IsTrue(label.IsVisible);
			Assert.IsTrue(label.Get<FontText>().IsVisible);
			label.IsVisible = false;
			Assert.IsFalse(label.IsVisible);
			Assert.IsFalse(label.Get<FontText>().IsVisible);
		}

		[Test]
		public void RenderLabelsThatChangeColorWhenInsideRubberBandSelection()
		{
			label.IsVisible = false;
			CreateLabels();
			CreateRubberBand();
		}

		private static void CreateLabels()
		{
			for (int i = 1; i <= 10; i++)
				new Label(CreateRandomRectangle()).Start<ChangeColorIfInsideMouseDrag>();
		}

		private static Rectangle CreateRandomRectangle()
		{
			return Rectangle.FromCenter(Randomizer.Current.Get(0.2f, 0.8f),
				Randomizer.Current.Get(0.3f, 0.7f), Randomizer.Current.Get(0.05f, 0.15f),
				Randomizer.Current.Get(0.05f, 0.15f));
		}

		//ncrunch: no coverage start
		private class ChangeColorIfInsideMouseDrag : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Label label in entities)
					label.Color = label.DrawArea.Contains(label.State.DragStart) ? Color.Red : Color.Blue;
			}
		}
		//ncrunch: no coverage end

		private static void CreateRubberBand()
		{
			var rectangle = new FilledRect(Rectangle.Unused, TransparentWhite);
			new Command((start, end, done) =>
			{
				rectangle.DrawArea = Rectangle.FromCorners(start, end);
				if (done)
					rectangle.DrawArea = Rectangle.Unused;
			}).Add(new MouseDragTrigger()).Add(new TouchDragTrigger());
		}

		private static readonly Color TransparentWhite = new Color(1.0f, 1.0f, 1.0f, 0.3f);

		[Test]
		public void RenderLabelAttachedToMouse()
		{
			new Command(point => label.DrawArea = Rectangle.FromCenter(point, label.DrawArea.Size)).Add(
				new MouseMovementTrigger());
		}

		[Test]
		public void ChangeColorIfInsideRotatedLabel()
		{
			label.Text = "";
			label.Rotation = 30;
			label.Start<ChangeColorIfMouseInside>();
		}

		//ncrunch: no coverage start
		private class ChangeColorIfMouseInside : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Label label in entities)
				{
					label.Color = label.State.IsInside ? Color.Red : Color.White;
					label.Text = "Relative Mouse Position: " + label.State.RelativePointerPosition;
				}
			}
		}
	}
}