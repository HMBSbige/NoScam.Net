using nBayes;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NoScam.Net
{
	public class SpamDetector
	{
		private readonly Index _spam = Index.CreateMemoryIndex();
		private readonly Index _ham = Index.CreateMemoryIndex();

		public CategorizationResult IsSpam(string str)
		{
			return Analyzer.Categorize(Entry.FromString(str), _spam, _ham);
		}

		public void SpamFound(string str)
		{
			_spam.Add(Entry.FromString(str));
		}

		public void HamFound(string str)
		{
			_ham.Add(Entry.FromString(str));
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

			foreach (var s in spams)
			{
				_spam.Add(Entry.FromString(s));
			}

			foreach (var s in hams)
			{
				_ham.Add(Entry.FromString(s));
			}


			stopwatch.Stop();
			Console.WriteLine($@"耗时：{stopwatch.Elapsed}");

			Console.WriteLine($@"训练完成");
			Console.WriteLine($@"垃圾短信：{spams.Length}条，非垃圾短信：{hams.Length}条");
			Console.WriteLine($@"垃圾词:{_spam.EntryCount}，非垃圾词：{_ham.EntryCount}");
		}
	}
}