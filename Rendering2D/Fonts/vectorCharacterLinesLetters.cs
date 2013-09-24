using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering2D.Fonts
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
					new Vector2D(0, 0.68571f), new Vector2D(0, 0.114285f), new Vector2D(0, 0.114285f),
					new Vector2D(0.128f, 0), new Vector2D(0.128f, 0), new Vector2D(0.384f, 0),
					new Vector2D(0.384f, 0), new Vector2D(0.512f, 0.114285f), new Vector2D(0.512f, 0.114285f),
					new Vector2D(0.512f, 0.68571f), new Vector2D(0.512f, 0.68571f),
					new Vector2D(0.512f, 0.342855f), new Vector2D(0.512f, 0.342855f),
					new Vector2D(0, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('B',
				new[]
				{
					new Vector2D(0, 0.342855f), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0.384f, 0), new Vector2D(0.384f, 0), new Vector2D(0.512f, 0.114285f),
					new Vector2D(0.512f, 0.114285f), new Vector2D(0.512f, 0.22857f),
					new Vector2D(0.512f, 0.22857f), new Vector2D(0.384f, 0.342855f),
					new Vector2D(0.384f, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f),
					new Vector2D(0, 0.68571f), new Vector2D(0, 0.68571f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.384f, 0.68571f), new Vector2D(0.512f, 0.571425f),
					new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.45714f),
					new Vector2D(0.512f, 0.45714f), new Vector2D(0.384f, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('C',
				new[]
				{
					new Vector2D(0.512f, 0), new Vector2D(0.128f, 0), new Vector2D(0.128f, 0),
					new Vector2D(0, 0.114285f), new Vector2D(0, 0.114285f), new Vector2D(0, 0.571425f),
					new Vector2D(0, 0.571425f), new Vector2D(0.128f, 0.68571f), new Vector2D(0.128f, 0.68571f)
					, new Vector2D(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('D',
				new[]
				{
					new Vector2D(0, 0.68571f), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0.384f, 0), new Vector2D(0.384f, 0), new Vector2D(0.512f, 0.114285f),
					new Vector2D(0.512f, 0.114285f), new Vector2D(0.512f, 0.571425f),
					new Vector2D(0.512f, 0.571425f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.384f, 0.68571f), new Vector2D(0, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('E',
				new[]
				{
					new Vector2D(0.512f, 0), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0.512f, 0.342855f),
					new Vector2D(0.512f, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f),
					new Vector2D(0, 0.68571f), new Vector2D(0, 0.68571f), new Vector2D(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('F',
				new[]
				{
					new Vector2D(0.512f, 0), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0.512f, 0.342855f),
					new Vector2D(0.512f, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f),
					new Vector2D(0, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('G',
				new[]
				{
					new Vector2D(0.512f, 0), new Vector2D(0.128f, 0), new Vector2D(0.128f, 0),
					new Vector2D(0, 0.114285f), new Vector2D(0, 0.114285f), new Vector2D(0, 0.571425f),
					new Vector2D(0, 0.571425f), new Vector2D(0.128f, 0.68571f), new Vector2D(0.128f, 0.68571f)
					, new Vector2D(0.384f, 0.68571f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.571425f),
					new Vector2D(0.512f, 0.342855f), new Vector2D(0.512f, 0.342855f),
					new Vector2D(0.256f, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('H',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0, 0.68571f), new Vector2D(0, 0.68571f),
					new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0.512f, 0.342855f),
					new Vector2D(0.512f, 0.342855f), new Vector2D(0.512f, 0), new Vector2D(0.512f, 0),
					new Vector2D(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('I',
				new[]
				{
					new Vector2D(0.128f, 0), new Vector2D(0.384f, 0), new Vector2D(0.384f, 0),
					new Vector2D(0.256f, 0), new Vector2D(0.256f, 0), new Vector2D(0.256f, 0.68571f),
					new Vector2D(0.256f, 0.68571f), new Vector2D(0.128f, 0.68571f),
					new Vector2D(0.128f, 0.68571f), new Vector2D(0.384f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('J',
				new[]
				{
					new Vector2D(0.512f, 0), new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.571425f)
					, new Vector2D(0.384f, 0.68571f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.128f, 0.68571f), new Vector2D(0.128f, 0.68571f), new Vector2D(0, 0.571425f)
				});
			vectorCharacterLines.linePoints.Add('K',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0, 0.68571f), new Vector2D(0, 0.68571f),
					new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0.512f, 0),
					new Vector2D(0.512f, 0), new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f),
					new Vector2D(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('L',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0, 0.68571f), new Vector2D(0, 0.68571f),
					new Vector2D(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('M',
				new[]
				{
					new Vector2D(0, 0.68571f), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0.256f, 0.22857f), new Vector2D(0.256f, 0.22857f), new Vector2D(0.512f, 0),
					new Vector2D(0.512f, 0), new Vector2D(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('N',
				new[]
				{
					new Vector2D(0, 0.68571f), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0.512f, 0.68571f), new Vector2D(0.512f, 0.68571f), new Vector2D(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('O',
				new[]
				{
					new Vector2D(0.128f, 0), new Vector2D(0.384f, 0), new Vector2D(0.384f, 0),
					new Vector2D(0.512f, 0.114285f), new Vector2D(0.512f, 0.114285f),
					new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.571425f),
					new Vector2D(0.384f, 0.68571f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.128f, 0.68571f), new Vector2D(0.128f, 0.68571f), new Vector2D(0, 0.571425f)
					, new Vector2D(0, 0.571425f), new Vector2D(0, 0.114285f), new Vector2D(0, 0.114285f),
					new Vector2D(0.128f, 0)
				});
			vectorCharacterLines.linePoints.Add('P',
				new[]
				{
					new Vector2D(0, 0.68571f), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0.384f, 0), new Vector2D(0.384f, 0), new Vector2D(0.512f, 0.114285f),
					new Vector2D(0.512f, 0.114285f), new Vector2D(0.512f, 0.22857f),
					new Vector2D(0.512f, 0.22857f), new Vector2D(0.384f, 0.342855f),
					new Vector2D(0.384f, 0.342855f), new Vector2D(0, 0.342855f)
				});
			vectorCharacterLines.linePoints.Add('Q',
				new[]
				{
					new Vector2D(0.384f, 0.68571f), new Vector2D(0.128f, 0.68571f),
					new Vector2D(0.128f, 0.68571f), new Vector2D(0, 0.571425f), new Vector2D(0, 0.571425f),
					new Vector2D(0, 0.114285f), new Vector2D(0, 0.114285f), new Vector2D(0.128f, 0),
					new Vector2D(0.128f, 0), new Vector2D(0.384f, 0), new Vector2D(0.384f, 0),
					new Vector2D(0.512f, 0.114285f), new Vector2D(0.512f, 0.114285f),
					new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.571425f),
					new Vector2D(0.384f, 0.68571f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.512f, 0.799995f)
				});
			vectorCharacterLines.linePoints.Add('R',
				new[]
				{
					new Vector2D(0, 0.68571f), new Vector2D(0, 0), new Vector2D(0, 0),
					new Vector2D(0.384f, 0), new Vector2D(0.384f, 0), new Vector2D(0.512f, 0.114285f),
					new Vector2D(0.512f, 0.114285f), new Vector2D(0.512f, 0.22857f),
					new Vector2D(0.512f, 0.22857f), new Vector2D(0.384f, 0.342855f),
					new Vector2D(0.384f, 0.342855f), new Vector2D(0, 0.342855f), new Vector2D(0, 0.342855f),
					new Vector2D(0.512f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('S',
				new[]
				{
					new Vector2D(0, 0.68571f), new Vector2D(0.384f, 0.68571f), new Vector2D(0.384f, 0.68571f)
					, new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.571425f),
					new Vector2D(0.512f, 0.45714f), new Vector2D(0.512f, 0.45714f),
					new Vector2D(0.384f, 0.342855f), new Vector2D(0.384f, 0.342855f),
					new Vector2D(0.128f, 0.342855f), new Vector2D(0.128f, 0.342855f),
					new Vector2D(0, 0.22857f), new Vector2D(0, 0.22857f), new Vector2D(0, 0.114285f),
					new Vector2D(0, 0.114285f), new Vector2D(0.128f, 0), new Vector2D(0.128f, 0),
					new Vector2D(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('T',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0.512f, 0), new Vector2D(0.512f, 0),
					new Vector2D(0.256f, 0), new Vector2D(0.256f, 0), new Vector2D(0.256f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('U',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0, 0.571425f), new Vector2D(0, 0.571425f),
					new Vector2D(0.128f, 0.68571f), new Vector2D(0.128f, 0.68571f),
					new Vector2D(0.384f, 0.68571f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('V',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0, 0.45714f), new Vector2D(0, 0.45714f),
					new Vector2D(0.256f, 0.68571f), new Vector2D(0.256f, 0.68571f),
					new Vector2D(0.512f, 0.45714f), new Vector2D(0.512f, 0.45714f), new Vector2D(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('W',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0, 0.571425f), new Vector2D(0, 0.571425f),
					new Vector2D(0.128f, 0.68571f), new Vector2D(0.128f, 0.68571f),
					new Vector2D(0.256f, 0.571425f), new Vector2D(0.256f, 0.571425f),
					new Vector2D(0.384f, 0.68571f), new Vector2D(0.384f, 0.68571f),
					new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0.571425f), new Vector2D(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('X',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0.512f, 0.68571f), new Vector2D(0.512f, 0.68571f),
					new Vector2D(0.256f, 0.342855f), new Vector2D(0.256f, 0.342855f),
					new Vector2D(0, 0.68571f), new Vector2D(0, 0.68571f), new Vector2D(0.512f, 0)
				});
			vectorCharacterLines.linePoints.Add('Y',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0.256f, 0.22857f), new Vector2D(0.256f, 0.22857f),
					new Vector2D(0.512f, 0), new Vector2D(0.512f, 0), new Vector2D(0.256f, 0.22857f),
					new Vector2D(0.256f, 0.22857f), new Vector2D(0.256f, 0.68571f)
				});
			vectorCharacterLines.linePoints.Add('Z',
				new[]
				{
					new Vector2D(0, 0), new Vector2D(0.512f, 0), new Vector2D(0.512f, 0),
					new Vector2D(0, 0.68571f), new Vector2D(0, 0.68571f), new Vector2D(0.512f, 0.68571f)
				});
		}
	}
}