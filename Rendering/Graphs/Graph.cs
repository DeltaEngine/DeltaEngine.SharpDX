using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering.Shapes;

namespace DeltaEngine.Rendering.Graphs
{
	/// <summary>
	/// Renders a graph with one or more lines at a given scale.
	/// Various logic can be turned on and off including autogrowing, autopruning and 
	/// rendering axes and percentiles.
	/// </summary>
	public class Graph : FilledRect, Updateable
	{
		public Graph(Rectangle drawArea)
			: base(drawArea, HalfBlack) {}

		internal static readonly Color HalfBlack = new Color(0, 0, 0, 0.75f);

		public void Update()
		{
			if (DidFootprintChange)
				RefreshAll();
		}

		private void RefreshAll()
		{
			renderKey.Refresh(this);
			renderAxes.Refresh(this);
			renderPercentiles.Refresh(this);
			renderPercentileLabels.Refresh(this);
			foreach (GraphLine line in Lines)
				line.Refresh();
		}

		private readonly RenderKey renderKey = new RenderKey();
		private readonly RenderAxes renderAxes = new RenderAxes { Visibility = Visibility.Hide };
		internal readonly List<GraphLine> Lines = new List<GraphLine>();

		private readonly RenderPercentiles renderPercentiles = new RenderPercentiles
		{
			Visibility = Visibility.Hide
		};

		private readonly RenderPercentileLabels renderPercentileLabels = new RenderPercentileLabels
		{
			Visibility = Visibility.Hide
		};

		public override Visibility Visibility
		{
			get
			{
				return base.Visibility;
			}
			set
			{
				base.Visibility = value;
				RefreshAll();
			}
		}

		public GraphLine CreateLine(string key, Color color)
		{
			var line = new GraphLine(this) { Key = key, Color = color };
			Lines.Add(line);
			renderKey.Refresh(this);
			return line;
		}

		public void RemoveLine(GraphLine line)
		{
			line.Clear();
			Lines.Remove(line);
			renderKey.Refresh(this);
		}

		internal void AddPoint(Point point)
		{
			removeOldestPoints.Process(this);
			if (IsAutogrowing)
				autogrowViewport.ProcessAddedPoint(this, point);
		}

		public bool IsAutogrowing { get; set; }
		private readonly AutogrowViewport autogrowViewport = new AutogrowViewport();
		
		internal void RefreshKey()
		{
			renderKey.Refresh(this);
		}

		public Visibility AxesVisibility
		{
			get { return renderAxes.Visibility; }
			set
			{
				if (renderAxes.Visibility == value)
					return;
				renderAxes.Visibility = value;
				renderAxes.Refresh(this);
			}
		}

		public Visibility PercentilesVisibility
		{
			get { return renderPercentiles.Visibility; }
			set
			{
				if (renderPercentiles.Visibility == value)
					return;
				renderPercentiles.Visibility = value;
				renderPercentiles.Refresh(this);
			}
		}

		public Visibility PercentileLabelsVisibility
		{
			get { return renderPercentileLabels.Visibility; }
			set
			{
				if (renderPercentileLabels.Visibility == value)
					return;
				renderPercentileLabels.Visibility = value;
				renderPercentileLabels.Refresh(this);
			}
		}

		public Visibility KeyVisibility
		{
			get { return renderKey.Visibility; }
			set
			{
				if (renderKey.Visibility == value)
					return;
				renderKey.Visibility = value;
				renderKey.Refresh(this);
			}
		}

		public Rectangle Viewport
		{
			get { return viewport; }
			set
			{
				if (viewport == value)
					return;
				viewport = value;
				renderAxes.Refresh(this);
				renderPercentileLabels.Refresh(this);
				foreach (GraphLine line in Lines)
					line.Refresh();
			}
		}
		private Rectangle viewport;

		public Color AxisColor
		{
			get { return renderAxes.XAxis.Color; }
			set
			{
				renderAxes.XAxis.Color = value;
				renderAxes.YAxis.Color = value;
			}
		}

		public Point Origin
		{
			get { return origin; }
			set
			{
				if (origin == value)
					return;
				origin = value;
				renderAxes.Refresh(this);
			}
		}
		private Point origin;

		public int MaximumNumberOfPoints
		{
			get { return removeOldestPoints.MaximumNumberOfPoints; }
			set
			{
				if (removeOldestPoints.MaximumNumberOfPoints == value)
					return;
				removeOldestPoints.MaximumNumberOfPoints = value;
				removeOldestPoints.Process(this);
			}
		}

		private readonly RemoveOldestPoints removeOldestPoints = new RemoveOldestPoints();

		public int NumberOfPercentiles
		{
			get { return renderPercentiles.NumberOfPercentiles; }
			set
			{
				if (renderPercentiles.NumberOfPercentiles == value)
					return;
				renderPercentiles.NumberOfPercentiles = value;
				renderPercentileLabels.NumberOfPercentiles = value;
				renderPercentiles.Refresh(this);
				renderPercentileLabels.Refresh(this);
			}
		}

		public Color PercentileColor
		{
			get { return renderPercentiles.PercentileColor; }
			set
			{
				if (renderPercentiles.PercentileColor == value)
					return;
				renderPercentiles.PercentileColor = value;
				renderPercentiles.Refresh(this);
			}
		}

		public string PercentilePrefix
		{
			get { return renderPercentileLabels.PercentilePrefix; }
			set
			{
				if (renderPercentileLabels.PercentilePrefix == value)
					return;
				renderPercentileLabels.PercentilePrefix = value;
				renderPercentileLabels.Refresh(this);
			}
		}

		public string PercentileSuffix
		{
			get { return renderPercentileLabels.PercentileSuffix; }
			set
			{
				if (renderPercentileLabels.PercentileSuffix == value)
					return;
				renderPercentileLabels.PercentileSuffix = value;
				renderPercentileLabels.Refresh(this);
			}
		}

		public Color PercentileLabelColor
		{
			get { return renderPercentileLabels.PercentileLabelColor; }
			set
			{
				if (renderPercentileLabels.PercentileLabelColor == value)
					return;
				renderPercentileLabels.PercentileLabelColor = value;
				renderPercentileLabels.Refresh(this);
			}
		}

		public bool ArePercentileLabelsInteger
		{
			get { return renderPercentileLabels.ArePercentileLabelsInteger; }
			set
			{
				if (renderPercentileLabels.ArePercentileLabelsInteger == value)
					return;
				renderPercentileLabels.ArePercentileLabelsInteger = value;
				renderPercentileLabels.Refresh(this);
			}
		}

		internal const float Border = 0.025f;
	}
}