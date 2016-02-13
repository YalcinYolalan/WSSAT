using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSAT.DataTypes
{
    public class WSParameter
    {
        public string Name { set; get; }
        public string TypeName { set; get; }
        public decimal MinOccurs { set; get; }
        public string MaxOccurs { set; get; }
    }
}
