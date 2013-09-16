using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DeltaEngine.Datatypes
{
	public class RangeGraph<T> : Range<T>
		where T : Lerp<T>
	{
		public RangeGraph()
		{
			Values = new T[2];
		}

		public RangeGraph(T minimum, T maximum)
			: base(minimum, maximum) {}

		public RangeGraph(List<T> values)
		{
			Values = values.ToArray();
		}

		public T[] Values { get; private set; }

		public T GetInterpolatedValue(float interpolation)
		{
			if (interpolation == 1.0f)
				return Values[Values.Length - 1];
			var intervalLeft = (int)(interpolation * (Values.Length-1));
			var intervalInterpolation = (interpolation * (Values.Length-1)) - intervalLeft;
			return Values[intervalLeft].Lerp(Values[intervalLeft+1], intervalInterpolation);
		}

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

		private void EnsureValuesInitialized()
		{
			if (Values == null)
				Values = new T[2];
		}

		public void SetValue(int index, T value)
		{
			if(index >= Values.Length)
				AddValueAfter(Values.Length, value);
			else if(index < 0)
				AddValueBefore(0, value);
			else
				Values[index] = value;
		}

		public void AddValueAfter(int leftIndex, T value)
		{
			if (leftIndex < 0)
			{
				AddValueBefore(0, value);
				return;
			}
			var insertIndex = leftIndex >= Values.Length ? Values.Length : leftIndex + 1;
			ExpandAndAddValue(insertIndex, value);
		}

		public void AddValueBefore(int rightIndex, T value)
		{
			if (rightIndex >= Values.Length)
			{
				AddValueAfter(Values.Length, value);
				return;
			}
			var insertIndex = rightIndex < 0 ? 0 : rightIndex;
			ExpandAndAddValue(insertIndex, value);
		}

		private void ExpandAndAddValue(int insertIndex, T value)
		{
			var valueBuffer = Values;
			var newLength = Values.Length + 1;
			Values = new T[newLength];
			for (int i = 0; i < newLength; i++)
			{
				if (i == insertIndex)
					Values[i] = value;
				else if (i > insertIndex)
					Values[i] = valueBuffer[i - 1];
				else
					Values[i] = valueBuffer[i];
			}
		}

		[Pure]
		public override string ToString()
		{
			string stringOfValues = GetType().Name + "<"+ typeof(T).Name + ">{" +Start;
			for (int i = 0; i < Values.Length - 1; i++)
			{
				stringOfValues += ", " + Values[i];
			}
			stringOfValues += "}";
			return stringOfValues;
		}
	}
}