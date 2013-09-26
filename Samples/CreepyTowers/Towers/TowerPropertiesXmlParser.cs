using System;
using System.Collections.Generic;
using System.Globalization;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;

namespace CreepyTowers.Towers
{
	/// <summary>
	/// TowerProperties.xml contains the values for the Towers
	/// </summary>
	public class TowerPropertiesXmlParser
	{
		public TowerPropertiesXmlParser()
		{
			TowerPropertiesData = new Dictionary<Tower.TowerType, TowerProperties>();
			ParseTowerPropertiesXml();
		}

		public Dictionary<Tower.TowerType, TowerProperties> TowerPropertiesData { get; private set; }

		private void ParseTowerPropertiesXml()
		{
			XmlData towerData = ContentLoader.Load<XmlContent>("TowerProperties").Data;
			foreach (var tower in towerData.GetChildren("Tower"))
				try
				{
					var towerProperty = CreateTowerProperties(tower);
					TowerPropertiesData.Add(towerProperty.TowerType, towerProperty);
				}
				catch (Exception ex)
				{
					throw new InvalidTowerProperties(ex);
				}
		}

		private static TowerProperties CreateTowerProperties(XmlData tower)
		{
			return new TowerProperties
			{
				Name = tower.GetAttributeValue("Name"),
				TowerType =
					(Tower.TowerType)Enum.Parse(typeof(Tower.TowerType), tower.GetAttributeValue("Type")),
				AttackType =
					(Tower.AttackType)
						Enum.Parse(typeof(Tower.AttackType), tower.GetAttributeValue("AttackType")),
				Range = float.Parse(tower.GetAttributeValue("Range"), CultureInfo.InvariantCulture),
				AttackFrequency =
					float.Parse(tower.GetAttributeValue("AttackFrequency"), CultureInfo.InvariantCulture),
				AttackDamage =
					float.Parse(tower.GetAttributeValue("AttackDamage"), CultureInfo.InvariantCulture),
				Cost = int.Parse(tower.GetAttributeValue("Cost"), CultureInfo.InvariantCulture)
			};
		}

		private class InvalidTowerProperties : Exception
		{
			public InvalidTowerProperties(Exception inner)
				: base("Invalid CombatProperties", inner) {}
		}
	}
}