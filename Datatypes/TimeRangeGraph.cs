using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DeltaEngine.Datatypes
{
	public class TimeRangeGraph<T> : BaseRangeGraph<T>
		where T : Lerp<T>
	{
		public TimeRangeGraph()
		{
			SetDefaultInterpolations(2);
		}

		public TimeRangeGraph(T minimum, T maximum)
			: base(minimum, maximum)
		{
			SetDefaultInterpolations(2);
		}

		public TimeRangeGraph(List<T> interpolationValues)
			: base(interpolationValues)
		{
			SetDefaultInterpolations(interpolationValues.Count);
		}

		private void SetDefaultInterpolations(int number)
		{
			Percentages = new float[number];
			float intervalPercentage = 1.0f / (number - 1);
			for (int i = 0; i < number; i++)
				Percentages[i] = i * intervalPercentage;
		}

		public float[] Percentages { get; private set; }

		public override void SetValue(int index, T value)
		{
			if (index >= Values.Length || index < 0)
				return;
			Values[index] = value;
		}

		public class PercentageOutsideScope : Exception {}

		public void SetValueAt(float totalPercentage, T value)
		{
			for (int i = 0; i < Percentages.Length; i++)
				if (Math.Abs(Percentages[i] - totalPercentage) < 0.01f)
				{
					SetValue(i, value);
					return;
				}
			AddValueAt(totalPercentage, value);
		}

		public override T GetInterpolatedValue(float interpolation)
		{
			if (interpolation >= 1.0f)
				return Values[Values.Length - 1];
			if (interpolation <= 0.0f)
				return Values[0];

			var intervalLeft = GetIntervalLeftForInterpolation(interpolation);
			var localInterpolation = GetInterpolationInInterval(intervalLeft, interpolation);
			return Values[intervalLeft].Lerp(Values[intervalLeft + 1], localInterpolation);
		}

		private int GetIntervalLeftForInterpolation(float interpolation)
		{
			for (int i = 0; i < Percentages.Length - 1; i++)
				if (Percentages[i] < interpolation && interpolation < Percentages[i + 1])
					return i;
			return Percentages.Length - 2;
		}

		private float GetInterpolationInInterval(int leftIndex, float totalInterpolation)
		{
			return (totalInterpolation - Percentages[leftIndex]) /
				(Percentages[leftIndex + 1] - Percentages[leftIndex]);
		}

		public bool TrySetPercentageNoOrderChange(int index, float percentage)
		{
			if (index >= Percentages.Length - 1 || index <= 0)
				return false;
			if (percentage <= Percentages[index - 1] || percentage >= Percentages[index + 1])
				return false;
			Percentages[index] = percentage;
			return true;
		}

		public bool TrySetAllPercentagesNoOrderChange(List<float> percentages)
		{
			if (percentages.Count != Percentages.Length)
				return false;
			for (int i = 0; i < percentages.Count - 1; i++)
				if (percentages[i] >= percentages[i + 1])
					return false;
			percentages[0] = 0.0f;
			percentages[percentages.Count - 1] = 1.0f;
			Percentages = percentages.ToArray();
			return true;
		}

		public void AddValueAt(float totalPercentage, T value)
		{
			var insertIndex = FindInsertIndex(totalPercentage);
			if (insertIndex == -1)
				throw new PercentageOutsideScope();
			ExpandAndAddValue(insertIndex, value);
			ExpandAndAddPercentage(insertIndex, totalPercentage);
		}

		private int FindInsertIndex(float insertPercentage)
		{
			for (int left = 0; left < Percentages.Length - 1; left++)
				if (Percentages[left] <= insertPercentage && insertPercentage <= Percentages[left + 1])
					return left + 1;
			return -1;
		}

		private void ExpandAndAddPercentage(int insertIndex, float value)
		{
			var percentageBuffer = Percentages;
			var newLength = Percentages.Length + 1;
			Percentages = new float[newLength];
			for (int i = 0; i < newLength; i++)
				if (i == insertIndex)
					Percentages[i] = value;
				else if (i > insertIndex)
					Percentages[i] = percentageBuffer[i - 1];
				else
					Percentages[i] = percentageBuffer[i];
		}
		
		[Pure]
		public override string ToString()
		{
			string stringOfValues = "{" + Percentages[0] + ": " + Start;
			for (int i = 1; i < Values.Length; i++)
				stringOfValues += ", " + Percentages[i] + ": " + Values[i];
			stringOfValues += "}";
			return stringOfValues;
		}
	}
}