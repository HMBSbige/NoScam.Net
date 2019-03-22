using System.Linq;

namespace nBayes
{
	internal class MemoryIndex : Index
	{
		internal IndexTable<string, int> Table = new IndexTable<string, int>();

		public override int EntryCount => Table.Values.Sum();

		public override void Add(Entry document)
		{
			foreach (var token in document)
			{
				if (Table.ContainsKey(token))
				{
					++Table[token];
				}
				else
				{
					Table.Add(token, 1);
				}
			}
		}

		public override int GetTokenCount(string token)
		{
			return Table.TryGetValue(token, out var res) ? res : 0;
		}
	}
}
