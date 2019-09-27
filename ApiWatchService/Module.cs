using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiWatchService
{
    public class Module
    {
        public static readonly string APPName = "ApiWatchService";
        public static readonly bool IsLogger = false;
        public static string Version = "1.0.2";
        public static string MysqlConnstr = "";
        public static ServiceProvider ServiceProvider { get; set; }
        public static WarnPushInfo WarnPusher { get; set; }
        public static List<ApiWatchInfo> ApiWatchInfos { get; set; }

        public static void Init(IConfiguration Configuration, ServiceProvider serviceProvider)
        {          
            Console.Title = $"{APPName} {Version}";
            Module.ServiceProvider = serviceProvider;

            
            MysqlConnstr = Configuration.GetConnectionString("MysqlConnection");
          

            Start();
        }
        public static void Start()
        {
            Log($"================程序启动 版本 {Version}================");
            LoopWorkerStarter.Start();
        }
        public static void Stop()
        {
            Log("================程序关闭================");
            LoopWorkerStarter.Stop();
        }
        public static void Log(string str)
        {
            Console.WriteLine(DateTime.Now.ToString("[HH:mm:ss] ") + str);
            if (IsLogger)
            {
                Logger.Info(str);
            }
        }
    }
}
