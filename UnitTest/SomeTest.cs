using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;

namespace UnitTest
{
	[TestClass]
	public class SomeTest
	{
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
