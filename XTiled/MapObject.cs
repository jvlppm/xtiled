using System;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XTiled {
    public class MapObject {
        public String Name;
        public String Type;
        public Int32 X;
        public Int32 Y;
        public Int32 Width;
        public Int32 Height;
        public Int32? TileID;
        public Boolean Visible;
        public PropertyCollection Properties;
        public Point[] Polygon;
        public Point[] Polyline;
    }
}
