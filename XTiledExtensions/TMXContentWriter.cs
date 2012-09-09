using System;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XTiled {
    [ContentTypeWriter]
    public class TMXContentWriter : ContentTypeWriter<Map> {
        protected override void Write(ContentWriter output, Map value) {

            output.Write(value.Orientation == MapOrientation.Orthogonal);
            output.Write(value.Width);
            output.Write(value.Height);
            output.Write(value.TileHeight);
            output.Write(value.TileWidth);
            output.Write(value.Bounds.X);
            output.Write(value.Bounds.Y);
            output.Write(value.Bounds.Height);
            output.Write(value.Bounds.Width);
            output.Write(value.LoadTextures);

            output.Write(value.Tilesets.Length);
            foreach (var ts in value.Tilesets) {
                output.Write(ts.ImageFileName);
                output.Write(ts.ImageHeight);
                output.Write(ts.ImageWidth);                
                output.Write(ts.Margin);
                output.Write(ts.Name);
                output.Write(ts.Spacing);
                output.Write(ts.TileHeight);
                output.Write(ts.TileOffsetX);
                output.Write(ts.TileOffsetY);
                output.Write(ts.TileWidth);

                output.Write(ts.ImageTransparentColor.HasValue);
                if (ts.ImageTransparentColor.HasValue)
                    output.Write(ts.ImageTransparentColor.Value.A);

                output.Write(ts.Tiles.Length);
                foreach (var t in ts.Tiles) {
                    output.Write(t.TilesetID);
                    output.Write(t.Origin.X);
                    output.Write(t.Origin.Y);
                    output.Write(t.Source.X);
                    output.Write(t.Source.Y);
                    output.Write(t.Source.Height);
                    output.Write(t.Source.Width);

                    output.Write(t.Properties.Count);
                    foreach (var kv in t.Properties) {
                        output.Write(kv.Key);
                        output.Write(kv.Value.Value);
                    }
                }

                output.Write(ts.Properties.Count);
                foreach (var kv in ts.Properties) {
                    output.Write(kv.Key);
                    output.Write(kv.Value.Value);
                }
            }

            output.Write(value.Properties.Count);
            foreach (var kv in value.Properties) {
                output.Write(kv.Key);
                output.Write(kv.Value.Value);
            }

            output.Write(value.TileLayers.Count);
            foreach (var l in value.TileLayers) {
                output.Write(l.Name);
                output.Write(l.Opacity);
                output.Write(l.OpacityColor.A);
                output.Write(l.Visible);
                
                output.Write(l.Properties.Count);
                foreach (var kv in l.Properties) {
                    output.Write(kv.Key);
                    output.Write(kv.Value.Value);
                }

                output.Write(l.Tiles.Length);
                foreach (var row in l.Tiles) {
                    output.Write(row.Length);
                    foreach (var t in row) {
                        output.Write(t.Rotation);
                        output.Write(t.SourceID);
                        output.Write(t.Target.X);
                        output.Write(t.Target.Y);
                        output.Write(t.Target.Height);
                        output.Write(t.Target.Width);
                        output.Write(t.Effects.HasFlag(SpriteEffects.FlipHorizontally));
                        output.Write(t.Effects.HasFlag(SpriteEffects.FlipVertically));
                    }
                }
            }

            //public ObjectLayerList ObjectLayers;
            //public LayerInfo[] LayerOrder;
            //public Tile[] Tiles;
        
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform) {
            return "FuncWorks.XNA.XTiled.MapReader, FuncWorks.XNA.XTiled";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform) {
            return "FuncWorks.XNA.XTiled.Map, FuncWorks.XNA.XTiled";
        }

    }
}
