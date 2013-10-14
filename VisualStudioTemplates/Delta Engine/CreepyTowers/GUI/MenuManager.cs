using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Scenes;

namespace $safeprojectname$.GUI
{
	public class MenuManager
	{
		public MenuManager()
		{
			scenesList = new List<Scene>();
			Current = this;
		}

		public static MenuManager Current;
		private readonly List<Scene> scenesList;

		public void SetCurrentScene(Menu menu)
		{
			if (CurrentScene != null)
				CurrentScene.Hide();

			menu.scene.Show();
			CurrentScene = menu.scene;
		}

		public Scene CurrentScene;

		public void AddScene(Scene scene)
		{
			scene.Hide();
			if (!scenesList.Contains(scene))
				scenesList.Add(scene);
		}

		public List<T> GetScenesOfType<T>() where T : Menu
		{
			return scenesList.OfType<T>().ToList();
		}
	}
}