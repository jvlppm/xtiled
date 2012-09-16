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

        public float Length;
        public float Angle;

        public static Line FromPoints(Point start, Point end) {
            Line l = new Line();
            l.Start = start;
            l.End = end;
            l.Length = Convert.ToSingle(Math.Sqrt(Math.Pow(Math.Abs(start.X - end.X), 2) + Math.Pow(Math.Abs(start.Y - end.Y), 2)));
            l.Angle =  Convert.ToSingle(Math.Atan2(end.Y - start.Y, end.X - start.X));
            return l;
        }

        public static void Draw(SpriteBatch spriteBatch, Line line, Rectangle region, Texture2D texture, Single lineWidth, Color color, Single layerDepth) {
            Point start = Map.Translate(line.Start, region);
            spriteBatch.Draw(texture, new Vector2(start.X, start.Y), null, color, line.Angle, Vector2.Zero, new Vector2(line.Length, lineWidth), SpriteEffects.None, layerDepth);
        }
    }
}
