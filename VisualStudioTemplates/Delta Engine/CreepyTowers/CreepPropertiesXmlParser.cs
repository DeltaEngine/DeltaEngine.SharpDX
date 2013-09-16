using System;
using System.Collections.Generic;
using System.Globalization;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;

namespace $safeprojectname$
{
	public class CreepPropertiesXmlParser
	{
		public CreepPropertiesXmlParser()
		{
			CreepPropertiesData = new Dictionary<Creep.CreepType, CreepProperties>();
			ParseCreepPropertiesXml();
		}

		public Dictionary<Creep.CreepType, CreepProperties> CreepPropertiesData
		{
			get;
			private set;
		}

		private void ParseCreepPropertiesXml()
		{
			XmlData creepData = ContentLoader.Load<XmlContent>("CreepProperties").Data;
			foreach (var creep in creepData.GetChild("Creeps").GetChildren("Creep"))
				try
				{
					var creepsProperties = CreateCreepProperties(creep);
					CreepPropertiesData.Add(creepsProperties.CreepType, creepsProperties);
				}
				catch (Exception ex)
				{
					throw new InvalidCreepProperties(ex);
				}
		}

		private static CreepProperties CreateCreepProperties(XmlData creep)
		{
			return new CreepProperties {
				Name = creep.GetAttributeValue("Name"),
				CreepType = (Creep.CreepType)Enum.Parse(typeof(Creep.CreepType), 
					creep.GetAttributeValue("Type")),
				MaxHp = float.Parse(creep.GetAttributeValue("MaxHp"), CultureInfo.InvariantCulture),
				Resistance = float.Parse(creep.GetAttributeValue("Resistance"), 
					CultureInfo.InvariantCulture),
				Speed = float.Parse(creep.GetAttributeValue("Speed"), CultureInfo.InvariantCulture),
				GoldReward = int.Parse(creep.GetAttributeValue("Gold"), CultureInfo.InvariantCulture),
				TypeDamageModifier = ParseTypeDamageModifier(creep)
			};
		}

		private static Dictionary<Tower.TowerType, float> ParseTypeDamageModifier(XmlData creep)
		{
			var typeDamageModifier = new Dictionary<Tower.TowerType, float>();
			foreach (var attribute in creep.GetChild("Modifiers").Attributes)
				typeDamageModifier.Add((Tower.TowerType)Enum.Parse(typeof(Tower.TowerType), 
					attribute.Name), float.Parse(attribute.Value));

			return typeDamageModifier;
		}
		private class InvalidCreepProperties : Exception
		{
			public InvalidCreepProperties(Exception inner) : base("Invalid CombatProperties", inner)
			{
			}
		}
	}
}