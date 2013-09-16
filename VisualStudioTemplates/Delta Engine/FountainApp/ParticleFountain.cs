using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Rendering.Particles;

namespace $safeprojectname$
{
	public class ParticleFountain
	{
		public ParticleFountain(Point position)
		{
			new Particle2DEmitter(EmitterData, position);
			CreateCommands();
		}

		private static ParticleEmitterData EmitterData
		{
			get
			{
				return emitterData = new ParticleEmitterData {
					StartVelocity = new RangeGraph<Vector>(new Point(0.0f, -1.0f), new Point(0.5f, 0.1f)),
					Acceleration = new RangeGraph<Vector>(new Point(0, 0.9f), new Point(0, 0.9f)),
					LifeTime = 1.0f,
					MaximumNumberOfParticles = 512,
					Size = new RangeGraph<Size>(new Size(0.01f), new Size(0.015f)),
					ParticleMaterial = new Material(Shader.Position2DColorUv, "Particle"),
					SpawnInterval = 0.01f,
					Color = new RangeGraph<Color>(Color.Red, Color.Orange),
				};
			}
		}

		private static ParticleEmitterData emitterData;

		private static void CreateCommands()
		{
			new Command(() => 
			{
				emitterData.StartVelocity = new RangeGraph<Vector>(emitterData.StartVelocity.Start + new 
					Point(0, -0.3f) * Time.Delta, emitterData.StartVelocity.End + new Point(0, -0.3f) * 
						Time.Delta);
			}).Add(new KeyTrigger(Key.CursorUp, State.Pressed));
			new Command(() => 
			{
				emitterData.StartVelocity = new RangeGraph<Vector>(emitterData.StartVelocity.Start + new 
					Point(0, 0.3f) * Time.Delta, emitterData.StartVelocity.End + new Point(0, 0.3f) * 
						Time.Delta);
			}).Add(new KeyTrigger(Key.CursorDown, State.Pressed));
			new Command(() => 
			{
				emitterData.Acceleration = new RangeGraph<Vector>(emitterData.Acceleration.Start + new 
					Point(0, -0.3f) * Time.Delta, emitterData.Acceleration.Start + new Point(0, -0.3f) * 
						Time.Delta);
			}).Add(new KeyTrigger(Key.CursorLeft, State.Pressed));
			new Command(() => 
			{
				emitterData.Acceleration = new RangeGraph<Vector>(emitterData.Acceleration.Start + new 
					Point(0, 0.3f) * Time.Delta, emitterData.Acceleration.Start + new Point(0, 0.3f) * 
						Time.Delta);
			}).Add(new KeyTrigger(Key.CursorRight, State.Pressed));
		}
	}
}