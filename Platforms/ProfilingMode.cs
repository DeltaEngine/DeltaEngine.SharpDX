using System;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Combination of profiling modes that can be turned on via Settings. Normally all modes are off.
	/// Implementation can be found in the DeltaEngine.Profiling namespace.
	/// </summary>
	[Flags]
	public enum ProfilingMode
	{
		None = 0,
		Fps = 1,
		Engine = 2,
		Application = 4,
		Rendering = 8,
		UI = 16,
		Physics = 32,
		AI = 64,
		Text = 128,
		AvailableRAM = 256
	}
}