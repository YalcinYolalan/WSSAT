using System.Collections.Generic;

namespace WSSAT.DataTypes
{
    public class WSDescriberForReport
    {
        public WSDescriber WsDesc { set; get; }
        public RESTApi RestAPI { set; get; }
        public List<VulnerabilityForReport> Vulns { set; get; }
        public List<StaticVulnerabilityForReport> StaticVulns { set; get; }
        public List<DisclosureVulnerabilityForReport> InfoVulns { set; get; }
    }
}
