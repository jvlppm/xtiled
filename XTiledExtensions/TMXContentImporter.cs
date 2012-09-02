using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Linq;

namespace FuncWorks.XNA.XTiled {
    [ContentImporter(".tmx", DisplayName = "TMX Map Importer", DefaultProcessor = "TMXContentProcessor")]
    public class TMXContentImporter : ContentImporter<XDocument> {
        public override XDocument Import(string filename, ContentImporterContext context) {
            return XDocument.Load(filename);
        }
    }
}
