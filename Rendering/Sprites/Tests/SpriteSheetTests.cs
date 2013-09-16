using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Sprites.Tests
{
	/// <summary>
	/// Tests for the Spritesheet-based animation
	/// </summary>
	public class SpriteSheetTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateMaterial()
		{
			material = new Material(Shader.Position2DUv, "SpriteSheetAnimation");
		}

		private Material material;

		[Test, ApproveFirstFrameScreenshot]
		public void RenderAnimatedSprite()
		{
			new Sprite(material, new Rectangle(0.4f, 0.4f, 0.2f, 0.2f));
		}

		[Test, CloseAfterFirstFrame]
		public void CheckDurationFromMetaData()
		{
			var animation = new Sprite(material, center);
			Assert.AreEqual(5, animation.Material.SpriteSheet.DefaultDuration);
			Assert.AreEqual(5, animation.Material.Duration);
		}

		private readonly Rectangle center = Rectangle.FromCenter(Point.Half, new Size(0.2f, 0.2f));

		[Test, CloseAfterFirstFrame]
		public void PlayFullAnimation()
		{
			var animation = new Sprite(material, center);
			bool endedHasBeenRaised = false;
			animation.AnimationEnded += () => endedHasBeenRaised = true;
			animation.Elapsed = animation.Material.Duration;
			AdvanceTimeAndUpdateEntities();
			Assert.True(endedHasBeenRaised);
		}

		[Test]
		public void CreateSpriteSheetAnimationWithNewTexture()
		{
			var data = new ImageCreationData(new Size(8, 8)) { BlendMode = BlendMode.Opaque };
			var customImage = ContentLoader.Create<Image>(data);
			var colors = new Color[8 * 8];
			for (int i = 0; i < 8 * 8; i++)
				colors[i] = Color.GetRandomColor();
			customImage.Fill(colors);
			var animationData = new SpriteSheetAnimationCreationData(customImage, 2, new Size(2, 2));
			var texturedShader = ContentLoader.Load<Shader>(Shader.Position2DUv);
			var newMaterial = new SpriteSheetAnimation(animationData).CreateMaterial(texturedShader);
			new Sprite(newMaterial, new Rectangle(0.25f, 0.25f, 0.5f, 0.5f));
		}

		[Test, CloseAfterFirstFrame]
		public void RenderingHiddenSpriteSheetAnimationDoesNotThrowException()
		{
			new Sprite(material, Rectangle.One)
			{
				Visibility = Visibility.Hide
			};
			Assert.DoesNotThrow(() => AdvanceTimeAndUpdateEntities());
		}
	}
}