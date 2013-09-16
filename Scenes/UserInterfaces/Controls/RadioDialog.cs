using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// A simple UI control which creates and groups Radio Buttons.
	/// </summary>
	public class RadioDialog : Control
	{
		public RadioDialog(Rectangle drawArea)
			: this(Theme.Default, drawArea) {}

		public RadioDialog(Theme theme, Rectangle drawArea)
			: base(drawArea)
		{
			Add(this.theme = theme);
			Add(buttons = new List<RadioButton>());
		}

		private readonly Theme theme;
		private readonly List<RadioButton> buttons;

		public void AddButton(string text)
		{
			var button = new RadioButton(theme, Rectangle.Unused, text);
			button.Clicked += () => ButtonClicked(button);
			buttons.Add(button);
			AddChild(button);
			if (buttons.Count == 1)
				button.State.IsSelected = true;
			for (int i = 0; i < buttons.Count; i++)
				buttons[i].DrawArea = GetButtonDrawArea(i);
		}

		private Rectangle GetButtonDrawArea(int position)
		{
			float height = DrawArea.Height / (buttons.Count);
			float aspectRatio = theme.RadioButtonBackground.Material.MaterialRenderSize.AspectRatio;
			float width = height * aspectRatio;
			var d = new Rectangle(DrawArea.Left, DrawArea.Top + position * height, width, height);
			return d;
		}

		private void ButtonClicked(RadioButton clicked)
		{
			foreach (RadioButton button in buttons)
				button.State.IsSelected = (button == clicked);
		}

		public override void Update()
		{
			base.Update();
			for (int i = 0; i < buttons.Count; i++)
				buttons[i].DrawArea = GetButtonDrawArea(i);
		}

		public RadioButton SelectedButton
		{
			get { return Get<List<RadioButton>>().FirstOrDefault(button => button.State.IsSelected); }
		}
	}
}