using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

		public void Train(string spamPath, string hamPath)
		{
			Console.WriteLine(@"加载中...");
			var utf8 = new UTF8Encoding(false);
			var spams = File.ReadAllLines(spamPath, utf8);
			var hams = File.ReadAllLines(hamPath, utf8);

			Console.WriteLine(@"加载完成");
			Console.WriteLine(@"训练中...");

			var stopwatch = new Stopwatch();
			stopwatch.Start();


			//Parallel.For(0, 40000, i =>
			//{
			//	SpamFound(Corpus.OfText(spams[i]));
			//});
			Parallel.ForEach(spams, spamText =>
			{
				SpamFound(Corpus.OfText(spamText));
			});
			Parallel.For(0, 4000, i =>
			{
				HamFound(Corpus.OfText(hams[i]));
			});

			//Parallel.ForEach(hams, hamText =>
			//{
			//	HamFound(Corpus.OfText(hamText));
			//});

			stopwatch.Stop();
			Console.WriteLine($@"耗时：{stopwatch.Elapsed}");

			Console.WriteLine($@"训练完成");
			Console.WriteLine($@"垃圾短信：{40000}条，非垃圾短信：{4000}条");
			Console.WriteLine($@"垃圾词:{_spamCorpus.Count}，非垃圾词：{_hamCorpus.Count}");
		}
	}
}