using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// A sequence of lines
    /// </summary>
    public class Polyline {
        /// <summary>
        /// The points that make up the polyline, in order
        /// </summary>
        public Point[] Points;
        /// <summary>
        /// The lines that make up the polyline, in order
        /// </summary>
        public Line[] Lines;
        /// <summary>
        /// Draws the lines that make up the Polyline
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

        public bool Intersects(ref Rectangle rect) {
            for (int i = 0; i < this.Lines.Length; i++) {
                if (this.Lines[i].Intersects(ref rect))
                    return true;
            }
            return false;
        }

        public bool Intersects(Rectangle rect) {
            return Intersects(ref rect);
        }

        public bool Intersects(ref Line line) {
            for (int i = 0; i < this.Lines.Length; i++) {
                if (this.Lines[i].Intersects(ref line))
                    return true;
            }
            return false;
        }
        public bool Intersects(Line line) {
            return Intersects(ref line);
        }
    }
}
