using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.GUI;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers.Levels
{
	/// <summary>
	/// LevelBathRoom telling the player how to do things in a visual way
	/// </summary>
	public class LevelBathRoom : Scene
	{
		public LevelBathRoom()
		{
			InitializeGameManager();
			CreateLevel();
		}

		private void InitializeGameManager()
		{
			manager = new Manager(6.0f)
			{
				playerData = new Manager.PlayerData { ResourceFinances = 190 }
			};

			//gameHud = new Hud(manager);
		}

		private readonly List<string> inactiveButtonsTagList = new List<string>
		{
			Names.ButtonAcidTower,
			Names.ButtonImpactTower
		};

		private Manager manager;
		// private Hud gameHud;

		private void CreateLevel()
		{
			bathRoom = new Level(Names.LevelsBathroom);
			manager.Level = bathRoom;
			SetupBathRoomScene();
		}

		private Level bathRoom;

		private void SetupBathRoomScene()
		{
			bathRoomScene = new CreateSceneFromXml(Names.XmlSceneBathroom, Messages.BathRoomMessages());

			foreach (InteractiveButton button in bathRoomScene.InteractiveButtonList)
				AttachButtonEvents(button.GetTags()[0], button);
		}

		private CreateSceneFromXml bathRoomScene;

		private void AttachButtonEvents(string buttonName, InteractiveButton button)
		{
			switch (buttonName)
			{
			case Names.ButtonMessageNext:
				button.Clicked += NextDialogue;
				break;

			case Names.ButtonContinue:
				button.Clicked += LoadLivingRoomLevel;
				break;
			}
		}

		private void NextDialogue()
		{
			if (++bathRoomScene.MessageCount > Messages.BathRoomMessages().Length)
				SpawnCreep();

			bathRoomScene.NextDialogue();
		}

		private void SpawnCreep()
		{
			if (!manager.Contains<InputCommands>())
				manager.Add(new InputCommands(manager, inactiveButtonsTagList));

			var gridData = bathRoom.Get<Level.GridData>();
			var randomWaypointsList = SelectRandomWaypointList(gridData);
			var startGridPos = randomWaypointsList[0];
			var position =
				Game.CameraAndGrid.Grid.PropertyMatrix[startGridPos.Item1, startGridPos.Item2].MidPoint;
			manager.CreateCreep(position, Names.CreepCottonMummy, Creep.CreepType.Cloth, 
				MovementData(startGridPos, randomWaypointsList.GetRange(1, randomWaypointsList.Count - 1)));
		}

		private static List<Tuple<int, int>> SelectRandomWaypointList(Level.GridData data)
		{
			var randomNo = Randomizer.Current.Get(0, 1);
			return data.CreepPathsList[randomNo];
		}

		private static MovementInGrid.MovementData MovementData(Tuple<int, int> startPos,
			List<Tuple<int, int>> waypoints)
		{
			return new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.2f, 0.0f, 0.2f),
				StartGridPos = startPos,
				Waypoints = waypoints
			};
		}

		private void LoadLivingRoomLevel()
		{
			//gameHud.Dispose();
			Dispose();
			new LevelLivingRoom();
		}

		protected override void DisposeData()
		{
			bathRoomScene.Dispose();
			bathRoom.Dispose();
			manager.Dispose();
		}
	}
}