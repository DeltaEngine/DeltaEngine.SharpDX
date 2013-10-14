using System.Linq;
using $safeprojectname$.Content;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Multimedia;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace $safeprojectname$.GUI
{
	public class MainMenu : Menu
	{
		public MainMenu()
		{
			AddEscCommand();
			AddMenuBackground();
			AddMenuItems();
			if (ContentLoader.Exists("MenuMusic", ContentType.Music))
			{
				var music = ContentLoader.Load<Music>("MenuMusic");
				music.Loop = true;
				music.Play();
			}
		}

		private void AddEscCommand()
		{
			escCommand = new Command(Command.Exit, ExitGame);
		}

		private Command escCommand;

		private void AddMenuBackground()
		{
		}

		private void AddMenuItems()
		{
			scene = ContentLoader.Load<Scene>(UI.SceneMainMenu.ToString());
			scene.SetQuadraticBackground(new Material(Shader.Position2DUV, 
				Content.GUI.TextureBackgroundBlue.ToString()));
			foreach (Control control in scene.Controls)
				if (control.GetType() == typeof(InteractiveButton))
					AttachButtonEvents(control);
		}

		private void AttachButtonEvents(Control control)
		{
			var tags = control.GetTags();
			foreach (var tag in tags)
				if (tag.Contains(Content.GUI.MenuButtonPlay.ToString()))
					control.Clicked += StartIntro;
				else if (tag.Contains(Content.GUI.MenuButtonHelpAndOptions.ToString()))
					control.Clicked += DisplayOptions;
				else if (tag.Contains(Content.GUI.MenuButtonCredits.ToString()))
					control.Clicked += DisplayCredits;
				else if (tag.Contains(Content.GUI.MenuButtonQuit.ToString()))
					control.Clicked += ExitGame;
		}

		private void StartIntro()
		{
			PlayClickedSound();
			scene.Hide();
			new IntroScene();
		}

		private static void PlayClickedSound()
		{
			var clickSound = ContentLoader.Load<Sound>(Sounds.PressButton.ToString());
			clickSound.Play();
		}

		public Manager manager;

		private static void DisplayOptions()
		{
			PlayClickedSound();
			var options = MenuManager.Current.GetScenesOfType<OptionMenu>().First();
			if (options != null)
				MenuManager.Current.SetCurrentScene(options);
		}

		private static void DisplayCredits()
		{
			PlayClickedSound();
			Credits credits = MenuManager.Current.GetScenesOfType<Credits>().First();
			if (credits != null)
				MenuManager.Current.SetCurrentScene(credits);
		}

		private void ExitGame()
		{
			PlayClickedSound();
			scene.Dispose();
			Game.EndGame();
		}
	}
}