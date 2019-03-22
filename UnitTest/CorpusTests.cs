using Microsoft.VisualStudio.TestTools.UnitTesting;
using nBayes;
using System.Linq;

namespace UnitTest
{
	[TestClass]
	public class CorpusTests
	{
		[TestMethod]
		public void SimpleCase()
		{
			var corpus = Entry.FromString(@"JoJo，我不做人了！");

			Assert.AreEqual(corpus.ToArray().Length, 5, @"JoJo/我/不/做人/了");
			Assert.IsTrue(corpus.Contains(@"jojo"), @"英文字母小写");
			Assert.IsTrue(corpus.Contains(@"了"), @"无标点符号");
		}

		[TestMethod]
		public void MoreComplex()
		{
			var corpus = Entry.FromString(@"不愧是DIO！我们不敢做的事，他毫不在乎地做了！真是佩服，真是我们的偶像！");

			Assert.AreEqual(corpus.ToArray().Length, 19);
			Assert.IsTrue(corpus.Contains(@"毫不在乎"), @"成语");
			Assert.IsTrue(corpus.Contains(@"我们"), @"正确分词");
		}

		[TestMethod]
		public void TestWrongCorpus()
		{
			var s1 = Entry.FromString(@"万一有突发情况");
			var s2 = Entry.FromString(@"实习狗好累");
			var s3 = Entry.FromString(@"一款你习惯了就放不下手的手机");
			var s4 = Entry.FromString(@"这狗肯定是看警花好看，于是做了污点证人讨好警察，最后入住警花家里。");

			Assert.IsFalse(s1.Contains(@"发情"));
			Assert.IsFalse(s2.Contains(@"习狗"));
			Assert.IsFalse(s3.Contains(@"你习"));
			Assert.IsFalse(s4.Contains(@"后入"));
		}
	}
}
