using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    public sealed class MapReader : ContentTypeReader<Map> {
        protected override Map Read(ContentReader input, Map existingInstance) {
            Map m = new Map();
            Int32 props = 0;

            m.Orientation = input.ReadBoolean() ? MapOrientation.Orthogonal : MapOrientation.Isometric;
            m.Width = input.ReadInt32();
            m.Height = input.ReadInt32();
            m.TileHeight = input.ReadInt32();
            m.TileWidth = input.ReadInt32();
            m.Bounds.X = input.ReadInt32();
            m.Bounds.Y = input.ReadInt32();
            m.Bounds.Height = input.ReadInt32();
            m.Bounds.Width = input.ReadInt32();
            m.LoadTextures = input.ReadBoolean();

            m.Tilesets = new Tileset[input.ReadInt32()];
            for (int i = 0; i < m.Tilesets.Length; i++) {
                m.Tilesets[i] = new Tileset();
                m.Tilesets[i].ImageFileName = input.ReadString();
                m.Tilesets[i].ImageHeight = input.ReadInt32();
                m.Tilesets[i].ImageWidth = input.ReadInt32();
                m.Tilesets[i].Margin = input.ReadInt32();
                m.Tilesets[i].Name = input.ReadString();
                m.Tilesets[i].Spacing = input.ReadInt32();
                m.Tilesets[i].TileHeight = input.ReadInt32();
                m.Tilesets[i].TileOffsetX = input.ReadInt32();
                m.Tilesets[i].TileOffsetY = input.ReadInt32();
                m.Tilesets[i].TileWidth = input.ReadInt32();

                if (input.ReadBoolean()) {
                    Color c = Color.White;
                    c.A = input.ReadByte();
                    m.Tilesets[i].ImageTransparentColor = c;
                }

                m.Tilesets[i].Tiles = new Tile[input.ReadInt32()];
                for (int j = 0; j < m.Tilesets[i].Tiles.Length; j++) {
                    m.Tilesets[i].Tiles[j] = new Tile();
                    m.Tilesets[i].Tiles[j].TilesetID = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Origin.X = input.ReadSingle();
                    m.Tilesets[i].Tiles[j].Origin.Y = input.ReadSingle();
                    m.Tilesets[i].Tiles[j].Source.X = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Source.Y = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Source.Height = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Source.Width = input.ReadInt32();

                    props = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Properties = new Dictionary<String, Property>();
                    for (int p = 0; p < props; p++) {
                        m.Tilesets[i].Tiles[j].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                    }
                }

                props = input.ReadInt32();
                m.Tilesets[i].Properties = new Dictionary<String, Property>(props);
                for (int p = 0; p < props; p++) {
                    m.Tilesets[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
            }

            props = input.ReadInt32();
            m.Properties = new Dictionary<String, Property>(props);
            for (int p = 0; p < props; p++) {
                m.Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
            }

            if (m.LoadTextures) {
                for (int i = 0; i < m.Tilesets.Length; i++) {
                    m.Tilesets[i].Texture = input.ContentManager.Load<Texture2D>(String.Format("{0}/{1:00}", input.AssetName, i));
                }
            }
            return m;
        }
    }
}
