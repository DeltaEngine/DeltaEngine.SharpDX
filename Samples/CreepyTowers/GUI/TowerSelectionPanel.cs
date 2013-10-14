using System.Collections.Generic;
using System.Linq;
using CreepyTowers.Content;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Multimedia;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers.GUI
{
	public class TowerSelectionPanel
	{
		public TowerSelectionPanel(Vector2D position, Manager manager)
		{
			SelectionPanel = ContentLoader.Load<Scene>("TowerSelectionPanel");
			clickedPosition = position;
			this.manager = manager;
			DisplayTowerSelectionPanel();
			PutButtonsOnRightPosition();
		}

		public readonly Scene SelectionPanel;
		private readonly Vector2D clickedPosition;
		private readonly Manager manager;

		private void DisplayTowerSelectionPanel()
		{
			foreach (var control in SelectionPanel.Controls)
				if (control.GetType() == typeof(Button))
				{
					var button = (Button)control;
					if (button.ContainsTag(TowerType.Acid.ToString()))
						AddClickEvent((InteractiveButton)button, TowerType.Acid,
							TowerModels.TowerAcidConeJanitorHigh.ToString());
					if (button.ContainsTag(TowerType.Fire.ToString()))
						AddClickEvent((InteractiveButton)button, TowerType.Fire,
							TowerModels.TowerFireCandlehulaHigh.ToString());
					if (button.ContainsTag(TowerType.Ice.ToString()))
						AddClickEvent((InteractiveButton)button, TowerType.Ice,
							TowerModels.TowerIceConeIceladyHigh.ToString());
					if (button.ContainsTag(TowerType.Impact.ToString()))
						AddClickEvent((InteractiveButton)button, TowerType.Impact,
							TowerModels.TowerImpactRangedKnightscalesHigh.ToString());
					if (button.ContainsTag(TowerType.Slice.ToString()))
						AddClickEvent((InteractiveButton)button, TowerType.Slice,
							TowerModels.TowerSliceConeKnifeblockHigh.ToString());
					if (button.ContainsTag(TowerType.Water.ToString()))
						AddClickEvent((InteractiveButton)button, TowerType.Water,
							TowerModels.TowerWaterRangedWatersprayHigh.ToString());
				}
		}

		private void AddClickEvent(InteractiveButton button, TowerType type, string towerName)
		{
			button.Clicked += () =>
			{
				ContentLoader.Load<Sound>(Sounds.PressButton.ToString()).Play();
				manager.Get<InGameCommands>().HideTowerPanel();
				manager.CreateTower(manager.Get<InGameCommands>().PositionInGrid, type, towerName);
			};
		}

		public void InactiveButtonsInPanel(List<string> inactiveButtonsTagList)
		{
			if (inactiveButtonsTagList == null)
				return;
			foreach (string tag in inactiveButtonsTagList)
				foreach (InteractiveButton button in
					SelectionPanel.Controls.Where(control => control.ContainsTag(tag)))
					button.IsEnabled = false;
		}

		private void PutButtonsOnRightPosition()
		{
			foreach (var control in SelectionPanel.Controls)
				if (control.GetType() == typeof(Button))
					control.DrawArea = new Rectangle(control.DrawArea.TopLeft + clickedPosition,
						control.DrawArea.Size);
		}
	}
}