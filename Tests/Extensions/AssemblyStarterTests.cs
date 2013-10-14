using System;
using DeltaEngine.Extensions;
using NUnit.Framework;

namespace DeltaEngine.Tests.Extensions
{
	public class AssemblyStarterTests
	{
		//ncrunch: no coverage start
		[Test, Ignore]
		public void StartRunTimeTestInAppDomain()
		{
			using (var starter = new AssemblyStarter("DeltaEngine.Tests.dll"))
				starter.Start("GlobalTimeTests", "CalculateFpsWithStopwatch");
		}

		[Test, Ignore]
		public void FindAllTestsInAppDomain()
		{
			using (var starter = new AssemblyStarter("DeltaEngine.Tests.dll"))
			{
				var tests = starter.GetTestNames();
				if (tests != null)
					foreach (var test in tests)
						Console.WriteLine(test);
			}
		}
	}
}