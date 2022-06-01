using System;
using System.Collections.Generic;
using System.Text;

namespace SDPingService.Classes
{
    [Serializable]
    internal class ConfigurationClass
    {
        public uint max;
        public float timer;
        public string directory;
        public List<string> websitesLinks;
        public List<string> postgresConnections;
    }
}
