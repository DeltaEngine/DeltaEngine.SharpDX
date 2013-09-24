using System.Collections.Generic;

namespace DeltaEngine.Datatypes
{
	public abstract class BaseRangeGraph<T> : Range<T>
		where T : Lerp<T>
	{
		protected BaseRangeGraph (T minimum, T maximum) : base(minimum , maximum ) {}

		protected BaseRangeGraph() { Values = new T[2]; }

		protected BaseRangeGraph(List<T> values)
		{
			Values = values.ToArray();
		} 

		public T[] Values { get; protected set; }

		public abstract void SetValue(int index, T value);

		public override T Start
		{
			get
			{
				EnsureValuesInitialized();
				return Values[0];
			}
			set
			{
				EnsureValuesInitialized();
				Values[0] = value;
			}
		}

		public override T End
		{
			get
			{
				EnsureValuesInitialized();
				return Values[Values.Length - 1];
			}
			set
			{
				EnsureValuesInitialized();
				Values[Values.Length - 1] = value;
			}
		}

		protected void ExpandAndAddValue(int insertIndex, T value)
		{
			var valueBuffer = Values;
			var newLength = Values.Length + 1;
			Values = new T[newLength];
			for (int i = 0; i < newLength; i++)
				if (i == insertIndex)
					Values[i] = value;
				else if (i > insertIndex)
					Values[i] = valueBuffer[i - 1];
				else
					Values[i] = valueBuffer[i];
		}

		protected void EnsureValuesInitialized()
		{
			if (Values == null)
				Values = new T[2];
		}

		public abstract T GetInterpolatedValue(float interpolation);
	}
}