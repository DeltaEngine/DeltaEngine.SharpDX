using System;
using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Input.Windows
{
	/// <summary>
	/// Native Windows implementation of the Touch interface.
	/// </summary>
	public sealed class WindowsTouch : Touch
	{
		public WindowsTouch(Window window)
		{
			var positionTranslator = new CursorPositionTranslater(window);
			touches = new TouchCollection(positionTranslator);
			hook = new TouchHook(window);
			IsAvailable = CheckIfWindows7OrHigher();
		}

		private readonly TouchHook hook;
		private readonly TouchCollection touches;

		public override bool IsAvailable { get; protected set; }

		public override void Dispose()
		{
			hook.Dispose();
		}

		private static bool CheckIfWindows7OrHigher()
		{
			Version version = Environment.OSVersion.Version;
			return version.Major >= 6 && version.Minor >= 1;
		}

		public override Point GetPosition(int touchIndex)
		{
			return touches.locations[touchIndex];
		}

		public override State GetState(int touchIndex)
		{
			return touches.states[touchIndex];
		}

		public override void Update(IEnumerable<Entity> entities)
		{
			//if (!IsAvailable)
			//	return;

			var newTouches = new List<NativeTouchInput>(hook.nativeTouches.ToArray());
			touches.UpdateAllTouches(newTouches);
			hook.nativeTouches.Clear();
			base.Update(entities);
		}
	}
}