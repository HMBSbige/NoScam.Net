using System.Threading;

namespace nBayes.Optimization
{
	public class Option
	{
		private int attempts = 1;
		private int successes = 1;

		public float Value => Successes / (float)Attempts;

		public int Attempts => attempts;
		public int Successes => successes;

		public void IncrementAttempt()
		{
			Interlocked.Increment(ref attempts);
		}

		public void IncrementSuccesses()
		{
			Interlocked.Increment(ref successes);
		}

		public override string ToString()
		{
			return $@"{successes} / {attempts} = {Value}";
		}

		public static Option Named(string value)
		{
			return new NamedOption(value);
		}

		private class NamedOption : Option
		{
			public NamedOption(string value)
			{
				Name = value;
			}

			public string Name { get; }

			public override string ToString()
			{
				return $@"{Name}: {base.ToString()}";
			}
		}
	}
}

