using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncWorks.XNA.XTiled {
    public class ObjectLayer {
        public String Name;
        public Microsoft.Xna.Framework.Color? Color;
        public Single Opacity;
        public Boolean Visible;
        public PropertyCollection Properties;
        public MapObject[] MapObjects;
    }
}
