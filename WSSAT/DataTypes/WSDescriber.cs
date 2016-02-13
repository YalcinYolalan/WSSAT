using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSAT.DataTypes
{
    public class WSDescriber
    {
        public Uri WSUri { get; set; }
        public string WSDLAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
