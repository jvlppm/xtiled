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

            //if (this.Orientation == MapOrientation.Orthogonal) {

            Int32 txMin = source.X / this.TileWidth;
            Int32 txMax = (source.X + source.Width) / this.TileWidth;
            Int32 tyMin = source.Y / this.TileHeight;
            Int32 tyMax = (source.Y + source.Height) / this.TileHeight;

            for (int l = 0; l < this.Layers.Length; l++) {
                if (this.Layers[l].Visible || drawHiddenLayers) {
                    for (int y = tyMin; y <= tyMax; y++) {
                        for (int x = txMin; x <= txMax; x++) {
                            Int32 i = x + (this.Width) * y;
                            if (i < this.Layers[l].Tiles.Length && this.Layers[l].Tiles[i] != null) {
                                Rectangle tileTarget = this.Layers[l].Tiles[i].Target;
                                tileTarget.X = tileTarget.X - source.X + target.X;
                                tileTarget.Y = tileTarget.Y - source.Y + target.Y;

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
            //}
        }
    }
}
