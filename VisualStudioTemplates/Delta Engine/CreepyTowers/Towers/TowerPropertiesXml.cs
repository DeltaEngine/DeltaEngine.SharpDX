using System;
using System.Globalization;
using System.IO;
using DeltaEngine.Content.Xml;

namespace $safeprojectname$.Towers
{
	public class TowerPropertiesXml : XmlContent
	{
		public TowerPropertiesXml(string contentName) : base(contentName)
		{
		}

		protected override void LoadData(Stream fileData)
		{
			base.LoadData(fileData);
			foreach (var tower in Data.GetChildren("TowerData"))
				try
				{
					var data = ParseTowerData(tower);
					towerData [(int)data.Type] = data;
				}
				catch (Exception ex)
				{
					throw new InvalidTowerProperties(ex);
				}
		}

		private static TowerData ParseTowerData(XmlData tower)
		{
			return new TowerData((TowerType)Enum.Parse(typeof(TowerType), 
				tower.GetAttributeValue("Type")), tower.GetAttributeValue("Name"), 
					(AttackType)Enum.Parse(typeof(AttackType), tower.GetAttributeValue("AttackType")), 
						float.Parse(tower.GetAttributeValue("Range"), CultureInfo.InvariantCulture), 
							float.Parse(tower.GetAttributeValue("AttackFrequency"), 
								CultureInfo.InvariantCulture), 
									float.Parse(tower.GetAttributeValue("AttackDamage"), 
										CultureInfo.InvariantCulture), int.Parse(tower.GetAttributeValue("Cost"), 
											CultureInfo.InvariantCulture));
		}

		private readonly TowerData[] towerData = new TowerData[Enum.GetNames(typeof(TowerType)).Length];
		private class InvalidTowerProperties : Exception
		{
			public InvalidTowerProperties(Exception inner) : base("Invalid CombatProperties", inner)
			{
			}
		}
		public TowerData Get(TowerType type)
		{
			return towerData [(int)type];
		}
	}
}