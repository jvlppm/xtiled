using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    public class MapObject {
        public String Name;
        public String Type;
        public Rectangle Bounds;
        public Int32? TileID;
        public Boolean Visible;
        public Dictionary<String, Property> Properties;
        public Point[] Polygon;
        public Point[] Polyline;
    }
}
