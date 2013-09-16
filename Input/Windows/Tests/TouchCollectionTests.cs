using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Input.Windows.Tests
{
	public class TouchCollectionTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateTouchCollection()
		{
			emptyTouchCollection = new TouchCollection(null);
			var positionTranslator = new CursorPositionTranslater(Resolve<Window>());
			touchCollection = new TouchCollection(positionTranslator);
		}

		private TouchCollection emptyTouchCollection;
		private TouchCollection touchCollection;

		[Test]
		public void FindIndexByIdOrGetFreeIndex()
		{
			Assert.AreEqual(0, emptyTouchCollection.FindIndexByIdOrGetFreeIndex(478));
		}

		[Test]
		public void FindIndexByIdWithExistingId()
		{
			emptyTouchCollection.ids[5] = 5893;
			Assert.AreEqual(5, emptyTouchCollection.FindIndexByIdOrGetFreeIndex(5893));
		}

		[Test]
		public void FindFreeIndex()
		{
			Assert.AreEqual(0, emptyTouchCollection.FindIndexByIdOrGetFreeIndex(456));
		}

		[Test]
		public void FindFreeIndexWithoutAnyFreeIndices()
		{
			for (int index = 0; index < emptyTouchCollection.ids.Length; index++)
				emptyTouchCollection.ids[index] = 1;
			Assert.AreEqual(-1, emptyTouchCollection.FindIndexByIdOrGetFreeIndex(546));
		}

		[Test]
		public void IsTouchDown()
		{
			Assert.True(TouchCollection.IsTouchDown(NativeTouchInput.FlagTouchDown));
			Assert.True(TouchCollection.IsTouchDown(NativeTouchInput.FlagTouchDownOrMoved));
			Assert.True(TouchCollection.IsTouchDown(NativeTouchInput.FlagTouchMove));
			Assert.False(TouchCollection.IsTouchDown(0x0008));
		}

		[Test]
		public void UpdateTouchState()
		{
			touchCollection.UpdateTouchState(0, NativeTouchInput.FlagTouchDown);
			Assert.AreEqual(State.Pressing, touchCollection.states[0]);
			touchCollection.UpdateTouchState(0, 0);
			Assert.AreEqual(State.Releasing, touchCollection.states[0]);
		}

		[Test]
		public void CalculateQuadraticPosition()
		{
			Point quadPosition = touchCollection.CalculateQuadraticPosition(400 * 100, 300 * 100);
			Assert.AreEqual(ScreenSpace.Current.FromPixelSpace(new Point(400, 300)), quadPosition);
		}

		[Test]
		public void ProcessNewTouches()
		{
			var newTouches = new List<NativeTouchInput> { GetTestTouchInput() };
			touchCollection.ProcessNewTouches(newTouches);
			Assert.AreEqual(15, touchCollection.ids[0]);
			Assert.AreEqual(ScreenSpace.Current.FromPixelSpace(new Point(400, 300)),
				touchCollection.locations[0]);
			Assert.AreEqual(State.Pressing, touchCollection.states[0]);
		}

		private static NativeTouchInput GetTestTouchInput()
		{
			return new NativeTouchInput(NativeTouchInput.FlagTouchDown, 15, 40000, 30000);
		}

		[Test]
		public void UpdateTouchStateWithoutNewData()
		{
			touchCollection.ids[0] = 15;
			touchCollection.states[0] = State.Releasing;
			touchCollection.UpdateTouchStateWithoutNewData(0);
			Assert.AreEqual(State.Released, touchCollection.states[0]);
			Assert.AreEqual(15, touchCollection.ids[0]);
			touchCollection.states[0] = State.Released;
			touchCollection.UpdateTouchStateWithoutNewData(0);
			Assert.AreEqual(State.Released, touchCollection.states[0]);
			Assert.AreEqual(-1, touchCollection.ids[0]);
		}

		[Test]
		public void UpdateAllTouches()
		{
			var newTouches = new List<NativeTouchInput> { GetTestTouchInput() };
			touchCollection.ids[0] = 15;
			touchCollection.states[0] = State.Pressing;
			touchCollection.UpdateAllTouches(newTouches);
			Assert.AreEqual(0, newTouches.Count);
			Assert.AreEqual(15, touchCollection.ids[0]);
			Assert.AreEqual(ScreenSpace.Current.FromPixelSpace(new Point(400, 300)),
				touchCollection.locations[0]);
			Assert.AreEqual(State.Pressed, touchCollection.states[0]);
		}

		[Test]
		public void UpdateTouchWithUpdatedActiveTouch()
		{
			var newTouches = new List<NativeTouchInput> { GetTestTouchInput() };
			touchCollection.ids[0] = 15;
			touchCollection.states[0] = State.Pressing;
			touchCollection.UpdatePreviouslyActiveTouches(newTouches);
			Assert.AreEqual(0, newTouches.Count);
			Assert.AreEqual(15, touchCollection.ids[0]);
			Assert.AreEqual(ScreenSpace.Current.FromPixelSpace(new Point(400, 300)),
				touchCollection.locations[0]);
			Assert.AreEqual(State.Pressed, touchCollection.states[0]);
		}

		[Test]
		public void UpdateTouchWithoutAnyActiveTouch()
		{
			var newTouches = new List<NativeTouchInput>();
			touchCollection.ids[0] = 15;
			touchCollection.states[0] = State.Releasing;
			touchCollection.UpdateTouchBy(0, newTouches);
			Assert.AreEqual(15, touchCollection.ids[0]);
			Assert.AreEqual(State.Released, touchCollection.states[0]);
			touchCollection.UpdateTouchBy(0, newTouches);
			Assert.AreEqual(-1, touchCollection.ids[0]);
			Assert.AreEqual(State.Released, touchCollection.states[0]);
		}

		[Test]
		public void UpdateTouchIfPreviouslyPresentWithMultipleNewTouches()
		{
			var newTouches = new List<NativeTouchInput>
			{
				new NativeTouchInput(3, 0, 0, 0),
				new NativeTouchInput(15, 0, 0, 0),
			};
			touchCollection.ids[0] = 15;
			touchCollection.states[0] = State.Releasing;
			touchCollection.UpdateTouchBy(0, newTouches);
			Assert.AreEqual(15, touchCollection.ids[0]);
			Assert.AreEqual(State.Released, touchCollection.states[0]);
		}
	}
}