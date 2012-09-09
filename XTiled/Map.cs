using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    /// <summary />
    public enum MapOrientation {
        /// <summary />
        Orthogonal,
        /// <summary />
        Isometric
    }

    /// <summary />
    public enum LayerType {
        /// <summary />
        TileLayer,
        /// <summary />
        ObjectLayer
    }

    /// <summary>
    /// References either a TileLayer or ObjectLayer
    /// </summary>
    public struct LayerInfo {
        /// <summary>
        /// The Layer Index/ID
        /// </summary>
        public Int32 ID;
        /// <summary>
        /// They type of layer
        /// </summary>
        public LayerType LayerType;
    }

    /// <summary>
    /// An XTiled TMX Map
    /// </summary>
    public class Map {
        /// <summary>
        /// Orientation of the map.
        /// </summary>
        public MapOrientation Orientation;
        /// <summary>
        /// Width, in tiles, of the map
        /// </summary>
        public Int32 Width;
        /// <summary>
        /// Height, in tiles, of the map
        /// </summary>
        public Int32 Height;
        /// <summary>
        /// Pixel width of a single tile
        /// </summary>
        public Int32 TileWidth;
        /// <summary>
        /// Pixel height of a single tile
        /// </summary>
        public Int32 TileHeight;
        /// <summary>
        /// Size of the map in pixels
        /// </summary>
        public Rectangle Bounds;
        /// <summary>
        /// Tilesets associated with this map
        /// </summary>
        public Tileset[] Tilesets;
        /// <summary>
        /// Custom properties
        /// </summary>
        public Dictionary<String, Property> Properties;
        /// <summary>
        /// Ordered collection of tile layers; first is the bottom layer
        /// </summary>
        public TileLayerList TileLayers;
        /// <summary>
        /// Ordered collection of object layers; first is the bottom layer
        /// </summary>
        public ObjectLayerList ObjectLayers;
        /// <summary>
        /// List of all source tiles from tilesets
        /// </summary>
        public Tile[] SourceTiles;
        /// <summary>
        /// True if XTiled loaded tileset textures during map load
        /// </summary>
        public Boolean LoadTextures;
        /// <summary>
        /// Order of tile and object layers combined, first is the bottom layer
        /// </summary>
        public LayerInfo[] LayerOrder;


        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="source">Region of the map in pixels to draw</param>
        /// <param name="target">Screen space to draw the map to; Height and Width should match source, will not scale the map</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle target) {
            this.Draw(spriteBatch, ref source, ref target, false);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="source">Region of the map in pixels to draw</param>
        /// <param name="target">Screen space to draw the map to; Height and Width should match source, will not scale the map</param>
        public void Draw(SpriteBatch spriteBatch, ref Rectangle source, ref Rectangle target) {
            this.Draw(spriteBatch, ref source, ref target, false);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="source">Region of the map in pixels to draw</param>
        /// <param name="target">Screen space to draw the map to; Height and Width should match source, will not scale the map</param>
        /// <param name="drawHiddenLayers">If true, draws layers regardless of TileLayer.Visible flag</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle source, Rectangle target, Boolean drawHiddenLayers) {
            this.Draw(spriteBatch, ref source, ref target, drawHiddenLayers);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="source">Region of the map in pixels to draw</param>
        /// <param name="target">Screen space to draw the map to; Height and Width should match source, will not scale the map</param>
        /// <param name="drawHiddenLayers">If true, draws layers regardless of TileLayer.Visible flag</param>
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

        /// <summary>
        /// Draws given tile layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="layerID">Index of the layer to draw in the Map.TileLayers collection</param>
        /// <param name="source">Region of the map in pixels to draw</param>
        /// <param name="target">Screen space to draw the map to; Height and Width should match source, will not scale the map</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawLayer(SpriteBatch spriteBatch, Int32 layerID, Rectangle source, Rectangle target, Single layerDepth) {
            DrawLayer(spriteBatch, layerID, ref source, ref target, layerDepth);
        }

        /// <summary>
        /// Draws given tile layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="layerID">Index of the layer to draw in the Map.TileLayers collection</param>
        /// <param name="source">Region of the map in pixels to draw</param>
        /// <param name="target">Screen space to draw the map to; Height and Width should match source, will not scale the map</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
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
