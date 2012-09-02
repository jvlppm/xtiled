using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FuncWorks.XNA.XTiled {
    [ContentProcessor(DisplayName = "TMX Map - XTiled")]
    public class TMXContentProcessor : ContentProcessor<XDocument, Map> {
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
                    map.Properties.Add(pElem.Attribute("name").Value, pElem.Attribute("value").Value);

            List<Tileset> tilesets = new List<Tileset>();
            foreach (var elem in input.Document.Root.Elements("tileset")) {
                Tileset t = new Tileset();
                XElement tElem = elem;
                t.FirstGID = Convert.ToInt32(tElem.Attribute("firstgid").Value);

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
                        System.Drawing.Color sdc = System.Drawing.ColorTranslator.FromHtml("#" + imgElem.Attribute("trans").Value);
                        img.TransparentColor = new Color(sdc.R, sdc.G, sdc.B);
                    }
                    images.Add(img);
                }
                t.Images = images.ToArray();
                t.Properties = new PropertyCollection();
                if (tElem.Element("properties") != null)
                    foreach (var pElem in tElem.Element("properties").Elements("property"))
                        t.Properties.Add(pElem.Attribute("name").Value, pElem.Attribute("value").Value);

                tilesets.Add(t);
            }
            map.Tilesets = tilesets.ToArray();

            return map;
        }
    }
}