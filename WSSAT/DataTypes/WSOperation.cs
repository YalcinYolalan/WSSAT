using System.Collections.Generic;

namespace WSSAT.DataTypes
{
    public class WSOperation
    {
        public string ClassName { set; get; }
        public string MethodName { set; get; }
        public string ReturnType { set; get; }
        public List<WSParameter> Parameters { set; get; }
    }
}
