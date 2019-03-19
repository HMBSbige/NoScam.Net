using JiebaNet.Segmenter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoScam.Net;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitTest
{
	[TestClass]
	public class SomeTest
	{
		[TestMethod]
		public void TestWrongCorpus()
		{
			var segmenter = new JiebaSegmenter();
			var s1 = segmenter.Cut(@"万一有突发情况");
			var s2 = segmenter.Cut(@"实习狗好累");
			var s3 = segmenter.Cut(@"一款你习惯了就放不下手的手机");

			Assert.IsFalse(s1.Contains(@"发情"));
			Assert.IsFalse(s2.Contains(@"习狗"));
			Assert.IsFalse(s3.Contains(@"你习"));
		}

		[TestMethod]
		public void TestConcurrent()
		{
			var t = new ConcurrentDictionary<string, int>();
			t.TryAdd(@"a", 1);
			t.AddOrUpdate(@"a", 1, (s, i) => ++i);
			Assert.AreEqual(t[@"a"], 2);
			t.GetOrAdd(@"a", 0);
			Assert.AreEqual(t[@"a"], 2);
		}
	}
}
