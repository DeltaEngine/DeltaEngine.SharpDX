using System.Runtime.InteropServices;

namespace DeltaEngine.Platforms.Windows
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal class WindowsDisplayDevice
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		internal string deviceName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		internal string deviceString;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		internal string deviceID;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		internal string deviceKey;
		internal uint stateFlags;
	}
}