using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.Controls;

namespace Blocks
{
	internal class MainMenu : Scene
	{
		public MainMenu(BlocksContent content)
		{
			this.content = content;
			CreateMenuTheme();
			CreateBackgroundElements();
			AddStartButton();
			AddHowToPlay();
			AddQuitButton();
			AddContentSwitcherButton();
		}

		private void CreateMenuTheme()
		{
			menuTheme = new Theme
			{
				Button = new Material(Shader.Position2DUV, "BlocksButtonDefault"),
				ButtonMouseover = new Material(Shader.Position2DUV, "BlocksButtonHover"),
				ButtonPressed = new Material(Shader.Position2DUV, "BlocksButtonPressed")
			};
		}

		private void CreateBackgroundElements()
		{
			var backgroundImage = content.Load<Image>("Background");
			var backgroundMaterial = new Material(ContentLoader.Load<Shader>(Shader.Position2DUV),
				backgroundImage, backgroundImage.PixelSize);
			SetViewportBackground(backgroundMaterial);
			var gameLogoImage = content.Load<Image>("GameLogo");
			var gameLogoMaterial = new Material(ContentLoader.Load<Shader>(Shader.Position2DUV),
				gameLogoImage, gameLogoImage.PixelSize);
			gameLogo = new Sprite(gameLogoMaterial, Rectangle.FromCenter(0.5f, 0.35f, 0.4f, 0.2f));
			Add(gameLogo);
		}

		private Theme menuTheme;
		private BlocksContent content;
		private Sprite gameLogo;

		private void AddStartButton()
		{
			var startButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.47f, 0.4f, 0.08f),
				"Start Game");
			startButton.Clicked += TryInvokeGameStart;
			Add(startButton);
		}

		//ncrunch: no coverage start
		private void TryInvokeGameStart()
		{
			if (InitGame != null)
				InitGame();
		}

		//ncrunch: no coverage end

		public event Action InitGame;

		private void AddHowToPlay()
		{
			var howToButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.57f, 0.4f, 0.08f),
				"How To Play");
			howToButton.Clicked += ShowHowToPlaySubMenu;
			Add(howToButton);
		}

		//ncrunch: no coverage start
		private void ShowHowToPlaySubMenu()
		{
			if (howToPlay == null)
				howToPlay = new HowToPlaySubMenu(this, menuTheme, content);
			howToPlay.Show();
			Hide();
		}

		//ncrunch: no coverage end

		private HowToPlaySubMenu howToPlay;

		private sealed class HowToPlaySubMenu : Scene
		{
			//ncrunch: no coverage start
			public HowToPlaySubMenu(Scene parent, Theme menuTheme, BlocksContent content)
			{
				this.parent = parent;
				this.menuTheme = menuTheme;
				var backgroundImage = content.Load<Image>("Background");
				var backgroundMaterial = new Material(ContentLoader.Load<Shader>(Shader.Position2DUV),
					backgroundImage, backgroundImage.PixelSize);
				SetViewportBackground(backgroundMaterial);
				var gameLogoImage = content.Load<Image>("GameLogo");
				var gameLogoMaterial = new Material(ContentLoader.Load<Shader>(Shader.Position2DUV),
					gameLogoImage, gameLogoImage.PixelSize);
				Add(new Sprite(gameLogoMaterial, Rectangle.FromCenter(0.5f, 0.35f, 0.4f, 0.2f)));
				AddControlDescription();
				AddBackButton();
			}

			private readonly Theme menuTheme;
			private readonly Scene parent;

			private void AddControlDescription()
			{
				const string DescriptionText =
					"Quite likely this won't be much of a surprise - here we expect you\n" +
						"to arrange the random falling blocks to horizontal rows.\n\n" + "- Controls -\n" +
						"You can move the current block to either side by pressing the left and right cursor " +
						"keys\nor click / tap on the corresponding side.\n" +
						"Rotate the block by click / tap above or by pressing cursor up or space.\n" +
						"To make the block fall faster, click / tap below or press cursor down!";
				Add(new FontText(Font.Default, DescriptionText, new Vector2D(0.5f, 0.55f))
				{
					Color = Color.CornflowerBlue
				});
			}

			private void AddBackButton()
			{
				var backButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.67f, 0.4f, 0.08f),
					"Back");
				backButton.Clicked += () =>
				{
					Hide();
					parent.Show();
				};
				Add(backButton);
			}

			//ncrunch: no coverage end
		}

		private void AddQuitButton()
		{
			var quitButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.67f, 0.4f, 0.08f),
				"Quit Game");
			quitButton.Clicked += TryInvokeQuit;
			Add(quitButton);
		}

		//ncrunch: no coverage start
		private void TryInvokeQuit()
		{
			if (QuitGame != null)
				QuitGame();
		}

		public event Action QuitGame;

		private void AddContentSwitcherButton()
		{
			switchButton = new InteractiveButton(menuTheme, new Rectangle(0.76f, 0.24f, 0.2f, 0.08f),
				"Jewel Blocks");
			switchButton.Clicked += TryInvokeSwitchContent;
			Add(switchButton);
		}

		private InteractiveButton switchButton;

		private void TryInvokeSwitchContent()
		{
			contentSwitched = !contentSwitched;
			switchButton.Text = contentSwitched ? "Fruit Blocks" : "Jewel Blocks";
			content = contentSwitched
				? new JewelBlocksContent() : (BlocksContent)new FruitBlocksContent();
			Remove(gameLogo);
			CreateBackgroundElements();
			if (SwitchContent != null)
				SwitchContent(contentSwitched);
		}

		private bool contentSwitched;

		public event Action<bool> SwitchContent;
	}
}