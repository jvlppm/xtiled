using System;

namespace FuncWorks.XNA.XTiled {
    public struct Property {
        public String Value;
        public Single? AsSingle;
        public Int32? AsInt32;
        public Boolean? AsBoolean;

        public static Property Create(String value) {
            Property p = new Property();
            p.Value = value;

            Boolean testBool;
            if (Boolean.TryParse(value, out testBool))
                p.AsBoolean = testBool;
            else
                p.AsBoolean = null;

            Single testSingle;
            if (Single.TryParse(value, out testSingle))
                p.AsSingle = testSingle;
            else
                p.AsSingle = null;

            Int32 testInt;
            if (Int32.TryParse(value, out testInt))
                p.AsInt32 = testInt;
            else
                p.AsInt32 = null;
            
            return p;
        }
    }
}
