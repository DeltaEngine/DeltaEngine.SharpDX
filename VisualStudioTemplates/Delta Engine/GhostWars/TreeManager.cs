using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using DeltaEngine.Rendering.Sprites;

namespace $safeprojectname$
{
	public class TreeManager : Entity, Updateable
	{
		public TreeManager(Team playerTeam)
		{
			MainMenu.PlayerTeam = playerTeam;
			if (MainMenu.State != GameState.CountDown)
				MainMenu.State = GameState.Game;

			statusText = new FontText(MainMenu.Font, "", Rectangle.FromCenter(new Point(0.5f, 0.25f), 
				new Size(0.2f))) {
				RenderLayer = 5
			};
			new Sprite(new Material(Shader.Position2DUv, "Logo"), new Rectangle(0.02f, 0.205f, 0.15f, 
				0.15f)) {
				RenderLayer = -15
			};
			CreateArrowSelectionAndBars();
			OnClickSelectTree();
		}

		private readonly FontText statusText;

		private void OnClickSelectTree()
		{
			new Command(Command.Click, position => 
			{
				var nearestTree = FindNearestTree(null, position);
				if (nearestTree != null)
					selection.DrawArea = Rectangle.FromCenter(nearestTree.Center, new 
						Size(nearestTree.Size.Height));

				if (nearestTree == lastSelectedTree || Time.Total - lastSelectedTreeSoundTime < 0.2f)
					return;

				ContentLoader.Load<Sound>("MalletSwing").Play(0.15f);
				lastSelectedTreeSoundTime = Time.Total;
				lastSelectedTree = nearestTree;
			});
		}

		private Tree lastSelectedTree;
		private float lastSelectedTreeSoundTime;

		private void CreateArrowSelectionAndBars()
		{
			arrow = Effects.CreateArrow(Point.Unused, Point.Unused);
			selection = new Sprite(new Material(Shader.Position2DColorUv, "SelectionCircle"), 
				Rectangle.Zero);
			bars = new[] {
				new FilledRect(barsArea, Team.HumanYellow.ToColor()) {
					RenderLayer = -20
				},
				new FilledRect(barsArea, Team.ComputerPurple.ToColor()) {
					RenderLayer = -20
				},
				new FilledRect(barsArea, Team.ComputerTeal.ToColor()) {
					RenderLayer = -20
				}
			};
			UpdateBars();
		}

		private Sprite arrow;
		private Sprite selection;
		private readonly Rectangle barsArea = new Rectangle(0.2f, 0.765f, 0.6f, 0.01f);
		private FilledRect[] bars;

		private void UpdateBars()
		{
			for (int i = 0; i < 3; i++)
				ghosts [i] = 0;

			foreach (var tree in trees)
				if (tree.Team != Team.None)
					ghosts [(int)tree.Team - 1] += tree.NumberOfGhosts;

			int totalGhosts = ghosts.Sum();
			if (totalGhosts == 0 || MainMenu.State != GameState.Game)
				return;

			float oldX = 0;
			for (int i = 0; i < 3; i++)
			{
				float width = (float)ghosts [i] / totalGhosts;
				bars [i].DrawArea = barsArea.GetInnerRectangle(new Rectangle(oldX, 0, width, 1));
				oldX += width;
			}
			if (lastSelectedTree != null)
				selection.DrawArea = Rectangle.FromCenter(lastSelectedTree.Center, new 
					Size(lastSelectedTree.Size.Height));
		}

		private readonly int[] ghosts = {
			0,
			0,
			0
		};

		public void AddTree(Point position, Team team)
		{
			var newTree = new Tree(position, team);
			trees.Add(newTree);
			UpdateBars();
			new Command(newTree.TryToUpgrade).Add(new MouseHoldTrigger(newTree.DrawArea)).Add(new 
				TouchHoldTrigger(newTree.DrawArea));
			new Command((start, end, dragDone) => MoveGhostsFromTreeToTree(start, end, dragDone, 
				newTree)).Add(new MouseDragTrigger()).Add(new TouchDragTrigger());
		}

		private void MoveGhostsFromTreeToTree(Point start, Point end, bool dragDone, Tree startTree)
		{
			if (start.DistanceTo(startTree.DrawArea.Center) > 0.04f)
				return;

			var targetTree = FindNearestTree(startTree, end);
			arrow.Visibility = Visibility.Hide;
			if (startTree.Team != MainMenu.PlayerTeam || targetTree == startTree || MainMenu.State != 
				GameState.Game || startTree.Center.DistanceTo(end) < 0.05f)
				return;

			arrow.IsActive = false;
			arrow = Effects.CreateArrow(startTree.Center, targetTree.Center);
			arrow.Color = startTree.Team.ToColor();
			arrow.Visibility = dragDone ? Visibility.Hide : Visibility.Show;
			if ((dragDone && Time.Total - lastWaveSend > TimeBetweenWaves || !dragDone && 
				Time.CheckEvery(TimeBetweenWaves)) && startTree.NumberOfGhosts >= 1)
			{
				lastWaveSend = Time.Total;
				SendWave(startTree, targetTree);
			}
		}

