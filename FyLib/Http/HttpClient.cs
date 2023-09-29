using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static Winhttp;

namespace FyLib.Http
{
    public class HttpClient
    {


        private Uri _url;
        private string _path;
        private System.Net.Http.HttpClient _client;
        private int _timeOut = 10000;
        private string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62";
        private string _accept = "application/json";
        private Map<string, string> _headers = new Map<string, string>();
        private bool _allowAutoRedirect = true;
        private Map<string,string> _querys = new Map<string, string>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="domain">根域名, 如https://www.baidu.com</param>
        public HttpClient(string domain)
        {
            _url = new Uri(domain);
            _path = _url.LocalPath;
        }
        /// <summary>
        /// 设置超时, 默认10000
        /// </summary>
        /// <param name="timeout">毫秒</param>
        /// <returns></returns>
        public HttpClient setTimeout(int timeout)
        {
            this._timeOut = timeout;
            return this;
        }
        /// <summary>
        /// 设置UA头
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public HttpClient setUserAgent(string userAgent)
        {
            this._userAgent = userAgent;
            return this;
        }
        /// <summary>
        /// 设置Accept 默认为[application/json]
        /// </summary>
        /// <param name="accept"></param>
        /// <returns></returns>
        public HttpClient setAccept(string accept)
        {
            this._accept = accept;
            return this;
        }
        /// <summary>
        /// 添加协议头
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpClient addHeader(string key, string value)
        {
            _headers.Add(key, value);
            return this;
        }
        /// <summary>
        /// 添加query字符
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpClient addQuery(string key, string value)
        {
            _querys.Add(key, value);    
            return this;
        }
        /// <summary>
        /// 移除协议头
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HttpClient removeHeader(string key)
        {
            _headers.Remove(key);
            return this;
        }
        /// <summary>
        /// 设置重定向, 默认为真
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpClient setAutoRedirect(bool value)
        {
            _allowAutoRedirect = value;
            return this;
        }
        /// <summary>
        /// 设置引用页面
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpClient setReferer(string value)
        {
            addHeader("Referer", value);
            return this;
        }
        /// <summary>
        /// 返回的Response
        /// </summary>
        public HttpResponseMessage? ResponseMessage { get; set; }

