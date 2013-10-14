using $safeprojectname$.Content;
using DeltaEngine.Content;
using DeltaEngine.Multimedia;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace $safeprojectname$.GUI
{
	public class Hud : Menu
	{
		public Hud()
		{
			CreateHud();
		}

		private void CreateHud()
		{
			scene = ContentLoader.Load<Scene>(UI.GameHud.ToString());
			foreach (Control control in scene.Controls)
				if (control.GetType() == typeof(InteractiveButton))
					AttachButtonEvents(control);
		}

		private void AttachButtonEvents(Control control)
		{
			var tags = control.GetTags();
			foreach (var tag in tags)
				if (tag.Contains(Content.GUI.UIOptions.ToString()))
					control.Clicked += DisplayOptions;
				else if (tag.Contains(Content.GUI.UIDragonSpecialAttackBreath.ToString()))
					control.Clicked += DisplayOptions;
				else if (tag.Contains(Content.GUI.UIDragonSpecialAttackCannon.ToString()))
					control.Clicked += DisplayOptions;
		}

		private void DisplayOptions()
		{
			PlayClickedSound();
		}

		private static void PlayClickedSound()
		{
			var clickSound = ContentLoader.Load<Sound>(Sounds.PressButton.ToString());
			clickSound.Play();
		}
	}
}