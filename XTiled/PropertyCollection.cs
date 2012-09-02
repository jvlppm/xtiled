using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;

namespace FuncWorks.XNA.XTiled {
    public class PropertyCollection : Dictionary<String, String>, IXmlSerializable {
        public System.Xml.Schema.XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader) {
            if (reader.MoveToContent() == XmlNodeType.Element) {
                while (reader.ReadToDescendant("Property")) {
                    this.Add(reader["name"], reader["value"]);
                }
            }
            reader.Read();
        }

        public void WriteXml(System.Xml.XmlWriter writer) {
            foreach (var key in this.Keys) {
                writer.WriteStartElement("Property");
                writer.WriteAttributeString("name", key);
                writer.WriteAttributeString("value", this[key]);
                writer.WriteEndElement();
            }
        }
    }
}
