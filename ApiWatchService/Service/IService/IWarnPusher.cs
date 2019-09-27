using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public interface IWarnPusher
    {
        void PushWarn(string title,string body);
    }
}
