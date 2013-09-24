using System;
using System.Collections.Generic;
using System.Globalization;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public class ParticlePropertiesXmlParser
	{
		public void ParseParticlePropertiesXml(string filename)
		{
			emitters = new Dictionary<string, ParticleEmitterData>();
			XmlData xmlData = ContentLoader.Load<XmlContent>(filename).Data;
			LoadEmitterPatern(xmlData);
			foreach (var instance in xmlData.GetChild("Instances").GetChildren("Instance"))
				GenerateInstance(instance);
		}

		private Dictionary<string, ParticleEmitterData> emitters;

		private void LoadEmitterPatern(XmlData xmlData)
		{
			foreach (var emitter in xmlData.GetChild("ParticleEmitters").GetChildren("Emitter"))
				try
				{
					CreateParticleProperties(emitter);
				}
				catch (Exception ex)
				{
					throw new InvalidParticleProperties(ex);
				}
		}

		private class InvalidParticleProperties : Exception
		{
			public InvalidParticleProperties(Exception inner)
				: base("Invalid ParticleProperties", inner) { }
		}

		private void CreateParticleProperties(XmlData emitter)
		{
			var name = emitter.GetAttributeValue("Name");
			var imageName = emitter.GetAttributeValue("Image");
			var emitterData = CreateEmitterData(emitter);
			emitterData.EmitterType = emitter.GetAttributeValue("Type");
			emitterData.ParticleMaterial = new Material(Shader.Position3DColorUv, imageName);
			emitterData.SpawnInterval = emitterData.LifeTime / emitterData.MaximumNumberOfParticles;
			emitters.Add(name, emitterData);
		}

		private static ParticleEmitterData CreateEmitterData(XmlData emitter)
		{
			var emitterData = new ParticleEmitterData();
			emitterData.LifeTime = float.Parse(emitter.GetAttributeValue("LifeTime"),
				CultureInfo.InvariantCulture);
			emitterData.MaximumNumberOfParticles = int.Parse(emitter.GetAttributeValue("MaxNumber"),
				CultureInfo.InvariantCulture);
			GetColorProperty(emitter, emitterData);
			GetAccelerationProperty(emitter, emitterData);
			GetSizeProperty(emitter, emitterData);
			GetVelocityProperty(emitter, emitterData);
			return emitterData;
		}

		private static void GetColorProperty(XmlData emitter, ParticleEmitterData emitterData)
		{
			var colorNode = emitter.GetChild("Color");
			var initialColor = GetColorProperty(colorNode, "InitialColor");
			var finalColor = GetColorProperty(colorNode, "FinalColor");
			emitterData.Color = new RangeGraph<Color>(initialColor, finalColor);
		}

		private static Color GetColorProperty(XmlData node, string propertyName)
		{
			var property = node.GetAttributeValue(propertyName).Split(' ');
			return new Color(int.Parse(property[0], CultureInfo.InvariantCulture),
				int.Parse(property[1], CultureInfo.InvariantCulture), 
				int.Parse(property[2], CultureInfo.InvariantCulture),
				int.Parse(property[3], CultureInfo.InvariantCulture));
		}

		private static void GetAccelerationProperty(XmlData emitter, ParticleEmitterData emitterData)
		{
			var accelerationNode = emitter.GetChild("Acceleration");
			var minAcceleration = GetVectorProperty(accelerationNode, "MinAcceleration");
			var maxAcceleration = GetVectorProperty(accelerationNode, "MaxAcceleration");
			emitterData.Acceleration = new RangeGraph<Vector3D>(minAcceleration, maxAcceleration);
		}

		private static Vector3D GetVectorProperty(XmlData node, string propertyName)
		{
			var value = node.GetAttributeValue(propertyName).Split(' ');
			return new Vector3D(float.Parse(value[0], CultureInfo.InvariantCulture),
				float.Parse(value[1], CultureInfo.InvariantCulture), 
				float.Parse(value[2], CultureInfo.InvariantCulture));
		}

		private static void GetSizeProperty(XmlData emitter, ParticleEmitterData emitterData)
		{
			var sizeNode = emitter.GetChild("Size");
			var initial = 
				float.Parse(sizeNode.GetAttributeValue("InitialSize"), CultureInfo.InvariantCulture);
			var final = 
				float.Parse(sizeNode.GetAttributeValue("FinalSize"), CultureInfo.InvariantCulture);
			emitterData.Size = new RangeGraph<Size>(new Size(initial), new Size(final));
		}

		private static void GetVelocityProperty(XmlData emitter, ParticleEmitterData emitterData)
		{
			var velocityNode = emitter.GetChild("Velocity");
			var initial = GetVectorProperty(velocityNode, "InitialVelocity");
			var final = GetVectorProperty(velocityNode, "Offset");
			emitterData.StartVelocity = new RangeGraph<Vector3D>(initial, final);
		}

		private void GenerateInstance(XmlData instance)
		{
			var id = instance.GetAttributeValue("Id");
			var position = GetVectorProperty(instance, "Position");
			var data = emitters[id];
			if (data.EmitterType == "Point")
				new Particle3DPointEmitter(data, position);
		}
	}
}
