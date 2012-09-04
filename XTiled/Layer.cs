using System;

namespace FuncWorks.XNA.XTiled {
    public class Layer {
        public String Name;
        public Single Opacity;
        public Microsoft.Xna.Framework.Color OpacityColor;
        public Boolean Visible;
        public PropertyCollection Properties;
        public TileData[] Tiles;
    }
}