        #region Get
        public Task<HttpResponseMessage> GetAsync()
        {
            PackClient();
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, _path);
            foreach (var item in _headers.ToList())
            {
                msg.Headers.Add(item.Key, item.Value);
            }
            var result = _client.SendAsync(msg);
            return result;
        }
        public async Task<string?> GetAsStringAsync()
        {
            var result = await GetAsync();
            this.ResponseMessage = result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            if (result.Content.Headers.ContentType != null)
            {
                if (result.Content.Headers.ContentType.CharSet == "GB2312")
                {
                    var bytes = await result.Content.ReadAsByteArrayAsync();
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("GB2312").GetString(bytes);
                }
            }
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<byte[]?> GetAsBytesAsync()
        {
            var result = await GetAsync();
            this.ResponseMessage = result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var t = await result.Content.ReadAsByteArrayAsync();
            return t;
        }
        public async Task<JToken?> GetAsJsonAsync()
        {
            var str = await GetAsStringAsync();
            if (str==null || str.IsNullOrEmpty()) return null;
            return JToken.Parse(str);
        }
        public async Task<T?> GetAsObjectAsync<T>()
        {
            var str = await GetAsStringAsync();
            if (str == null || str.IsNullOrEmpty()) return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        #endregion
        #region Post
        private Task<HttpResponseMessage> PostAsync( HttpContent? content=null)
        {
            PackClient();
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, _path);
            msg.Content = content;
            foreach (var item in _headers.ToList())
            {
                msg.Headers.Add(item.Key, item.Value);
            }
            var result = _client.SendAsync(msg);
            return result;
        }
        public async Task<HttpResponseMessage> PostAsync(string body, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var content = new StringContent(body, encoding);
            var result = await PostAsync(content);
            return result;
        }
        public async Task<HttpResponseMessage> PostAsync(object body, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var content = new StringContent(JsonConvert.SerializeObject(body), encoding);
            var result = await PostAsync(content);
            return result;
        }
        public async Task<HttpResponseMessage> PostAsync( JToken JToken, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            JsonSerializerSettings val = new JsonSerializerSettings
            {
                NullValueHandling = (NullValueHandling)1
            };
            if (JToken == null) JToken = JToken.Parse("{}");
            string str = JsonConvert.SerializeObject(JToken, val);
            var content = new StringContent(str, encoding,MediaTypeHeaderValue.Parse("application/json"));
            var result = await PostAsync(content);
            return result;
        }
        public async Task<HttpResponseMessage> PostAsync(byte[] bytes)
        {
            if(bytes==null) bytes = new byte[0];
            var content = new ByteArrayContent(bytes);
            var result = await PostAsync(content);
            return result;
        }

        public async Task<string?> PostAsStringAsync(string body, Encoding? encoding = null)
        {
            var result = await PostAsync(body,encoding);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            if (result.Content.Headers.ContentType != null)
            {
                if (result.Content.Headers.ContentType.CharSet == "GB2312")
                {
                    var bytes = await result.Content.ReadAsByteArrayAsync();
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("GB2312").GetString(bytes);
                }
            }
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<JToken> PostAsJTokenAsync(string body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        public async Task<string?> PostAsStringAsync(JToken body, Encoding? encoding = null)
        {
            var result = await PostAsync(body, encoding);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            if (result.Content.Headers.ContentType != null)
            {
                if (result.Content.Headers.ContentType.CharSet == "GB2312")
                {
                    var bytes = await result.Content.ReadAsByteArrayAsync();
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("GB2312").GetString(bytes);
                }
            }
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<JToken> PostAsJTokenAsync(JToken body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        public async Task<string?> PostAsStringAsync(byte[] body)
        {
            var result = await PostAsync(body);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            if (result.Content.Headers.ContentType != null)
            {
                if (result.Content.Headers.ContentType.CharSet == "GB2312")
                {
                    var bytes = await result.Content.ReadAsByteArrayAsync();
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("GB2312").GetString(bytes);
                }
            }
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<JToken> PostAsJTokenAsync(byte[] body)
        {
            var str = await PostAsStringAsync(body);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        public async Task<string?> PostAsStringAsync(object body, Encoding? encoding = null)
        {
            var result = await PostAsync(body, encoding);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            if (result.Content.Headers.ContentType != null)
            {
                if (result.Content.Headers.ContentType.CharSet == "GB2312")
                {
                    var bytes = await result.Content.ReadAsByteArrayAsync();
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("GB2312").GetString(bytes);
                }
            }
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<JToken> PostAsJTokenAsync(object body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        public async Task<byte[]?> PostAsBytesAsync(string body , Encoding? encoding = null)
        {
            var result = await PostAsync(body, encoding);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var t = await result.Content.ReadAsByteArrayAsync();
            return t;
        }
        public async Task<byte[]?> PostAsBytesAsync(byte[] body)
        {
            var result = await PostAsync(body);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var t = await result.Content.ReadAsByteArrayAsync();
            return t;
        }
        public async Task<byte[]?> PostAsBytesAsync(object body, Encoding? encoding = null)
        {
            var result = await PostAsync(body, encoding);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var t = await result.Content.ReadAsByteArrayAsync();
            return t;
        }
        public async Task<byte[]?> PostAsBytesAsync(JToken body, Encoding? encoding = null)
        {
            var result = await PostAsync(body, encoding);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var t = await result.Content.ReadAsByteArrayAsync();
            return t;
        }
        public async Task<T?> PostAsObjectAsync<T>(string body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        public async Task<T?> PostAsObjectAsync<T>(JToken body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        public async Task<T?> PostAsObjectAsync<T>(object body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        public async Task<T?> PostAsObjectAsync<T>(byte[] body)
        {
            var str = await PostAsStringAsync(body);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        #endregion

        private void PackClient()
        {
            SocketsHttpHandler handler = new SocketsHttpHandler();
            handler.AllowAutoRedirect = _allowAutoRedirect;
            handler.EnableMultipleHttp2Connections = true;
            handler.AutomaticDecompression = System.Net.DecompressionMethods.All;
            _client = new System.Net.Http.HttpClient(handler);
            _client.BaseAddress = _url;
            _client.Timeout = Other.GetTimeSpan(_timeOut);
            _client.DefaultRequestHeaders.UserAgent.TryParseAdd(_userAgent);
            _client.DefaultRequestHeaders.AcceptCharset.TryParseAdd("UTF-8");
            _client.DefaultRequestHeaders.Accept.TryParseAdd(_accept);
            string query = string.Join("&", _querys.ToList().Select(a => $"{a.Key}={Uri.EscapeDataString(a.Value)}"));
            _path += "?" + query;
        }
    }
}
