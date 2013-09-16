using System;
using System.Globalization;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Scenes;

namespace CreepyTowers
{
	public class Hud : Scene, IDisposable
	{
		public Hud(Manager manager)
		{
			this.manager = manager;
			elapsedTimer = new TimeCounter();
			resourceDisplay = CreateNewFont();
			resourceDisplay.RenderLayer = (int)CreepyTowersRenderLayer.Interface;

			Add(elapsedTimer);
			Add(resourceDisplay);
			AttachEvents();
		}

		private readonly Manager manager;
		private readonly TimeCounter elapsedTimer;
		private readonly FontText resourceDisplay;

		private FontText CreateNewFont()
		{
			return new FontText(ContentLoader.Load<Font>(Names.FontChelseaMarket14),
				"$ " + manager.playerData.ResourceFinances.ToString(CultureInfo.InvariantCulture),
				new Rectangle(0.85f, 0.18f, 0.2f, 0.1f));
		}

		private void AttachEvents()
		{
			manager.CreditsUpdated += i => UpdateCredits();
			manager.InsufficientCredits += i => InsufficientFunds();
		}

		private void UpdateCredits()
		{
			resourceDisplay.Text = "$ " +
				manager.playerData.ResourceFinances.ToString(CultureInfo.InvariantCulture);
			//resourceDisplay.Start<Transition>();
			//resourceDisplay.Set(new Transition.ColorRange(Color.Yellow, Color.White));
			//resourceDisplay.Set(new Transition.Duration(0.2f));
		}

		private void InsufficientFunds()
		{
			//resourceDisplay.Start<Transition>();
			//resourceDisplay.Set(new Transition.ColorRange(Color.Red, Color.White));
			//resourceDisplay.Set(new Transition.Duration(0.2f));
		}

		public new void Dispose()
		{
			Clear();
		}
	}
}