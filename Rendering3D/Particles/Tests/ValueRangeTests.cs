using DeltaEngine.Core;
using NUnit.Framework;
using Randomizer = DeltaEngine.Core.Randomizer;

namespace DeltaEngine.Rendering3D.Particles.Tests
{
	public class ValueRangeTests
	{
		[SetUp]
		public void SetUp()
		{
			valueRange = new ValueRange(0.2f, 0.3f);
		}

		private ValueRange valueRange;

		[Test]
		public void SingleValueConstructor()
		{
			valueRange = new ValueRange(0.1f);
			Assert.AreEqual(0.1f, valueRange.Start);
			Assert.AreEqual(0.1f, valueRange.End);
		}

		[Test]
		public void MinMaxConstructor()
		{
			Assert.AreEqual(0.2f, valueRange.Start);
			Assert.AreEqual(0.3f, valueRange.End);
		}

		[Test]
		public void GetRandomValue()
		{
			Randomizer.Use(new FixedRandom(new[] { 0.4f }));
			Assert.AreEqual(0.24f, valueRange.GetRandomValue(), 0.0001f);
		}

		[Test]
		public void ValueRangeToString()
		{
			Assert.AreEqual("[0.2, 0.3]", valueRange.ToString());
		}
	}
}