using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering.Shapes;

namespace CreepyTowers.Simple2D
{
	/// <summary>
	/// Places a tower in the BasicDisplaySystem grid
	/// TODO: separate Tower2D from Tower2DView
	/// </summary>
	public class Tower2D : FilledRect, Updateable
	{
		public Tower2D(Basic2DDisplaySystem display, Point position, Tower.TowerType towerType)
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
			var realPosition = display.GetPositionOfNode((int)Position.X, (int)Position.Y);
			var offsetPosition = realPosition + new Point(0.0f, 0.1f);
			var rightLine = offsetPosition.RotateAround(realPosition, 15);
			var leftLine = offsetPosition.RotateAround(realPosition, -15);
			circleSectorLines = new Line2D[2];
			circleSectorLines[0] = new Line2D(realPosition, rightLine, Color.White);
			circleSectorLines[1] = new Line2D(realPosition, leftLine, Color.White);
		}

		private Line2D[] circleSectorLines;

		public static Color GetColor(Tower.TowerType type)
		{
			switch (type)
			{
			case Tower.TowerType.Acid:
				return Color.Green;
			case Tower.TowerType.Blade:
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

		public Tower.TowerType Type { get; private set; }
		public Point Position { get; private set; }
		public float Range
		{
			get { return data.Range; }
		}
		public float Damage
		{
			get { return data.AttackDamage; }
		}

		private readonly TowerProperties data;

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
	}
}