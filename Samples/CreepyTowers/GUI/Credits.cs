using System.Linq;
using CreepyTowers.Content;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Multimedia;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace CreepyTowers.GUI
{
	public class Credits : Menu
	{
		public Credits()
		{
			remap = new RemapCoordinates();
			CreateCreditsScene();
		}

		private readonly RemapCoordinates remap;

		private void CreateCreditsScene()
		{
			scene = ContentLoader.Load<Scene>(UI.SceneCredits.ToString());
			scene.SetQuadraticBackground(new Material(Shader.Position2DUV, Content.GUI.CreditsScreen.ToString()));
			foreach (Control control in scene.Controls)
			{
				var imageSize = remap.RemapCoordinateSpaces(control.DrawArea.Size);
				var centerPos = remap.RemapCoordinateSpaces(control.DrawArea.TopLeft);
				var drawArea = Rectangle.FromCenter(centerPos, imageSize);
				DefineBackButton(control, drawArea);
			}
		}

		private static void DefineBackButton(Control button, Rectangle drawArea)
		{
			button.Clicked += () =>
			{
				var clickSound = ContentLoader.Load<Sound>(Sounds.PressButton.ToString());
				clickSound.Play();
				MainMenu mainMenu = MenuManager.Current.GetScenesOfType<MainMenu>().First();
				if (mainMenu != null)
					MenuManager.Current.SetCurrentScene(mainMenu);
			};
		}
	}
}