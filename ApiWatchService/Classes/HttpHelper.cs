using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiWatchService
{
    public class HttpHelper
    {
        public enum HttpParaType
        {
            Json, Form
        }
        private  CookieContainer cookieContainer = new CookieContainer();

        public async Task<T> Get<T>(string url, object obj = null, int timeoutSecond = 0)
        {
            string result = await Get(url, obj, timeoutSecond);
            if (string.IsNullOrEmpty(result)) return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        public async Task<T> Post<T>(string url, object obj = null, HttpParaType type = HttpParaType.Json, int timeout = 0)
        {
            string result = await Post(url, obj, type, timeout);
            if (string.IsNullOrEmpty(result)) return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception)
            {
                return default(T);
            }
        }





        public  async Task<string> Get(string url, object obj = null, int timeoutSecond = 0)
        {
            var dik = new Dictionary<string, object>();
            string param = "";
            if (obj != null)
            {
                foreach (var itm in obj.GetType().GetProperties())
                {
                    string key = Uri.EscapeDataString(itm.Name);
                    string value = Uri.EscapeDataString(itm.GetValue(obj).ToString());
                    param = param + key + "=" + value + "&";
                }
            }
            if (!string.IsNullOrEmpty(param))
            {
                param = param.Substring(0, param.Length - 1);
                url = url + "?" + param;
            }
            return await GetResponse(url, timeoutSecond);
        }

        public  async Task<string> Post(string url, object obj = null, HttpParaType type = HttpParaType.Json, int timeout = 0)
        {
            string param = "";
            if (type == HttpParaType.Json)
            {
                param = JsonConvert.SerializeObject(obj);
            }
            else
            {
                if (obj != null)
                {
                    foreach (var itm in obj.GetType().GetProperties())
                    {
                        string key = Uri.EscapeDataString(itm.Name);
                        string value = Uri.EscapeDataString(itm.GetValue(obj).ToString());
                        param = param + key + "=" + value + "&";
                    }
                }
                param = param.Substring(0, param.Length - 1);
            }
            string result = await PostResponse(url, param, type, timeout);
            return result;
        }


        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>       
        private  async Task<string> GetResponse(string url, int timeout = 0)
        {
            try
            {
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                var handler = new HttpClientHandler() { UseCookies = true };
                handler.CookieContainer = cookieContainer;
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
                httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                // httpClient.DefaultRequestHeaders.Add("Keep-Alive", "timeout=60000");
                if (timeout > 0) httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var contentType = response.Content.Headers.ContentType;
                    if (string.IsNullOrEmpty(contentType.CharSet))
                    {
                        contentType.CharSet = "utf-8";
                    }
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        private  async Task<string> PostResponse(string url, string postData, HttpParaType type = HttpParaType.Json, int timeout = 0)
        {
            try
            {
                if (url.StartsWith("https"))
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                HttpContent httpContent = new StringContent(postData);
                if (type == HttpParaType.Json)
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                else
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                }

                var handler = new HttpClientHandler() { UseCookies = true };
                handler.CookieContainer = cookieContainer;
                HttpClient httpClient = new HttpClient(handler);

                httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
                httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                //  httpClient.DefaultRequestHeaders.Add("Keep-Alive", "timeout=60000");
                if (timeout > 0) httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }

        }

    }
}
