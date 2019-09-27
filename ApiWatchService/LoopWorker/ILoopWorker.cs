using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public interface ILoopWorker
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        string WorkName { get; }
        /// <summary>
        /// 开始工作
        /// </summary>
        /// <param name="interval">时间间隔（ms）</param>
        void Start(int interval);
        /// <summary>
        /// 单次工作方法
        /// </summary>
        Task DoWork();
        /// <summary>
        /// 停止循环工作
        /// </summary>
        void Stop();
       
    }
}
