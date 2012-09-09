using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    public class ObjectLayer {
        public String Name;
        public Single Opacity;
        public Color OpacityColor;
        public Boolean Visible;
        public Dictionary<String, Property> Properties; public Color? Color;
        public MapObject[] MapObjects;
    }
}
