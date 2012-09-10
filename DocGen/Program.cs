using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace DocGen {
    class Program {
        static void Main(string[] args) {
            XDocument doc = XDocument.Load("FuncWorks.XNA.XTiled.XML");
            FileStream output = File.Open("objects.wiki", FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(output);
            sw.NewLine = "\n";

            sw.WriteLine("= Object Reference =");

            sw.WriteLine("== Map ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.Map");

            sw.WriteLine("== Tileset ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.Tileset");

            sw.WriteLine("== Tile ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.Tile");

            sw.WriteLine("== TileLayer ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.TileLayer");

            sw.WriteLine("== TileData ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.TileData");

            sw.WriteLine("== ObjectLayer ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.ObjectLayer");

            sw.WriteLine("== MapObject ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.MapObject");

            sw.WriteLine("== Property ==");
            WriteObject(doc, sw, "FuncWorks.XNA.XTiled.Property");

            sw.Close();
            output.Close();
        }

        private static void WriteObject(XDocument doc, StreamWriter sw, String name) {
            sw.WriteLine(doc.Document.Root.Element("members").Elements().First(x => x.Attribute("name").Value.Equals("T:" + name)).Element("summary").Value.Trim());
            sw.WriteLine("=== Members ===");
            foreach (var elem in doc.Document.Root.Element("members").Elements().Where(x => x.Attribute("name").Value.StartsWith("F:" + name + ".") || x.Attribute("name").Value.StartsWith("P:" + name + "."))) {
                sw.WriteLine("* **{0}**\\\\{1}", elem.Attribute("name").Value.Remove(0, ("F:" + name + ".").Length), elem.Element("summary").Value.Trim());
            }

            if (doc.Document.Root.Element("members").Elements().Where(x => x.Attribute("name").Value.StartsWith("M:" + name + ".")).Count() > 0) {
                sw.WriteLine("=== Methods ===");

                foreach (var elem in doc.Document.Root.Element("members").Elements().Where(x => x.Attribute("name").Value.StartsWith("M:" + name + "."))) {
                    String[] parts = elem.Attribute("name").Value.Remove(0, ("M:" + name + ".").Length).Split("()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    String sig = parts[0];
                    if (parts.Length == 1) {
                        sig += " ()";
                    }
                    else {
                        var argTypes = parts[1].Split(',').Select(x => x.Split('.').Last());
                        List<String> args = new List<string>();
                        for (int i = 0; i < argTypes.Count(); i++) {
                            String argtype = argTypes.ElementAt(i).EndsWith("@") ? "ref " + argTypes.ElementAt(i).TrimEnd('@') : argTypes.ElementAt(i);
                            String argname = elem.Elements("param").ElementAt(i).Attribute("name").Value.Trim();
                            String argdoc = elem.Elements("param").ElementAt(i).Value.Trim();
                            args.Add(String.Format("{0} //{1}//", argtype, argname));
                        }
                        sig += String.Format(" ({0})", String.Join(", ", args));
                    }
                    sw.WriteLine("* **{0}**\\\\{1}", sig, elem.Element("summary").Value.Trim());
                }
            }
        }
    }
}
