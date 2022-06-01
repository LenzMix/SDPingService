using SDPingService.Types;
using System;
using System.Net.NetworkInformation;
using System.Xml.Serialization;

namespace SDPingService.Classes
{
    [Serializable]
    public class PingInformation
    {
        /*public PingInformation(string target, long ping)
        {
            this.ping = ping;
            this.target = target;
        }*/
        [XmlAttribute]
        public string target { get; set; }
        [XmlAttribute]
        public long ping { get; set; }
    }

    [Serializable]
    public class WebsitePingInformation : PingInformation
    {
        [XmlAttribute]
        public IPStatus status { get; set; }
        /*public WebsitePingInformation(string target, long ping, IPStatus status) : base(target, ping)
        {
            this.status = status;
        }*/
    }

    [Serializable]
    public class PostgresPingInformation : PingInformation
    {
        [XmlAttribute]
        internal EResponseType status { get; set; }
        /*internal PostgresPingInformation(string target, long ping, EResponseType status) : base(target, ping)
        {
            this.status = status;
        }*/
    }
}
