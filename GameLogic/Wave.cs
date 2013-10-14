using System.Collections.Generic;
using DeltaEngine.Content.Xml;
using DeltaEngine.Extensions;

namespace DeltaEngine.GameLogic
{
	public class Wave
	{
		public Wave(float waitTime, float spawnInterval, string thingsToSpawn, float maxTime = 0.0f,
			int maxNumber = 1)
		{
			WaitTime = waitTime;
			SpawnInterval = spawnInterval;
			MaxNumber = maxNumber;
			MaxTime = maxTime;
			if (string.IsNullOrEmpty(thingsToSpawn))
				return;
			foreach (var thing in thingsToSpawn.SplitAndTrim(','))
				SpawnTypeList.Add(thing);
		}

		protected Wave(Wave wave)
		{
			WaitTime = wave.WaitTime;
			SpawnInterval = wave.SpawnInterval;
			MaxTime = wave.MaxTime;
			SpawnTypeList = wave.SpawnTypeList;
			MaxNumber = wave.MaxNumber;
		}

		public float WaitTime { get; private set; }
		public float SpawnInterval { get; private set; }
		public float MaxTime { get; private set; }
		public readonly List<string> SpawnTypeList = new List<string>();
		public int MaxNumber { get; protected set; }

		public XmlData AsXmlData()
		{
			var xml = new XmlData("Wave");
			xml.AddAttribute("WaitTime", WaitTime);
			xml.AddAttribute("SpawnInterval", SpawnInterval);
			xml.AddAttribute("MaxNumber", MaxNumber);
			xml.AddAttribute("MaxTime", MaxTime);
			xml.AddAttribute("SpawnTypeList", SpawnTypeList.ToText());
			return xml;
		}

		public override string ToString()
		{
			return "Wave " + WaitTime + ", " + SpawnInterval + ", " + MaxNumber + ", " + MaxTime + ", " +
			  SpawnTypeList.ToText();
		}
	}
}