using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Fonts
{
	internal class VectorCharacterLinesLetters
	{
		public VectorCharacterLinesLetters(VectorCharacterLines vectorCharacterLines)
		{
			this.vectorCharacterLines = vectorCharacterLines;
		}

		private readonly VectorCharacterLines vectorCharacterLines;

		public void AddLetters()
		{
			vectorCharacterLines.linePoints.Add('A',
				new[]
				{
					new Point(0, 0.68571f), new Point(0, 0.114285f), new Point(0, 0.114285f),
					new Point(0.128f, 0), new Point(0.128f, 0), new Point(0.384f, 0), new Point(0.384f, 0),
					new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f), new Point(0.512f, 0.68571f),
					new Point(0.512f, 0.68571f), new Point(0.512f, 0.342855f), new Point(0.512f, 0.342855f),
					new Point(0, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('B',
				new[]
				{
					new Point(0, 0.342855f), new Point(0, 0), new Point(0, 0), new Point(0.384f, 0),
					new Point(0.384f, 0), new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.22857f), new Point(0.512f, 0.22857f), new Point(0.384f, 0.342855f),
					new Point(0.384f, 0.342855f), new Point(0, 0.342855f), new Point(0, 0.342855f),
					new Point(0, 0.68571f), new Point(0, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.384f, 0.68571f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0.45714f), new Point(0.512f, 0.45714f), new Point(0.384f, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('C',
				new[]
				{
					new Point(0.512f, 0), new Point(0.128f, 0), new Point(0.128f, 0), new Point(0, 0.114285f)
					, new Point(0, 0.114285f), new Point(0, 0.571425f), new Point(0, 0.571425f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('D',
				new[]
				{
					new Point(0, 0.68571f), new Point(0, 0), new Point(0, 0), new Point(0.384f, 0),
					new Point(0.384f, 0), new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f), new Point(0.384f, 0.68571f),
					new Point(0.384f, 0.68571f), new Point(0, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('E',
				new[]
				{
					new Point(0.512f, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0.342855f),
					new Point(0, 0.342855f), new Point(0.512f, 0.342855f), new Point(0.512f, 0.342855f),
					new Point(0, 0.342855f), new Point(0, 0.342855f), new Point(0, 0.68571f),
					new Point(0, 0.68571f), new Point(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('F',
				new[]
				{
					new Point(0.512f, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0.342855f),
					new Point(0, 0.342855f), new Point(0.512f, 0.342855f), new Point(0.512f, 0.342855f),
					new Point(0, 0.342855f), new Point(0, 0.342855f), new Point(0, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('G',
				new[]
				{
					new Point(0.512f, 0), new Point(0.128f, 0), new Point(0.128f, 0), new Point(0, 0.114285f)
					, new Point(0, 0.114285f), new Point(0, 0.571425f), new Point(0, 0.571425f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.384f, 0.68571f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0.342855f), new Point(0.512f, 0.342855f), new Point(0.256f, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('H',
				new[]
				{
					new Point(0, 0), new Point(0, 0.68571f), new Point(0, 0.68571f), new Point(0, 0.342855f),
					new Point(0, 0.342855f), new Point(0.512f, 0.342855f), new Point(0.512f, 0.342855f),
					new Point(0.512f, 0), new Point(0.512f, 0), new Point(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('I',
				new[]
				{
					new Point(0.128f, 0), new Point(0.384f, 0), new Point(0.384f, 0), new Point(0.256f, 0),
					new Point(0.256f, 0), new Point(0.256f, 0.68571f), new Point(0.256f, 0.68571f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.384f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('J',
				new[]
				{
					new Point(0.512f, 0), new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f),
					new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f), new Point(0.128f, 0.68571f),
					new Point(0.128f, 0.68571f), new Point(0, 0.571425f)
				});
			vectorCharacterLines.linePoints.Add('K',
				new[]
				{
					new Point(0, 0), new Point(0, 0.68571f), new Point(0, 0.68571f), new Point(0, 0.342855f),
					new Point(0, 0.342855f), new Point(0.512f, 0), new Point(0.512f, 0),
					new Point(0, 0.342855f), new Point(0, 0.342855f), new Point(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('L',
				new[]
				{
					new Point(0, 0), new Point(0, 0.68571f), new Point(0, 0.68571f),
					new Point(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('M',
				new[]
				{
					new Point(0, 0.68571f), new Point(0, 0), new Point(0, 0), new Point(0.256f, 0.22857f),
					new Point(0.256f, 0.22857f), new Point(0.512f, 0), new Point(0.512f, 0),
					new Point(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('N',
				new[]
				{
					new Point(0, 0.68571f), new Point(0, 0), new Point(0, 0), new Point(0.512f, 0.68571f),
					new Point(0.512f, 0.68571f), new Point(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('O',
				new[]
				{
					new Point(0.128f, 0), new Point(0.384f, 0), new Point(0.384f, 0),
					new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0.571425f), new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0, 0.571425f),
					new Point(0, 0.571425f), new Point(0, 0.114285f), new Point(0, 0.114285f),
					new Point(0.128f, 0)
				});
			vectorCharacterLines.linePoints.Add('P',
				new[]
				{
					new Point(0, 0.68571f), new Point(0, 0), new Point(0, 0), new Point(0.384f, 0),
					new Point(0.384f, 0), new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.22857f), new Point(0.512f, 0.22857f), new Point(0.384f, 0.342855f),
					new Point(0.384f, 0.342855f), new Point(0, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('Q',
				new[]
				{
					new Point(0.384f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f),
					new Point(0, 0.571425f), new Point(0, 0.571425f), new Point(0, 0.114285f),
					new Point(0, 0.114285f), new Point(0.128f, 0), new Point(0.128f, 0), new Point(0.384f, 0),
					new Point(0.384f, 0), new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f), new Point(0.384f, 0.68571f),
					new Point(0.384f, 0.68571f), new Point(0.512f, 0.799995f)
				});
			vectorCharacterLines.linePoints.Add('R',
				new[]
				{
					new Point(0, 0.68571f), new Point(0, 0), new Point(0, 0), new Point(0.384f, 0),
					new Point(0.384f, 0), new Point(0.512f, 0.114285f), new Point(0.512f, 0.114285f),
					new Point(0.512f, 0.22857f), new Point(0.512f, 0.22857f), new Point(0.384f, 0.342855f),
					new Point(0.384f, 0.342855f), new Point(0, 0.342855f), new Point(0, 0.342855f),
					new Point(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('S',
				new[]
				{
					new Point(0, 0.68571f), new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.45714f),
					new Point(0.512f, 0.45714f), new Point(0.384f, 0.342855f), new Point(0.384f, 0.342855f),
					new Point(0.128f, 0.342855f), new Point(0.128f, 0.342855f), new Point(0, 0.22857f),
					new Point(0, 0.22857f), new Point(0, 0.114285f), new Point(0, 0.114285f),
					new Point(0.128f, 0), new Point(0.128f, 0), new Point(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('T',
				new[]
				{
					new Point(0, 0), new Point(0.512f, 0), new Point(0.512f, 0), new Point(0.256f, 0),
					new Point(0.256f, 0), new Point(0.256f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('U',
				new[]
				{
					new Point(0, 0), new Point(0, 0.571425f), new Point(0, 0.571425f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.384f, 0.68571f), new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f),
					new Point(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('V',
				new[]
				{
					new Point(0, 0), new Point(0, 0.45714f), new Point(0, 0.45714f),
					new Point(0.256f, 0.68571f), new Point(0.256f, 0.68571f), new Point(0.512f, 0.45714f),
					new Point(0.512f, 0.45714f), new Point(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('W',
				new[]
				{
					new Point(0, 0), new Point(0, 0.571425f), new Point(0, 0.571425f),
					new Point(0.128f, 0.68571f), new Point(0.128f, 0.68571f), new Point(0.256f, 0.571425f),
					new Point(0.256f, 0.571425f), new Point(0.384f, 0.68571f), new Point(0.384f, 0.68571f),
					new Point(0.512f, 0.571425f), new Point(0.512f, 0.571425f), new Point(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('X',
				new[]
				{
					new Point(0, 0), new Point(0.512f, 0.68571f), new Point(0.512f, 0.68571f),
					new Point(0.256f, 0.342855f), new Point(0.256f, 0.342855f), new Point(0, 0.68571f),
					new Point(0, 0.68571f), new Point(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('Y',
				new[]
				{
					new Point(0, 0), new Point(0.256f, 0.22857f), new Point(0.256f, 0.22857f),
					new Point(0.512f, 0), new Point(0.512f, 0), new Point(0.256f, 0.22857f),
					new Point(0.256f, 0.22857f), new Point(0.256f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('Z',
				new[]
				{
					new Point(0, 0), new Point(0.512f, 0), new Point(0.512f, 0), new Point(0, 0.68571f),
					new Point(0, 0.68571f), new Point(0.512f, 0.68571f)
				});
		}
	}
}