using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace EmptyGame
{
	/// <summary>
	/// Changes the window's background to a random color each second.
	/// </summary>
	public class Game : Entity, Updateable
	{
		public Game(Window window)
		{
			this.window = window;
		}

		public readonly Window window;
		public float ElapsedTimeSinceColorChange { get; private set; }
		public Color CurrentColor { get; private set; }
		public Color NextColor { get; private set; }

		public void Update()
		{
			ElapsedTimeSinceColorChange += Time.Delta;
			if (ElapsedTimeSinceColorChange >= 1.0f)
				SwitchToNextRandomColor();
			window.BackgroundColor = CurrentColor.Lerp(NextColor, ElapsedTimeSinceColorChange);
		}

		private void SwitchToNextRandomColor()
		{
			CurrentColor = NextColor;
			NextColor = Color.GetRandomColor();
			ElapsedTimeSinceColorChange = 0;
		}

		public bool IsPauseable { get { return true; } }
	}
}