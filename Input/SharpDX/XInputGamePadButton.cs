namespace DeltaEngine.Input.SharpDX
{
	/// <summary>
	/// Buttons are read from a bitmask, set means the corresponding button is pressed.
	/// </summary>
	internal enum XInputGamePadButton
	{
		DPadUp = 0x00000001,
		DPadDown = 0x00000002,
		DPadLeft = 0x00000004,
		DPadRight = 0x00000008,
		Start = 0x00000010,
		Back = 0x00000020,
		LeftThumb = 0x00000040,
		RightThumb = 0x00000080,
		LeftShoulder = 0x0100,
		RightShoulder = 0x0200,
		A = 0x1000,
		B = 0x2000,
		X = 0x4000,
		Y = 0x8000,
	}
}