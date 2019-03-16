using System.Diagnostics;

namespace NoScam.Net
{
	public class SpamDetector
	{
		private readonly Corpus _spamCorpus = new Corpus();
		private readonly Corpus _hamCorpus = new Corpus();
		private readonly Corpus _completeCorpus = new Corpus();

		public bool IsSpam(Corpus corpus)
		{
			var countAllOccurences = (double)_completeCorpus.CountAllOccurences();
			var beliefInCorpusBeingSpam = _spamCorpus.CountAllOccurences() / countAllOccurences;

			corpus.ForEveryOccurenceOfEachWord(word =>
			{
				var occurencesOfWord = _spamCorpus.CountOccurencesOf(word);
				if (occurencesOfWord == 0)
				{
					beliefInCorpusBeingSpam = beliefInCorpusBeingSpam * .4;
				}
				else
				{
					var beliefInWordOccuringAtAll = _completeCorpus.CountOccurencesOf(word) / countAllOccurences;
					var beliefInEvidenceAndSpam = _spamCorpus.CountOccurencesOf(word) / countAllOccurences;

					beliefInCorpusBeingSpam = beliefInCorpusBeingSpam * beliefInEvidenceAndSpam / beliefInWordOccuringAtAll;
				}
			});

			Debug.WriteLine($@"Belief: {beliefInCorpusBeingSpam}");
			return beliefInCorpusBeingSpam > .16;
		}

		public void SpamFound(Corpus corpus)
		{
			_spamCorpus.Add(corpus);
			_completeCorpus.Add(corpus);
		}

		public void HamFound(Corpus corpus)
		{
			_hamCorpus.Add(corpus);
			_completeCorpus.Add(corpus);
		}
	}
}