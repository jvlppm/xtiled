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
        /// Bounding rectangle of this Polygon
        /// </summary>
        public Rectangle Bounds;

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

        /// <summary>
        /// Determines if a Vector2 is inside this Polygon
        /// </summary>
        /// <param name="vector">Vector2 to compare to</param>
        /// <returns>True if the Vector2 is inside the Polygon</returns>
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

        /// <summary>
        /// Determines if a Vector2 is inside this Polygon
        /// </summary>
        /// <param name="vector">Vector2 to compare to</param>
        /// <returns>True if the Vector2 is inside the Polygon</returns>
        public bool Contains(Vector2 vector) {
            return Contains(ref vector);
        }

        /// <summary>
        /// Determines if a Point is inside this Polygon
        /// </summary>
        /// <param name="point">Point to compare to</param>
        /// <returns>True if the Point is inside the Polygon</returns>
        public bool Contains(ref Point point) {
            Vector2 v = new Vector2(point.X, point.Y);
            return Contains(ref v);
        }

        /// <summary>
        /// Determines if a Point is inside this Polygon
        /// </summary>
        /// <param name="point">Point to compare to</param>
        /// <returns>True if the Point is inside the Polygon</returns>
        public bool Contains(Point point) {
            return Contains(ref point);
        }

        /// <summary>
        /// Determines if a Line is inside this Polygon; a Line is considered inside if both Start and End points are inside
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is inside the Polygon</returns>
        public bool Contains(ref Line line) {
            return Contains(ref line.Start) && Contains(ref line.End);
        }

        /// <summary>
        /// Determines if a Line is inside this Polygon; a Line is considered inside if both Start and End points are inside
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is inside the Polygon</returns>
        public bool Contains(Line line) {
            return Contains(ref line);
        }

        /// <summary>
        /// Determines if a Rectangle is inside this Polygon; a Rectangle is considered inside if all four corners are inside
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is inside the Polygon</returns>
        public bool Contains(ref Rectangle rect) {
            return Contains(new Vector2(rect.Left, rect.Top)) &&
                   Contains(new Vector2(rect.Left, rect.Bottom)) &&
                   Contains(new Vector2(rect.Right, rect.Top)) &&
                   Contains(new Vector2(rect.Right, rect.Bottom));
        }

        /// <summary>
        /// Determines if a Rectangle is inside this Polygon; a Rectangle is considered inside if all four corners are inside
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is inside the Polygon</returns>
        public bool Contains(Rectangle rect) {
            return Contains(ref rect);
        }


        /// <summary>
        /// Determines if a Rectangle intersects this Polygon; a Rectangle is considered to intersect if at least one corner is contained but not all four corners are contained
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is intersects the Polygon</returns>
        public bool Intersects(ref Rectangle rect) {
            int pointsContained = ((Contains(new Vector2(rect.Left, rect.Top)) ? 1 : 0) +
                (Contains(new Vector2(rect.Left, rect.Bottom)) ? 1 : 0) +
                (Contains(new Vector2(rect.Right, rect.Top)) ? 1 : 0) +
                (Contains(new Vector2(rect.Right, rect.Bottom)) ? 1 : 0));

            return pointsContained > 0 && pointsContained < 4;
        }

        /// <summary>
        /// Determines if a Rectangle intersects this Polygon; a Rectangle is considered to intersect if at least one corner is contained but not all four corners are contained
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is intersects the Polygon</returns>
        public bool Intersects(Rectangle rect) {
            return Intersects(ref rect);
        }

        /// <summary>
        /// Determines if a Line intersects this Polygon; a Line is considered to intersect if one but not both points are contained
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is intersects the Polygon</returns>
        public bool Intersects(ref Line line) {
            int pointsContained = ((Contains(line.Start) ? 1 : 0) + (Contains(line.End) ? 1 : 0));
            return pointsContained == 1;
        }

        /// <summary>
        /// Determines if a Line intersects this Polygon; a Line is considered to intersect if one but not both points are contained
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is intersects the Polygon</returns>
        public bool Intersects(Line line) {
            return Intersects(ref line);
        }
    }
}
