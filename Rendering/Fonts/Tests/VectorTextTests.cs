using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Fonts.Tests
{
	public class VectorTextTests : TestWithMocksOrVisually
	{
		[Test, ApproveFirstFrameScreenshot]
		public void DrawHi()
		{
			new VectorText("Hi", Point.Half);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawSampleText()
		{
			new VectorText("The Quick Brown Fox...", Point.Half) { Color = Color.Red };
			new VectorText("Jumps Over The Lazy Dog", new Point(0.5f, 0.6f)) { Color = Color.Teal };
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawBigText()
		{
			new VectorText("Yo yo, whats up", Point.Half) { Size = new Size(0.1f) };
		}

		[Test, CloseAfterFirstFrame]
		public void DrawingTwoVectorTextsWithTheSameRenderLayerOnlyIssuesOneDrawCall()
		{
			new VectorText("Yo yo, whats up", Point.Half) { Size = new Size(0.1f) };
			new VectorText("Jumps Over The Lazy Dog", new Point(0.5f, 0.6f)) { Color = Color.Teal };
			RunAfterFirstFrame(
				() => Assert.AreEqual(1, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame));
		}

		[Test, CloseAfterFirstFrame]
		public void DrawingTwoVectorTextsWithDifferentRenderLayersIssuesTwoDrawCalls()
		{
			new VectorText("Yo yo, whats up", Point.Half) { Size = new Size(0.1f), RenderLayer = 1 };
			new VectorText("Jumps Over The Lazy Dog", Point.One) { Color = Color.Teal, RenderLayer = 2 };
			RunAfterFirstFrame(
				() => Assert.AreEqual(2, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame));
		}

		[Test, CloseAfterFirstFrame]
		public void RenderingHiddenVectorTextDoesNotThrowException()
		{
			new VectorText("The Quick Brown Fox...", Point.Half) { Visibility = Visibility.Hide };
			Assert.DoesNotThrow(() => AdvanceTimeAndUpdateEntities());
		}

		[Test, ApproveFirstFrameScreenshot]
		public void ChangeText()
		{
			var text = new VectorText("Unchanged", Point.Half) { Text = "Changed" };
			Assert.AreEqual("Changed", text.Text);
		}
	}
}