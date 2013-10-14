using System.Collections.Generic;
using CreepyTowers.Creeps;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Shapes;

namespace CreepyTowers.Tests.Simple2D
{
//TODO: remove and directly use Creep class!
	public class Creep2D : Ellipse, Updateable
	{
		public Creep2D(Basic2DDisplaySystem display, Vector2D position, CreepType creepType)
			: base(display.CalculateGridScreenDrawArea(position), GetColor(creepType))
		{
			this.display = display;
			Type = creepType;
			Target = Position = position;
			data = ContentLoader.Load<CreepPropertiesXml>("CreepProperties").Get(creepType);
			CurrentHp = data.MaxHp;
			hitpointBar = new FilledRect(CurrentHpRect, Color.Green);
			listOfNodes = new List<Vector3D>();
			state = new CreepState();
			SetStartStateOfCreep(creepType, this);
		}

		public List<Vector3D> listOfNodes;
		internal readonly Basic2DDisplaySystem display;
		public readonly FilledRect hitpointBar;
		internal readonly CreepData data;
		public readonly CreepState state;

		public CreepType Type { get; private set; }
		public Vector3D Position { get; private set; }
		public Vector3D Target { get; set; }
		public float CurrentHp { get; set; }

		private Rectangle CurrentHpRect
		{
			get
			{
				return new Rectangle(DrawArea.TopLeft,
					new Size((CurrentHp / data.MaxHp) * DrawArea.Width, 0.2f));
			}
		}

		//TODO: wtf, this is duplicated so many times
		private static void SetStartStateOfCreep(CreepType creepType, Creep2D creep2D)
		{
			if (creepType == CreepType.Cloth)
				ClothCreepStateChanger2D.ChangeStartStatesIfClothCreep(creep2D);
			else if (creepType == CreepType.Sand)
				SandCreepStateChanger2D.ChangeStartStatesIfSandCreep(creep2D);
			else if (creepType == CreepType.Glass)
				GlassCreepStateChanger2D.ChangeStartStatesIfGlassCreep(creep2D);
			else if (creepType == CreepType.Wood)
				WoodCreepStateChanger2D.ChangeStartStatesIfWoodCreep(creep2D);
			else if (creepType == CreepType.Plastic)
				PlasticCreepStateChanger2D.ChangeStartStatesIfPlasticCreep(creep2D);
			else if (creepType == CreepType.Iron)
				IronCreepStateChanger2D.ChangeStartStatesIfIronCreep(creep2D);
			else if (creepType == CreepType.Paper)
				PaperCreepStateChanger2D.ChangeStartStatesIfPaperCreep(creep2D);
		}

		private static Color GetColor(CreepType creepType)
		{
			switch (creepType)
			{
			case CreepType.Cloth:
				return Color.CornflowerBlue;
			case CreepType.Glass:
				return Color.LightBlue;
			case CreepType.Iron:
				return Color.DarkGray;
			case CreepType.Paper:
				return Color.VeryLightGray;
			case CreepType.Plastic:
				return Color.PaleGreen;
			case CreepType.Sand:
				return Color.Yellow;
			default:
				return Color.Brown;
			}
		}

		public void Update()
		{
			if (Target == Position)
				return;
			var previous = Direction;
			Position += previous * GetVelocity(this, data.Speed) * Time.Delta;
			CheckIfWeCanRemoveFirstNode(previous.GetVector2D());
			DrawArea = display.CalculateGridScreenDrawArea(Position);
			RecalculateHitpointBar();
		}

		protected Vector3D Direction
		{
			get
			{
				if (listOfNodes.Count == 0)
					return Vector3D.Normalize(Target - Position);
				if (listOfNodes[0] == Position && listOfNodes.Count > 1)
					return Vector3D.Normalize(listOfNodes[1] - Position);
				if (listOfNodes[0] == Position)
					return Vector3D.Normalize(Target - Position);
				return Vector3D.Normalize(listOfNodes[0] - Position);
			}
		}

		private static float GetVelocity(Creep2D creep, float velocity)
		{
			if (creep.state.Paralysed || creep.state.Frozen)
				return 0;
			if (creep.state.Delayed)
				return velocity / 3;
			if (creep.state.Slow)
				return velocity / 2;
			if (creep.state.Fast)
				return velocity * 2;
			return velocity;
		}

		private void CheckIfWeCanRemoveFirstNode(Vector2D previous)
		{
			if (listOfNodes.Count <= 0)
				return;
			var current = listOfNodes[0] - Position;
			if (!(previous.DotProduct(current.GetVector2D()) < 0.0f))
				return;
			Position = listOfNodes[0];
			listOfNodes.Remove(listOfNodes[0]);
		}

		public void RecalculateHitpointBar()
		{
			hitpointBar.DrawArea = CurrentHpRect;
			hitpointBar.Color = CurrentHp > data.MaxHp / 2
				? Color.Green : CurrentHp > data.MaxHp / 4 ? Color.Orange : Color.Red;
		}

		public int GoldReward { get { return data.GoldReward; } }
		
		public void Shatter()
		{
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep2D>();
			foreach (Creep2D creep in creepList)
				if (creep != this)
				{
					var distance = ((creep.Position.X - Position.X) * (creep.Position.X - Position.X)) +
						((creep.Position.Y - Position.Y) * (creep.Position.Y - Position.Y));
					if (distance <= 4)
						creep.CurrentHp -= 40;
				}
		}

		//TODO: so much copy+paste, merge with Creep!
		public void UpdateStateTimersAndTimeBasedDamage()
		{
			if (state.Burst)
				if (Time.CheckEvery(1))
					CurrentHp -= data.MaxHp / 12;
			if (state.Burn)
				if (Time.CheckEvery(1))
					CurrentHp -= data.MaxHp / 16;
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

		public bool IsPauseable { get { return true; } }
	}
}