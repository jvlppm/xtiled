using System;

namespace FuncWorks.XNA.XTiled {
    public class Tileset {
        public String Name;
        public Int32 TileWidth;
        public Int32 TileHeight;
        public Int32 Spacing;
        public Int32 Margin;
        public Int32 TileOffsetX;
        public Int32 TileOffsetY;
        public PropertyCollection Properties;
        public Tile[] Tiles;
        public String ImageFileName;
        public Microsoft.Xna.Framework.Color? ImageTransparentColor;
        public Int32 ImageWidth;
        public Int32 ImageHeight;
        public Microsoft.Xna.Framework.Graphics.Texture2D Texture;
    }
}
