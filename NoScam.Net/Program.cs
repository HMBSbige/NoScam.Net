using nBayes;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoScam.Net
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var spamPath = Path.Combine(currentPath, @"Resources", @"spam.txt");
			var hamPath = Path.Combine(currentPath, @"Resources", @"ham.txt");

			var spamDetector = new SpamDetector();

			spamDetector.Train(spamPath, hamPath);

			while (true)
			{
				var s = Console.ReadLine();
				var result = spamDetector.IsSpam(s);

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
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}
