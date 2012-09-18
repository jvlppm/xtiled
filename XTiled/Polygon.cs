using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// A sequence of lines that form a closed shape
    /// </summary>
    public class Polygon {
        /// <summary>
        /// The points that make up the polygon, in order
        /// </summary>
        public Point[] Points;
        /// <summary>
        /// The lines that make up the polygon, in order
        /// </summary>
        public Line[] Lines;
        /// <summary>
        /// Draws the lines that make up the Polygon
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param> 
        /// <param name="texture">A texture to use in drawing the lines</param>
        /// <param name="lineWidth">The width of the lines in pixels</param>
        /// <param name="color">The color value to apply to the given texture</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle region, Texture2D texture, Single lineWidth, Color color, Single layerDepth) {
            for (int i = 0; i < Lines.Length; i++)
                Line.Draw(spriteBatch, Lines[i], region, texture, lineWidth, color, layerDepth);
        }

        public bool Contains(ref Vector2 vector) {
            bool result = false;

            // modified method from http://stackoverflow.com/questions/2379818/does-xna-have-a-polygon-like-rectangle
            // fixed bugs by following http://funplosion.com/devblog/collision-detection-line-vs-point-circle-and-rectangle.html
            foreach (var side in Lines) {
                if (vector.Y > Math.Min(side.Start.Y, side.End.Y))
                    if (vector.Y <= Math.Max(side.Start.Y, side.End.Y))
                        if (vector.X <= Math.Max(side.Start.X, side.End.X)) {
                            if (side.Start.Y != side.End.Y) {
                                float xIntersection = (vector.Y - side.Start.Y) * (side.End.X - side.Start.X) / (side.End.Y - side.Start.Y) + side.Start.X;
                                if (side.Start.X == side.End.X || vector.X <= xIntersection)
                                    result = !result;
                            }
                        }
            }

            return result;
        }

        public bool Contains(Vector2 vector) {
            return Contains(ref vector);
        }

        public bool Contains(ref Point point) {
            Vector2 v = new Vector2(point.X, point.Y);
            return Contains(ref v);
        }

        public bool Contains(Point point) {
            return Contains(ref point);
        }

        public bool Contains(ref Line line) {
            return Contains(ref line.Start) && Contains(ref line.End);
        }

        public bool Contains(Line line) {
            return Contains(ref line);
        }

        public bool Contains(ref Rectangle rect) {
            return Contains(new Point(rect.Left, rect.Top)) &&
                   Contains(new Point(rect.Left, rect.Bottom)) && 
                   Contains(new Point(rect.Right, rect.Top)) &&
                   Contains(new Point(rect.Right, rect.Bottom));
        }
        public bool Contains(Rectangle rect) {
            return Contains(ref rect);
        }

        public bool Intersects(ref Rectangle rect) {
            int pointsContained = ((Contains(new Point(rect.Left, rect.Top)) ? 1 : 0) +
                (Contains(new Point(rect.Left, rect.Bottom)) ? 1 : 0) +
                (Contains(new Point(rect.Right, rect.Top)) ? 1 : 0) +
                (Contains(new Point(rect.Right, rect.Bottom)) ? 1 : 0));

            return pointsContained > 0 && pointsContained < 4;
        }

        public bool Intersects(Rectangle rect) {
            return Intersects(ref rect);
        }

        public bool Intersects(ref Line line) {
            int pointsContained = ((Contains(line.Start) ? 1 : 0) + (Contains(line.End) ? 1 : 0));
            return pointsContained == 1;
        }
        public bool Intersects(Line line) {
            return Intersects(ref line);
        }    
    }
}
