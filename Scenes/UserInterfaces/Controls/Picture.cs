using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// The most basic visible control which is just a Sprite which can change appearance 
	/// on request
	/// </summary>
	public class Picture : Control
	{
		protected Picture(List<object> createFromComponents)
			: base(createFromComponents)
		{
			Add(theme = Theme.Default);
			// theme needs to be extracted
		}

		public Picture(Theme theme, Theme.Appearance appearance, Rectangle drawArea)
			: base(drawArea)
		{
			Add(this.theme = theme);
			SetAppearance(appearance);
		}

		protected readonly Theme theme;

		public void SetAppearance(Theme.Appearance appearance)
		{
			Material = appearance.Material;
			Color = appearance.Color;
		}
	}
}