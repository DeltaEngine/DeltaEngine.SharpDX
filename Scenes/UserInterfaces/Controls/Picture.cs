using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
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

		protected Theme Theme
		{
			set { Set(value); }
			get { return Get<Theme>(); }
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