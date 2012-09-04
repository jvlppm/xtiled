using System;

namespace FuncWorks.XNA.XTiled {
    public enum MapOrientation {
        Orthogonal,
        Isometric
    }
    
    public enum LayerType {
        TileLayer,
        ObjectLayer
    }

    public struct LayerInfo {
        public Int32 ID;
        public LayerType LayerType;
    }
    
    public class Map {
        public Decimal Version;
        public MapOrientation Orientation;
        public Int32 Width;
        public Int32 Height;
        public Int32 TileWidth;
        public Int32 TileHeight;
        public Microsoft.Xna.Framework.Rectangle Bounds;
        public Tileset[] Tilesets;
        public PropertyCollection Properties;
        public Layer[] Layers;
        public ObjectLayer[] ObjectLayers;
        public Tile[] Tiles;
        public Boolean LoadTextures;
        public LayerInfo[] LayerOrder;
    }
}
