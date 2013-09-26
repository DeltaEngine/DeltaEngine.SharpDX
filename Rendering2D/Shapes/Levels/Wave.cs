using System.Collections.Generic;
using DeltaEngine.Content.Xml;
using DeltaEngine.Extensions;

namespace DeltaEngine.Rendering2D.Shapes.Levels
{
	public class Wave
	{
		public Wave(float waitTime, float spawnInterval, float maxTime, string thingsToSpawn)
		{
			WaitTime = waitTime;
			SpawnInterval = spawnInterval;
			MaxTime = maxTime;
			if (string.IsNullOrEmpty(thingsToSpawn))
				return;
			foreach (var thing in thingsToSpawn.SplitAndTrim(','))
				SpawnTypeList.Add(thing);
		}

		public float WaitTime;
		public float SpawnInterval;
		public float MaxTime;
		public List<string> SpawnTypeList = new List<string>();

		public XmlData AsXmlData()
		{
			var xml = new XmlData("Wave");
			xml.AddAttribute("WaitTime", WaitTime);
			xml.AddAttribute("SpawnInterval", SpawnInterval);
			xml.AddAttribute("MaxTime", MaxTime);
			xml.AddAttribute("SpawnTypeList", SpawnTypeList.ToText());
			return xml;
		}

		public override string ToString()
		{
			return "Wave " + WaitTime + ", " + SpawnInterval + ", " + MaxTime + ", " +
			  SpawnTypeList.ToText();
		}
	}
}