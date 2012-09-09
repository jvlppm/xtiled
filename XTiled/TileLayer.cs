using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    public class TileLayer {
        public String Name;
        public Single Opacity;
        public Color OpacityColor;
        public Boolean Visible;
        public Dictionary<String, Property> Properties; public TileData[][] Tiles;
    }
}
