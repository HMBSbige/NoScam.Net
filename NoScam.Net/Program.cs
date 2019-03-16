using JiebaNet.Segmenter;
using System;

namespace NoScam.Net
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var segmenter = new JiebaSegmenter();
			//var segments = segmenter.Cut("万一有突发情况");
			var segments = segmenter.Cut("实习狗好累");
			//var segments = segmenter.Cut("一款你习惯了就放不下手的手机");
			Console.WriteLine($@"{string.Join("/", segments)}");
			Console.ReadLine();
		}
	}
}
