using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
