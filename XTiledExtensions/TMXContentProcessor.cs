using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.IO.Compression;

namespace FuncWorks.XNA.XTiled {
    [ContentProcessor(DisplayName = "TMX Map - XTiled")]
    public class TMXContentProcessor : ContentProcessor<XDocument, Map> {

        private const UInt32 FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        private const UInt32 FLIPPED_VERTICALLY_FLAG = 0x40000000;
        private const UInt32 FLIPPED_DIAGONALLY_FLAG = 0x20000000;

        public override Map Process(XDocument input, ContentProcessorContext context) {
            Map map = new Map();

            map.Version = Convert.ToDecimal(input.Document.Root.Attribute("version").Value);
            if (map.Version != 1.0M)
                throw new NotSupportedException("XTiled only supports TMX maps version 1.0");

            switch (input.Document.Root.Attribute("orientation").Value) {
                case "orthogonal":
                    map.Orientation = MapOrientation.Orthogonal;
                    break;

                case "isometric":
                    map.Orientation = MapOrientation.Isometric;
                    break;

                default:
                    throw new NotSupportedException("XTiled supports only orthogonal or isometric maps");
            }

            map.Width = Convert.ToInt32(input.Document.Root.Attribute("width").Value);
            map.Height = Convert.ToInt32(input.Document.Root.Attribute("height").Value);
            map.TileWidth = Convert.ToInt32(input.Document.Root.Attribute("tilewidth").Value);
            map.TileHeight = Convert.ToInt32(input.Document.Root.Attribute("tileheight").Value);

            map.Properties = new PropertyCollection();
            if (input.Document.Root.Element("properties") != null)
                foreach (var pElem in input.Document.Root.Element("properties").Elements("property"))
                    map.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

            List<Tileset> tilesets = new List<Tileset>();
            foreach (var elem in input.Document.Root.Elements("tileset")) {
                Tileset t = new Tileset();
                XElement tElem = elem;
                t.FirstGID = Convert.ToUInt32(tElem.Attribute("firstgid").Value);

                if (elem.Attribute("source") != null) {
                    XDocument tsx = XDocument.Load(elem.Attribute("source").Value);
                    tElem = tsx.Root;
                }

                t.Name = tElem.Attribute("name") == null ? null : tElem.Attribute("name").Value;
                t.TileWidth = tElem.Attribute("tilewidth") == null ? 0 : Convert.ToInt32(tElem.Attribute("tilewidth").Value);
                t.TileHeight = tElem.Attribute("tileheight") == null ? 0 : Convert.ToInt32(tElem.Attribute("tileheight").Value);
                t.Spacing = tElem.Attribute("spacing") == null ? 0 : Convert.ToInt32(tElem.Attribute("spacing").Value);
                t.Margin = tElem.Attribute("margin") == null ? 0 : Convert.ToInt32(tElem.Attribute("margin").Value);

                if (tElem.Element("tileoffset") != null) {
                    t.TileOffsetX = Convert.ToInt32(tElem.Element("tileoffset").Attribute("x").Value);
                    t.TileOffsetY = Convert.ToInt32(tElem.Element("tileoffset").Attribute("y").Value);
                }
                else {
                    t.TileOffsetX = 0;
                    t.TileOffsetY = 0;
                }

                List<Image> images = new List<Image>();
                foreach (var imgElem in tElem.Elements("image")) {
                    Image img = new Image();
                    img.Source = imgElem.Attribute("source").Value;
                    img.Width = imgElem.Attribute("width") == null ? null : (Int32?)Convert.ToInt32(imgElem.Attribute("width").Value);
                    img.Height = imgElem.Attribute("height") == null ? null : (Int32?)Convert.ToInt32(imgElem.Attribute("height").Value);
                    img.TransparentColor = null;
                    if (imgElem.Attribute("trans") != null) {
                        System.Drawing.Color sdc = System.Drawing.ColorTranslator.FromHtml("#" + imgElem.Attribute("trans").Value.TrimStart('#'));
                        img.TransparentColor = new Color(sdc.R, sdc.G, sdc.B);
                    }
                    images.Add(img);
                }
                t.Images = images.ToArray();
                t.Properties = new PropertyCollection();
                if (tElem.Element("properties") != null)
                    foreach (var pElem in tElem.Element("properties").Elements("property"))
                        t.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                List<Tile> tiles = new List<Tile>();
                foreach (var tileElem in tElem.Elements("tile")) {
                    Tile tile = new Tile();
                    tile.ID = Convert.ToUInt32(tileElem.Attribute("id").Value);
                    tile.Properties = new PropertyCollection();
                    if (tileElem.Element("properties") != null)
                        foreach (var pElem in tileElem.Element("properties").Elements("property"))
                            tile.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));
                    tiles.Add(tile);
                }
                t.Tiles = tiles.ToArray();

