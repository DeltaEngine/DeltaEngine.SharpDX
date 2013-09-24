using System.Collections.Generic;
using $safeprojectname$.Creeps;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Shapes;

namespace $safeprojectname$.Simple2D
{
	public class Creep2D : Ellipse, Updateable
	{
		public Creep2D(Basic2DDisplaySystem display, Vector2D position, Creep.CreepType creepType) : 
			base(display.CalculateGridScreenDrawArea(position), GetColor(creepType))
		{
			this.display = display;
			Type = creepType;
			Target = Position = position;
			data = new CreepPropertiesXmlParser().CreepPropertiesData [creepType];
			data.CurrentHp = data.MaxHp;
			Hitpoints = data.MaxHp;
			hitpointBar = new FilledRect(HitpointsRect, Color.Green);
			listOfNodes = new List<Vector2D>();
			state = new CreepState();
			SetStartStateOfCreep(creepType, this);
		}

		public List<Vector2D> listOfNodes;
		internal readonly Basic2DDisplaySystem display;
		public FilledRect hitpointBar;
		internal readonly CreepProperties data;
		public CreepState state;

		public Creep.CreepType Type
		{
			get;
			private set;
		}

		public Vector2D Position
		{
			get;
			private set;
		}

		public Vector2D Target
		{
			get;
			set;
		}

		public float Hitpoints
		{
			get;
			set;
		}

		private Rectangle HitpointsRect
		{
			get
			{
				return new Rectangle(DrawArea.TopLeft, new Size((Hitpoints / data.MaxHp) * 
					DrawArea.Width, 0.002f));
			}
		}

		private static void SetStartStateOfCreep(Creep.CreepType creepType, Creep2D creep2D)
		{
			if (creepType == Creep.CreepType.Cloth)
				ClothCreepStateChanger2D.ChangeStartStatesIfClothCreep(creep2D);
			else if (creepType == Creep.CreepType.Sand)
				SandCreepStateChanger2D.ChangeStartStatesIfSandCreep(creep2D);
			else if (creepType == Creep.CreepType.Glass)
				GlassCreepStateChanger2D.ChangeStartStatesIfGlassCreep(creep2D);
			else if (creepType == Creep.CreepType.Wood)
				WoodCreepStateChanger2D.ChangeStartStatesIfWoodCreep(creep2D);
			else if (creepType == Creep.CreepType.Plastic)
				PlasticCreepStateChanger2D.ChangeStartStatesIfPlasticCreep(creep2D);
			else if (creepType == Creep.CreepType.Iron)
				IronCreepStateChanger2D.ChangeStartStatesIfIronCreep(creep2D);
			else if (creepType == Creep.CreepType.Paper)
				PaperCreepStateChanger2D.ChangeStartStatesIfPaperCreep(creep2D);
		}

		private static Color GetColor(Creep.CreepType creepType)
		{
			switch (creepType)
			{
				case Creep.CreepType.Cloth:
					return Color.CornflowerBlue;
				case Creep.CreepType.Glass:
					return Color.LightBlue;
				case Creep.CreepType.Iron:
					return Color.DarkGray;
				case Creep.CreepType.Paper:
					return Color.VeryLightGray;
				case Creep.CreepType.Plastic:
					return Color.PaleGreen;
				case Creep.CreepType.Sand:
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
			CheckIfWeCanRemoveFirstNode(previous);
			DrawArea = display.CalculateGridScreenDrawArea(Position);
			hitpointBar.DrawArea = HitpointsRect;
			hitpointBar.Color = Hitpoints > data.MaxHp / 2 ? Color.Green : Hitpoints > data.MaxHp / 4 ? 
				Color.Orange : Color.Red;
		}

		protected Vector2D Direction
		{
			get
			{
				if (listOfNodes.Count == 0)
					return Vector2D.Normalize(Target - Position);

				if (listOfNodes [0] == Position && listOfNodes.Count > 1)
					return Vector2D.Normalize(listOfNodes [1] - Position);

				if (listOfNodes [0] == Position)
					return Vector2D.Normalize(Target - Position);

				return Vector2D.Normalize(listOfNodes [0] - Position);
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

			var current = listOfNodes [0] - Position;
			if (!(previous.DotProduct(current) < 0.0f))
				return;

			Position = listOfNodes [0];
			listOfNodes.Remove(listOfNodes [0]);
		}

		public int GoldReward
		{
			get
			{
				return data.GoldReward;
			}
		}

		public void Shatter()
		{
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep2D>();
			foreach (Creep2D creep in creepList)
				if (creep != this)
				{
					var distance = ((creep.Position.X - Position.X) * (creep.Position.X - Position.X)) + 
						((creep.Position.Y - Position.Y) * (creep.Position.Y - Position.Y));
					if (distance <= 4)
						creep.data.CurrentHp -= 40;
				}
		}

		public void UpdateStateTimersAndTimeBasedDamage()
		{
			var properties = data;
			if (state.Burst)
				if (Time.CheckEvery(1))
					Hitpoints -= properties.MaxHp / 12;

			if (state.Burn)
				if (Time.CheckEvery(1))
					Hitpoints -= properties.MaxHp / 16;

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
			if (!state.Frozen)
				state.Paralysed = false;
		}

		private void UpdateMeltState()
		{
			state.MeltTimer += Time.Delta;
			state.Melt = !(state.MeltTimer > state.MaxTimeShort);
			if (!state.Melt)
			{
				state.Slow = false;
				state.Enfeeble = false;
			}
		}

		private void UpdateWetState()
		{
			state.WetTimer += Time.Delta;
			state.Wet = !(state.WetTimer > state.MaxTimeShort);
			if (!state.Wet)
				state.Slow = false;
		}
	}
}