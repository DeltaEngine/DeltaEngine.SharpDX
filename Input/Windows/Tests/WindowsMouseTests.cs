using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Input.Windows.Tests
{
	[Category("Slow")]
	internal class WindowsMouseTests : TestWithMocksOrVisually
	{
		//ncrunch: no coverage start
		[SetUp]
		public void CreateWindowsMouseExplicitly()
		{
			Resolve<Mouse>().Dispose();
			mouse = new WindowsMouse(new CursorPositionTranslater(Resolve<Window>()));
		}

		private WindowsMouse mouse;

		[TearDown]
		public void DisposingMouse()
		{
			mouse.Dispose();
		}

		[Test]
		public void SetPositionAndUpdateTrigger()
		{
			var setPoint = new Vector2D(0.8f, 0.4f);
			var moveTrigger = new MouseHoverTrigger();
			mouse.SetPosition(setPoint);
			mouse.Update(new List<Entity>(new[] { moveTrigger }));
			Assert.AreEqual(setPoint.X, moveTrigger.LastPosition.X, 0.1f);
			Assert.AreEqual(setPoint.Y, moveTrigger.LastPosition.Y, 0.1f);
		}
	}
}