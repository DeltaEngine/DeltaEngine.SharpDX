using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;

namespace Blocks
{
	/// <summary>
	/// Holds the individual bricks making up a block and handles rotating them
	/// </summary>
	public class Block : Entity
	{
		public Block(Orientation displayMode, BlocksContent content, Point topLeft)
		{
			this.content = content;
			CreateBricks();
			Left = topLeft.X;
			Top = topLeft.Y;
			this.displayMode = displayMode;
		}

		private readonly BlocksContent content;

		private void CreateBricks()
		{
			int numberOfBricks = content.AreFiveBrickBlocksAllowed
				? GetNumberOfBricks() : NormalNumberOfBricks;
			var image = content.Load<Image>("Block" + Randomizer.Current.Get(1, 8));
			var shader = ContentLoader.Load<Shader>(Shader.Position2DColorUv);
			var material = new Material(shader, image);
			var newBrick = new Brick(material, Point.Zero, displayMode);
			Bricks = new List<Brick> { newBrick };
			for (int i = 1; i < numberOfBricks; i++)
				AddBrick(Bricks[i - 1], material);

			ShiftToTopLeft();
		}

		private const int NormalNumberOfBricks = 4;
		public List<Brick> Bricks { get; private set; }

		private static int GetNumberOfBricks()
		{
			return Randomizer.Current.Get() < 0.9f ? NormalNumberOfBricks : NormalNumberOfBricks + 1;
		}

		private void AddBrick(Brick lastBrick, Material material)
		{
			Brick newBrick;
			do
				newBrick = new Brick(material, lastBrick.Offset + GetRandomOffset(), displayMode)
				{
					IsActive = false
				}; while (Bricks.Any(brick => brick.Offset == newBrick.Offset));

			Bricks.Add(newBrick);
			newBrick.IsActive = true;
		}

		private static Point GetRandomOffset()
		{
			return Randomizer.Current.Get(0, 2) == 0
				? new Point(Randomizer.Current.Get(0, 2) * 2 - 1, 0)
				: new Point(0, Randomizer.Current.Get(0, 2) * 2 - 1);
		}

		private void ShiftToTopLeft()
		{
			var left = (int)Bricks.Min(brick => brick.Offset.X);
			var top = (int)Bricks.Min(brick => brick.Offset.Y);
			foreach (Brick brick in Bricks)
				brick.Offset = new Point(brick.Offset.X - left, brick.Offset.Y - top);

			UpdateCenter();
		}

		private void UpdateCenter()
		{
			float minX = Bricks.Min(brick => brick.Offset.X);
			float maxX = Bricks.Max(brick => brick.Offset.X);
			float minY = Bricks.Min(brick => brick.Offset.Y);
			float maxY = Bricks.Max(brick => brick.Offset.Y);
			center = new Point((minX + maxX + 1) / 2, (minY + maxY + 1) / 2);
		}

		private Point center;

		public Point Center
		{
			get { return center; }
		}

		public float Left
		{
			get { return Bricks[0].TopLeftGridCoord.X; }
			set
			{
				foreach (Brick brick in Bricks)
					brick.TopLeftGridCoord.X = value;
			}
		}

		public float Top
		{
			get { return Bricks[0].TopLeftGridCoord.Y; }
			set
			{
				foreach (Brick brick in Bricks)
					brick.TopLeftGridCoord.Y = value;
			}
		}

		private readonly Orientation displayMode;

		public void RotateClockwise()
		{
			Point oldCenter = center;
			foreach (Brick brick in Bricks)
				brick.Offset = new Point(-brick.Offset.Y, brick.Offset.X);

			ShiftToTopLeft();
			Left += (int)oldCenter.X - (int)center.X;
		}

		public void RotateAntiClockwise()
		{
			Point oldCenter = center;
			foreach (Brick brick in Bricks)
				brick.Offset = new Point(brick.Offset.Y, -brick.Offset.X);

			ShiftToTopLeft();
			Left += (int)oldCenter.X - (int)center.X;
		}

		public void UpdateBrickDrawAreas(float fallSpeed)
		{
			Top += MathExtensions.Min(fallSpeed * Time.Delta, 1.0f);
			foreach (var brick in Bricks)
				brick.UpdateDrawArea();
		}

		public override string ToString()
		{
			string result = "";
			for (int y = 0; y < Bricks.Count; y++)
				result += LineToString(y);

			return result;
		}

		private string LineToString(int y)
		{
			string line = y > 0 ? "/" : "";
			for (int x = 0; x < Bricks.Count; x++)
				line += Bricks.Any(brick => brick.Offset == new Point(x, y)) ? 'O' : '.';

			return line;
		}
	}
}