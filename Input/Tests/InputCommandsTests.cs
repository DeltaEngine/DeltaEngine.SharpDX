using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using NUnit.Framework;

namespace DeltaEngine.Input.Tests
{
	public class InputCommandsTests : TestWithMocksOrVisually
	{
		[Test]
		public void CountPressingAndReleasing()
		{
			int pressed = 0;
			int released = 0;
			var fontText = new FontText(Font.Default,
				"MouseLeft pressed: " + pressed + " released: " + released, Rectangle.One);
			new Command( //ncrunch: no coverage start
				() => fontText.Text = "MouseLeft pressed: " + ++pressed + " released: " + released).Add(
					new MouseButtonTrigger());
			//ncrunch: no coverage end
			new Command( //ncrunch: no coverage start
				() => fontText.Text = "MouseLeft pressed: " + pressed + " released: " + ++released).Add(
					new MouseButtonTrigger(MouseButton.Left, State.Releasing));
			//ncrunch: no coverage end
		}

		[Test, CloseAfterFirstFrame]
		public void GetInputCommands()
		{
			var inputCommands = ContentLoader.Load<InputCommands>("InputCommands");
			Assert.AreEqual(ContentType.InputCommand, inputCommands.MetaData.Type);
		}
	}
}