using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nBayes.Optimization
{
	/// <summary>
	/// Implementing the epsilon-greedy algorithm from:
	/// http://stevehanov.ca/blog/index.php?id=132
	/// </summary>
	public class Optimizer
	{
		private List<Option> options = new List<Option>();
		private Random random = new Random();

		public void Add(Option value)
		{
			options.Add(value);
		}

		public IEnumerable<Option> Options => options;

		public float ExplorationThreshold = 0.2f;

		public Task<Option> Choose()
		{
			return Task.Factory.StartNew(() =>
			{
				var explore = (float)random.NextDouble();

				var useTheBestOption = explore > ExplorationThreshold;

				Option optionToUse;

				if (useTheBestOption)
				{
					optionToUse = options.OrderByDescending(o => o.Value).Take(1).Single();
				}
				else
				{
					// we should experiment
					var randomIndex = (int)(random.NextDouble() * options.Count);
					optionToUse = options[randomIndex];
				}

				optionToUse.IncrementAttempt();

				return optionToUse;
			});
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var option in options.OrderByDescending(o => o.Value))
			{
				sb.Append('\t');
				sb.AppendLine(option.ToString());
			}
			return sb.ToString();
		}
	}
}

