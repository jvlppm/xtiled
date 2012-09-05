using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FuncWorks.XNA.XTiled {
    public class PropertyCollection : Dictionary<String, Property>, IXmlSerializable {
        public System.Xml.Schema.XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader) {
            if (!reader.IsEmptyElement) {
                if (reader.ReadToDescendant("Property")) {
                    do {
                        this.Add(reader["name"], Property.Create(reader["value"]));
                        reader.Read();
                    }
                    while (reader.Name.Equals("Property"));
                }
                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer) {
            foreach (var key in this.Keys) {
                writer.WriteStartElement("Property");
                writer.WriteAttributeString("name", key);
                writer.WriteAttributeString("value", this[key].Value);
                writer.WriteEndElement();
            }
        }
    }
}
