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
			XAudio2 = new XAudio2();
			MasteringVoice = new MasteringVoice(XAudio2);
		}

		public XAudio2 XAudio2 { get; private set; }
		public MasteringVoice MasteringVoice { get; private set; }

		public override void RapidUpdate()
		{
			base.RapidUpdate();
			XAudio2.CommitChanges(XAudio2.CommitAll);
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
			if (XAudio2 != null)
				XAudio2.Dispose();
			XAudio2 = null;
		}
	}
}