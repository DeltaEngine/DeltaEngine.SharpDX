using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Fonts
{
	/// <summary>
	/// Points for the VectorText lines for each character supported.
	/// </summary>
	internal class VectorCharacterLines
	{
		public VectorCharacterLines()
		{
			AddNumbers();
			AddPoint();
			vectorCharacterLinesLetters = new VectorCharacterLinesLetters(this);
			vectorCharacterLinesLetters.AddLetters();
		}

		internal readonly Dictionary<char, Point[]> linePoints = new Dictionary<char, Point[]>();
		private readonly VectorCharacterLinesLetters vectorCharacterLinesLetters;

		private void AddNumbers()
		{
			linePoints.Add('0',
				new[]
				{
					new Point(0.128f, 0), new Point(0.384f, 0), new Point(0.384f, 0),
					new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0.571425f), new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0, 0.571425f),
					new Point(0, 0.571425f), new Point(0, 0.114285f), new Point(0, 0.114285f),
					new Point(0.128f, 0)
				});
			linePoints.Add('1', new[] { new Point(0.256f, 0), new Point(0.256f, 0.68571f) });
			linePoints.Add('2',
				new[]
				{
					new Point(0, 0.114285f), new Point(0.128f, 0), new Point(0.128f, 0), new Point(0.384f, 0),
					new Point(0.384f, 0), new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.22857f), new Point(0.512f, 0.22857f), new Point(0, 0.68571f),
					new Point(0, 0.68571f), new Point(0.512f, 0.68571f)
				});
			linePoints.Add('3',
				new[]
				{
					new Point(0, 0.114285f), new Point(0.128f, 0), new Point(0.128f, 0), new Point(0.384f, 0),
					new Point(0.384f, 0), new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.22857f), new Point(0.512f, 0.22857f), new Point(0.384f, 0.342855f),
					new Point(0.384f, 0.342855f), new Point(0, 0.342855f), new Point(0, 0.342855f),
					new Point(0.384f, 0.342855f), new Point(0.384f, 0.342855f), new Point(0.512f, 0.45714f),
					new Point(0.512f, 0.45714f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f),
					new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f), new Point(0.128f, 0.68571f),
					new Point(0.128f, 0.68571f), new Point(0, 0.571425f)
				});
			linePoints.Add('4',
				new[]
				{
					new Point(0.512f, 0.342855f), new Point(0, 0.342855f), new Point(0, 0.342855f),
					new Point(0.384f, 0), new Point(0.384f, 0), new Point(0.384f, 0.68571f)
				});
			linePoints.Add('5',
				new[]
				{
					new Point(0.512f, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0.342855f),
					new Point(0, 0.342855f), new Point(0.384f, 0.342855f), new Point(0.384f, 0.342855f),
					new Point(0.512f, 0.45714f), new Point(0.512f, 0.45714f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0.571425f), new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0, 0.68571f)
				});
			linePoints.Add('6',
				new[]
				{
					new Point(0.512f, 0), new Point(0.128f, 0), new Point(0.128f, 0), new Point(0, 0.114285f),
					new Point(0, 0.114285f), new Point(0, 0.571425f), new Point(0, 0.571425f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.384f, 0.68571f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0.45714f), new Point(0.512f, 0.45714f), new Point(0.384f, 0.342855f),
					new Point(0.384f, 0.342855f), new Point(0, 0.342855f)
				});
			linePoints.Add('7',
				new[]
				{ new Point(0, 0), new Point(0.512f, 0), new Point(0.512f, 0), new Point(0, 0.68571f) });
			linePoints.Add('8',
				new[]
				{
					new Point(0.128f, 0), new Point(0.384f, 0), new Point(0.384f, 0),
					new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f), new Point(0.512f, 0.22857f),
					new Point(0.512f, 0.22857f), new Point(0.384f, 0.342855f), new Point(0.384f, 0.342855f),
					new Point(0.128f, 0.342855f), new Point(0.128f, 0.342855f), new Point(0, 0.45714f),
					new Point(0, 0.45714f), new Point(0, 0.571425f), new Point(0, 0.571425f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.384f, 0.68571f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0.45714f), new Point(0.512f, 0.45714f), new Point(0.384f, 0.342855f),
					new Point(0.384f, 0.342855f), new Point(0.128f, 0.342855f), new Point(0.128f, 0.342855f),
					new Point(0, 0.22857f), new Point(0, 0.22857f), new Point(0, 0.114285f),
					new Point(0, 0.114285f), new Point(0.128f, 0)
				});
			linePoints.Add('9',
				new[]
				{
					new Point(0, 0.68571f), new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.114285f), new Point(0.384f, 0), new Point(0.384f, 0),
					new Point(0.128f, 0), new Point(0.128f, 0), new Point(0, 0.114285f),
					new Point(0, 0.114285f), new Point(0, 0.22857f), new Point(0, 0.22857f),
					new Point(0.128f, 0.342855f), new Point(0.128f, 0.342855f), new Point(0.512f, 0.342855f)
				});
		}

		private void AddPoint()
		{
			linePoints.Add('.',
				new[]
				{
					new Point(0.256f, 0.571425f), new Point(0.384f, 0.571425f), new Point(0.384f, 0.571425f),
					new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f), new Point(0.256f, 0.68571f),
					new Point(0.256f, 0.68571f), new Point(0.256f, 0.571425f)
				});
		}

		public Point[] GetPoints(char c)
		{
			c = char.ToUpperInvariant(c);
			Point[] points;
			return linePoints.TryGetValue(c, out points) ? points : new Point[0];
		}
	}
}