using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public static class LoopWorkerStarter
    {
        private static List<ILoopWorker> loopWorkers;
        private static int loopworkInterval = 10 * 1000;
        public static void Start()
        {
            loopWorkers = Module.ServiceProvider.GetServices<ILoopWorker>().ToList();
          
            if (loopWorkers != null) loopWorkers.ForEach(a => a.Start(loopworkInterval));
        }
        public static void Stop()
        {
            if (loopWorkers != null) loopWorkers.ForEach(a => a.Stop());
        }
    }
}
