using System;
using System.Collections.Generic;
using System.Linq;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers.GUI
{
	public class TowerSelectionPanel : Scene, IDisposable
	{
		public TowerSelectionPanel(Vector2D position, Manager manager)
		{
			clickedPosition = position;
			this.manager = manager;
			DisplayTowerSelectionPanel();
		}

		private Vector2D clickedPosition;
		private readonly Manager manager;

		private void DisplayTowerSelectionPanel()
		{
			DrawAcidTowerButton();
			DrawFireTowerButton();
			DrawIceTowerButton();
			DrawImpactTowerButton();
			DrawSliceTowerButton();
			DrawWaterTowerButton();
		}

		private void DrawAcidTowerButton()
		{
			var button = CreateInteractiveButton(0.0f, Names.ButtonAcidTower);
			AddClickEvent(button, Tower.TowerType.Acid, Names.TowerAcidConeJanitor);
			Add(button);
		}

		private InteractiveButton CreateInteractiveButton(float angle, string imageName)
		{
			var drawArea = CalculateDrawArea(angle);
			var button = new InteractiveButton(CreateTheme(imageName), drawArea);
			button.AddTag(imageName);
			button.RenderLayer = (int)CreepyTowersRenderLayer.Interface;
			return button;
		}

		private Rectangle CalculateDrawArea(float angle)
		{
			var drawAreaCenterX = (float)(clickedPosition.X + 0.07f * Math.Cos(angle * Math.PI / 180));
			var drawAreaCenterY = (float)(clickedPosition.Y + 0.07f * Math.Sin(angle * Math.PI / 180));
			var drawArea = Rectangle.FromCenter(new Vector2D(drawAreaCenterX, drawAreaCenterY),
				new Size(0.05f));
			return drawArea;
		}

		private static Theme CreateTheme(string buttonImageName)
		{
			var appearance = new Theme.Appearance(buttonImageName);
			return new Theme
			{
				Button = appearance,
				ButtonDisabled = new Theme.Appearance(buttonImageName, Color.Gray),
				ButtonMouseover = appearance,
				ButtonPressed = appearance,
				Font = ContentLoader.Load<Font>(Names.FontChelseaMarket14)
			};
		}

		private void AddClickEvent(InteractiveButton button, Tower.TowerType type, string towerName)
		{
			button.Clicked += () =>
			{
				manager.Get<InputCommands>().HideTowerPanel();
				manager.CreateTower(manager.Get<InputCommands>().PositionInGrid, type, towerName);
			};
		}

		private void DrawFireTowerButton()
		{
			var button = CreateInteractiveButton(60.0f, Names.ButtonFireTower);
			AddClickEvent(button, Tower.TowerType.Fire, Names.TowerFireCandlehula);
			Add(button);
		}

		private void DrawIceTowerButton()
		{
			var button = CreateInteractiveButton(120.0f, Names.ButtonIceTower);
			AddClickEvent(button, Tower.TowerType.Ice, Names.TowerIceConeIcelady);
			Add(button);
		}

		private void DrawImpactTowerButton()
		{
			var button = CreateInteractiveButton(180.0f, Names.ButtonImpactTower);
			AddClickEvent(button, Tower.TowerType.Impact, Names.TowerImpactRangedKnightscales);
			Add(button);
		}

		private void DrawSliceTowerButton()
		{
			var button = CreateInteractiveButton(240.0f, Names.ButtonSliceTower);
			AddClickEvent(button, Tower.TowerType.Blade, Names.TowerSliceConeKnifeblock);
			Add(button);
		}

		private void DrawWaterTowerButton()
		{
			var button = CreateInteractiveButton(300.0f, Names.ButtonWaterTower);
			AddClickEvent(button, Tower.TowerType.Water, Names.TowerWaterRangedWaterspray);
			Add(button);
		}

		public void InactiveButtonsInPanel(List<string> inactiveButtonsTagList)
		{
			if (inactiveButtonsTagList == null)
				return;

			foreach (string tag in inactiveButtonsTagList)
				foreach (InteractiveButton button in Controls.Where(control => control.ContainsTag(tag)))
					button.IsEnabled = false;
		}

		public new void Dispose()
		{
			Hide();
			Clear();
		}
	}
}