using System;
using System.IO;
using CreepyTowers.GUI;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace CreepyTowers.Tests.GUI
{
	public class IntroSceneTests : TestWithMocksOrVisually
	{
		[Test]
		public void ClickToStartIntroScreen()
		{
			Resolve<Window>();
			introScreen =
				new Sprite(new Material(Shader.Position2DColorUV, "ComicStripsStoryboardPanel1"),
					ScreenSpace.Current.Viewport);
			new Command(StartFadeOutEffect).Add(new MouseButtonTrigger(MouseButton.Left, State.Releasing));
		}

		private Sprite introScreen;

		private void StartFadeOutEffect()
		{
			introScreen.Add(new FadeEffect.TransitionData
			{
				Colour = new RangeGraph<Color>(Color.TransparentWhite, Color.Black),
				Duration = 1.0f,
			});
			introScreen.Start<FadeEffect>();
		}

		[Test]
		public void StartNextImageFadeEffectAfterPreviousEnds()
		{
			Resolve<Window>();
			introScreen =
				new Sprite(new Material(Shader.Position2DColorUV, "ComicStripsStoryboardPanel1"),
					ScreenSpace.Current.Viewport);
			new Command(StartFadeOutEffect).Add(new MouseButtonTrigger(MouseButton.Left, State.Releasing));
			EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver += StartFadeInEffect;
		}

		private static void StartFadeInEffect()
		{
			var image1 =
				new Sprite(new Material(Shader.Position2DColorUV, "ComicStripsStoryboardPanel2"),
					ScreenSpace.Current.Viewport);
			image1.IsVisible = false;
			image1.Add(new FadeEffect.TransitionData
			{
				Colour = new RangeGraph<Color>(Color.TransparentBlack, Color.White),
				Duration = 1.0f,
			});
			image1.Start<FadeEffect>();
			//EntitiesRunner.Current.GetUpdateBehavior<FadeEffect>().EffectOver -= StartFadeInEffect;
		}

		[Test]
		public void PlayIntroScene()
		{
			new Game(Resolve<Window>());
			//window.ViewportPixelSize = new Size(1920, 1080);
			new IntroScene();
		}

		[Test]
		public void FadeOutButton()
		{
			button = new InteractiveButton(CreateTheme(Content.GUI.IntroButtonFlipLeft.ToString()),
				ScreenSpace.Current.Viewport);
			button.UpdatePriority = Priority.Last;
			new Command(StartButtonFade).Add(new MouseButtonTrigger(MouseButton.Right, State.Releasing));
		}

		private InteractiveButton button;

		private void StartButtonFade()
		{
			button.Add(new FadeEffect.TransitionData
			{
				Colour = new RangeGraph<Color>(Color.TransparentWhite, Color.Black),
				Duration = 1.0f,
			});
			button.Start<FadeEffect>();
		}

		private static Theme CreateTheme(string buttonImageName)
		{
			var appearance = new Theme.Appearance(buttonImageName);
			return new Theme
			{
				Button = appearance,
				ButtonDisabled = new Theme.Appearance(buttonImageName, Color.Gray),
				ButtonMouseover = appearance,
				ButtonPressed = appearance
				//Font = ContentLoader.Load<Font>(Names.FontChelseaMarket14)
			};
		}

		[Test]
		public void CheckForFadeOutEffect()
		{
			var sprite = CreateDummySprite();
			sprite.Start<FadeEffect>();
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.IsTrue(sprite.IsVisible);
		}

		private static Sprite CreateDummySprite()
		{
			var sprite =
				new Sprite(new Material(Shader.Position2DColorUV, "ComicStripsStoryboardPanel1"),
					ScreenSpace.Current.Viewport);
			sprite.Add(new FadeEffect.TransitionData
			{
				Colour = new RangeGraph<Color>(Color.TransparentWhite, Color.Black),
				Duration = 1.0f,
			});
			return sprite;
		}

		[Test]
		public void EffectStopsWhenTImeDurationIsOver()
		{
			var sprite = CreateDummySprite();
			var transitionData = sprite.Get<FadeEffect.TransitionData>();
			var timeBeforeStart = transitionData.ElapsedTime;
			sprite.Start<FadeEffect>();
			AdvanceTimeAndUpdateEntities(1.5f);
			Assert.Greater(sprite.Get<FadeEffect.TransitionData>().ElapsedTime, timeBeforeStart);
			Assert.Greater(sprite.Get<FadeEffect.TransitionData>().ElapsedTime, transitionData.Duration);
			Assert.IsFalse(sprite.Contains<FadeEffect>());
		}

		[Test]
		public void CreateSampleScene()
		{
			var window = Resolve<Window>();
			window.ViewportPixelSize = new Size(800, 600);
			window.Title = "Test";
			//CreateDummySceneWithOneButton();
			var scene = new Scene();
			var drawArea = Rectangle.FromCenter(Vector2D.Half, new Size(0.2f, 0.1f));
			//var buttonA =
			//	new InteractiveButton(
			//		CreateTheme(Enum.GetName(typeof(Content.GUI), Content.GUI.ButtonBack)), drawArea);
			//scene.Add(buttonA);
			Scene loadedScene;
			using (var memoryStream = new MemoryStream())
			{
				var writer = new BinaryWriter(memoryStream);
				BinaryDataExtensions.Save(scene, writer);
				writer.Flush();
				memoryStream.Position = 0;
				loadedScene = (Scene)BinaryDataExtensions.Create(new BinaryReader(memoryStream));
			}

		loadedScene.Show();
		}

		private static void CreateDummySceneWithOneButton()
		{
			var scene = new Scene();
			var drawArea = Rectangle.FromCenter(Vector2D.Half, new Size(0.2f, 0.1f));
			var buttonA =
				new InteractiveButton(
					CreateTheme(Enum.GetName(typeof(Content.GUI), Content.GUI.ButtonBack)), drawArea);
			scene.Add(buttonA);

			if (!Directory.Exists("Content"))
				Directory.CreateDirectory("Content");

			using (var file = File.Create(Path.Combine("Content", "DummyScene" + ".deltascene")))
				BinaryDataExtensions.Save(scene, new BinaryWriter(file));
		}
	}
}