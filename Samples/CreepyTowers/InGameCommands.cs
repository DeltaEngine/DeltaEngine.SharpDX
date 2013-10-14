using System;
using System.Collections.Generic;
using CreepyTowers.GUI;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering3D.Cameras;

namespace CreepyTowers
{
	public class InGameCommands : IDisposable
	{
		public InGameCommands(Manager manager, List<string> inactiveButtonsTagList)
		{
			CommandsList = new List<Command>();
			this.manager = manager;
			SetInputCommands(inactiveButtonsTagList);
		}

		public List<Command> CommandsList { get; private set; }
		private readonly Manager manager;

		private void SetInputCommands(List<string> inactiveButtonsTagList)
		{
			CommandsList.Add(
				new Command(Command.Click, pos => DisplayTowerSelectionPanel(pos, inactiveButtonsTagList)));
			CommandsList.Add(new Command(Command.RightClick, HideTowerPanel));
			CommandsList.Add(new Command(Command.Exit, Game.EndGame));
		}

		private void DisplayTowerSelectionPanel(Vector2D pos, List<string> inactiveButtonsTagList)
		{
			if (isPanelDisplayed)// || !ClickedPositionInGrid(ConvertMouseClickTo3D(pos)))
				return;
			TowerPanel = new TowerSelectionPanel(pos, manager);
			TowerPanel.InactiveButtonsInPanel(inactiveButtonsTagList);
			isPanelDisplayed = true;
		}

		private bool isPanelDisplayed;
		public TowerSelectionPanel TowerPanel { get; private set; }

		private static Vector3D ConvertMouseClickTo3D(Vector2D pos)
		{
			var floor = new Plane(Vector3D.UnitY, 0.0f);
			var ray = Camera.Current.ScreenPointToRay(pos);
			return (Vector3D)floor.Intersect(ray);
		}

		//private bool ClickedPositionInGrid(Vector3D clickedPos)
		//{
		//	var interactablePointsList = manager.GameLevel.Get<GameLevel.GridData>().InteractablePointsList;
		//	PositionInGrid = grid.ComputeGridCoordinates(grid, clickedPos, interactablePointsList);
		//	return grid.IsClickInGrid;
		//}

		public Vector3D PositionInGrid { get; private set; }

		public void Dispose()
		{
			HideTowerPanel();
			foreach (Command command in CommandsList)
				command.IsActive = false;
		}

		public void HideTowerPanel()
		{
			if (TowerPanel == null)
				return;
			TowerPanel.SelectionPanel.Clear();
			isPanelDisplayed = false;
		}
	}
}