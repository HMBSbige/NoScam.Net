using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoScam.Net
{
	public class Corpus
	{
		public static Corpus OfText(string text)
		{
			return new Corpus(text);
		}

		private readonly Dictionary<string, int> Words = new Dictionary<string, int>();

		public Corpus(string value)
		{
			foreach (var word in ParseTextToWords(value))
			{
				if (Words.ContainsKey(word))
				{
					++Words[word];
				}
				else
				{
					Words.Add(word, 1);
				}
			}
		}

		public Corpus()
		{

		}

		public int Count => Words.Count;

		public bool Contains(string word)
		{
			return Words.ContainsKey(word);
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
			foreach (var pair in corpus.Words)
			{
				var word = pair.Key;
				var count = pair.Value;
				if (Words.ContainsKey(word))
				{
					++Words[word];
				}
				else
				{
					Words.Add(word, count);
				}
			}
		}

		public bool Contains(Corpus corpus)
		{
			var result = true;
			foreach (var word in corpus.Words)
			{
				result &= Words.ContainsKey(word.Key);
			}
			return result;
		}

		public int CountOccurencesOf(string word)
		{
			return Words.ContainsKey(word) ? Words[word] : 0;
		}

		public int CountAllOccurences()
		{
			return Words.Sum(x => x.Value);
		}

		public void ForEveryOccurenceOfEachWord(Action<string> action)
		{
			foreach (var word in Words)
			{
				foreach (var unused in Enumerable.Range(0, word.Value))
				{
					action(word.Key);
				}
			}
		}
	}
}