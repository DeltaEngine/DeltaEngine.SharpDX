using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Content.Xml.Tests
{
	internal class InputCommandsTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void TestInputCommands()
		{
			var inputCommands = ContentLoader.Load<InputCommands>("DefaultCommands");
			Assert.AreEqual("DefaultCommands", inputCommands.Name);
			Assert.IsTrue(inputCommands.InternalAllowCreationIfContentNotFound);
		}

		[Test, CloseAfterFirstFrame]
		public void ThrowExceptionAndLogErrorIfTriggerTypeDoesNotExist()
		{
			const string LogErrorMessage = NonTriggerTypeName + "' not found";
			ContentLoader.Load<NoDataInputCommands>("NoDataInputCommands");
			Assert.IsTrue(Resolve<Logger>().LastMessage.Contains(LogErrorMessage));
		}

		private const string NonTriggerTypeName = "MockTrigger";

		private class NoDataInputCommands : InputCommands
		{
			protected NoDataInputCommands(string contentName)
				: base(contentName) {}

			protected override bool AllowCreationIfContentNotFound
			{
				get { return true; }
			}

			protected override void CreateDefault()
			{
				var click = new XmlData("Command");
				click.AddChild(NonTriggerTypeName, "");
				Command.Register(Command.Click, ParseTriggers(click));
			}
		}
	}
}