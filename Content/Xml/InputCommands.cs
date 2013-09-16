using System;
using System.IO;
using DeltaEngine.Commands;
using DeltaEngine.Core;

namespace DeltaEngine.Content.Xml
{
	/// <summary>
	/// Commands from a content file which trigger input events.
	/// </summary>
	public class InputCommands : XmlContent
	{
		protected InputCommands(string contentName)
			: base(contentName) {}

		protected override bool AllowCreationIfContentNotFound
		{
			get { return Name == "DefaultCommands"; }
		}

		protected override void CreateDefault()
		{
			var exit = new XmlData("Command");
			exit.AddChild("KeyTrigger", "Escape");
			Command.Register(Command.Exit, ParseTriggers(exit));
			var click = new XmlData("Command");
			click.AddChild("KeyTrigger", "Space");
			click.AddChild("MouseButtonTrigger", "Left");
			click.AddChild("TouchPressTrigger", "");
			click.AddChild("GamePadButtonTrigger", "A");
			Command.Register(Command.Click, ParseTriggers(click));
		}

		protected override void LoadData(Stream fileData)
		{
			base.LoadData(fileData);
			foreach (var child in Data.Children)
				Command.Register(child.GetAttributeValue("Name"), ParseTriggers(child));
		}

		private static Trigger[] ParseTriggers(XmlData command)
		{
			var triggers = new Trigger[command.Children.Count];
			for (int index = 0; index < command.Children.Count; index++)
			{
				var triggerNode = command.Children[index];
				var triggerType = triggerNode.Name.GetTypeFromShortNameOrFullNameIfNotFound();
				if (triggerType == null)
					throw new UnableToCreateTriggerTypeIsUnknown(triggerNode.Name); //ncrunch: no coverage
				try
				{
					triggers[index] = Activator.CreateInstance(triggerType, triggerNode.Value) as Trigger;
				}
				catch (Exception e)
				{	
					Logger.Error(e);
				}
			}
			return triggers;
		}

		private class UnableToCreateTriggerTypeIsUnknown : Exception
		{  //ncrunch: no coverage start
			public UnableToCreateTriggerTypeIsUnknown(string name)
				: base(name) {}
		}  //ncrunch: no coverage end
	}
}