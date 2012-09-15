using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
    }
}
