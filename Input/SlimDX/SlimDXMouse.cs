using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input.Windows;
using DInput = SlimDX.DirectInput;

namespace DeltaEngine.Input.SlimDX
{
	/// <summary>
	/// Native implementation of the Mouse interface using DirectInput
	/// </summary>
	public class SlimDXMouse : Mouse
	{
		public SlimDXMouse(Window window)
		{
			positionTranslater = new CursorPositionTranslater(window);
			mouseCounter = new MouseDeviceCounter();
			directInput = new DInput.DirectInput();
			mouse = new DInput.Mouse(directInput);
			mouse.Properties.AxisMode = DInput.DeviceAxisMode.Absolute;
			mouse.Acquire();
			currentState = new DInput.MouseState();
		}

		private readonly CursorPositionTranslater positionTranslater;
		private readonly MouseDeviceCounter mouseCounter;
		private DInput.DirectInput directInput;
		private DInput.Mouse mouse;
		private DInput.MouseState currentState;

		public override bool IsAvailable
		{
			get { return mouseCounter.GetNumberOfAvailableMice() > 0; }
			protected set { }
		}

		public override void SetNativePosition(Vector2D position)
		{
			positionTranslater.SetCursorPosition(position);
		}

		public override void Update(IEnumerable<Entity> entities)
		{
			mouse.GetCurrentState(ref currentState);
			ScrollWheelValue = currentState.Z / MouseWheelDivider;
			UpdateMousePosition();
			UpdateMouseButtons();
			base.Update(entities);
		}

		private const int MouseWheelDivider = 120;

		private void UpdateMousePosition()
		{
			Position = positionTranslater.GetCursorPosition();
		}

		private void UpdateMouseButtons()
		{
			LeftButton = LeftButton.UpdateOnNativePressing(currentState.GetButtons()[0]);
			RightButton = RightButton.UpdateOnNativePressing(currentState.GetButtons()[1]);
			MiddleButton = MiddleButton.UpdateOnNativePressing(currentState.GetButtons()[2]);
			X1Button = X1Button.UpdateOnNativePressing(currentState.GetButtons()[3]);
			X2Button = X2Button.UpdateOnNativePressing(currentState.GetButtons()[4]);
		}

		public override void Dispose()
		{
			if (mouse != null)
				mouse.Unacquire();
			mouse = null;
			directInput = null;
		}
	}
}