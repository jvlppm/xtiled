using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    public class Tileset {
        public String Name;
        public Int32 TileWidth;
        public Int32 TileHeight;
        public Int32 Spacing;
        public Int32 Margin;
        public Int32 TileOffsetX;
        public Int32 TileOffsetY;
        public Dictionary<String, Property> Properties;
        public Tile[] Tiles;
        public String ImageFileName;
        public Color? ImageTransparentColor;
        public Int32 ImageWidth;
        public Int32 ImageHeight;
        public Texture2D Texture;
    }
}
