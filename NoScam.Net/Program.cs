using nBayes;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoScam.Net
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var spamPath = @"D:\Cloud\Git\NoScam.Net\NoScam.Net\Resources\spam.txt";
			var hamPath = @"D:\Cloud\Git\NoScam.Net\NoScam.Net\Resources\ham.txt";
			var utf8 = new UTF8Encoding(false);

			var spams = File.ReadAllLines(spamPath, utf8);
			var hams = File.ReadAllLines(hamPath, utf8);
			Index spam = Index.CreateMemoryIndex();
			Index notspam = Index.CreateMemoryIndex();

			// train the indexes
			Console.WriteLine(@"训练中...");

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			//Parallel.For(0, 80000, i =>
			//{
			//	spam.Add(Entry.FromString(spams[i]));
			//});
			//Parallel.For(0, 80000, i =>
			//{
			//	notspam.Add(Entry.FromString(hams[i]));
			//});

			for (int i = 0; i < 72000; i++)
			{
				spam.Add(Entry.FromString(spams[i]));
			}

			for (int i = 0; i < 648000; i++)
			{
				notspam.Add(Entry.FromString(hams[i]));
			}

			stopwatch.Stop();
			Console.WriteLine(@"训练完成");
			Console.WriteLine($@"耗时：{stopwatch.Elapsed}");

			var r = 0;

			stopwatch.Restart();
			Parallel.For(72001, 80000, i =>
			{
				var result = Analyzer.Categorize(Entry.FromString(spams[i]), spam, notspam);
				if (result == CategorizationResult.First)
				{
					Interlocked.Increment(ref r);
				}
			});
			stopwatch.Stop();


			Console.WriteLine($@"{r / 8000d * 100}%");
			Console.WriteLine($@"耗时：{stopwatch.Elapsed}");

			var r1 = 0;
			stopwatch.Restart();
			Parallel.For(648001, 720000, i =>
			{
				var result = Analyzer.Categorize(Entry.FromString(hams[i]), spam, notspam);
				if (result != CategorizationResult.First)
				{
					Interlocked.Increment(ref r1);
				}
			});

			Console.WriteLine($@"{r1 / 72000d * 100}%");
			Console.WriteLine($@"耗时：{stopwatch.Elapsed}");

			while (true)
			{
				var s = Console.ReadLine();
				var result = Analyzer.Categorize(Entry.FromString(s), spam, notspam);

				switch (result)
				{
					case CategorizationResult.First:
						Console.WriteLine(@"Spam");
						break;
					case CategorizationResult.Undetermined:
						Console.WriteLine(@"Undecided");
						break;
					case CategorizationResult.Second:
						Console.WriteLine(@"Not Spam");
						break;
				}
			}
		}
	}
}
