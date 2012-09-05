using System;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XTiled {
    public class Layer {
        public String Name;
        public Single Opacity;
        public Color OpacityColor;
        public Boolean Visible;
        public PropertyCollection Properties;
        public TileData[] Tiles;
    }
}
