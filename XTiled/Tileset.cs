﻿using System;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    public class Tileset {
        public Int32 FirstGID;
        public String Name;
        public Int32 TileWidth;
        public Int32 TileHeight;
        public Int32 Spacing;
        public Int32 Margin;
        public Int32 TileOffsetX;
        public Int32 TileOffsetY;
        public Image[] Images;
        public PropertyCollection Properties;
    }
}
