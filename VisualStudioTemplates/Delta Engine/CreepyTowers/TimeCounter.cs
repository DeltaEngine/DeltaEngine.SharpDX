using System.Collections.Generic;
using System.Globalization;
using $safeprojectname$.Content;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Fonts;

namespace $safeprojectname$
{
	public class TimeCounter : FontText
	{
		public TimeCounter() : base(ContentLoader.Load<Font>(Fonts.ChelseaMarket14.ToString()), "0 : " +
			"0", new Rectangle(0.0f, 0.18f, 0.08f, 0.1f))
		{
			Start<TimerUpdate>();
			isCounting = true;
			RenderLayer = (int)CreepyTowersRenderLayer.Interface + 1;
			elapsedSec = 0;
		}

		private readonly bool isCounting;
		private float elapsedSec;
		private class TimerUpdate : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (TimeCounter timer in entities)
					timer.CountIfRunning();
			}
		}
		private void CountIfRunning()
		{
			if (!isCounting)
				return;

			elapsedSec += Time.Delta;
			UpdateMinSecFromElapsed();
		}

		private void UpdateMinSecFromElapsed()
		{
			var minutes = (int)elapsedSec / 60;
			var seconds = (int)elapsedSec % 60;
			Text = minutes.ToString(CultureInfo.InvariantCulture) + " : " + 
				seconds.ToString(CultureInfo.InvariantCulture);
		}
	}
}