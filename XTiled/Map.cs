using System;

namespace FuncWorks.XNA.XTiled {
    public enum MapOrientation {
        Orthogonal,
        Isometric
    }

    public class Map {
        public Decimal Version;
        public MapOrientation Orientation;
        public Int32 Width;
        public Int32 Height;
        public Int32 TileWidth;
        public Int32 TileHeight;
        public Tileset[] Tilesets;
        public PropertyCollection Properties;
        public Layer[] Layers;
        public ObjectLayer[] ObjectLayers;
    }
}
