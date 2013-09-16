using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class DropdownListTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			dropdownList = new DropdownList(Center, Values);
			InitializeMouse();
		}

		private DropdownList dropdownList;

		private static readonly Rectangle Center = new Rectangle(0.4f, 0.3f, 0.2f, 0.05f);
		private static readonly List<object> Values = new List<object>
		{
			"value 1",
			"value 2",
			"value 3"
		};

		private void InitializeMouse()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse == null)
				return; //ncrunch: no coverage

			mouse.SetPosition(Point.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		private MockMouse mouse;

		[Test]
		public void RenderDropdownListWithTenValues()
		{
			dropdownList.Values = new List<object>
			{
				"value 1",
				"value 2",
				"value 3",
				"value 4",
				"value 5",
				"value 6",
				"value 7",
				"value 8",
				"value 9",
				"value 10"
			};
		}

		[Test, CloseAfterFirstFrame]
		public void DefaultProperties()
		{
			Assert.AreEqual(Values, dropdownList.Values);
			Assert.AreEqual(Values[0], dropdownList.SelectedValue);
			Assert.AreEqual(Center, dropdownList.DrawArea);
			Assert.AreEqual(Color.White, dropdownList.Color);
			Assert.AreEqual(3, dropdownList.selectBox.texts.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void NoValuesThrowsException()
		{
			Assert.Throws<SelectBox.MustBeAtLeastOneValue>(() => new DropdownList(Rectangle.Zero, null));
			Assert.Throws<SelectBox.MustBeAtLeastOneValue>(
				() => new DropdownList(Rectangle.Zero, new List<object>()));
		}

		[Test, CloseAfterFirstFrame]
		public void IfValuesAreChangedButStillContainSelectedValueItRemainsSelected()
		{
			var newValues = new List<object> { 1, "value 1", 2 };
			dropdownList.Values = newValues;
			Assert.AreEqual(newValues, dropdownList.Values);
			Assert.AreEqual("value 1", dropdownList.SelectedValue);
			Assert.AreEqual(3, dropdownList.selectBox.texts.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void IfValuesAreChangedAndNoLongerContainSelectedValueItRevertsToFirstValue()
		{
			var newValues = new List<object> { 1, 2 };
			dropdownList.Values = newValues;
			Assert.AreEqual(1, dropdownList.SelectedValue);
			Assert.AreEqual(2, dropdownList.selectBox.texts.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void IfSelectedValueIsChangedToSomethingInValuesThatIsOk()
		{
			dropdownList.SelectedValue = "value 3";
			Assert.AreEqual("value 3", dropdownList.SelectedValue);
		}

		[Test, CloseAfterFirstFrame]
		public void IfSelectedValueIsChangedToSomethingNotInValuesItThrowsAnException()
		{
			Assert.Throws<DropdownList.SelectedValueMustBeOneOfTheListOfValues>(
				() => dropdownList.SelectedValue = "value 4");
		}

		[Test, CloseAfterFirstFrame]
		public void HidingDropdownListHidesEverything()
		{
			dropdownList.Visibility = Visibility.Hide;
			Assert.AreEqual(Visibility.Hide, dropdownList.selectBox.Visibility);
			Assert.AreEqual(Visibility.Hide, dropdownList.Get<FontText>().Visibility);
			foreach (FontText text in dropdownList.selectBox.texts)
				Assert.AreEqual(Visibility.Hide, text.Visibility);
		}

		[Test, CloseAfterFirstFrame]
		public void ClickingRevealsValues()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			Click(new Point(0.5f, 0.31f));
			Assert.AreEqual(Visibility.Show, dropdownList.selectBox.Visibility);
		}

		private void Click(Point position)
		{
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, State.Pressing);
			AdvanceTimeAndUpdateEntities();
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, State.Releasing);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void ClickToRevealValuesThenClickToSelectNewValueAndHideValues()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			Click(new Point(0.5f, 0.31f));
			Click(new Point(0.5f, 0.41f));
			Assert.AreEqual("value 2", dropdownList.SelectedValue);
			Assert.AreEqual(Visibility.Hide, dropdownList.selectBox.Visibility);
		}

		[Test, CloseAfterFirstFrame]
		public void HideSelectBoxAndThenClickOnSelectBoxLine()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			dropdownList.selectBox.Visibility = Visibility.Hide;
			dropdownList.selectBox.LineClicked(0);
			Assert.AreEqual(Visibility.Hide, dropdownList.selectBox.Visibility);
		}

		[Test, CloseAfterFirstFrame]
		public void ClickingHiddenSelectionBoxDoesNothing()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			Click(new Point(0.5f, 0.41f));
			Assert.AreEqual("value 1", dropdownList.SelectedValue);
		}

		[Test, CloseAfterFirstFrame]
		public void ClickToRevealValuesThenMouseoverToChangeValueColor()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			Click(new Point(0.5f, 0.31f));
			MoveMouse(new Point(0.5f, 0.41f));
			Assert.AreEqual(Color.VeryLightGray, dropdownList.selectBox.texts[0].Color);
			Assert.AreEqual(Color.White, dropdownList.selectBox.texts[1].Color);
			Assert.AreEqual(Color.VeryLightGray, dropdownList.selectBox.texts[2].Color);
		}

		private void MoveMouse(Point position)
		{
			mouse.SetPosition(position);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void WhenDisabledSelectionBoxIsHidden()
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			Click(new Point(0.5f, 0.31f));
			dropdownList.IsEnabled = false;
			Assert.AreEqual(Visibility.Hide, dropdownList.selectBox.Visibility);
			foreach (FontText text in dropdownList.selectBox.texts)
				Assert.AreEqual(Visibility.Hide, text.Visibility);
		}

		[Test]
		public void RenderGrowingDropdownList()
		{
			dropdownList.Values = new List<object> { "value 1", "value 2", "value 3", "value 4" };
			dropdownList.Start<Grow>();
		}

		//ncrunch: no coverage start
		private class Grow : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (DropdownList dropdownList in entities)
				{
					var center = dropdownList.DrawArea.Center + new Point(0.01f, 0.01f) * Time.Delta;
					var size = dropdownList.DrawArea.Size * (1.0f + Time.Delta / 10);
					dropdownList.DrawArea = Rectangle.FromCenter(center, size);
				}
			}
		}

		//ncrunch: no coverage end

		[Test]
		public void RenderDropdownListAttachedToMouse()
		{
			dropdownList.Values = new List<object> { "value 1", "value 2", "value 3", "value 4" };
			new Command(
				point => dropdownList.DrawArea = Rectangle.FromCenter(point, dropdownList.DrawArea.Size)).
				Add(new MouseMovementTrigger());
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeMaxDisplayCount()
		{
			dropdownList.MaxDisplayCount = 4;
			Assert.AreEqual(4, dropdownList.MaxDisplayCount);
		}
	}
}