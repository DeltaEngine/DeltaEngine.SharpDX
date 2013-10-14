using System.Collections.Generic;
using System.Linq;
using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D.Shapes;

namespace CreepyTowers.Tests.Simple2D
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

		public void ShootCreeps(IEnumerable<Tower2D> towers)
		{
			foreach (var line in lines)
				line.IsActive = false;
			lines.Clear();
			foreach (var creepInList in display.Creeps)
				creepInList.UpdateStateTimersAndTimeBasedDamage();
			foreach (var tower in towers)
				if (tower.IsTowerActive)
					SelectTypeOfShot(tower);
		}

		private void SelectTypeOfShot(Tower2D tower)
		{
			if (tower.GetAttackType() == AttackType.RadiusFull)
				ShootRadiusShot(tower);
			else if (tower.GetAttackType() == AttackType.DirectShot)
				ShootDirectShot(tower);
			else if (tower.GetAttackType() == AttackType.CircleSector)
				ShootCircleSector(tower);
		}

		private void ShootRadiusShot(Tower2D tower)
		{
			if (tower.IsOnCooldown)
				return; 
			foreach (var creep in display.Creeps)
			{
				if (!((tower.Position - creep.Position).Length < tower.Range))
					continue;
				if (IsAWallBetweenTowerAndCreep(tower, creep))
					continue;
				ContentLoader.Load<Sound>("Shot").Play();
				lines.Add(new Line2D(tower.Center, creep.Center, tower.Color));
				creep.CurrentHp = DamageCalculation(creep, tower);
				creep.RecalculateHitpointBar();
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
				if (display.ThereIsWallInPosition(position.GetVector2D()))
					return true;
			}
			return false;
		}

		//TODO: this is duplicated code, it is already in Creep.cs!
		private float DamageCalculation(Creep2D creep, Tower2D tower)
		{
			CheckCreepState(tower.Type, creep);
			if (!display.IsActive)
				return 0;
			var interactionEffect = calculateDamage.CalculateResistanceBasedOnStates(tower.Type, creep);
			var damage = CalculateDamage(creep, tower, interactionEffect);
			creep.CurrentHp = creep.CurrentHp - damage < creep.data.MaxHp ?
				creep.CurrentHp - damage : creep.data.MaxHp;
			if (creep.CurrentHp <= 0)
				ContentLoader.Load<Sound>("Death").Play();
			return creep.CurrentHp;
		}

		//TODO: more duplicate code
		private void CheckCreepState(TowerType type, Creep2D creep)
		{
			if (creep.Type == CreepType.Cloth)
				ClothCreepStateChanger2D.ChangeStatesIfClothCreep(type, creep);
			else if (creep.Type == CreepType.Sand)
				SandCreepStateChanger2D.ChangeStatesIfSandCreep(type, creep, display);
			else if (creep.Type == CreepType.Glass)
				GlassCreepStateChanger2D.ChangeStatesIfGlassCreep(type, creep);
			else if (creep.Type == CreepType.Wood)
				WoodCreepStateChanger2D.ChangeStatesIfWoodCreep(type, creep);
			else if (creep.Type == CreepType.Plastic)
				PlasticCreepStateChanger2D.ChangeStatesIfPlasticCreep(type, creep);
			else if (creep.Type == CreepType.Iron)
				IronCreepStateChanger2D.ChangeStatesIfIronCreep(type, creep);
			else if (creep.Type == CreepType.Paper)
				PaperCreepStateChanger2D.ChangeStatesIfPaperCreep(type, creep);
		}

		//TODO: more duplicate code
		private static float CalculateDamage(Creep2D creep, Tower2D tower, float interactionEffect)
		{
			float damage;
			if (creep.state.Healing)
			{
				damage = -(tower.Damage * 0.5f);
				creep.state.Healing = false;
			}
			else if (creep.state.Enfeeble)
				damage = (tower.Damage - (creep.data.Resistance / 2)) * interactionEffect;
			else
				damage = (tower.Damage - creep.data.Resistance) * interactionEffect;
			return damage;
		}

		private void ShootDirectShot(Tower2D tower)
		{
			if (tower.IsOnCooldown)
				return;
			var creep = SelectCreepToShot(tower);
			if (creep == null)
				return;
			ContentLoader.Load<Sound>("Shot").Play();
			lines.Add(new Line2D(tower.Center, creep.Center, tower.Color));
			creep.CurrentHp = DamageCalculation(creep, tower);
			creep.RecalculateHitpointBar();
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
			tower.UpdateTowerOrientation(creep.Position.GetVector2D());
			if (tower.IsOnCooldown)
				return;
			ContentLoader.Load<Sound>("Shot").Play();
			creep.UpdateStateTimersAndTimeBasedDamage();
			lines.Add(new Line2D(tower.Center, creep.Center, tower.Color));
			creep.CurrentHp = DamageCalculation(creep, tower);
			creep.RecalculateHitpointBar();
			AffectsCreepsInTheRange(tower, creep);
			tower.SetOnCooldown();
		}

		private void AffectsCreepsInTheRange(Tower2D tower, Creep2D creep)
		{
			var towerPosition = tower.Position;
			var creepPosition = creep.Position.GetVector2D();
			var aimVector = creepPosition - towerPosition;
			aimVector = Vector2D.Normalize(aimVector);
			var dotProduct = GetDotProduct(creepPosition, towerPosition, aimVector);
			foreach (var creep2D in display.Creeps.Where(x => x != creep))
			{
				var position = creep2D.Position.GetVector2D();
				if (aimVector.DotProduct(Vector2D.Normalize(position - towerPosition)) > dotProduct)
					AffectCreepByTheAttack(tower, creep2D);
			}
		}

		private static float GetDotProduct(Vector2D creepPosition, Vector2D towerPosition,
			Vector2D aimVector)
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
			creep2D.CurrentHp = DamageCalculation(creep2D, tower);
		}
	}
}