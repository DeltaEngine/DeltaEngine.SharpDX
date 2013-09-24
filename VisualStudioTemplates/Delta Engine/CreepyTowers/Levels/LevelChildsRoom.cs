using System;
using System.Collections.Generic;
using $safeprojectname$.GUI;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Sprites;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace $safeprojectname$.Levels
{
	public class LevelChildsRoom
	{
		public LevelChildsRoom()
		{
			InitializeGameManager();
			CreateLevel();
			ShowHud();
			SetupChildsRoomScene();
		}

		private void InitializeGameManager()
		{
			manager = new Manager(3.8f) {
				playerData = new Manager.PlayerData {
					ResourceFinances = 190
				}
			};
		}

		private readonly List<string> inactiveButtonsTagList = new List<string> {
			Names.ButtonAcidTower,
			Names.ButtonIceTower,
			Names.ButtonImpactTower,
			Names.ButtonSliceTower
		};
		private Manager manager;

		private void CreateLevel()
		{
			childrensRoom = new Level(Names.LevelsChildrensRoom);
			manager.Level = childrensRoom;
		}

		private Level childrensRoom;

		private void ShowHud()
		{
			hud = new Hud();
			hud.Hide();
		}

		private Hud hud;

		private void SetupChildsRoomScene()
		{
			childsRoomScene = new CreateSceneFromXml(Names.XmlSceneChildsRoom, 
				Messages.ChildsRoomMessages());
			foreach (InteractiveButton button in childsRoomScene.InteractiveButtonList)
				AttachButtonEvents(button.GetTags() [0], button);
		}

		private CreateSceneFromXml childsRoomScene;

		private void AttachButtonEvents(string buttonName, InteractiveButton button)
		{
			switch (buttonName)
			{
				case Names.ButtonMessageNext:
					button.Clicked += NextDialogue;
					break;
				case Names.ButtonMessageBack:
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

		private void SpawnCreep()
		{
			DisplayAttackIcons();
			if (!manager.Contains<InputCommands>())
				manager.Add(new InputCommands(manager, inactiveButtonsTagList));

			var gridData = childrensRoom.Get<Level.GridData>();
			var randomWaypointsList = SelectRandomWaypointList(gridData);
			var startGridPos = randomWaypointsList [0];
			var position = Game.CameraAndGrid.Grid.PropertyMatrix [startGridPos.Item1, 
				startGridPos.Item2].MidPoint;
			manager.CreateCreep(position, Names.CreepCottonMummy, MovementData(startGridPos, 
				randomWaypointsList.GetRange(1, randomWaypointsList.Count - 1)));
		}

		private void DisplayAttackIcons()
		{
			foreach (InteractiveButton button in childsRoomScene.InteractiveButtonList)
				button.Visibility = Visibility.Show;
		}

		private static List<Tuple<int, int>> SelectRandomWaypointList(Level.GridData data)
		{
			var randomNo = Randomizer.Current.Get(0, 1);
			return data.CreepPathsList [randomNo];
		}

		private static MovementInGrid.MovementData MovementData(Tuple<int, int> startPos, 
			List<Tuple<int, int>> waypoints)
		{
			return new MovementInGrid.MovementData {
				Velocity = new Vector3D(0.2f, 0.0f, 0.2f),
				StartGridPos = startPos,
				Waypoints = waypoints
			};
		}

		private void NextDialogue()
		{
			if (++childsRoomScene.MessageCount >= Messages.ChildsRoomMessages().Length)
			{
				childsRoomScene.Hide();
				hud.Show();
			}
			childsRoomScene.NextDialogue();
		}

		private void DialogueBack()
		{
			childsRoomScene.DialogueBack();
		}

		private void LoadBathroomLevel()
		{
			Dispose();
			new LevelBathRoom();
		}

		private static void DisplayAttackImage()
		{
			var attackImage = new Sprite(new Material(Shader.Position2DUv, Names.DragonAttackMockup), 
				Rectangle.One) {
				RenderLayer = (int)CreepyTowersRenderLayer.Dialogues
			};
		}

		public void Dispose()
		{
			childsRoomScene.Dispose();
			childrensRoom.Dispose();
			manager.Dispose();
		}
	}
}