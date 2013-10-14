using System.Linq;
using $safeprojectname$.Content;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace $safeprojectname$.GUI
{
	public class OptionMenu : Menu
	{
		public OptionMenu()
		{
			AddOptionMenuItems();
		}

		private void AddOptionMenuItems()
		{
			scene = ContentLoader.Load<Scene>(UI.SceneOptionsMenu.ToString());
			scene.SetQuadraticBackground(new Material(Shader.Position2DUV, 
				Content.GUI.TextureBackgroundBlue.ToString()));
			foreach (Control control in scene.Controls)
			{
				if (control.GetType() == typeof(InteractiveButton))
					AttachButtonEvents(control);
			}
			scene.Show();
		}

		private static void AttachButtonEvents(Control control)
		{
			control.Clicked += BackMainMenu;
		}

		private static void BackMainMenu()
		{
			var clickSound = ContentLoader.Load<Sound>(Sounds.PressButton.ToString());
			clickSound.Play();
			var mainMenu = MenuManager.Current.GetScenesOfType<MainMenu>().First();
			if (mainMenu != null)
				MenuManager.Current.SetCurrentScene(mainMenu);
		}
	}
}