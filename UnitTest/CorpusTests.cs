using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoScam.Net;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
	[TestClass]
	public class CorpusTests
	{
		[TestMethod]
		public void SimpleCase()
		{
			var corpus = Corpus.OfText(@"JoJo，我不做人了！");

			Assert.AreEqual(corpus.Count, 5, @"JoJo/我/不/做人/了");
			Assert.IsTrue(corpus.Contains(@"jojo"), @"英文字母小写");
			Assert.IsTrue(corpus.Contains(@"了"), @"无标点符号");
		}

		[TestMethod]
		public void MoreComplex()
		{
			var corpus = Corpus.OfText(@"不愧是DIO！我们不敢做的事，他毫不在乎地做了！真是佩服，真是我们的偶像！");

			Assert.AreEqual(corpus.Count, 15, @"一个词算一个");
			Assert.IsTrue(corpus.Contains(@"毫不在乎"), @"成语");
			Assert.IsTrue(corpus.Contains(@"我们"), @"正确分词");
			Assert.AreEqual(corpus.CountOccurencesOf(@"真是"), 2, @"单词出现次数");

			Assert.AreEqual(corpus.CountAllOccurences(), 19, @"单词总数：不愧/是/dio/我们/不敢/做/的/事/他/毫不在乎/地/做/了/真是/佩服/真是/我们/的/偶像");
		}

		[TestMethod]
		public void CombiningCorpuses()
		{
			var corpus1 = Corpus.OfText(@"我真是high到不行了");
			var corpus2 = Corpus.OfText(@"不过，我拒绝");

			corpus1.Add(corpus2);

			Assert.IsTrue(corpus1.Contains(@"拒绝"));
		}

		[TestMethod]
		public void CorpusContainsOtherCorpus()
		{
			var corpus1 = Corpus.OfText(@"我真是high到不行了");
			var corpus2 = Corpus.OfText(@"我不行了");

			Assert.IsTrue(corpus1.Contains(corpus2), @"应包含其它语料");
		}

		[TestMethod]
		public void CorpusIteratesThroughEveryOccurenceOfEveryWord()
		{
			var words = new List<string>();
			var text = @"人类的赞歌就是勇气的赞歌！";
			var corpus1 = Corpus.OfText(text);
			corpus1.ForEveryOccurenceOfEachWord(words.Add);

			//CollectionAssert.AreEqual(words, new[] { @"人类", @"的", @"的", @"赞歌", @"赞歌", @"就是", @"勇气" });
			
		}
	}
}
