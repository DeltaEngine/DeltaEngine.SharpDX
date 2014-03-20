using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input.Windows;
using DInput = SharpDX.DirectInput;
using SysPoint = System.Drawing.Point;

namespace DeltaEngine.Input.SharpDX
{
	/// <summary>
	/// Native implementation of the BaseMouse class using DirectInput.
	/// </summary>
	public class SharpDXMouse : Mouse
	{
		public SharpDXMouse(Window window)
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
			LeftButton = LeftButton.UpdateOnNativePressing(currentState.Buttons[0]);
			RightButton = RightButton.UpdateOnNativePressing(currentState.Buttons[1]);
			MiddleButton = MiddleButton.UpdateOnNativePressing(currentState.Buttons[2]);
			X1Button = X1Button.UpdateOnNativePressing(currentState.Buttons[3]);
			X2Button = X2Button.UpdateOnNativePressing(currentState.Buttons[4]);
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