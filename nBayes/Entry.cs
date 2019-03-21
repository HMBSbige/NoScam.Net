using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JiebaNet.Segmenter;

namespace nBayes
{
	public abstract class Entry : IEnumerable<string>
	{
		public Entry()
		{
		}

		public static Entry FromString(string content)
		{
			return new StringEntry(content);
		}

		public abstract IEnumerator<string> GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private class StringEntry : Entry
		{
			private IEnumerable<string> tokens;

			public StringEntry(string stringcontent)
			{
				tokens = Parse(stringcontent);
			}

			public override IEnumerator<string> GetEnumerator()
			{
				return tokens.GetEnumerator();
			}

			/// <summary>
			/// Tokenizes a string
			/// </summary>
			private static IEnumerable<string> Parse(string source)
			{
				var segmenter = new JiebaSegmenter();
				var segments = segmenter.Cut(source);

				return from segment in segments
					   where !string.IsNullOrWhiteSpace(segment) && !char.IsPunctuation(segment[0])
					   select segment.ToLowerInvariant();
			}

			/// <summary>
			/// Replace invalid characters with spaces.
			/// </summary>
			private static string CleanInput(string strIn)
			{
				return Regex.Replace(strIn, @"[^\w\'@-]", " ");
			}
		}

	}
}
