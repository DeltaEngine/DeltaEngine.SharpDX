using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;

namespace $safeprojectname$
{
	public class FadeEffect : UpdateBehavior
	{
		public override void Update(IEnumerable<Entity> entities)
		{
			foreach (Entity2D sprite in entities)
			{
				sprite.ToggleVisibility(Visibility.Show);
				var transitionData = sprite.Get<TransitionData>();
				transitionData.ElapsedTime += Time.Delta;
				if (transitionData.ElapsedTime > transitionData.Duration)
				{
					sprite.Stop<FadeEffect>();
					if (EffectOver != null)
						EffectOver();
				}
				sprite.Color = transitionData.Colour.Start.Lerp(transitionData.Colour.End, 
					transitionData.ElapsedTime / transitionData.Duration);
				sprite.Set(transitionData);
			}
		}

		public event Action EffectOver;
		public struct TransitionData
		{
			public RangeGraph<Color> Colour;
			public float Duration;
			public float ElapsedTime;
		}
	}
}