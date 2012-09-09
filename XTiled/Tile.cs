using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    public class Tile {
        public Int32 TilesetID;
        public Dictionary<String, Property> Properties;
        public Rectangle Source;
        public Vector2 Origin;
    }
}
