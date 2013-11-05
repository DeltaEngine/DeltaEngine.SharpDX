using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Shapes;

namespace $safeprojectname$
{
	public class Body
	{
		public Body(int gridSize, Color color)
		{
			this.gridSize = gridSize;
			blockSize = 1.0f / gridSize;
			this.color = color;
			Spawn$safeprojectname$();
		}

		private readonly int gridSize;
		private readonly float blockSize;
		private Color color;

		private void Spawn$safeprojectname$()
		{
			BodyParts = new List<FilledRect>();
			Direction = new Vector2D(0, -blockSize);
			Place$safeprojectname$Head();
			Add$safeprojectname$Body();
		}

		public List<FilledRect> BodyParts { get; private set; }
		public Vector2D Direction { get; set; }
		public Vector2D HeadPosition
		{
			get
			{
				if (BodyParts.Count == 0)
					return Vector2D.Half;
				return BodyParts[0].Center;
			}
		}

		private void Place$safeprojectname$Head()
		{
			var startPosition = blockSize * (float)Math.Floor(gridSize / 2.0f);
			var firstPart = new FilledRect(CalculateHeadDrawArea(startPosition), color) {RenderLayer = 3};
			BodyParts.Add(firstPart);
		}

		private Rectangle CalculateHeadDrawArea(float startPosition)
		{
			return new Rectangle(new Vector2D(startPosition, startPosition), new Size(1.0f / gridSize));
		}

		public void Add$safeprojectname$Body()
		{
			var snakeHead = BodyParts[BodyParts.Count - 1].DrawArea;
			var newTail = new FilledRect(CalculateBodyDrawArea(snakeHead), color){RenderLayer = 3};
			BodyParts.Add(newTail);
		}

		private Rectangle CalculateBodyDrawArea(Rectangle snakeHead)
		{
			return new Rectangle(snakeHead.Left, snakeHead.Top + blockSize, blockSize, blockSize);
		}

		internal void MoveBody()
		{
			trailingVector = GetTrailingVector();
			MoveBodyTowardsHead();
			MoveHeadInDesiredDirection();
		}

		private Vector2D trailingVector;

		public Vector2D GetTrailingVector()
		{
			var tail = BodyParts[BodyParts.Count - 1].DrawArea.TopLeft;
			var partBeforeTail = BodyParts[BodyParts.Count - 2].DrawArea.TopLeft;
			return new Vector2D(tail.X - partBeforeTail.X, tail.Y - partBeforeTail.Y);
		}

		private void MoveBodyTowardsHead()
		{
			for (int count = BodyParts.Count - 1; count >= 1; count--)
				BodyParts[count].DrawArea = BodyParts[count - 1].DrawArea;
		}

		private void MoveHeadInDesiredDirection()
		{
			var newHeadPos = new Vector2D(BodyParts[0].DrawArea.Left + Direction.X,
				BodyParts[0].DrawArea.Top + Direction.Y);
			BodyParts[0].DrawArea = new Rectangle(newHeadPos, new Size(blockSize));
		}

		internal void Check$safeprojectname$CollidesWithChunk()
		{
			if (Detect$safeprojectname$CollisionWithChunk != null)
				Detect$safeprojectname$CollisionWithChunk(trailingVector);
		}

		public event Action<Vector2D> Detect$safeprojectname$CollisionWithChunk;

		internal void Check$safeprojectname$CollisionWithBorderOrItself()
		{
			if ($safeprojectname$CollidesWithBorderOrItself == null)
				return; //ncrunch: no coverage
			var snakeHead = BodyParts[0];
			if ($safeprojectname$CollidesWithItself(snakeHead) || $safeprojectname$CollidesWithBorders(snakeHead))
				$safeprojectname$CollidesWithBorderOrItself();
		}

		private bool $safeprojectname$CollidesWithItself(FilledRect snakeHead)
		{
			for (int count = 3; count < BodyParts.Count; count++)
				if (snakeHead.DrawArea.TopLeft == BodyParts[count].DrawArea.TopLeft)
					return true;
			return false;
		}

		private bool $safeprojectname$CollidesWithBorders(FilledRect snakeHead)
		{
			if ((snakeHead.DrawArea.Left < blockSize - 0.01f ||
				snakeHead.DrawArea.Top < blockSize - 0.01f ||
				snakeHead.DrawArea.Left > 1 - blockSize - 0.01f ||
				snakeHead.DrawArea.Top > 1 - blockSize - 0.01f))
				return true;
			return false;
		}

		public event Action $safeprojectname$CollidesWithBorderOrItself;
	}
}