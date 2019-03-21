namespace nBayes
{
	public class Analyzer
	{
		public Analyzer()
		{
		}

		protected const float Tolerance = .05f;

		public static CategorizationResult Categorize(Entry item, Index first, Index second)
		{
			var prediction = GetPrediction(item, first, second);

			//Debug.WriteLine(prediction);

			if (prediction <= .5f - Tolerance)
			{
				return CategorizationResult.Second;
			}

			if (prediction >= .5 + Tolerance)
			{
				return CategorizationResult.First;
			}

			return CategorizationResult.Undetermined;
		}

		public static float GetPrediction(Entry item, Index first, Index second)
		{
			float I = 0;

			float invI = 0;

			foreach (var token in item)
			{
				var firstCount = first.GetTokenCount(token);
				var secondCount = second.GetTokenCount(token);

				var probability = CalcProbability(firstCount, first.EntryCount, secondCount, second.EntryCount);
				LogProbability(probability, ref I, ref invI);

				//Debug.WriteLine($@"{token}: [{probability}] ({firstCount}-{_first.EntryCount}), ({secondCount}-{_second.EntryCount})");
			}

			return I / (I + invI);
		}

		#region Private Methods

		/// <summary>
		/// Calculates a probability value based on statistics from two categories
		/// </summary>
		private static float CalcProbability(float cat1count, float cat1total, float cat2count, float cat2total)
		{
			var bw = cat1count / cat1total;
			var gw = cat2count / cat2total;
			var pw = bw / (bw + gw);
			const float s = 1f;
			const float x = .5f;
			var n = cat1count + cat2count;
			var fw = (s * x + n * pw) / (s + n);

			return fw;
		}

		private static void LogProbability(float prob, ref float i, ref float invi)
		{
			if (!float.IsNaN(prob))
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				i = i == 0 ? prob : i * prob;
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				invi = invi == 0 ? 1 - prob : invi * (1 - prob);
			}
		}

		#endregion
	}
}
