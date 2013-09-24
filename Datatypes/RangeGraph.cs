using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DeltaEngine.Datatypes
{
	public class RangeGraph<T> : BaseRangeGraph<T>
		where T : Lerp<T>
	{
		public RangeGraph() {}

		public RangeGraph(T minimum, T maximum)
			: base(minimum, maximum) {}

		public RangeGraph(List<T> values) : base(values)
		{}

		public override T GetInterpolatedValue(float interpolation)
		{
			if (interpolation >= 1.0f)
				return Values[Values.Length - 1];
			var intervalLeft = (int)(interpolation * (Values.Length - 1));
			var intervalInterpolation = (interpolation * (Values.Length - 1)) - intervalLeft;
			return Values[intervalLeft].Lerp(Values[intervalLeft + 1], intervalInterpolation);
		}

		public override void SetValue(int index, T value)
		{
			if (index >= Values.Length)
				AddValueAfter(Values.Length, value);
			else if (index < 0)
				AddValueBefore(0, value);
			else
				Values[index] = value;
		}

		public void AddValueAfter(int leftIndex, T value, float percentageInbetween = 0.5f)
		{
			if (leftIndex < 0)
			{
				AddValueBefore(0, value);
				return;
			}
			var insertIndex = leftIndex >= Values.Length ? Values.Length : leftIndex + 1;
			ExpandAndAddValue(insertIndex, value);
		}

		public void AddValueBefore(int rightIndex, T value, float percentageInbetween = 0.5f)
		{
			if (rightIndex >= Values.Length)
			{
				AddValueAfter(Values.Length, value);
				return;
			}
			var insertIndex = rightIndex < 0 ? 0 : rightIndex;
			ExpandAndAddValue(insertIndex, value);
		}

		[Pure]
		public override string ToString()
		{
			string stringOfValues = "{" + Start;
			for (int i = 1; i < Values.Length; i++)
				stringOfValues += ", " + Values[i];
			stringOfValues += "}";
			return stringOfValues;
		}
	}
}