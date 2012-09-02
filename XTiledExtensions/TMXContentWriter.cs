using System;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.IO.Compression;

namespace FuncWorks.XNA.XTiled {
    [ContentTypeWriter]
    public class TMXContentWriter : ContentTypeWriter<Map> {
        protected override void Write(ContentWriter output, Map value) {
            XmlSerializer xmlSerial = new XmlSerializer(value.GetType());
            xmlSerial.Serialize(output.BaseStream, value);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform) {
            return "FuncWorks.XNA.XTiled.MapReader, FuncWorks.XNA.XTiled";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform) {
            return "FuncWorks.XNA.XTiled.Map, FuncWorks.XNA.XTiled";
        }

    }
}
