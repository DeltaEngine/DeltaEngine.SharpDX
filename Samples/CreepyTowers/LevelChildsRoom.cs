using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers
{
	/// <summary>
	/// LevelChildsRoom telling the player how to do things in a visual way
	/// </summary>
	public class LevelChildsRoom
	{
		public LevelChildsRoom()
		{
			InitializeGameManager();
			CreateLevel();
		}

		private void InitializeGameManager()
		{
			manager = new Manager(3.8f)
			{
				playerData = new Manager.PlayerData { ResourceFinances = 190 }
			};

			//gameHud = new Hud(manager);
		}

		private readonly List<string> inactiveButtonsTagList = new List<string>
		{
			Names.ButtonAcidTower,
			Names.ButtonIceTower,
			Names.ButtonImpactTower,
			Names.ButtonSliceTower
		};

		private Manager manager;
		// private Hud gameHud;

		private void CreateLevel()
		{
			childrensRoom = new Level(Names.LevelsChildsRoom);
			manager.Level = childrensRoom;
			SetupChildsRoomScene();
		}

		private Level childrensRoom;

		private void SetupChildsRoomScene()
		{
			childsRoomScene = new CreateSceneFromXml(Names.XmlSceneChildsRoom,
				Messages.ChildsRoomMessages());

			foreach (InteractiveButton button in childsRoomScene.InteractiveButtonList)
				AttachButtonEvents(button.GetTags()[0], button);
		}

		private CreateSceneFromXml childsRoomScene;

		private void AttachButtonEvents(string buttonName, InteractiveButton button)
		{
			switch (buttonName)
			{
			case Names.ButtonRefresh:
				button.Clicked += Refresh;
				break;

			case Names.ButtonCreep:
				button.Clicked += SpawnCreep;
				break;

			case Names.ButtonNext:
				button.Clicked += NextDialogue;
				break;

			case Names.ButtonBackLeft:
				button.Clicked += DialogueBack;
				break;

			case Names.ButtonContinue:
				button.Clicked += LoadBathroomLevel;
				break;

			case Names.UIDragonSpecialAttackBreath:
				button.Clicked += DisplayAttackImage;
				break;

			case Names.UIDragonSpecialAttackCannon:
				button.Clicked += DisplayAttackImage;
				break;
			}
		}

		private void Refresh()
		{
			manager.Dispose();
			childsRoomScene.MessageCount = 0;
		}

		private void SpawnCreep()
		{
			DisplayAttackIcons();

			if (!manager.Contains<InputCommands>())
				manager.Add(new InputCommands(manager, inactiveButtonsTagList));

			var gridData = childrensRoom.Get<Level.GridData>();
			var randomWaypointsList = SelectRandomWaypointList(gridData);
			var startGridPos = randomWaypointsList[0];
			var position =
				Game.CameraAndGrid.Grid.PropertyMatrix[startGridPos.Item1, startGridPos.Item2].MidPoint;
			manager.CreateCreep(position, Names.CreepCottonMummy,
				MovementData(startGridPos, randomWaypointsList.GetRange(1, randomWaypointsList.Count - 1)));
		}

		private void DisplayAttackIcons()
		{
			foreach (InteractiveButton button in childsRoomScene.InteractiveButtonList)
				button.Visibility = Visibility.Show;
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
				Velocity = new Vector(0.2f, 0.0f, 0.2f),
				StartGridPos = startPos,
				Waypoints = waypoints
			};
		}

		private void NextDialogue()
		{
			if (++childsRoomScene.MessageCount >= Messages.ChildsRoomMessages().Length)
				SpawnCreep();

			childsRoomScene.NextDialogue();
		}

		private void DialogueBack()
		{
			childsRoomScene.DialogueBack();
		}

		private void LoadBathroomLevel()
		{
			//gameHud.Dispose();
			Dispose();
			new LevelBathRoom();
		}

		private static void DisplayAttackImage()
		{
			var attackImage = new Sprite(new Material(Shader.Position2DUv, Names.DragonAttackMockup),
				Rectangle.One) { RenderLayer = (int)CreepyTowersRenderLayer.Dialogues };
			//attackImage.Add(new Transition.Duration(2.0f)).Add(new Transition.FadingColor(Color.White));
			//attackImage.Start<Transition>();
			//attackImage.Start<FinalTransition>();
		}

		public void Dispose()
		{
			childsRoomScene.Dispose();
			childrensRoom.Dispose();
			manager.Dispose();
		}
	}
}