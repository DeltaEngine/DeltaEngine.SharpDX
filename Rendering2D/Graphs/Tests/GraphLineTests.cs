using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Rendering2D.Graphs.Tests
{
	public class GraphLineTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			graph = new Graph(Center) { Viewport = Rectangle.One };
		}

		private Graph graph;
		private static readonly Rectangle Center = Rectangle.FromCenter(0.5f, 0.5f, 0.4f, 0.2f);

		[Test, CloseAfterFirstFrame]
		public void CheckLineColor()
		{
			var line = graph.CreateLine("", Color.Blue);
			line.AddPoint(Vector2D.Zero);
			line.AddPoint(Vector2D.One);
			Assert.AreEqual(Color.Blue, line.Color);
			Assert.AreEqual(Color.Blue, line.lines[0].Color);
			line.Color = Color.Green;
			Assert.AreEqual(Color.Green, line.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeLineColor()
		{
			var line = graph.CreateLine("", Color.Blue);
			line.AddPoint(Vector2D.Zero);
			line.AddPoint(Vector2D.One);
			line.Color = Color.Green;
			Assert.AreEqual(Color.Green, line.Color);
			Assert.AreEqual(Color.Green, line.lines[0].Color);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeLineKey()
		{
			var line = graph.CreateLine("ABC", Color.Blue);
			Assert.AreEqual("ABC", line.Key);
			line.Key = "DEF";
			Assert.AreEqual("DEF", line.Key);
		}

		[Test, CloseAfterFirstFrame]
		public void AddingFirstPointDoesntCreateALine()
		{
			var line = graph.CreateLine("", Color.Blue);
			Assert.AreEqual(0, line.points.Count);
			Assert.AreEqual(0, line.lines.Count);
			line.AddPoint(Vector2D.Zero);
			Assert.AreEqual(1, line.points.Count);
			Assert.AreEqual(0, line.lines.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void TwoPointsDrawALine()
		{
			GraphLine line = CreateLineWithTwoPoints();
			Assert.AreEqual(2, line.points.Count);
			Assert.AreEqual(1, line.lines.Count);
			Line2D line2D = line.lines[0];
			Assert.AreEqual(new Vector2D(0.462f, 0.5f), line2D.StartPoint);
			Assert.AreEqual(new Vector2D(0.538f, 0.462f), line2D.EndPoint);
			Assert.AreEqual(LineColor, line.Color);
		}

		private GraphLine CreateLineWithTwoPoints()
		{
			var line = graph.CreateLine("", LineColor);
			line.AddPoint(new Vector2D(0.4f, 0.5f));
			line.AddPoint(new Vector2D(0.6f, 0.7f));
			return line;
		}

		private static readonly Color LineColor = Color.Blue;

		[Test, CloseAfterFirstFrame]
		public void AddThirdPointAtTheEnd()
		{
			GraphLine line = CreateLineWithTwoPoints();
			line.AddPoint(new Vector2D(0.8f, 0.5f));
			Assert.AreEqual(3, line.points.Count);
			Assert.AreEqual(2, line.lines.Count);
			Line2D line2D = line.lines[1];
			Assert.AreEqual(new Vector2D(0.538f, 0.462f), line2D.StartPoint);
			Assert.AreEqual(new Vector2D(0.6143f, 0.5f), line2D.EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void AddThirdPointAtTheStart()
		{
			GraphLine line = CreateLineWithTwoPoints();
			line.AddPoint(new Vector2D(0.2f, 0.5f));
			Assert.AreEqual(3, line.points.Count);
			Assert.AreEqual(2, line.lines.Count);
			Line2D line0 = line.lines[0];
			Assert.AreEqual(new Vector2D(0.3857f, 0.5f), line0.StartPoint);
			Assert.AreEqual(new Vector2D(0.462f, 0.5f), line0.EndPoint);
			Line2D line1 = line.lines[1];
			Assert.AreEqual(new Vector2D(0.462f, 0.5f), line1.StartPoint);
			Assert.AreEqual(new Vector2D(0.538f, 0.462f), line1.EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void AddThirdPointInTheMiddle()
		{
			GraphLine line = CreateLineWithTwoPoints();
			line.AddPoint(new Vector2D(0.5f, 0.4f));
			Assert.AreEqual(3, line.points.Count);
			Assert.AreEqual(2, line.lines.Count);
			Line2D line0 = line.lines[0];
			Assert.AreEqual(new Vector2D(0.462f, 0.5f), line0.StartPoint);
			Assert.AreEqual(new Vector2D(0.5f, 0.519f), line0.EndPoint);
			Line2D line1 = line.lines[1];
			Assert.AreEqual(new Vector2D(0.5f, 0.519f), line1.StartPoint);
			Assert.AreEqual(new Vector2D(0.538f, 0.462f), line1.EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void RemoveFirstPoint()
		{
			GraphLine line = CreateLineWithThreePoints();
			line.RemoveAt(0);
			Assert.AreEqual(2, line.points.Count);
			Assert.AreEqual(1, line.lines.Count);
			Assert.AreEqual(new Vector2D(0.6f, 0.7f), line.points[0]);
			Assert.AreEqual(new Vector2D(0.8f, 0.4f), line.points[1]);
			Assert.AreEqual(new Vector2D(0.538f, 0.462f), line.lines[0].StartPoint);
			Assert.AreEqual(new Vector2D(0.6143f, 0.519f), line.lines[0].EndPoint);
		}

		private GraphLine CreateLineWithThreePoints()
		{
			var line = graph.CreateLine("", LineColor);
			line.AddPoint(new Vector2D(0.4f, 0.5f));
			line.AddPoint(new Vector2D(0.6f, 0.7f));
			line.AddPoint(new Vector2D(0.8f, 0.4f));
			return line;
		}

		[Test, CloseAfterFirstFrame]
		public void RemoveMiddlePoint()
		{
			GraphLine line = CreateLineWithThreePoints();
			line.RemoveAt(1);
			Assert.AreEqual(2, line.points.Count);
			Assert.AreEqual(1, line.lines.Count);
			Assert.AreEqual(new Vector2D(0.4f, 0.5f), line.points[0]);
			Assert.AreEqual(new Vector2D(0.8f, 0.4f), line.points[1]);
			Assert.AreEqual(new Vector2D(0.462f, 0.5f), line.lines[0].StartPoint);
			Assert.AreEqual(new Vector2D(0.6143f, 0.519f), line.lines[0].EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void RemoveLastPoint()
		{
			GraphLine line = CreateLineWithThreePoints();
			line.RemoveAt(2);
			Assert.AreEqual(2, line.points.Count);
			Assert.AreEqual(1, line.lines.Count);
			Assert.AreEqual(new Vector2D(0.462f, 0.5f), line.lines[0].StartPoint);
			Assert.AreEqual(new Vector2D(0.538f, 0.462f), line.lines[0].EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void ClearRemovesAllLinesAndClearsAllPoints()
		{
			GraphLine line = CreateLineWithTwoPoints();
			Line2D line2D = line.lines[0];
			Assert.IsTrue(line2D.IsActive);
			line.Clear();
			Assert.AreEqual(0, line.lines.Count);
			Assert.AreEqual(0, line.points.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void RefreshDoesNothingIfViewportDidntChange()
		{
			GraphLine line = CreateLineWithTwoPoints();
			Vector2D start = line.lines[0].StartPoint;
			Vector2D end = line.lines[0].EndPoint;
			line.Refresh();
			Assert.AreEqual(start, line.lines[0].StartPoint);
			Assert.AreEqual(end, line.lines[0].EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void RefreshUpdatesLinesIfViewportChanged()
		{
			GraphLine line = CreateLineWithTwoPoints();
			line.graph.Viewport = Rectangle.FromCenter(0.4f, 0.4f, 0.8f, 0.8f);
			line.Refresh();
			Assert.AreEqual(new Vector2D(0.5f, 0.4762f), line.lines[0].StartPoint);
			Assert.AreEqual(new Vector2D(0.5952f, 0.4286f), line.lines[0].EndPoint);
		}

		[Test, CloseAfterFirstFrame]
		public void AddValueAddsToTheEnd()
		{
			GraphLine line = CreateLineWithTwoPoints();
			line.AddValue(0.4f, 4.0f);
			Assert.AreEqual(new Vector2D(1.0f, 4.0f), line.points[2]);
			line.AddValue(3.0f);
			Assert.AreEqual(new Vector2D(2.0f, 3.0f), line.points[3]);
		}
	}
}