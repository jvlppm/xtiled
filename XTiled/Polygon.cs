using System;
using Microsoft.Xna.Framework;

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
    }
}
