using JiebaNet.Segmenter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoScam.Net
{
	public class Corpus
	{
		public static Corpus OfText(string text)
		{
			return new Corpus(text);
		}

		private readonly ConcurrentDictionary<string, int> _words = new ConcurrentDictionary<string, int>();

		public Corpus(string value)
		{
			Parallel.ForEach(ParseTextToWords(value), word =>
			{
				_words.AddOrUpdate(word, 1, (s, i) => ++i);
			});
		}

		public Corpus()
		{

		}

		public int Count => _words.Count;

		public bool Contains(string word)
		{
			return _words.ContainsKey(word);
		}

		private static IEnumerable<string> ParseTextToWords(string text)
		{
			var segmenter = new JiebaSegmenter();
			var segments = segmenter.Cut(text);

			return from segment in segments
				   where !string.IsNullOrWhiteSpace(segment) && !char.IsPunctuation(segment[0])
				   select segment.ToLowerInvariant();
		}

		public void Add(Corpus corpus)
		{
			Parallel.ForEach(corpus._words, pair =>
			{
				var word = pair.Key;
				var count = pair.Value;
				_words.AddOrUpdate(word, count, (s, i) => ++i);
			});
		}

		public bool Contains(Corpus corpus)
		{
			var result = true;
			Parallel.ForEach(corpus._words, word =>
			{
				result &= _words.ContainsKey(word.Key);
			});
			return result;
		}

		public int CountOccurencesOf(string word)
		{
			return _words.TryGetValue(word, out var rValue) ? rValue : 0;
		}

		public int CountAllOccurences()
		{
			return _words.Sum(x => x.Value);
		}

		public void ForEveryOccurenceOfEachWord(Action<string> action)
		{
			Parallel.ForEach(_words, word =>
			{
				Parallel.ForEach(Enumerable.Range(0, word.Value), unused =>
				{
					action(word.Key);
				});
			});
		}
	}
}