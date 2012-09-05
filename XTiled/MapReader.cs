using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XTiled {
    public sealed class MapReader : ContentTypeReader<Map>{
        protected override Map Read(ContentReader input, Map existingInstance) {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            Map m = (Map)serializer.Deserialize(input.BaseStream);
            if (m.LoadTextures) {
                for (int i = 0; i < m.Tilesets.Length; i++) {
                    m.Tilesets[i].Texture = input.ContentManager.Load<Texture2D>(String.Format("{0}/{1:00}", input.AssetName, i));
                }
            }
            return m;
        }
    }
}
