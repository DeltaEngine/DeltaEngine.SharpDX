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
	public class RadioDialogTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			dialog = new RadioDialog(Center);
			AdvanceTimeAndUpdateEntities();
			dialog.AddButton("Top Button");
			dialog.AddButton("Middle Button");
			dialog.AddButton("Bottom Button");
			dialog.Add(new FontText(Font.Default, "", new Rectangle(0.4f, 0.7f, 0.2f, 0.1f)));
			AdvanceTimeAndUpdateEntities();
			dialog.Start<UpdateText>();
			InitializeMouse();
		}

		private RadioDialog dialog;
		private static readonly Rectangle Center = Rectangle.FromCenter(0.5f, 0.5f, 0.4f, 0.3f);

		private class UpdateText : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (RadioDialog dialog in entities)
					dialog.Get<FontText>().Text = "Button '" + dialog.SelectedButton.Text + "'";
			}
		}

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
		public void RenderRadioDialogWithThreeButtonsWithTheMiddleDisabled()
		{
			var buttons = dialog.Get<List<RadioButton>>();
			buttons[1].IsEnabled = false;
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(Color.White, buttons[0].Color);
			Assert.AreEqual(Color.Gray, buttons[1].Color);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderGrowingRadioDialog()
		{
			dialog.Start<Grow>();
		}

		//ncrunch: no coverage start
		private class Grow : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (RadioDialog dialog in entities)
				{
					var center = dialog.DrawArea.Center + new Point(0.01f, 0.01f) * Time.Delta;
					var size = dialog.DrawArea.Size * (1.0f + Time.Delta / 10);
					dialog.DrawArea = Rectangle.FromCenter(center, size);
					dialog.Get<FontText>().Text = "Button '" + dialog.SelectedButton.Text + "'";
				}
			}
		}

		//ncrunch: no coverage end

		[Test, CloseAfterFirstFrame]
		public void ClickingRadioButtonSelectsIt()
		{
			if (mouse == null)
				return; // ncrunch: no coverage
			var buttons = dialog.Get<List<RadioButton>>();
			Assert.IsFalse(buttons[1].State.IsSelected);
			PressAndReleaseMouse(Point.One);
			Assert.IsFalse(buttons[1].State.IsSelected);
			PressAndReleaseMouse(new Point(0.35f, 0.5f));
			Assert.IsTrue(buttons[1].State.IsSelected);
		}

		private void PressAndReleaseMouse(Point position)
		{
			SetMouseState(State.Pressing, position);
			SetMouseState(State.Releasing, position);
		}

		private void SetMouseState(State state, Point position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage
			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test, CloseAfterFirstFrame]
		public void ClickingOneRadioButtonCausesTheOthersToUnselect()
		{
			if (mouse == null)
				return; // ncrunch: no coverage
			var buttons = dialog.Get<List<RadioButton>>();
			PressAndReleaseMouse(new Point(0.35f, 0.5f));
			Assert.IsTrue(buttons[1].State.IsSelected);
			PressAndReleaseMouse(new Point(0.35f, 0.6f));
			Assert.IsFalse(buttons[1].State.IsSelected);
			Assert.IsTrue(buttons[2].State.IsSelected);
		}

		[Test, CloseAfterFirstFrame]
		public void ClickingRadioButtonDoesNotSelectItIfDisabled()
		{
			var buttons = dialog.Get<List<RadioButton>>();
			buttons[1].IsEnabled = false;
			PressAndReleaseMouse(new Point(0.35f, 0.5f));
			Assert.IsFalse(buttons[1].State.IsSelected);
		}

		[Test, CloseAfterFirstFrame]
		public void ClickingDisabledRadioButtonDoesNotCauseTheOthersToUnselect()
		{
			if (mouse == null)
				return; // ncrunch: no coverage
			var buttons = dialog.Get<List<RadioButton>>();
			buttons[2].IsEnabled = false;
			PressAndReleaseMouse(new Point(0.35f, 0.5f));
			PressAndReleaseMouse(new Point(0.35f, 0.6f));
			Assert.IsTrue(buttons[1].State.IsSelected);
			Assert.IsFalse(buttons[2].State.IsSelected);
		}

		[Test, CloseAfterFirstFrame]
		public void DisablingDialogDisablesAllButtons()
		{
			dialog.IsEnabled = false;
			var buttons = dialog.Get<List<RadioButton>>();
			foreach (RadioButton button in buttons)
				Assert.IsFalse(button.IsEnabled);
		}

		[Test, CloseAfterFirstFrame]
		public void ReenablingDialogEnablesAllButtons()
		{
			dialog.IsEnabled = false;
			dialog.IsEnabled = true;
			var buttons = dialog.Get<List<RadioButton>>();
			foreach (RadioButton button in buttons)
				Assert.IsTrue(button.IsEnabled);
		}

		[Test]
		public void RenderRadioDialogAttachedToMouse()
		{
			new Command(point => dialog.DrawArea = Rectangle.FromCenter(point, dialog.DrawArea.Size)).
				Add(new MouseMovementTrigger());
		}
	}
}