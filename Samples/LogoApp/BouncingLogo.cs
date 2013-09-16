using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Multimedia;
using DeltaEngine.Physics2D;
using DeltaEngine.Rendering.Sprites;

namespace LogoApp
{
	/// <summary>
	/// Colored Delta Engine logo which spins and bounces around the screen plus input and sound.
	/// </summary>
	public class BouncingLogo : Sprite
	{
		public BouncingLogo()
			: base(new Material(Shader.Position2DColorUv, "DeltaEngineLogo"), Point.Half)
		{
			Color = Color.GetRandomColor();
			Add(new SimplePhysics.Data
			{
				RotationSpeed = random.Get(-50, 50),
				Velocity = new Point(random.Get(-0.4f, 0.4f), random.Get(-0.4f, 0.4f)),
				Bounced = () => sound.Play(0.1f)
			});
			Start<SimplePhysics.BounceIfAtScreenEdge>();
			Start<SimplePhysics.Rotate>();
			new Command(Command.Click, position => Center = position);
		}

		private readonly Randomizer random = Randomizer.Current;
		private readonly Sound sound = ContentLoader.Load<Sound>("BorderHit");
	}
}