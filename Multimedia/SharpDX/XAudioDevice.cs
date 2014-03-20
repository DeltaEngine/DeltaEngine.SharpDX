using DeltaEngine.Extensions;
using SharpDX.XAudio2;

namespace DeltaEngine.Multimedia.SharpDX
{
	/// <summary>
	/// Native implementation of an audio context.
	/// </summary>
	public class XAudioDevice : SoundDevice
	{
		public XAudioDevice()
		{
			if (StackTraceExtensions.StartedFromNUnitConsoleButNotFromNCrunch)
				return;
			XAudio = new XAudio2();
			MasteringVoice = new MasteringVoice(XAudio);
		}

		public XAudio2 XAudio { get; private set; }
		public MasteringVoice MasteringVoice { get; private set; }

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
			if (MasteringVoice != null)
				MasteringVoice.Dispose();
			MasteringVoice = null;
		}
		private void DisposeXAudio()
		{
			if (XAudio != null)
				XAudio.Dispose();
			XAudio = null;
		}
	}
}