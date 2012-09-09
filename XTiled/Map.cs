using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
        public Dictionary<String, Property> Properties;
        public TileLayerList TileLayers;
        public ObjectLayerList ObjectLayers;
        public Tile[] SourceTiles;
        public Boolean LoadTextures;
        public LayerInfo[] LayerOrder;

        public void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle target) {
            this.Draw(spriteBatch, ref source, ref target, false);
        }

        public void Draw(SpriteBatch spriteBatch, ref Rectangle source, ref Rectangle target) {
            this.Draw(spriteBatch, ref source, ref target, false);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle target, Boolean drawHiddenLayers) {
            this.Draw(spriteBatch, ref source, ref target, drawHiddenLayers);
        }

        public void Draw(SpriteBatch spriteBatch, ref Rectangle source, ref Rectangle target, Boolean drawHiddenLayers) {

            Int32 txMin = source.X / this.TileWidth;
            Int32 txMax = (source.X + source.Width) / this.TileWidth;
            Int32 tyMin = source.Y / this.TileHeight;
            Int32 tyMax = (source.Y + source.Height) / this.TileHeight;

            if (this.Orientation == MapOrientation.Isometric) {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            for (int l = 0; l < this.TileLayers.Count; l++) {
                if (this.TileLayers[l].Visible || drawHiddenLayers) {
                    DrawLayer(spriteBatch, l, ref source, ref target, txMin, txMax, tyMin, tyMax, 0);
                }
            }
        }

        public void DrawLayer(SpriteBatch spriteBatch, Int32 layerID, Rectangle source, Rectangle target, Single layerDepth) {
            DrawLayer(spriteBatch, layerID, ref source, ref target, layerDepth);
        }

        public void DrawLayer(SpriteBatch spriteBatch, Int32 layerID, ref Rectangle source, ref Rectangle target, Single layerDepth) {
            Int32 txMin = source.X / this.TileWidth;
            Int32 txMax = (source.X + source.Width) / this.TileWidth;
            Int32 tyMin = source.Y / this.TileHeight;
            Int32 tyMax = (source.Y + source.Height) / this.TileHeight;

            if (this.Orientation == MapOrientation.Isometric) {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            DrawLayer(spriteBatch, layerID, ref source, ref target, txMin, txMax, tyMin, tyMax, layerDepth);
        }

        private void DrawLayer(SpriteBatch spriteBatch, Int32 layerID, ref Rectangle source, ref Rectangle target, Int32 txMin, Int32 txMax, Int32 tyMin, Int32 tyMax, Single layerDepth) {
            for (int y = tyMin; y <= tyMax; y++) {
                for (int x = txMin; x <= txMax; x++) {
                    if (x < this.TileLayers[layerID].Tiles.Length && y < this.TileLayers[layerID].Tiles[x].Length && this.TileLayers[layerID].Tiles[x][y] != null) {
                        Rectangle tileTarget = this.TileLayers[layerID].Tiles[x][y].Target;
                        tileTarget.X = tileTarget.X - source.X + target.X;
                        tileTarget.Y = tileTarget.Y - source.Y + target.Y;

                        spriteBatch.Draw(
                            this.Tilesets[this.SourceTiles[this.TileLayers[layerID].Tiles[x][y].SourceID].TilesetID].Texture,
                            tileTarget,
                            this.SourceTiles[this.TileLayers[layerID].Tiles[x][y].SourceID].Source,
                            this.TileLayers[layerID].OpacityColor,
                            this.TileLayers[layerID].Tiles[x][y].Rotation,
                            this.SourceTiles[this.TileLayers[layerID].Tiles[x][y].SourceID].Origin,
                            this.TileLayers[layerID].Tiles[x][y].Effects,
                            layerDepth);
                    }
                }
            }
        }
    }
}
