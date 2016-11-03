using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NonPlugin
{
    public class NonPlugin
    {
        public string Name => "NonPlugin";

        public NonPlugin()
        {
            const string ubuntuServer = "http://releases.ubuntu.com/16.04/ubuntu-16.04.1-desktop-amd64.iso";
            WebClient webClient = new WebClient();
            webClient.DownloadFile(ubuntuServer, "ubuntu.iso");
        }
    }
}
