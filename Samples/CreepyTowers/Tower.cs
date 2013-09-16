using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering.Models;
using DeltaEngine.Rendering.Shapes3D;

namespace CreepyTowers
{
	/// <summary>
	/// Tower, stats and behaviour varying depending on its type.
	/// </summary>
	public class Tower : Model
	{
		public Tower(Vector position, TowerType towerType, string name)
			: base(name, position)
		{
			Position = position;
			SetDefaultValues();
			Add(new TowerProperties());
		}

		private void SetDefaultValues()
		{
			RenderLayer = (int)CreepyTowersRenderLayer.Towers;
			TimeOfLastAttack = Time.Total;
		}

		public float TimeOfLastAttack { get; private set; }

		public enum TowerType
		{
			Acid = 0,
			Blade = 1,
			Fire = 2,
			Ice = 3,
			Impact = 4,
			Water = 5
		}

		public enum AttackType
		{
			DirectShot,
			RadiusFull,
			CircleSector
		}

		public void FireAtCreep(Creep creep)
		{
			if (creep == null || !creep.IsActive)
				return;

			if ((Time.Total - TimeOfLastAttack) < 1 / Get<TowerProperties>().AttackFrequency)
				return;

			var startPoint = new Vector(Position.X, Position.Y + 0.1f, Position.Z);
			var endPoint = new Vector(creep.Position.X, creep.Position.Y + 0.1f, creep.Position.Z);
			fireLine = new Line3D(startPoint, endPoint, Color.Red)
			{
				RenderLayer = (int)CreepyTowersRenderLayer.Interface
			};
			TimeOfLastAttack = Time.Total;
			var properties = Get<TowerProperties>();
			creep.ReceiveAttack(properties.TowerType, properties.AttackDamage);
			creep.CreepIsDead += RemoveFireLine;
		}

		public void RemoveFireLine()
		{
			if (fireLine != null)
				fireLine.IsActive = false;
		}

		private Line3D fireLine;

		public void Dispose()
		{
			RemoveFireLine();
			IsActive = false;
		}
	}
}