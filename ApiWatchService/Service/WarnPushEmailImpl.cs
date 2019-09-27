using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public class WarnPushEmailImpl : IWarnPusher
    {
        private WarnPushInfo warnPusher;
        public WarnPushEmailImpl(IOptions<WarnPushInfo> warnPusherOption)
        {
            this.warnPusher = warnPusherOption.Value;
        }
        public void PushWarn(string title, string body)
        {
            EmailPushInfo emailPusher = warnPusher.Email;

            if (emailPusher == null) return;
            if (!emailPusher.Flag) return;
            if (emailPusher.ReceiveList == null || emailPusher.ReceiveList.Count == 0) return;
            string emailAddress = string.Join(',', emailPusher.ReceiveList.ToArray());
            NormalResponse np =EmailHelper.SendMail(emailAddress, title, body);
        }
    }
}
