using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D;

namespace $safeprojectname$
{
	public class BouncingLogo : Sprite
	{
		public BouncingLogo() : base(new Material(Shader.Position2DColorUV, "DeltaEngineLogo"), 
			Vector2D.Half)
		{
			Color = Color.GetRandomColor();
			Add(new SimplePhysics.Data {
				RotationSpeed = random.Get(-50, 50),
				Velocity = new Vector2D(random.Get(-0.4f, 0.4f), random.Get(-0.4f, 0.4f)),
				Bounced = () => 
				{
					if (sound.NumberOfPlayingInstances < 4)
						sound.Play(0.1f);
				}
			});
			Start<SimplePhysics.BounceIfAtScreenEdge>();
			Start<SimplePhysics.Rotate>();
			new Command(Command.Click, position => Center = position);
		}

		private readonly Randomizer random = Randomizer.Current;
		private readonly Sound sound = ContentLoader.Load<Sound>("BorderHit");
	}
}