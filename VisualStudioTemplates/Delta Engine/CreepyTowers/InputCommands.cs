using System;
using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Rendering.Cameras;

namespace $safeprojectname$
{
	public class InputCommands : IDisposable
	{
		public InputCommands(Manager manager, List<string> inactiveButtonsTagList)
		{
			grid = Game.CameraAndGrid.Grid;
			CommandsList = new List<Command>();
			this.manager = manager;
			SetInputCommands(inactiveButtonsTagList);
		}

		public List<Command> CommandsList
		{
			get;
			private set;
		}

		private readonly LevelGrid grid;
		private readonly Manager manager;

		private void SetInputCommands(List<string> inactiveButtonsTagList)
		{
			CommandsList.Add(new Command(pos => DisplayTowerSelectionPanel(pos, 
				inactiveButtonsTagList)).Add(new MouseButtonTrigger(MouseButton.Left, State.Releasing)));
			CommandsList.Add(new Command(HideTowerPanel).Add(new MouseButtonTrigger(MouseButton.Right, 
				State.Releasing)));
			CommandsList.Add(new Command(Game.EndGame).Add(new KeyTrigger(Key.Escape, State.Pressed)));
		}

		private void DisplayTowerSelectionPanel(Point pos, List<string> inactiveButtonsTagList)
		{
			if (isPanelDisplayed)
				return;

			if (!ClickedPositionInGrid(ConvertMouseClickTo3D(pos)))
				return;

			TowerPanel = new TowerSelectionPanel(pos, manager);
			TowerPanel.InactiveButtonsInPanel(inactiveButtonsTagList);
			isPanelDisplayed = true;
		}

		private bool isPanelDisplayed;

		public TowerSelectionPanel TowerPanel
		{
			get;
			private set;
		}

		private static Vector ConvertMouseClickTo3D(Point pos)
		{
			var floor = new Plane(Vector.UnitY, 0.0f);
			var ray = Camera.Current.ScreenPointToRay(pos);
			return (Vector)floor.Intersect(ray);
		}

		private bool ClickedPositionInGrid(Vector clickedPos)
		{
			var interactablePointsList = manager.Level.Get<Level.GridData>().InteractablePointsList;
			PositionInGrid = grid.ComputeGridCoordinates(grid, clickedPos, interactablePointsList);
			return grid.IsClickInGrid;
		}

		public Vector PositionInGrid
		{
			get;
			private set;
		}

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

			TowerPanel.Clear();
			isPanelDisplayed = false;
		}
	}
}