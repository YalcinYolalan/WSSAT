using System;

namespace WSSAT.DataTypes
{
    public class WSDescriber
    {
        public Uri WSUri { get; set; }
        public string WSDLAddress { get; set; }
        public BasicAuthentication BasicAuthentication { get; set; }
    }
}
