using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoScam.Net;

namespace UnitTest
{
	[TestClass]
	public class BayesianTests
	{
		[TestMethod]
		public void Given_a_corpus_what_are_the_odds_of_a_certain_word_being_chosen_randomly_from_within_it()
		{
			var corpus = Corpus.OfText(@"不愧是DIO！我们不敢做的事，他毫不在乎地做了！真是佩服，真是我们的偶像！");
			var belief = Bayes.BeliefOf(@"真是", corpus);

			Assert.AreEqual(belief, 2 / 19d, @"概率应该匹配");
		}
	}

	public static class Bayes
	{
		public static double BeliefOf(string word, Corpus corpus)
		{
			var occurencesOfWordInCorpus = corpus.CountOccurencesOf(word);
			var allOccurencesOfAllWords = corpus.CountAllOccurences();
			return occurencesOfWordInCorpus / (double)allOccurencesOfAllWords;
		}
	}
}