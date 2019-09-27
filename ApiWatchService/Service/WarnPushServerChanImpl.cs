using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public class WarnPushServerChanImpl : IWarnPusher
    {
        private WarnPushInfo warnPusher;
        private HttpHelper httphelper = new HttpHelper();
        public WarnPushServerChanImpl(IOptions<WarnPushInfo> warnPusherOption)
        {
            this.warnPusher = warnPusherOption.Value;
        }
        public async void PushWarn(string title, string body)
        {
            if (warnPusher.ServerChan == null) return;
            if (!warnPusher.ServerChan.Flag) return;
            string url = $"https://sc.ftqq.com/{warnPusher.ServerChan.Secret}.send";
            var d = new
            {
                text=title,
                desp= body
            };
            string result = await httphelper.Get(url, d, 10 * 1000);
        }
    }
}
