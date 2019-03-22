using System.Collections.Generic;
using System.Xml.Serialization;

namespace nBayes
{
	// Original code found here: http://weblogs.asp.net/pwelter34/archive/2006/05/03/444961.aspx
	[XmlRoot(@"index")]
	public class IndexTable<TKey, TValue>
		: Dictionary<TKey, TValue>, IXmlSerializable
	{
		#region IXmlSerializable Members
		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader)
		{
			var keySerializer = new XmlSerializer(typeof(TKey));
			var valueSerializer = new XmlSerializer(typeof(TValue));

			var wasEmpty = reader.IsEmptyElement;
			reader.Read();

			if (wasEmpty)
			{
				return;
			}

			while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
			{
				reader.ReadStartElement(@"entry");

				reader.ReadStartElement(@"word");
				var key = (TKey)keySerializer.Deserialize(reader);
				reader.ReadEndElement();

				reader.ReadStartElement(@"count");
				var value = (TValue)valueSerializer.Deserialize(reader);
				reader.ReadEndElement();

				Add(key, value);

				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			var keySerializer = new XmlSerializer(typeof(TKey));
			var valueSerializer = new XmlSerializer(typeof(TValue));

			foreach (var key in Keys)
			{
				writer.WriteStartElement(@"entry");

				writer.WriteStartElement(@"word");
				keySerializer.Serialize(writer, key);
				writer.WriteEndElement();

				writer.WriteStartElement(@"count");
				var value = this[key];
				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();

				writer.WriteEndElement();
			}
		}
		#endregion
	}
}