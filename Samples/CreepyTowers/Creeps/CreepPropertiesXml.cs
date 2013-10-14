using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CreepyTowers.Towers;
using DeltaEngine.Content.Xml;

namespace CreepyTowers.Creeps
{
	/// <summary>
	/// CreepProperties.xml contains the values for the Creeps
	/// </summary> 
	public class CreepPropertiesXml : XmlContent
	{
		public CreepPropertiesXml(string contentName)
			: base(contentName) {}

		protected override void LoadData(Stream fileData)
		{
			base.LoadData(fileData);
			foreach (var creep in Data.GetChildren("CreepData"))
				try
				{
					var data = ParseCreepData(creep);
					creepData[(int)data.Type] = data;
				}
				catch (Exception ex)
				{
					throw new InvalidCreepProperties(ex);
				}
		}

		private static CreepData ParseCreepData(XmlData creep)
		{
			return
				new CreepData((CreepType)Enum.Parse(typeof(CreepType), creep.GetAttributeValue("Type")),
					creep.GetAttributeValue("Name"),
					float.Parse(creep.GetAttributeValue("MaxHp"), CultureInfo.InvariantCulture),
					float.Parse(creep.GetAttributeValue("Resistance"), CultureInfo.InvariantCulture),
					float.Parse(creep.GetAttributeValue("Speed"), CultureInfo.InvariantCulture),
					int.Parse(creep.GetAttributeValue("Gold"), CultureInfo.InvariantCulture),
					ParseTypeDamageModifier(creep));
		}

		private static Dictionary<TowerType, float> ParseTypeDamageModifier(XmlData creep)
		{
			var typeDamageModifier = new Dictionary<TowerType, float>();
			foreach (var attribute in creep.GetChild("Modifiers").Attributes)
				typeDamageModifier.Add((TowerType)Enum.Parse(typeof(TowerType), attribute.Name),
					float.Parse(attribute.Value, CultureInfo.InvariantCulture));
			return typeDamageModifier;
		}

		private readonly CreepData[] creepData =
			new CreepData[Enum.GetNames(typeof(CreepType)).Length];

		private class InvalidCreepProperties : Exception
		{
			public InvalidCreepProperties(Exception inner)
				: base("Invalid CombatProperties", inner) {}
		}

		public CreepData Get(CreepType type)
		{
			return creepData[(int)type];
		}
	}
}