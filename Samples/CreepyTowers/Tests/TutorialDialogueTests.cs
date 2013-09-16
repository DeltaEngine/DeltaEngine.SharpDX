//using DeltaEngine.Input;
//using DeltaEngine.Platforms;
//using DeltaEngine.ScreenSpaces;
//using NUnit.Framework;

//namespace CreepyTowers.Tests
//{
//	/// <summary>
//	/// Tests and Visual Tests for the ChildsRoom-Dialogue
//	/// </summary>
//	internal class TutorialDialogueTests : TestWithMocksOrVisually
//	{
//		[Test]
//		public void DisplayFullDialogueAndAdvanceByClick()
//		{
//			var screen = Resolve<ScreenSpace>();
//			//InitDialogue(screen);
//			dialogue.DialogueEnded += () => screen.Window.Dispose();
//		}

//		//[Test]
//		//public void AdvanceDialogueToEnd()
//		//{
//		//	var screen = Resolve<ScreenSpace>();
//		//	InitDialogue(screen);
//		//	bool ended = false;
//		//	dialogue.DialogueEnded += () => { ended = true; };
//		//	for (int i = 0; i < 9; i++)
//		//		dialogue.NextDialogue();
//		//	Assert.IsTrue(ended);
//		//}

//		//[Test]
//		//public void SkipTutorial()
//		//{
//		//	var screen = Resolve<ScreenSpace>();
//		//	InitDialogue(screen);
//		//	bool ended = false;
//		//	dialogue.DialogueEnded += () => { ended = true; };
//		//	dialogue.SkipTutorial();
//		//	Assert.IsTrue(ended);
//		//}

//		//private void InitDialogue(ScreenSpace screen)
//		//{
//		//	manager = new Manager(screen, new MainMenu(screen));
//		//	manager.TutorialStarter();
//		//	dialogue = manager.ChildsRoom;
//		//}

//		private LevelChildsRoom dialogue;
//		private Manager manager;
//	}
//}