		private const float TimeBetweenWaves = 0.5f;

		private void SendWave(Tree startTree, Tree targetTree)
		{
			ContentLoader.Load<Sound>("GhostWaveStart").Play(0.5f);
			var ghostsToSend = Math.Min(startTree.NumberOfGhosts, 5);
			startTree.NumberOfGhosts -= ghostsToSend;
			UpdateBars();
			if (targetTree == null)
				return;

			var wave = new GhostWave(startTree.Center, targetTree.Center, ghostsToSend, 
				startTree.Team.ToColor());
			wave.Attacker = startTree.Team;
			wave.TargetReached += (attacker, waveSize) => 
			{
				targetTree.Attack((Team)attacker, waveSize);
				UpdateBars();
			};
		}

		private float lastWaveSend;

		private Tree FindNearestTree(Tree previousTree, Point target)
		{
			float nearestDistance = float.MaxValue;
			Tree nearestTree = null;
			foreach (var tree in trees)
				if (tree != previousTree)
				{
					float treeDistance = tree.Center.DistanceTo(target);
					if (previousTree != null)
						treeDistance += Math.Abs(previousTree.Center.RotationTo(target) - 
							previousTree.Center.RotationTo(tree.Center)) / 500.0f;

					if (treeDistance > nearestDistance)
						continue;

					nearestDistance = treeDistance;
					nearestTree = tree;
				}

			return nearestTree;
		}

		private readonly List<Tree> trees = new List<Tree>();
		private float countdownTimer = 4.1f;

		public void Update()
		{
			if (MainMenu.State == GameState.CountDown)
				HandleCountDownGameState();

			if (MainMenu.State == GameState.Game)
				HandleGameState();
		}

		private void HandleCountDownGameState()
		{
			if ((int)countdownTimer <= 0)
				statusText.Text = "Go Go Go";
			else
				statusText.Text = "Game starting in " + (int)countdownTimer;
			countdownTimer -= Time.Delta;
			if ((int)countdownTimer != (int)(countdownTimer + Time.Delta))
				ContentLoader.Load<Sound>("GhostWaveStart").Play();

			if (countdownTimer < 0)
				MainMenu.State = GameState.Game;
		}

		private void HandleGameState()
		{
			if (AllTreesBelongTo(MainMenu.PlayerTeam))
				HandleWinSitutation();
			else if (AllTreesBelongTo(Team.ComputerTeal) || AllTreesBelongTo(Team.ComputerPurple))
				HandleLostSituation();
			else if (Time.Total > 2.5f)
				statusText.Text = "";

			if (!Time.CheckEvery(TimeBetweenWaves))
				return;

			UpdateBars();
			foreach (var tree in trees.Where(tree => tree.IsAi))
				HandleAi(tree);
		}

		private void HandleWinSitutation()
		{
			statusText.Text = "";
			MainMenu.State = GameState.GameOver;
			new Sprite(new Material(Shader.Position2DUv, "YouWin"), Point.Half) {
				RenderLayer = 4000
			};
		}

		private void HandleLostSituation()
		{
			statusText.Text = "You Lost!";
			MainMenu.State = GameState.GameOver;
			new Sprite(new Material(Shader.Position2DUv, "GameOver"), Point.Half) {
				RenderLayer = 4000
			};
		}

		private void HandleAi(Tree tree)
		{
			var nearestFreeTree = FindNearestTreeFreeTree(tree);
			var nearestTree = FindNearestTreeWeakTree(tree);
			if (tree.NumberOfGhosts > 10 && nearestFreeTree != null)
				SendWave(tree, nearestFreeTree);
			else if (tree.NumberOfGhosts > 15 && nearestTree != null)
				SendWave(tree, nearestTree);
			else if (tree.NumberOfGhosts > 45)
				SendWave(tree, trees [Randomizer.Current.Get(0, trees.Count)]);
		}

		private Tree FindNearestTreeFreeTree(Tree sourceTree)
		{
			float nearestDistance = float.MaxValue;
			Tree nearestTree = null;
			foreach (var tree in trees)
				if (tree.Team == Team.None)
				{
					float treeDistance = tree.Center.DistanceTo(sourceTree.Center);
					if (treeDistance > nearestDistance)
						continue;

					nearestDistance = treeDistance;
					nearestTree = tree;
				}

			return nearestTree;
		}

		private Tree FindNearestTreeWeakTree(Tree sourceTree)
		{
			float nearestDistance = float.MaxValue;
			Tree nearestTree = null;
			foreach (var tree in trees)
				if (tree.Team != Team.None && tree.Team != sourceTree.Team && tree.NumberOfGhosts < 
					sourceTree.NumberOfGhosts)
				{
					float treeDistance = tree.Center.DistanceTo(sourceTree.Center);
					if (treeDistance > nearestDistance)
						continue;

					nearestDistance = treeDistance;
					nearestTree = tree;
				}

			return nearestTree;
		}

		private bool AllTreesBelongTo(Team team)
		{
			return trees.All(tree => tree.Team == team);
		}
	}
}