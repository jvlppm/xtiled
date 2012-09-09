using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FuncWorks.XNA.XTiled {
    public class TileLayerList : List<TileLayer> {
        public TileLayer this[String name] {
            get {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }

    public class ObjectLayerList : List<ObjectLayer> {
        public ObjectLayer this[String name] {
            get {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }
}
