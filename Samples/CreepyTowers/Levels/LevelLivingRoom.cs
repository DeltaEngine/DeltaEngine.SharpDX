using System;
using System.Collections.Generic;
using CreepyTowers.GUI;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers.Levels
{
	/// <summary>
	/// LevelChildsRoom telling the player how to do things in a visual way
	/// </summary>
	public class LevelLivingRoom : Scene, IDisposable
	{
		public LevelLivingRoom()
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
		}

		private readonly List<string> inactiveButtonsTagList = new List<string>
		{
			Names.ButtonAcidTower,
			Names.ButtonImpactTower
		};

		private Manager manager;

		private void CreateLevel()
		{
			livingRoom = new Level(Names.LevelsLivingRoom);
			manager.Level = livingRoom;
			SetupLivingRoomScene();
		}

		private Level livingRoom;

		private void SetupLivingRoomScene()
		{
			livingRoomScene = new CreateSceneFromXml(Names.XmlSceneLivingRoom,
				Messages.LivingRoomMessages());

			foreach (InteractiveButton button in livingRoomScene.InteractiveButtonList)
				AttachButtonEvents(button.GetTags()[0], button);
		}

		private CreateSceneFromXml livingRoomScene;

		private void AttachButtonEvents(string buttonName, InteractiveButton button)
		{
			switch (buttonName)
			{
			case Names.ButtonMessageNext:
				button.Clicked += NextDialogue;
				break;
			}
		}

		private void SpawnCreep()
		{
			if (!manager.Contains<InputCommands>())
				manager.Add(new InputCommands(manager, inactiveButtonsTagList));

			var gridData = livingRoom.Get<Level.GridData>();
			var randomWaypointsList = SelectRandomWaypointList(gridData);
			var startGridPos = randomWaypointsList[0];
			var position =
				Game.CameraAndGrid.Grid.PropertyMatrix[startGridPos.Item1, startGridPos.Item2].MidPoint;
			manager.CreateCreep(position, Names.CreepCottonMummy,
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

		private void NextDialogue()
		{
			if (++livingRoomScene.MessageCount > Messages.LivingRoomMessages().Length)
				SpawnCreep();

			livingRoomScene.NextDialogue();
		}

		public new void Dispose()
		{
			livingRoomScene.Dispose();
			livingRoom.Dispose();
			manager.Dispose();
		}
	}
}