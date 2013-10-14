using $safeprojectname$.Content;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$.GUI
{
	public class IntroScene : Menu
	{
		public IntroScene()
		{
			count = 0;
			introSprites = new Sprite[5];
			InitializeIntroScene();
			InitializeImagesAndSetVisibility();
		}

		private int count;
		private readonly Sprite[] introSprites;

		private void InitializeIntroScene()
		{
			scene = ContentLoader.Load<Scene>(UI.SceneIntroMenu.ToString());
			CreateIntroScene();
		}

		private void CreateIntroScene()
		{
			foreach (Control control in scene.Controls)
			{
				if (control.ContainsTag(Content.GUI.IntroButtonFlipLeft.ToString()))
					control.IsVisible = false;

				AttachButtonEvents(control);
			}
		}

		private void AttachButtonEvents(Control control)
		{
			var tags = control.GetTags();
			foreach (var tag in tags)
				if (tag.Contains(Content.GUI.IntroButtonFlipRight.ToString()))
					control.Clicked += MoveIntroSceneForward;
				else if (tag.Contains(Content.GUI.IntroButtonFlipLeft.ToString()))
				{
					control.Clicked += MoveIntroSceneBackward;
					backButton = control;
				} else if (tag.Contains(Content.GUI.IntroButtonSkip.ToString()))
				{
					control.Clicked += StartTutorial;
					forwardButton = control;
				}
		}

		private Control backButton;
		private Control forwardButton;

		private void InitializeImagesAndSetVisibility()
		{
			introSprites [0] = new Sprite(new Material(Shader.Position2DColorUV, 
				ComicStrips.ComicStripsStoryboardPanel1.ToString()), ScreenSpace.Current.Viewport);
			introSprites [1] = new Sprite(new Material(Shader.Position2DColorUV, 
				ComicStrips.ComicStripsStoryboardPanel2.ToString()), ScreenSpace.Current.Viewport);
			introSprites [1].IsVisible = false;
			introSprites [2] = new Sprite(new Material(Shader.Position2DColorUV, 
				ComicStrips.ComicStripsStoryboardPanel3.ToString()), ScreenSpace.Current.Viewport);
			introSprites [2].IsVisible = false;
			introSprites [3] = new Sprite(new Material(Shader.Position2DColorUV, 
				ComicStrips.ComicStripsStoryboardPanel4.ToString()), ScreenSpace.Current.Viewport);
			introSprites [3].IsVisible = false;
			introSprites [4] = new Sprite(new Material(Shader.Position2DColorUV, 
				ComicStrips.ComicStripsStoryboardPanel5.ToString()), ScreenSpace.Current.Viewport);
			introSprites [4].IsVisible = false;
		}

		private void MoveIntroSceneForward()
		{
			PlayClickedSound();
			if (count == introSprites.Length - 1)
				StartTutorial();

			FadeOut();
			count++;
			EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver += FadeIn;
			ToggleButtonStatesMovingForward();
		}

		private static void PlayClickedSound()
		{
			var clickSound = ContentLoader.Load<Sound>(Sounds.PressButton.ToString());
			clickSound.Play();
		}

		private void FadeOut()
		{
			ShowFadeEffect(Color.TransparentWhite, Color.Black);
		}

		private void ShowFadeEffect(Color startColor, Color endColor)
		{
			if (count == introSprites.Length)
				return;

			var image = introSprites [count];
			if (image.Contains<FadeEffect.TransitionData>())
				image.Remove<FadeEffect.TransitionData>();

			image.Add(new FadeEffect.TransitionData {
				Colour = new RangeGraph<Color>(startColor, endColor),
				Duration = 1.0f,
			});
			image.Start<FadeEffect>();
			EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver -= FadeIn;
		}

		private void FadeIn()
		{
			if (count < introSprites.Length - 1)
				introSprites [count + 1].IsVisible = false;

			ShowFadeEffect(Color.TransparentBlack, Color.White);
		}

		private void ToggleButtonStatesMovingForward()
		{
			if (count > 0 && count < introSprites.Length - 1)
				backButton.IsVisible = true;
		}

		private void MoveIntroSceneBackward()
		{
			PlayClickedSound();
			if (count == 0)
				return;

			FadeOut();
			count--;
			EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver += FadeIn;
			ToggleButtonStatesMovingBackwards();
		}

		private void ToggleButtonStatesMovingBackwards()
		{
			if (count == introSprites.Length - 2)
			{
				forwardButton.IsVisible = true;
				backButton.IsVisible = true;
			} else if (count == 0)
			{
				forwardButton.IsVisible = true;
				backButton.IsVisible = false;
			}
		}

		private void StartTutorial()
		{
			PlayClickedSound();
			Dispose();
		}

		protected void Dispose()
		{
			foreach (Sprite sprite in introSprites)
				sprite.IsActive = false;

			scene.Dispose();
		}
	}
}