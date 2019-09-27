using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public class ApiServiceWatcher : ILoopWorker
    {
        //
        public  List<ApiWatchInfo> ApiWatchInfos { get; }
        public  string WorkName => "ApiServiceWatcher";

        private MTask mTask;
        private HttpHelper httpHelper = new HttpHelper();

        private List<IWarnPusher> warnPushers;

        public ApiServiceWatcher(IEnumerable<IWarnPusher> warnPusher, IOptions<List<ApiWatchInfo>> options)
        {
            warnPushers = warnPusher.ToList();
            ApiWatchInfos = options.Value;
            Module.ApiWatchInfos = ApiWatchInfos;
        }
        public void Start(int interval)
        {
            Module.Log($"开启{WorkName}工作");
            mTask = new MTask();
            mTask.SetAction(async () =>
            {
                while (!mTask.IsCancelled())
                {
                    Console.WriteLine("");
                    Module.Log($"==============={WorkName} Start===============");
                    await DoWork();
                    Module.Log($"==============={WorkName} End===============");
                    await Task.Delay(interval);
                }
            });
            mTask.Start();
        }

        public void Stop()
        {
            mTask?.Cancel();
        }

        public async Task DoWork()
        {
            if (ApiWatchInfos != null)
            {
                ConsoleHelper.PrintLine();
                ConsoleHelper.PrintRow("Name", "Result", "CostTime", "RequestCount");
                ConsoleHelper.PrintLine();

                foreach (var itm in ApiWatchInfos)
                {
                    if (itm.WatchCmd == "ignore") continue;
                    await TestUrl(itm);
                }
            }
        }
        private async Task TestUrl(ApiWatchInfo info)
        {
            Stopwatch sp = new Stopwatch();
            NormalResponse np = null;
            int count = 0;
            while (count++<5)
            {            
                sp.Start();
                np = await httpHelper.Get<NormalResponse>(info.Url, null, 5000);
                sp.Stop();
                if (np != null)
                {
                    info.RequestCount = count;
                    break;
                }     
            }         
            long costTime = sp.ElapsedMilliseconds;
            if (np != null)
            {
               // Module.Log($"{info.Name} 工作正常,耗时 {costTime}");
                if (info.Status == -1)
                {
                    string title = $"[服务已恢复正常]{info.Name}";
                    string body = $"{info.Name} 工作已恢复正常！";
                    warnPushers.ForEach(a => a.PushWarn(title, body));
                }
                info.Status = 1;
                if (info.ResponseTimes == null) info.ResponseTimes = new List<long>();
                info.ResponseTimes.Add(costTime);
                if (info.ResponseTimes.Count > 10) info.ResponseTimes.RemoveAt(0);
            }
            else
            {
               // Module.Log($"{info.Name} 工作异常,耗时 {costTime}");
                if (info.Status != -1)
                {
                    if (warnPushers != null)
                    {
                        string title = $"[服务异常]{info.Name}";
                        string body = $"{info.Name} 工作异常，请检查服务";
                        warnPushers.ForEach(a => a.PushWarn(title, body));
                    }
                }
                info.Status = -1;
            }
         
            ConsoleHelper.PrintRow(info.Name, info.Status==-1? "Error" : "OK", costTime+"",info.RequestCount+"");
            ConsoleHelper.PrintLine();
        }
    }
}
