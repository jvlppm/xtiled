using System;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XTiled {
    public class ObjectLayer {
        public String Name;
        public Color? Color;
        public Single Opacity;
        public Color OpacityColor;
        public Boolean Visible;
        public PropertyCollection Properties;
        public MapObject[] MapObjects;
    }
}
