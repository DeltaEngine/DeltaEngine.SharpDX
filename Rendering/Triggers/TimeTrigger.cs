using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering.Triggers
{
	public class TimeTrigger : UpdateBehavior
	{
		public TimeTrigger() : base(Priority.High){}

		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (var entity in entities.OfType<Entity2D>())
			{
				var data = entity.Get<Data>();
				if (!Time.CheckEvery(data.Interval))
					continue;
				entity.Color = entity.Color == data.FirstColor ? data.SecondColor : data.FirstColor;
			}
		}

		internal class Data
		{
			public Data(Color firstColor, Color secondColor, float interval)
			{
				FirstColor = firstColor;
				SecondColor = secondColor;
				Interval = interval;
			}

			public Color FirstColor;
			public Color SecondColor;
			public float Interval;
		}
	}
}