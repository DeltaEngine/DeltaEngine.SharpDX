using DeltaEngine.Extensions;
using SlimDX.XAudio2;

namespace DeltaEngine.Multimedia.SlimDX
{
	/// <summary>
	/// Native audio context implementation. Crashes rarely on our CI server, there it is disabled.
	/// </summary>
	public class XAudioDevice : SoundDevice
	{
		public XAudioDevice()
		{
			if (StackTraceExtensions.StartedFromNUnitConsoleButNotFromNCrunch)
				return;
			XAudio = new XAudio2();
			masteringVoice = new MasteringVoice(XAudio);
		}

		public XAudio2 XAudio { get; private set; }
		private MasteringVoice masteringVoice;

		public override void RapidUpdate()
		{
			base.RapidUpdate();
			if (XAudio != null)
				XAudio.CommitChanges(XAudio2.CommitAll);
		}

		public override void Dispose()
		{
			base.Dispose();
			DisposeMasteringVoice();
			DisposeXAudio();
		}

		private void DisposeMasteringVoice()
		{
			if (masteringVoice != null)
				masteringVoice.Dispose();
			masteringVoice = null;
		}

		private void DisposeXAudio()
		{
			if (XAudio != null)
				XAudio.Dispose();
			XAudio = null;
		}
	}
}