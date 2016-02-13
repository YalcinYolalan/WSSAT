using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSAT.DataTypes
{
    public class WSDescriberForReport
    {
        public WSDescriber WsDesc { set; get; }
        public List<VulnerabilityForReport> Vulns { set; get; }
        public List<StaticVulnerabilityForReport> StaticVulns { set; get; }
    }
}
