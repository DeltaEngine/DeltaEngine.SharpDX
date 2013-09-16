using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using NUnit.Framework;
using Randomizer = DeltaEngine.Core.Randomizer;

namespace DeltaEngine.Graphics.Tests
{
	public class DrawingTests : TestWithMocksOrVisually
	{
		[Test, ApproveFirstFrameScreenshot]
		public void DrawRedLine()
		{
			new Line(Point.Zero, new Point(1280, 720), Color.Red);
			RunAfterFirstFrame(
				() => Assert.AreEqual(2, Resolve<Drawing>().NumberOfDynamicVerticesDrawnThisFrame));
		}

		public sealed class Line : DrawableEntity
		{
			public Line(Point start, Point end, Color color)
			{
				Add(new[]
				{ new VertexPosition2DColor(start, color), new VertexPosition2DColor(end, color) });
				OnDraw<DrawLine>();
			}

			public class DrawLine : DrawBehavior
			{
				public DrawLine(Drawing drawing, Window window)
				{
					this.drawing = drawing;
					this.window = window;
					testName = AssemblyExtensions.GetTestNameOrProjectName();
					material = new Material(Shader.Position2DColor, "");
				}

				private readonly Drawing drawing;
				private readonly Window window;
				private readonly string testName;
				private readonly Material material;

				public void Draw(IEnumerable<DrawableEntity> entities)
				{
					window.Title = testName + " Fps: " + GlobalTime.Current.Fps;
					foreach (var entity in entities)
						drawing.AddLines(material, entity.Get<VertexPosition2DColor[]>());
				}
			}
		}

		[Test]
		public void IncreaseNumberOfLinesOverTime()
		{
			new LineAdder(Point.Zero, new Point(100, 80), Color.Red);
		}

		public sealed class LineAdder : DrawableEntity, Updateable
		{
			public LineAdder(Point start, Point end, Color color)
			{
				Add(new[]
				{ new VertexPosition2DColor(start, color), new VertexPosition2DColor(end, color) });
				OnDraw<Line.DrawLine>();
			}

			public void Update()
			{
				if (Time.CheckEvery(0.1f))
					CreateRandomLines((int)(Time.Total * 10));
			}

			private void CreateRandomLines(int numberOfLines)
			{
				var oldVertices = Get<VertexPosition2DColor[]>();
				var vertices = new VertexPosition2DColor[2 * numberOfLines];
				var random = Randomizer.Current;
				for (int i = 0; i < oldVertices.Length; i++)
					vertices[i] = oldVertices[i];
				for (int i = oldVertices.Length / 2; i < numberOfLines; i++)
				{
					var startPoint = new Point(random.Get(0, 1280), random.Get(0, 720));
					var endPoint = startPoint + new Point(random.Get(-100, 100), random.Get(-100, 100));
					vertices[i * 2 + 0] = new VertexPosition2DColor(startPoint, Color.GetRandomColor());
					vertices[i * 2 + 1] = new VertexPosition2DColor(endPoint, Color.GetRandomColor());
				}
				Set(vertices);
			}
		}

		/// <summary>
		/// Via Lerp we can do fun things like update all lines to new positions each update tick and
		/// the drawing will interpolate between them. Here we use just 2 update ticks per second.
		/// </summary>
		[Test]
		public void WrapLinesRandomly()
		{
			EntitiesRunner.Current.ChangeUpdateTimeStep(0.5f);
			new RandomLines(100);
		}

		public sealed class RandomLines : DrawableEntity, Updateable
		{
			public RandomLines(int numberOfLines)
			{
				CreateInitialVertices(numberOfLines);
				Add(vertices);
				Update();
				OnDraw<DrawLine>();
			}

			private void CreateInitialVertices(int numberOfLines)
			{
				var random = Randomizer.Current;
				for (int line = 0; line < numberOfLines; line++)
				{
					var startPoint = new Point(random.Get(0, 640), random.Get(0, 360));
					var endPoint = startPoint + new Point(random.Get(-50, 50), random.Get(-50, 50));
					vertices.Add(new VertexPosition2DColor(startPoint, Color.GetRandomColor()));
					vertices.Add(new VertexPosition2DColor(endPoint, Color.GetRandomColor()));
				}
			}

			private readonly List<VertexPosition2DColor> vertices = new List<VertexPosition2DColor>();

			public void Update()
			{
				var random = Randomizer.Current;
				for (int line = 0; line < vertices.Count; line++)
					vertices[line] =
						new VertexPosition2DColor(
							vertices[line].Position + new Point(random.Get(-30, 30), random.Get(-20, 20)),
							Color.GetRandomColor());
			}

			public class DrawLine : DrawBehavior
			{
				public DrawLine(Drawing draw, Window window)
				{
					this.draw = draw;
					this.window = window;
					testName = AssemblyExtensions.GetTestNameOrProjectName();
					material = new Material(Shader.Position2DColor, "");
				}

				private readonly Drawing draw;
				private readonly Window window;
				private readonly string testName;
				private readonly Material material;

				public void Draw(IEnumerable<DrawableEntity> entities)
				{
					window.Title = testName + " Fps: " + GlobalTime.Current.Fps;
					foreach (var entity in entities)
						draw.AddLines(material, entity.GetInterpolatedList<VertexPosition2DColor>().ToArray());
				}
			}
		}

		[Test, ApproveFirstFrameScreenshot]
		public void ShowYellowLineFullscreen()
		{
			var settings = Resolve<Settings>();
			settings.StartInFullscreen = true;
			settings.Resolution = new Size(1920, 1080);
			new Line(Point.Zero, settings.Resolution, Color.Yellow);
			settings.StartInFullscreen = false;
			settings.Resolution = new Size(640, 360);
		}
	}
}