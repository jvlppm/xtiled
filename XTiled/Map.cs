using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public MapOrientation Orientation;
        public Int32 Width;
        public Int32 Height;
        public Int32 TileWidth;
        public Int32 TileHeight;
        public Rectangle Bounds;
        public Tileset[] Tilesets;
        public PropertyCollection Properties;
        public Layer[] Layers;
        public ObjectLayer[] ObjectLayers;
        public Tile[] Tiles;
        public Boolean LoadTextures;
        public LayerInfo[] LayerOrder;

        public void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle target) {
            this.Draw(spriteBatch, source, target, false);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle target, Boolean drawHiddenLayers) {

            if(this.Orientation == MapOrientation.Orthogonal) {
                for (int l = 0; l < this.Layers.Length; l++) {
                    if (this.Layers[l].Visible || drawHiddenLayers) {
                        for (int i = 0; i < this.Layers[l].Tiles.Length; i++) {
                            if (this.Layers[l].Tiles[i] != null) {
                                Rectangle tileTarget = this.Layers[l].Tiles[i].Target;
                                tileTarget.X = tileTarget.X - source.X + target.X - Convert.ToInt32(this.Tiles[this.Layers[l].Tiles[i].SourceID].Origin.X);
                                tileTarget.Y = tileTarget.Y - source.Y + target.Y - Convert.ToInt32(this.Tiles[this.Layers[l].Tiles[i].SourceID].Origin.Y);

                                if (target.Contains(tileTarget) || target.Intersects(tileTarget)) {
                                    tileTarget.X += Convert.ToInt32(this.Tiles[this.Layers[l].Tiles[i].SourceID].Origin.X);
                                    tileTarget.Y += Convert.ToInt32(this.Tiles[this.Layers[l].Tiles[i].SourceID].Origin.Y);

                                    spriteBatch.Draw(
                                        this.Tilesets[this.Tiles[this.Layers[l].Tiles[i].SourceID].TilesetID].Texture,
                                        tileTarget,
                                        this.Tiles[this.Layers[l].Tiles[i].SourceID].Source,
                                        this.Layers[l].OpacityColor,
                                        this.Layers[l].Tiles[i].Rotation,
                                        this.Tiles[this.Layers[l].Tiles[i].SourceID].Origin,
                                        this.Layers[l].Tiles[i].Effects,
                                        0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
