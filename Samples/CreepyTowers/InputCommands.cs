using System;
using System.Collections.Generic;
using CreepyTowers.GUI;
using CreepyTowers.Levels;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Rendering3D.Cameras;

namespace CreepyTowers
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

		public List<Command> CommandsList { get; private set; }

		private readonly LevelGrid grid;
		private readonly Manager manager;

		private void SetInputCommands(List<string> inactiveButtonsTagList)
		{
			CommandsList.Add(
				new Command(pos => DisplayTowerSelectionPanel(pos, inactiveButtonsTagList)).Add(
					new MouseButtonTrigger(MouseButton.Left, State.Releasing)));

			CommandsList.Add(
				new Command(HideTowerPanel).Add(new MouseButtonTrigger(MouseButton.Right, State.Releasing)));

			CommandsList.Add(
				new Command(Game.EndGame).Add(new KeyTrigger(Key.Escape, State.Pressed)));
		}

		private void DisplayTowerSelectionPanel(Vector2D pos, List<string> inactiveButtonsTagList)
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
		public TowerSelectionPanel TowerPanel { get; private set; }

		private static Vector3D ConvertMouseClickTo3D(Vector2D pos)
		{
			var floor = new Plane(Vector3D.UnitY, 0.0f);
			var ray = Camera.Current.ScreenPointToRay(pos);
			return (Vector3D)floor.Intersect(ray);
		}

		private bool ClickedPositionInGrid(Vector3D clickedPos)
		{
			var interactablePointsList = manager.Level.Get<Level.GridData>().InteractablePointsList;
			PositionInGrid = grid.ComputeGridCoordinates(grid, clickedPos, interactablePointsList);
			return grid.IsClickInGrid;
		}

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

			TowerPanel.Clear();
			isPanelDisplayed = false;
		}
	}
}