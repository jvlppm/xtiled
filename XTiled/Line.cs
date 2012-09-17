using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// A list of two points that define a line
    /// </summary>
    public struct Line {
        /// <summary>
        /// The starting point of the line
        /// </summary>
        public Point Start;
        /// <summary>
        /// The ending point of the line
        /// </summary>
        public Point End;
        /// <summary>
        /// Length of the line
        /// </summary>
        public float Length;
        /// <summary>
        /// Rotation of the line, suitable for using with SpriteBatch
        /// </summary>
        public float Angle;

        /// <summary>
        /// Create a line from start and end points and calculate the length and angle
        /// </summary>
        /// <param name="start">The first point of the line</param>
        /// <param name="end">The end of the line</param>
        /// <returns>A Line created from the points</returns>
        public static Line FromPoints(Point start, Point end) {
            Line l = new Line();
            l.Start = start;
            l.End = end;
            l.Length = Convert.ToSingle(Math.Sqrt(Math.Pow(Math.Abs(start.X - end.X), 2) + Math.Pow(Math.Abs(start.Y - end.Y), 2)));
            l.Angle =  Convert.ToSingle(Math.Atan2(end.Y - start.Y, end.X - start.X));
            return l;
        }

        /// <summary>
        /// Draws a Line
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="line">The Line to draw</param>
        /// <param name="region">Region of the map in pixels to draw</param> 
        /// <param name="texture">A texture to use in drawing the line</param>
        /// <param name="lineWidth">The width of the line in pixels</param>
        /// <param name="color">The color value to apply to the given texture</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public static void Draw(SpriteBatch spriteBatch, Line line, Rectangle region, Texture2D texture, Single lineWidth, Color color, Single layerDepth) {
            Point start = Map.Translate(line.Start, region);
            spriteBatch.Draw(texture, new Vector2(start.X, start.Y), null, color, line.Angle, Vector2.Zero, new Vector2(line.Length, lineWidth), SpriteEffects.None, layerDepth);
        }
    }
}
