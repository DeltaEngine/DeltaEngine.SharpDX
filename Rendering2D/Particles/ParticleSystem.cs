using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.GameLogic;

namespace DeltaEngine.Rendering2D.Particles
{
	public class ParticleSystem : HierarchicalObject
	{
		public ParticleSystem()
		{
			AttachedEmitters = new List<ParticleEmitter>();
		}

		public ParticleSystem(ParticleSystemData emittersToLoad)
		{
			AttachedEmitters = new List<ParticleEmitter>();
			foreach (string emitterName in emittersToLoad.emitterNames)
			{
				var emitterData = ContentLoader.Load<ParticleEmitterData>(emitterName);
				AttachEmitter(new ParticleEmitter(emitterData, Position));
			}
		}

		public List<ParticleEmitter> AttachedEmitters { get; private set; }

		protected override void OnPositionChange()
		{
			if (AttachedEmitters == null || AttachedEmitters.Count == 0)
				return;
			foreach (ParticleEmitter attachedEmitter in AttachedEmitters)
				attachedEmitter.Position = Position;
		}

		protected override void OnRotationChange()
		{
			if (AttachedEmitters == null || AttachedEmitters.Count == 0)
				return;
			foreach (ParticleEmitter attachedEmitter in AttachedEmitters)
				attachedEmitter.Rotation = Rotation;
		}

		public void AttachEmitter(ParticleEmitter emitter)
		{
			AttachedEmitters.Add(emitter);
			emitter.Position = Position;
			emitter.Rotation = Rotation;
		}

		public void RemoveEmitter(ParticleEmitter emitter)
		{
			AttachedEmitters.Remove(emitter);
		}

		public void RemoveEmitter(int indexOfEmitter)
		{
			AttachedEmitters.RemoveAt(indexOfEmitter);
		}

		public void DisposeEmitter(ParticleEmitter emitter)
		{
			AttachedEmitters.Remove(emitter);
			emitter.IsActive = false;
		}

		public void DisposeEmitter(int indexOfEmitter)
		{
			var emitter = AttachedEmitters[indexOfEmitter];
			AttachedEmitters.RemoveAt(indexOfEmitter);
			emitter.IsActive = false;
		}

		public void DisposeSystem()
		{
			foreach (var attachedEmitter in AttachedEmitters)
				attachedEmitter.IsActive = false;
			AttachedEmitters.Clear();
		}
	}
}