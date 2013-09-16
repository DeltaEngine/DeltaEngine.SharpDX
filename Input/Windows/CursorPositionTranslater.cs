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

		public void SetCursorPosition(Point newPosition)
		{
			var newScreenPosition = ToSysPoint(ToScreenPositionFromScreenSpace(newPosition));
			NativeMethods.SetCursorPos(newScreenPosition.X, newScreenPosition.Y);
		}

		private static SysPoint ToSysPoint(Point position)
		{
			return new SysPoint((int)Math.Round(position.X), (int)Math.Round(position.Y));
		}

		internal Point ToScreenPositionFromScreenSpace(Point newPosition)
		{
			newPosition = ScreenSpace.Current.ToPixelSpace(newPosition);
			var newScreenPosition = ToSysPoint(newPosition);
			if ((IntPtr)window.Handle != IntPtr.Zero)
				//ncrunch: no coverage start
				NativeMethods.ClientToScreen((IntPtr)window.Handle, ref newScreenPosition);
				//ncrunch: no coverage end
			return FromSysPoint(newScreenPosition);
		}

		private static Point FromSysPoint(SysPoint newPosition)
		{
			return new Point(newPosition.X, newPosition.Y);
		}

		public Point GetCursorPosition()
		{
			var newPosition = new SysPoint();
			NativeMethods.GetCursorPos(ref newPosition);
			var screenspace = FromScreenPositionToScreenSpace(FromSysPoint(newPosition));
			return new Point((float)Math.Round(screenspace.X, 3), (float)Math.Round(screenspace.Y, 3));
		}

		internal Point FromScreenPositionToScreenSpace(Point newPosition)
		{
			var screenPoint = ToSysPoint(newPosition);
			if ((IntPtr)window.Handle != IntPtr.Zero)
				//ncrunch: no coverage start
				NativeMethods.ScreenToClient((IntPtr)window.Handle, ref screenPoint);
					//ncrunch: no coverage end
			return ScreenSpace.Current.FromPixelSpace(FromSysPoint(screenPoint));
		}
	}
}