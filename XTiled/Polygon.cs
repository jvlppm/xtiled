using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XTiled {
    public class Polygon {
        public Point[] Points;
        public Rectangle Bounds;

        public static Polygon FromPoints(Point[] points) {
            Polygon p = new Polygon();
            p.Points = points;
           
            p.Bounds.X = points.Min(x => x.X);
            p.Bounds.Y = points.Min(x => x.Y);
            p.Bounds.Width = points.Max(x => x.X) - points.Min(x => x.X);
            p.Bounds.Height = points.Max(x => x.Y) - points.Min(x => x.Y);
            
            return p;
        }
    }
}
