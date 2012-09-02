using System;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace FuncWorks.XNA.XTiled {
    public sealed class MapReader : ContentTypeReader<Map>{
        protected override Map Read(ContentReader input, Map existingInstance) {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            return (Map)serializer.Deserialize(input.BaseStream);
        }
    }
}
