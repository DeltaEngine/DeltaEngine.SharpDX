using CreepyTowers.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Shapes;

namespace CreepyTowers.Simple2D
{
	/// <summary>
	/// Places a tower in the BasicDisplaySystem grid
	/// TODO: separate Tower2D from Tower2DView
	/// </summary>
	public class Tower2D : FilledRect, Updateable
	{
		public Tower2D(Basic2DDisplaySystem display, Vector2D position, Tower.TowerType towerType)
			: base(display.CalculateGridScreenDrawArea(position), GetColor(towerType))
		{
			this.display = display;
			Type = towerType;
			Position = position;
			data = new TowerPropertiesXmlParser().TowerPropertiesData[towerType];
			PaintAttackType();
			this.display.Gold -= data.Cost;
			RenderLayer = 3;
		}

		internal readonly Basic2DDisplaySystem display;
		private readonly TowerProperties data;

		public Tower.TowerType Type { get; private set; }
		public Vector2D Position { get; private set; }

		private void PaintAttackType()
		{
			if (data.AttackType == Tower.AttackType.RadiusFull)
				PaintRadiusFull();
			else if (data.AttackType == Tower.AttackType.CircleSector)
				PaintCircleSector();
		}

		private void PaintRadiusFull()
		{
			var realPosition = display.GetPositionOfNode((int)Position.X, (int)Position.Y);
			radiusDrawing = new Ellipse(realPosition, 0.05f, 0.05f, Color.LightGray);
			radiusDrawing.RenderLayer = -1;
		}

		private Ellipse radiusDrawing;

		private void PaintCircleSector()
		{
			circleSectorLines = new Line2D[2];
			var realPosition = display.GetPositionOfNode((int)Position.X, (int)Position.Y);
			var offsetPosition = realPosition + new Vector2D(0.0f, 0.1f);
			PaintLines(offsetPosition, realPosition);
		}

		private Line2D[] circleSectorLines;

		private void PaintLines(Vector2D offsetPosition, Vector2D realPosition)
		{
			var rightLine = offsetPosition.RotateAround(realPosition, 15) - realPosition;
			var leftLine = offsetPosition.RotateAround(realPosition, -15) - realPosition;
			rightLine = Vector2D.Normalize(rightLine);
			leftLine = Vector2D.Normalize(leftLine);
			circleSectorLines[0] = new Line2D(realPosition, realPosition + rightLine * 0.2f, Color.White);
			circleSectorLines[1] = new Line2D(realPosition, realPosition + leftLine * 0.2f, Color.White);
		}

		//public void UpdateTowerOrientation(Vector2D position)
		//{
		//	circleSectorLines[0].IsActive = false;
		//	circleSectorLines[1].IsActive = false;
		//	var realPosition = display.GetPositionOfNode((int)Position.X, (int)Position.Y);
		//	var offsetPosition = display.GetPositionOfNode((int)position.X, (int)position.Y);
		//	PaintLines(offsetPosition, realPosition);
		//}

		public static Color GetColor(Tower.TowerType type)
		{
			switch (type)
			{
			case Tower.TowerType.Acid:
				return Color.Green;
			case Tower.TowerType.Slice:
				return Color.LightGray;
			case Tower.TowerType.Fire:
				return Color.Red;
			case Tower.TowerType.Ice:
				return Color.LightBlue;
			case Tower.TowerType.Impact:
				return Color.Brown;
			default:
				return Color.Blue;
			}
		}

		public float Range
		{
			get { return data.Range; }
		}
		public float Damage
		{
			get { return data.AttackDamage; }
		}

		public void Update()
		{
			if (cooldown > 0)
				cooldown -= Time.Delta;
			else
				IsOnCooldown = false;
		}

		private float cooldown;
		public bool IsOnCooldown { get; internal set; }

		public void SetOnCooldown()
		{
			cooldown = data.AttackFrequency;
			IsOnCooldown = true;
		}

		public void Dispose()
		{
			display.Gold += data.Cost / 2;
			if (radiusDrawing != null)
			{
				radiusDrawing.IsActive = false;
				radiusDrawing = null;
			}
			if (circleSectorLines != null)
				foreach (var line in circleSectorLines)
					line.IsActive = false;
			IsActive = false;
		}

		public Tower.AttackType GetAttackType()
		{
			return data.AttackType;
		}

		public void UpdateTowerOrientation(Vector2D position)
		{
			var realPosition = display.GetPositionOfNode((int)Position.X, (int)Position.Y);
			var offsetPosition = display.GetPositionOfNode((int)position.X, (int)position.Y);
			var rightLine = offsetPosition.RotateAround(realPosition, 15) - realPosition;
			var leftLine = offsetPosition.RotateAround(realPosition, -15) - realPosition;
			rightLine = Vector2D.Normalize(rightLine);
			leftLine = Vector2D.Normalize(leftLine);
			circleSectorLines[0].IsActive = false;
			circleSectorLines[1].IsActive = false;
			circleSectorLines[0] = new Line2D(realPosition, realPosition + rightLine * 0.2f, Color.White);
			circleSectorLines[1] = new Line2D(realPosition, realPosition + leftLine * 0.2f, Color.White);
		}

		public bool IsPauseable { get { return true; } }
	}
}