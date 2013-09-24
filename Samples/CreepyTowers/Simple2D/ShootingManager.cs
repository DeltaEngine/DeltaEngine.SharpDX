using System.Collections.Generic;
using System.Linq;
using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering2D.Shapes;

namespace CreepyTowers.Simple2D
{
	public class ShootingManager
	{
		public ShootingManager(Basic2DDisplaySystem display)
		{
			this.display = display;
			calculateDamage = new CalculateDamage2D();
		}

		private readonly Basic2DDisplaySystem display;
		private readonly CalculateDamage2D calculateDamage;

		public void ShootCreeps(ChangeableList<Tower2D> towers)
		{
			foreach (var line in lines)
				line.IsActive = false;
			lines.Clear();
			foreach (var tower in towers)
				if (tower.GetAttackType() == Tower.AttackType.RadiusFull)
					ShootRadiusShot(tower);
				else if (tower.GetAttackType() == Tower.AttackType.DirectShot)
					ShootDirectShot(tower);
				else if (tower.GetAttackType() == Tower.AttackType.CircleSector)
					ShootCircleSector(tower);
		}

		private void ShootRadiusShot(Tower2D tower)
		{
			if (tower.IsOnCooldown)
				return;
			foreach (var creep in display.Creeps)
			{
				creep.UpdateStateTimersAndTimeBasedDamage();
				if (!((tower.Position - creep.Position).Length < tower.Range))
					continue;
				if (IsAWallBetweenTowerAndCreep(tower, creep))
					continue;
				lines.Add(new Line2D(tower.Center, creep.Center, tower.Color));
				creep.Hitpoints = DamageCalculation(creep, tower);
				tower.SetOnCooldown();
			}
		}

		private readonly List<Line2D> lines = new List<Line2D>();

		private bool IsAWallBetweenTowerAndCreep(Tower2D tower, Creep2D creep)
		{
			var vector = creep.Position - tower.Position;
			for (int i = 0; i < 100; i++)
			{
				var position = tower.Position + (vector * (i / 100.0f));
				if (display.ThereIsWallInPosition(position))
					return true;
			}
			return false;
		}

		private float DamageCalculation(Creep2D creep, Tower2D tower)
		{
			var properties = creep.data;
			CheckCreepState(tower.Type, creep);
			if (!display.IsActive)
				return 0;
			var interactionEffect = calculateDamage.CalculateResistanceBasedOnStates(tower.Type, creep);
			var dmg = CalculateDamage(creep, tower, interactionEffect);
			properties.CurrentHp = properties.CurrentHp - dmg < properties.MaxHp ?
				properties.CurrentHp - dmg : properties.MaxHp;
			return properties.CurrentHp;
		}

		private void CheckCreepState(Tower.TowerType type, Creep2D creep)
		{
			if (creep.Type == Creep.CreepType.Cloth)
				ClothCreepStateChanger2D.ChangeStatesIfClothCreep(type, creep);
			else if (creep.Type == Creep.CreepType.Sand)
				SandCreepStateChanger2D.ChangeStatesIfSandCreep(type, creep, display);
			else if (creep.Type == Creep.CreepType.Glass)
				GlassCreepStateChanger2D.ChangeStatesIfGlassCreep(type, creep);
			else if (creep.Type == Creep.CreepType.Wood)
				WoodCreepStateChanger2D.ChangeStatesIfWoodCreep(type, creep);
			else if (creep.Type == Creep.CreepType.Plastic)
				PlasticCreepStateChanger2D.ChangeStatesIfPlasticCreep(type, creep);
			else if (creep.Type == Creep.CreepType.Iron)
				IronCreepStateChanger2D.ChangeStatesIfIronCreep(type, creep);
			else if (creep.Type == Creep.CreepType.Paper)
				PaperCreepStateChanger2D.ChangeStatesIfPaperCreep(type, creep);
		}

		private static float CalculateDamage(Creep2D creep, Tower2D tower, float interactionEffect)
		{
			var properties = creep.data;
			float dmg;
			if (creep.state.Healing)
			{
				dmg = -(tower.Damage * 0.5f);
				creep.state.Healing = false;
			}
			else if (creep.state.Enfeeble)
				dmg = (tower.Damage - (properties.Resistance / 2)) * interactionEffect;
			else
				dmg = (tower.Damage - properties.Resistance) * interactionEffect;
			return dmg;
		}

		private void ShootDirectShot(Tower2D tower)
		{
			if (tower.IsOnCooldown)
				return;
			var creep = SelectCreepToShot(tower);
			if (creep == null)
				return;
			creep.UpdateStateTimersAndTimeBasedDamage();
			lines.Add(new Line2D(tower.Center, creep.Center, tower.Color));
			creep.Hitpoints = DamageCalculation(creep, tower);
			tower.SetOnCooldown();
		}

		private Creep2D SelectCreepToShot(Tower2D tower)
		{
			Creep2D creepToReturn = null;
			foreach (var creep in display.Creeps)
			{
				if (!((tower.Position - creep.Position).Length < tower.Range))
					continue;
				if (IsAWallBetweenTowerAndCreep(tower, creep))
					continue;
				if (creepToReturn == null || creep.listOfNodes.Count < creepToReturn.listOfNodes.Count)
					creepToReturn = creep;
			}
			return creepToReturn;
		}

		private void ShootCircleSector(Tower2D tower)
		{
			var creep = SelectCreepToShot(tower);
			if (creep == null)
				return;
			tower.UpdateTowerOrientation(creep.Position);
			if (tower.IsOnCooldown)
				return;
			creep.UpdateStateTimersAndTimeBasedDamage();
			lines.Add(new Line2D(tower.Center, creep.Center, tower.Color));
			creep.Hitpoints = DamageCalculation(creep, tower);
			AffectsCreepsInTheRange(tower, creep);
			tower.SetOnCooldown();
		}

		private void AffectsCreepsInTheRange(Tower2D tower, Creep2D creep)
		{
			var towerPosition = display.GetPositionOfNode((int)tower.Position.X, (int)tower.Position.Y);
			var creepPosition = display.GetPositionOfNode((int)creep.Position.X, (int)creep.Position.Y);
			var aimVector = creepPosition - towerPosition;
			aimVector = Vector2D.Normalize(aimVector);
			var dotProduct = GetDotProduct(creepPosition, towerPosition, aimVector);
			foreach (var creep2D in display.Creeps.Where(x => x != creep))
			{
				var position = display.GetPositionOfNode((int)creep2D.Position.X, (int)creep2D.Position.Y);
				if (aimVector.DotProduct(Vector2D.Normalize(position - towerPosition)) > dotProduct)
					AffectCreepByTheAttack(tower, creep2D);
			}
		}

		private static float GetDotProduct(Vector2D creepPosition, Vector2D towerPosition, Vector2D aimVector)
		{
			var rightLine = creepPosition.RotateAround(towerPosition, 15) - towerPosition;
			rightLine = Vector2D.Normalize(rightLine);
			return aimVector.DotProduct(rightLine);
		}

		private void AffectCreepByTheAttack(Tower2D tower, Creep2D creep2D)
		{
			if (!((tower.Position - creep2D.Position).Length < tower.Range))
				return;
			if (IsAWallBetweenTowerAndCreep(tower, creep2D))
				return;
			creep2D.UpdateStateTimersAndTimeBasedDamage();
			lines.Add(new Line2D(tower.Center, creep2D.Center, tower.Color));
			creep2D.Hitpoints = DamageCalculation(creep2D, tower);
		}
	}
}
