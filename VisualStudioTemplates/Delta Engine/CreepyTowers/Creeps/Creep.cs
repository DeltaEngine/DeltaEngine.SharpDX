using System;
using System.Collections.Generic;
using $safeprojectname$.Content;
using $safeprojectname$.Levels;
using $safeprojectname$.Towers;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.GameLogic;
using DeltaEngine.Rendering2D.Shapes;
using DeltaEngine.Rendering3D;
using DeltaEngine.Rendering3D.Particles;

namespace $safeprojectname$.Creeps
{
	public sealed class Creep : DrawableEntity, Actor
	{
		public Creep(CreepType type, Vector3D position, float rotationZ)
		{
			Add(Data = ContentLoader.Load<CreepPropertiesXml>("CreepProperties").Get(type));
			Position = position;
			Orientation = Quaternion.FromAxisAngle(Vector3D.UnitZ, rotationZ);
			hitpointBar = new FilledRect(CurrentHpRect, Color.Green);
			Scale = Vector3D.One;
			state = new CreepState();
			SetRenderLayer();
			SetStartStateOfCreep();
			Add(ContentLoader.Load<ModelData>(Data.Name));
			OnDraw<ModelRenderer>();
		}

		public readonly CreepData Data;

		public float CurrentHp
		{
			get;
			set;
		}

		public readonly FilledRect hitpointBar;

		public Vector3D Position
		{
			get;
			set;
		}

		public Quaternion Orientation
		{
			get;
			set;
		}

		public Vector3D Scale
		{
			get;
			set;
		}

		public Vector3D Target
		{
			get;
			set;
		}

		public List<Vector3D> Path
		{
			get;
			set;
		}

		public LevelGrid Grid;
		public CreepState state;

		private Rectangle CurrentHpRect
		{
			get
			{
				return new Rectangle(Position.GetVector2D(), new Size((CurrentHp / Data.MaxHp) * 0.2f, 
					0.2f));
			}
		}

		private void SetRenderLayer()
		{
			RenderLayer = (int)CreepyTowersRenderLayer.Creeps;
		}

		private void SetStartStateOfCreep()
		{
			StateChanger.SetStartStateOfCreep(this);
		}

		public void RecalculateHitpointBar()
		{
			hitpointBar.DrawArea = CurrentHpRect;
			hitpointBar.Color = CurrentHp > Data.MaxHp / 2 ? Color.Green : CurrentHp > Data.MaxHp / 4 ? 
				Color.Orange : Color.Red;
		}

		public void ReceiveAttack(TowerType damageType, float rawDamage)
		{
			if (!IsActive)
				return;

			CheckCreepState(damageType);
			var interactionEffect = CalculateResistanceBasedOnStates(damageType);
			float resistance = state.Enfeeble ? Data.Resistance * 0.5f : Data.Resistance;
			float damage = (rawDamage - resistance) * interactionEffect;
			CurrentHp -= damage;
			if (CurrentHp <= 0.0f)
				Die();
		}

		private void CheckCreepState(TowerType type)
		{
			StateChanger.CheckCreepState(type, this);
		}

		public float CalculateResistanceBasedOnStates(TowerType damageType)
		{
			return Data.TypeDamageModifier [damageType];
		}

		private void Die()
		{
			DisplayCreepDieEffect();
			if (CreepIsDead != null)
				CreepIsDead();

			Dispose();
		}

		public event Action CreepIsDead;

		private void DisplayCreepDieEffect()
		{
			var emitterData = 
				ContentLoader.Load<ParticleEmitterData>(Effects.SmokecloudEffect.ToString());
			var emitter = new Particle3DPointEmitter(emitterData, Position);
			emitter.SpawnAndDispose();
		}

		public void UpdateHealthBarPositionAndImage()
		{
			if (UpdateHealthBar != null)
				UpdateHealthBar();
		}

		public event Action UpdateHealthBar;

		public void Dispose()
		{
			if (hitpointBar != null)
			{
				hitpointBar.IsActive = false;
				hitpointBar.IsVisible = false;
			}
			IsActive = false;
		}

		public void UpdateStateTimersAndTimeBasedDamage()
		{
			if (state.Burst)
				if (Time.CheckEvery(1))
					CurrentHp -= Data.MaxHp / 12;

			if (state.Burn)
				if (Time.CheckEvery(1))
					CurrentHp -= Data.MaxHp / 16;

			UpdateTimers();
		}

		private void UpdateTimers()
		{
			if (state.Paralysed)
				UpdateParalyzedState();

			if (state.Frozen)
				UpdateFrozenState();

			if (state.Melt)
				UpdateMeltState();

			if (state.Wet)
				UpdateWetState();

			if (state.Slow)
				UpdateSlowState();

			if (state.Unfreezable)
				UpdateUnfreezableState();
		}

		private void UpdateParalyzedState()
		{
			state.ParalysedTimer += Time.Delta;
			state.Paralysed = !(state.ParalysedTimer > state.MaxTimeShort);
		}

		private void UpdateFrozenState()
		{
			state.FrozenTimer += Time.Delta;
			state.Frozen = !(state.FrozenTimer > state.MaxTimeShort);
			if (state.Frozen)
				return;

			state.Paralysed = false;
			state.Unfreezable = true;
			state.UnfreezableTimer = 0;
			state.Wet = true;
			state.WetTimer = 0;
		}

		private void UpdateMeltState()
		{
			state.MeltTimer += Time.Delta;
			state.Melt = !(state.MeltTimer > state.MaxTimeShort);
			if (state.Melt)
				return;

			state.Slow = false;
			state.Enfeeble = false;
		}

		private void UpdateWetState()
		{
			state.WetTimer += Time.Delta;
			state.Wet = !(state.WetTimer > state.MaxTimeShort);
			if (!state.Wet)
				state.Slow = false;
		}

		private void UpdateSlowState()
		{
			if (state.SlowTimer == -1)
				return;

			state.SlowTimer += Time.Delta;
			state.Slow = !(state.SlowTimer > state.MaxTimeShort);
		}

		private void UpdateUnfreezableState()
		{
			state.UnfreezableTimer += Time.Delta;
			state.Unfreezable = !(state.UnfreezableTimer > state.MaxTimeMedium);
		}

		public void Shatter()
		{
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			foreach (Creep creep in creepList)
				if (creep != this)
					if (creep.Position.DistanceSquared(Position) <= DistanceToReceiveShatter)
						creep.CurrentHp -= AmountHpHurtReceived;
		}

		private const float DistanceToReceiveShatter = 4.0f;
		private const float AmountHpHurtReceived = 40.0f;
	}
}