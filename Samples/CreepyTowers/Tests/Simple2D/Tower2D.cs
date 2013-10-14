using CreepyTowers.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Shapes;

namespace CreepyTowers.Tests.Simple2D
{
	//TODO: merge with Tower
	/// <summary>
	/// Places a tower in the BasicDisplaySystem grid
	/// </summary>
	public class Tower2D : FilledRect, Updateable
	{
		public Tower2D(Basic2DDisplaySystem display, Vector2D position, TowerType towerType)
			: base(display.CalculateGridScreenDrawArea(position), GetColor(towerType))
		{
			this.display = display;
			Type = towerType;
			Position = position;
			data = new TowerPropertiesXml("TowerProperties").Get(towerType);
			PaintAttackType();
			this.display.Gold -= data.Cost;
			RenderLayer = 3;
			IsTowerActive = true;
		}

		internal readonly Basic2DDisplaySystem display;
		private readonly TowerData data;

		public TowerType Type { get; private set; }
		public Vector2D Position { get; private set; }
		public bool IsTowerActive { get; set; }

		private void PaintAttackType()
		{
			if (data.AttackType == AttackType.RadiusFull)
				PaintRadiusFull();
			else if (data.AttackType == AttackType.CircleSector)
				PaintCircleSector();
		}

		private void PaintRadiusFull()
		{
			radiusDrawing = new Ellipse(Position, 0.05f, 0.05f, Color.LightGray);
			radiusDrawing.RenderLayer = -1;
		}

		private Ellipse radiusDrawing;

		private void PaintCircleSector()
		{
			circleSectorLines = new Line2D[2];
			var offsetPosition = Position + new Vector2D(0.0f, 0.1f);
			PaintLines(offsetPosition, Position);
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

		public static Color GetColor(TowerType type)
		{
			switch (type)
			{
			case TowerType.Acid:
				return Color.Green;
			case TowerType.Slice:
				return Color.LightGray;
			case TowerType.Fire:
				return Color.Red;
			case TowerType.Ice:
				return Color.LightBlue;
			case TowerType.Impact:
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

		public AttackType GetAttackType()
		{
			return data.AttackType;
		}

		public void UpdateTowerOrientation(Vector2D offsetPosition)
		{
			var rightLine = offsetPosition.RotateAround(Position, 15) - Position;
			var leftLine = offsetPosition.RotateAround(Position, -15) - Position;
			rightLine = Vector2D.Normalize(rightLine);
			leftLine = Vector2D.Normalize(leftLine);
			circleSectorLines[0].IsActive = false;
			circleSectorLines[1].IsActive = false;
			circleSectorLines[0] = new Line2D(Position, Position + rightLine * 0.2f, Color.White);
			circleSectorLines[1] = new Line2D(Position, Position + leftLine * 0.2f, Color.White);
		}

		public bool IsPauseable { get { return true; } }
	}
}