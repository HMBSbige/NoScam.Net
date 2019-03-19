using System;
using System.IO;
using System.Text;
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

			var filter = new SpamDetector();
			filter.Train(spamPath, hamPath);

			var spams = File.ReadAllLines(spamPath, utf8);
			var r = 0d;
			//Parallel.ForEach(spams, spam =>
			//{
			//	if (filter.IsSpam(Corpus.OfText(spam)))
			//	{
			//		++r;
			//	}
			//});
			Parallel.For(40001, 80000, i =>
			{
				if (filter.IsSpam(Corpus.OfText(spams[i])))
				{
					++r;
				}
			});
			Console.WriteLine($@"{r / spams.Length * 100}%");
			while (true)
			{
				var s = Console.ReadLine();
				Console.WriteLine(filter.IsSpam(new Corpus(s)));
			}
		}
	}
}
