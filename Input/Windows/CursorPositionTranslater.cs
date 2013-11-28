using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.ScreenSpaces;
using SysPoint = System.Drawing.Point;

namespace DeltaEngine.Input.Windows
{
	/// <summary>
	/// Used to translate courser and touch from and to client coordinates. Also used for SharpDX.
	/// </summary>
	public class CursorPositionTranslater
	{
		public CursorPositionTranslater(Window window)
		{
			this.window = window;
		}

		private readonly Window window;

		// ncrunch: no coverage start
		public void SetCursorPosition(Vector2D position)
		{
			var newScreenPosition = ToSysPoint(ToScreenPositionFromScreenSpace(position));
			NativeMethods.SetCursorPos(newScreenPosition.X, newScreenPosition.Y);
		}

		public Vector2D GetCursorPosition()
		{
			var newPosition = new SysPoint();
			NativeMethods.GetCursorPos(ref newPosition);
			var screenspace = FromScreenPositionToScreenSpace(FromSysPoint(newPosition));
			return new Vector2D((float)Math.Round(screenspace.X, 3), (float)Math.Round(screenspace.Y, 3));
		}
		// ncrunch: no coverage end

		private static SysPoint ToSysPoint(Vector2D position)
		{
			return new SysPoint((int)Math.Round(position.X), (int)Math.Round(position.Y));
		}

		internal Vector2D ToScreenPositionFromScreenSpace(Vector2D position)
		{
			position = ScreenSpace.Current.ToPixelSpace(position);
			var newScreenPosition = ToSysPoint(position);
			if (WindowHandleIsValidIntPtr())
				//ncrunch: no coverage start
				NativeMethods.ClientToScreen((IntPtr)window.Handle, ref newScreenPosition);
				//ncrunch: no coverage end
			return FromSysPoint(newScreenPosition);
		}

		private bool WindowHandleIsValidIntPtr()
		{
			return window.Handle is IntPtr && (IntPtr)window.Handle != IntPtr.Zero;
		}

		private static Vector2D FromSysPoint(SysPoint position)
		{
			return new Vector2D(position.X, position.Y);
		}

		internal Vector2D FromScreenPositionToScreenSpace(Vector2D position)
		{
			var screenPoint = ToSysPoint(position);
			if (WindowHandleIsValidIntPtr())
				//ncrunch: no coverage start
				NativeMethods.ScreenToClient((IntPtr)window.Handle, ref screenPoint);
					//ncrunch: no coverage end
			return ScreenSpace.Current.FromPixelSpace(FromSysPoint(screenPoint));
		}
	}
}