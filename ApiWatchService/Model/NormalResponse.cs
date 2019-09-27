using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ApiWatchService
{
    /// <summary>
    /// 一般返回格式,JSON格式
    /// </summary>
    public class NormalResponse
    {
        /// <summary>
        /// 处理结果，true:成功，false:失败
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 处理消息或处理过程
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Errmsg { get; set; }
        /// <summary>
        /// 数据，可能是string或者json或者json数组
        /// </summary>
        public object Data { get; set; }
        public NormalResponse()
        {

        }
        public NormalResponse(bool result, string msg, string errmsg, object data) // 基本构造函数() As _errmsg,string
        {
            this.Result = result;
            this.Msg = msg;
            this.Errmsg = errmsg;
            this.Data = data;
        }

        public NormalResponse(bool result, string msg) // 重载构造函数，为了方便写new,很多时候，只需要一个结果和一个参数() As _result,string
        {
            this.Result = result;
            this.Msg = msg;
            this.Errmsg = "";
            this.Data = "";
        }
        public NormalResponse(string msg)
        {
            this.Result = false;
            if (msg == "success")
            {
                this.Result = true;
            }
            this.Msg = msg;
            this.Errmsg = "";
            this.Data = "";
        }
        public T Parse<T>()
        {
            if (Data == null) return default(T);
            string json = JsonConvert.SerializeObject(Data);
            // string json = data.ToString();
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
    }
}
