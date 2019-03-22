using JiebaNet.Segmenter;
using System.Collections.Generic;
using System.Linq;

namespace nBayes
{
	public abstract class Entry : IEnumerable<string>
	{
		public static Entry FromString(string content)
		{
			return new StringEntry(content);
		}

		public abstract IEnumerator<string> GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class StringEntry : Entry
		{
			private readonly IEnumerable<string> _tokens;

			public StringEntry(string str)
			{
				_tokens = Parse(str);
			}

			public override IEnumerator<string> GetEnumerator()
			{
				return _tokens.GetEnumerator();
			}

			private static IEnumerable<string> Parse(string source)
			{
				var segmenter = new JiebaSegmenter();
				var segments = segmenter.Cut(source);

				return from segment in segments
					   where !string.IsNullOrWhiteSpace(segment) && !char.IsPunctuation(segment[0])
					   select segment.ToLowerInvariant();
			}
		}
	}
}
