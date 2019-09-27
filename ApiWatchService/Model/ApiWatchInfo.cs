using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public class ApiWatchInfo
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string WatchCmd { get; set; }
        public List<long> ResponseTimes { get; set; }
        public int RequestCount { get; set; }
    }
}
