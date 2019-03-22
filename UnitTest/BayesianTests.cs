using Microsoft.VisualStudio.TestTools.UnitTesting;
using nBayes;
using NoScam.Net;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest
{
	[TestClass]
	public class BayesianTests
	{
		[TestMethod]
		public void LongTest()
		{
			var corpus = @"不愧是DIO！我们不敢做的事，他毫不在乎地做了！真是佩服，真是我们的偶像！";
			var currentPath = @".\";
			var spamPath = Path.Combine(currentPath, @"Resources", @"spam.txt");
			var hamPath = Path.Combine(currentPath, @"Resources", @"ham.txt");

			var spamDetector = new SpamDetector();

			spamDetector.Train(spamPath, hamPath);
			var utf8 = new UTF8Encoding(false);
			var spams = File.ReadAllLines(spamPath, utf8);
			var hams = File.ReadAllLines(hamPath, utf8);
			var r0 = 0;
			Parallel.For(72001, 80000, i =>
			{
				var result = spamDetector.IsSpam(spams[i]);
				if (result == CategorizationResult.First)
				{
					Interlocked.Increment(ref r0);
				}
			});
			var r = r0 / 8000d * 100;
			Console.WriteLine($@"{r}%");
			Assert.IsTrue(r > 0.97);
			var r1 = 0;
			Parallel.For(648001, 720000, i =>
			{
				var result = spamDetector.IsSpam(hams[i]);
				if (result != CategorizationResult.First)
				{
					Interlocked.Increment(ref r1);
				}
			});
			r = r1 / 72000d * 100;
			Console.WriteLine($@"{r}%");
			Assert.IsTrue(r > 0.97);
			Assert.AreEqual(spamDetector.IsSpam(corpus), CategorizationResult.Second);
		}

		[TestMethod]
		public void Test()
		{
			var corpus = @"不愧是DIO！我们不敢做的事，他毫不在乎地做了！真是佩服，真是我们的偶像！";
			var currentPath = @".\";
			var spamPath = Path.Combine(currentPath, @"Resources", @"spam.txt");
			var hamPath = Path.Combine(currentPath, @"Resources", @"ham.txt");

			var spamDetector = new SpamDetector();

			spamDetector.Train(spamPath, hamPath);

			Assert.AreEqual(spamDetector.IsSpam(corpus), CategorizationResult.Second);
		}
	}
}