                tilesets.Add(t);
            }
            map.Tilesets = tilesets.ToArray();

            List<Layer> layers = new List<Layer>();
            foreach (var lElem in input.Document.Root.Elements("layer")) {
                Layer l = new Layer();
                l.Name = lElem.Attribute("name") == null ? null : lElem.Attribute("name").Value;
                l.Opacity = lElem.Attribute("opacity") == null ? 1.0f : Convert.ToSingle(lElem.Attribute("opacity").Value);
                l.Visible = lElem.Attribute("visible") == null ? true : lElem.Attribute("visible").Equals("1");

                l.Properties = new PropertyCollection();
                if (lElem.Element("properties") != null)
                    foreach (var pElem in lElem.Element("properties").Elements("property"))
                        l.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                List<TileData> tiles = new List<TileData>();
                if (lElem.Element("data") != null) {
                    List<UInt32> gids = new List<UInt32>();
                    if (lElem.Element("data").Attribute("encoding") != null || lElem.Element("data").Attribute("compression") != null) {

                        // parse csv formatted data
                        if (lElem.Element("data").Attribute("encoding") != null && lElem.Element("data").Attribute("encoding").Value.Equals("csv")) {
                            foreach (var gid in lElem.Element("data").Value.Split(",\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                                gids.Add(Convert.ToUInt32(gid));
                        }
                        else if (lElem.Element("data").Attribute("encoding") != null && lElem.Element("data").Attribute("encoding").Value.Equals("base64")) {
                            Byte[] data = Convert.FromBase64String(lElem.Element("data").Value);

                            if (lElem.Element("data").Attribute("compression") == null) {
                                // uncompressed data
                                for (int i = 0; i < data.Length; i += sizeof(UInt32)) {
                                    gids.Add(BitConverter.ToUInt32(data, i));
                                }
                            }
                            else if (lElem.Element("data").Attribute("compression").Value.Equals("gzip")) {
                                // gzip data
                                GZipStream gz = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
                                Byte[] buffer = new Byte[sizeof(UInt32)];
                                while (gz.Read(buffer, 0, buffer.Length) == buffer.Length) {
                                    gids.Add(BitConverter.ToUInt32(buffer, 0));
                                }
                            }
                            else if (lElem.Element("data").Attribute("compression").Value.Equals("zlib")) {
                                // zlib data - first two bytes zlib specific and not part of deflate
                                MemoryStream ms = new MemoryStream(data);
                                ms.ReadByte();
                                ms.ReadByte();
                                DeflateStream gz = new DeflateStream(ms, CompressionMode.Decompress);
                                Byte[] buffer = new Byte[sizeof(UInt32)];
                                while (gz.Read(buffer, 0, buffer.Length) == buffer.Length) {
                                    gids.Add(BitConverter.ToUInt32(buffer, 0));
                                }
                            }
                            else {
                                throw new NotSupportedException(String.Format("Compression '{0}' not supported.  XTiled supports gzip or zlib", lElem.Element("data").Attribute("compression").Value));
                            }
                        }
                        else {
                            throw new NotSupportedException(String.Format("Encoding '{0}' not supported.  XTiled supports csv or base64", lElem.Element("data").Attribute("encoding").Value));
                        }
                    }
                    else {

                        // parse xml formatted data
                        foreach (var tElem in lElem.Element("data").Elements("tile"))
                            gids.Add(Convert.ToUInt32(tElem.Attribute("gid").Value));
                    }

                    foreach (var gid in gids) {
                        TileData td = new TileData();
                        td.ID = gid & ~(FLIPPED_HORIZONTALLY_FLAG | FLIPPED_VERTICALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);
                        td.FlippedVertically = Convert.ToBoolean(gid & FLIPPED_HORIZONTALLY_FLAG);
                        td.FlippedHorizontally = Convert.ToBoolean(gid & FLIPPED_VERTICALLY_FLAG);
                        td.FlippedDiagonally = Convert.ToBoolean(gid & FLIPPED_DIAGONALLY_FLAG);
                        tiles.Add(td);
                    }
                }
                l.Tiles = tiles.ToArray();

                layers.Add(l);
            }
            map.Layers = layers.ToArray();

            List<ObjectLayer> oLayers = new List<ObjectLayer>();
            foreach (var olElem in input.Document.Root.Elements("objectgroup")) {
                ObjectLayer ol = new ObjectLayer();
                ol.Name = olElem.Attribute("name") == null ? null : olElem.Attribute("name").Value;
                ol.Opacity = olElem.Attribute("opacity") == null ? 1.0f : Convert.ToSingle(olElem.Attribute("opacity").Value);
                ol.Visible = olElem.Attribute("visible") == null ? true : olElem.Attribute("visible").Equals("1");

                ol.Color = null;
                if (olElem.Attribute("color") != null) {
                    System.Drawing.Color sdc = System.Drawing.ColorTranslator.FromHtml("#" + olElem.Attribute("color").Value.TrimStart('#'));
                    ol.Color = new Color(sdc.R, sdc.G, sdc.B);
                }

                ol.Properties = new PropertyCollection();
                if (olElem.Element("properties") != null)
                    foreach (var pElem in olElem.Element("properties").Elements("property"))
                        ol.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                List<MapObject> objects = new List<MapObject>();
                foreach (var oElem in olElem.Elements("object")) {
                    MapObject o = new MapObject();
                    o.Name = oElem.Attribute("name") == null ? null : oElem.Attribute("name").Value;
                    o.Type = oElem.Attribute("type") == null ? null : oElem.Attribute("type").Value;
                    o.X = oElem.Attribute("x") == null ? 0 : Convert.ToInt32(oElem.Attribute("x").Value);
                    o.Y = oElem.Attribute("y") == null ? 0 : Convert.ToInt32(oElem.Attribute("y").Value);
                    o.Width = oElem.Attribute("width") == null ? 0 : Convert.ToInt32(oElem.Attribute("width").Value);
                    o.Height = oElem.Attribute("height") == null ? 0 : Convert.ToInt32(oElem.Attribute("height").Value);
                    o.TileID = oElem.Attribute("gid") == null ? null : (Int32?)Convert.ToInt32(oElem.Attribute("gid").Value);
                    o.Visible = oElem.Attribute("visible") == null ? true : oElem.Attribute("visible").Equals("1");

                    o.Properties = new PropertyCollection();
                    if (oElem.Element("properties") != null)
                        foreach (var pElem in oElem.Element("properties").Elements("property"))
                            o.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                    o.Polygon = null;
                    if (oElem.Element("polygon") != null) {
                        List<Point> points = new List<Point>();
                        foreach (var point in oElem.Element("polygon").Attribute("points").Value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) {
                            String[] coord = point.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            points.Add(new Point(Convert.ToInt32(coord[0]), Convert.ToInt32(coord[1])));
                        }
                        o.Polygon = points.ToArray();
                    }

                    o.Polyline = null;
                    if (oElem.Element("polyline") != null) {
                        List<Point> points = new List<Point>();
                        foreach (var point in oElem.Element("polyline").Attribute("points").Value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) {
                            String[] coord = point.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            points.Add(new Point(Convert.ToInt32(coord[0]), Convert.ToInt32(coord[1])));
                        }
                        o.Polyline = points.ToArray();
                    }

                    objects.Add(o);
                }
                ol.MapObjects = objects.ToArray();

                oLayers.Add(ol);
            }
            map.ObjectLayers = oLayers.ToArray();

            return map;
        }
    }
}