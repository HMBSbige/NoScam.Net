using Microsoft.VisualStudio.TestTools.UnitTesting;
using nBayes;
using NoScam.Net;

namespace UnitTest
{
	[TestClass]
	public class SpamDetectorTests
	{
		[TestMethod]
		public void Given_never_before_seen_text()
		{
			var detector = new SpamDetector();
			var corpus = @"欧拉";

			Assert.AreEqual(detector.IsSpam(corpus), CategorizationResult.Undetermined);
		}

		[TestMethod]
		public void Given_text_previously_marked_as_spam()
		{
			var detector = new SpamDetector();
			var corpus = @"无駄";
			var ham = @"我在短暂的人生里发现...一个人越是玩弄阴谋，就越会感到人类的能力是有极限的....";

			detector.SpamFound(corpus);
			detector.HamFound(ham);

			var isSpam = detector.IsSpam(corpus);

			Assert.AreEqual(isSpam, CategorizationResult.First);
		}

		[TestMethod]
		public void Given_text_marked_as_spam_then_marked_as_ham()
		{
			var detector = new SpamDetector();
			var spamCorpus = @"你要说的下一句话是……";
			detector.SpamFound(spamCorpus);
			detector.HamFound(spamCorpus);

			Assert.AreEqual(detector.IsSpam(spamCorpus), CategorizationResult.Undetermined);
		}

		[TestMethod]
		public void Given_text_that_has_known_ham_and_unknown_words()
		{
			var detector = new SpamDetector();
			var ham = @"我在短暂的人生里发现...一个人越是玩弄阴谋，就越会感到人类的能力是有极限的....";
			var mysteryMeat = @"欧拉欧拉欧拉欧拉欧拉欧拉欧拉";

			detector.HamFound(ham);

			var result = detector.IsSpam(mysteryMeat);

			Assert.AreEqual(result, CategorizationResult.Undetermined);
		}

		[TestMethod]
		public void Given_text_that_has_an_equivalent_number_of_ham_and_spam()
		{
			var detector = new SpamDetector();
			var ham = @"不愧是DIO！";
			var spam = @"不过，我拒绝";

			detector.HamFound(ham);
			detector.SpamFound(spam);

			var result = detector.IsSpam(@"不愧是DIO！不过，我拒绝");

			Assert.AreEqual(result, CategorizationResult.Undetermined);
		}

		[TestMethod]
		public void Given_a_corpus_that_is_heavily_weighted_towards_being_spam()
		{
			var ham = @"做人了";
			var spam = @"我不做人了！";

			var detector = new SpamDetector();
			detector.HamFound(ham);
			detector.SpamFound(spam);

			var result = detector.IsSpam(@"我不做人了！");

			Assert.AreEqual(result, CategorizationResult.First);
		}

		[TestMethod]
		public void Given_a_corpus_that_is_half_spam_and_half_unknown()
		{
			var detector = new SpamDetector();
			detector.SpamFound(@"做人了");

			var result = detector.IsSpam(@"我不做人了！");

			Assert.AreEqual(result, CategorizationResult.Undetermined);
		}
	}
}