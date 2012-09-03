using System;

namespace FuncWorks.XNA.XTiled {
    public class TileData {
        public UInt32 ID;
        public Int32 SourceID;
        public Boolean FlippedHorizontally;
        public Boolean FlippedVertically;
        public Boolean FlippedDiagonally;
        public Microsoft.Xna.Framework.Graphics.SpriteEffects Effects;
        public Microsoft.Xna.Framework.Rectangle Target;
        public float Rotation;
    }
}
