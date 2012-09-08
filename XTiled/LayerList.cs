using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FuncWorks.XNA.XTiled {
    public class LayerList : List<Layer> {

        public Layer this[String name] {
            get {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }
}
