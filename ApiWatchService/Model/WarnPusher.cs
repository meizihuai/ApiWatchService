using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public class WarnPushInfo
    {
        public EmailPushInfo Email { get; set; }
        public ServerChanInfo ServerChan { get; set; }
    }
    public class ServerChanInfo
    {
        public bool Flag { get; set; }
        public string Secret { get; set; }
    }
    public class EmailPushInfo
    {
        public bool Flag { get; set; }
        public EmailPusherConfig Pusher { get; set; }
        public List<string> ReceiveList { get; set; }
    }

    public class EmailPusherConfig
    {
        public string Usr { get; set; }
        public string Pwd { get; set; }
    }
}
