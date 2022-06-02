using System;
using System.Collections.Generic;

namespace SDPingService.Classes
{
    [Serializable]
    public class ConfigurationClass
    {
        public string SMTPserver;
        public int SMTPport;
        public string SMTPlogin;
        public string SMTPpassword;
        public string SMTPfromMail;
        public string SMTPtoMail;
        public string directory;
        public List<string> websitesLinks;
        public List<string> postgresConnections;
    }
}
