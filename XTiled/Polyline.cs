using System;
using Microsoft.Xna.Framework;

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
    }
}
