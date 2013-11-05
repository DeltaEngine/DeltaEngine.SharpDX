using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

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
		public void HiddenLabelDoesNotRender()
		{
			label.IsVisible = false;
		}

		[Test]
		public void InactiveLabelDoesNotRender()
		{
			label.IsActive = false;
		}

		[Test]
		public void RenderLabelAttachedToMouse()
		{
			new Command(point => label.DrawArea = Rectangle.FromCenter(point, label.DrawArea.Size)).Add(
				new MouseMovementTrigger());
		}

		[Test]
		public void ReportIfInsideRotatedLabel()
		{
			label.Text = "";
			label.Rotation = 30;
			label.Start<ChangeLabelText>();
		}

		//ncrunch: no coverage start
		private class ChangeLabelText : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Label label in entities)
				{
					string isInside = label.State.IsInside ? "Inside" : "Outside";
					label.Text = isInside + " - Relative Mouse Position: " +
						label.State.RelativePointerPosition;
				}
			}
		}

		//ncrunch: no coverage end

		[Test, CloseAfterFirstFrame]
		public void ColorDoesNotInterpolateAtCreation()
		{
			Assert.AreEqual(label.Color, label.LastColor);
		}

		[Test, CloseAfterFirstFrame]
		public void SaveAndLoad()
		{
			var stream = BinaryDataExtensions.SaveToMemoryStream(label);
			var loadedLabel = (Label)stream.CreateFromMemoryStream();
			Assert.AreEqual(Center, loadedLabel.DrawArea);
			Assert.AreEqual("Hello World", loadedLabel.Text);
			Assert.AreEqual(label.children.Count, loadedLabel.children.Count);
		}

		[Test]
		public void DrawLoadedLabel()
		{
			label.Text = "Original";
			var stream = BinaryDataExtensions.SaveToMemoryStream(label);
			var loadedLabel = (Label)stream.CreateFromMemoryStream();
			loadedLabel.Text = "Loaded";
			loadedLabel.DrawArea = loadedLabel.DrawArea.Move(0.0f, 0.15f);
		}
	}
}