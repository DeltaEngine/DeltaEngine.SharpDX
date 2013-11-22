using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Scenes.Controls
{
	/// <summary>
	/// Most basic visible control which is just a Sprite which can change appearance on request
	/// </summary>
	public class Picture : Control
	{
		protected Picture() {}

		public Picture(Theme theme, Material material, Rectangle drawArea)
			: base(drawArea)
		{
			Theme = theme;
			SetAppearanceWithoutInterpolation(material);
		}

		public Theme Theme
		{
			get
			{
				if (Contains<Theme>())
					return Get<Theme>();
				var theme = new Theme(); //ncrunch: no coverage start
				Add(theme);
				return theme; //ncrunch: no coverage end
			}
			set { Set(value); }
		}

		public void SetAppearanceWithoutInterpolation(Material material)
		{
			Material = material;
			SetWithoutInterpolation(material.DefaultColor);
		}

		public void SetAppearance(Material material)
		{
			Material = material;
			Color = material.DefaultColor;
		}
	}
}