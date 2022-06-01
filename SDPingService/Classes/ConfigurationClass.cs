using System;
using System.Collections.Generic;

namespace SDPingService.Classes
{
    [Serializable]
    public class ConfigurationClass
    {
        public uint max;
        public float timer;
        public string directory;
        public List<string> websitesLinks;
        public List<string> postgresConnections;
    }
}
