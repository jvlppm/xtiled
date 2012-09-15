using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XTiled {
    public class Polyline {
        public Point[] Points;
        public Line[] Lines;
        public Rectangle Bounds;

        public static Polyline FromPoints(Point[] points) {
            Polyline p = new Polyline();
            p.Points = points;

            p.Bounds.X = points.Min(x => x.X);
            p.Bounds.Y = points.Min(x => x.Y);
            p.Bounds.Width = points.Max(x => x.X) - points.Min(x => x.X);
            p.Bounds.Height = points.Max(x => x.Y) - points.Min(x => x.Y);

            if (points.Length > 1) {
                p.Lines = new Line[points.Length - 1];
                for (int i = 0; i < p.Lines.Length; i++) {
                    p.Lines[i].Start = points[i];
                    p.Lines[i].End = points[i + 1];
                }
            }

            return p;
        }
    }
}
