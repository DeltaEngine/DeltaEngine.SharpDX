using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D.Graphs;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	internal class ColorGradient : Control
	{
		public ColorGradient(Rectangle drawArea, Color defaultColor = default(Color))
			: this(drawArea, new RangeGraph<Color>(defaultColor, defaultColor)) {}

		public ColorGradient(Rectangle drawArea, RangeGraph<Color> colorRange)
			: base(drawArea)
		{
			gradientGraph = new GradientGraph(drawArea, colorRange);
		}

		private GradientGraph gradientGraph;

		public override void Update()
		{
			base.Update();
			CheckForMarkerClick();
		}

		private void CheckForMarkerClick() {}
	}
}