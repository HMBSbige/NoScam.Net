using Microsoft.VisualStudio.TestTools.UnitTesting;
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
			var corpus = Corpus.OfText(@"欧拉");

			Assert.IsFalse(detector.IsSpam(corpus), @"应该是正常文本");
		}

		[TestMethod]
		public void Given_text_previously_marked_as_spam()
		{
			var detector = new SpamDetector();
			var corpus = Corpus.OfText(@"无駄");

			detector.SpamFound(corpus);

			var isSpam = detector.IsSpam(corpus);

			Assert.IsTrue(isSpam, @"应该是垃圾文本");
		}

		[TestMethod]
		public void Given_text_marked_as_spam_then_marked_as_ham()
		{
			var detector = new SpamDetector();
			var spamCorpus = Corpus.OfText(@"你要说的下一句话是……");
			detector.SpamFound(spamCorpus);
			detector.HamFound(spamCorpus);

			Assert.IsFalse(detector.IsSpam(spamCorpus), @"应该是正常文本");
		}

		[TestMethod]
		public void Given_text_that_has_known_ham_and_unknown_words()
		{
			var detector = new SpamDetector();
			var ham = Corpus.OfText(@"我在短暂的人生里发现...一个人越是玩弄阴谋，就越会感到人类的能力是有极限的....");
			var mysteryMeat = Corpus.OfText(@"欧拉欧拉欧拉欧拉欧拉欧拉欧拉");

			detector.HamFound(ham);

			var result = detector.IsSpam(mysteryMeat);

			Assert.IsFalse(result, @"应该是正常文本");
		}

		[TestMethod]
		public void Given_text_that_has_an_equivalent_number_of_ham_and_spam()
		{
			var detector = new SpamDetector();
			var ham = Corpus.OfText(@"不愧是DIO！");
			var spam = Corpus.OfText(@"不过，我拒绝");

			detector.HamFound(ham);
			detector.SpamFound(spam);

			var result = detector.IsSpam(Corpus.OfText(@"不愧是DIO！不过，我拒绝"));

			Assert.IsFalse(result, @"应该是正常文本");
		}

		[TestMethod]
		public void Given_a_corpus_that_is_heavily_weighted_towards_being_spam()
		{
			var ham = Corpus.OfText(@"做人了");
			var spam = Corpus.OfText(@"我不做人了！");

			var detector = new SpamDetector();
			detector.HamFound(ham);
			detector.SpamFound(spam);

			var result = detector.IsSpam(Corpus.OfText(@"我不做人了！"));

			Assert.IsTrue(result, @"应该是垃圾文本");
		}

		[TestMethod]
		public void Given_a_corpus_that_is_half_spam_and_half_unknown()
		{
			var detector = new SpamDetector();
			detector.SpamFound(Corpus.OfText(@"做人了"));

			var result = detector.IsSpam(Corpus.OfText(@"我不做人了！"));

			Assert.IsTrue(result, @"无所谓");
		}
	}
}