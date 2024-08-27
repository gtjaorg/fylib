using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static Winhttp;

namespace FyLib.Http
{
    /// <summary>
    /// 快速Http对象
    /// </summary>
    public class QuickHttp
    {

        private Uri _url;
        private string _path;
        private HttpClient _client;
        private int _timeOut = 10000;
        private string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0";
        private string _accept = "application/json";
        private Map<string, string> _headers = new Map<string, string>();
        private bool _allowAutoRedirect = true;
        private Map<string, string> _querys = new Map<string, string>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="domain">根域名, 如https://www.baidu.com</param>
        public QuickHttp(string domain)
        {
            _url = new Uri(domain);
            _path = _url.LocalPath;
            if (!_url.Query.IsNullOrEmpty())
                _url.Query.Split("&").Select(a => a.Split("=")).ToList().ForEach(a =>
                {
                    if (a[0].StartsWith("?"))
                    {
                        a[0] = a[0][1..];
                    }
                    _querys.Add(a[0], a[1]);
                });
        }
        /// <summary>
        /// 设置超时, 默认10000
        /// </summary>
        /// <param name="timeout">毫秒</param>
        /// <returns></returns>
        public QuickHttp setTimeout(int timeout)
        {
            this._timeOut = timeout;
            return this;
        }
        /// <summary>
        /// 设置UA头
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public QuickHttp setUserAgent(string userAgent)
        {
            this._userAgent = userAgent;
            return this;
        }
        /// <summary>
        /// 设置Accept 默认为[application/json]
        /// </summary>
        /// <param name="accept"></param>
        /// <returns></returns>
        public QuickHttp setAccept(string accept)
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
        public QuickHttp addHeader(string key, string value)
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
        public QuickHttp addQuery(string key, string value)
        {
            _querys.Add(key, value);
            return this;
        }
        /// <summary>
        /// 移除协议头
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public QuickHttp removeHeader(string key)
        {
            _headers.Remove(key);
            return this;
        }
        /// <summary>
        /// 设置重定向, 默认为真
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public QuickHttp setAutoRedirect(bool value)
        {
            _allowAutoRedirect = value;
            return this;
        }
        /// <summary>
        /// 设置引用页面
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public QuickHttp setReferer(string value)
        {
            addHeader("Referer", value);
            return this;
        }
        /// <summary>
        /// 返回的Response
        /// </summary>
        public HttpResponseMessage? ResponseMessage { get; set; }

        #region Get
        /// <summary>
        /// Get
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
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
        /// <summary>
        /// Get
        /// </summary>
        /// <returns>string</returns>
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
        /// <summary>
        /// Get
        /// </summary>
        /// <returns>byte[]</returns>
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
        /// <summary>
        /// Get
        /// </summary>
        /// <returns>JToken</returns>
        public async Task<JToken?> GetAsJsonAsync()
        {
            var str = await GetAsStringAsync();
            if (str == null || str.IsNullOrEmpty()) return null;
            return JToken.Parse(str);
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T</returns>
        public async Task<T?> GetAsObjectAsync<T>()
        {
            var str = await GetAsStringAsync();
            if (str == null || str.IsNullOrEmpty()) return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        #endregion
        #region Post
        private Task<HttpResponseMessage> PostAsync(HttpContent? content = null)
        {
            PackClient();
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, _path);
            msg.Content = content;
            foreach (var item in _headers.ToList())
            {
                msg.Headers.Add(item.Key, item.Value);
            }
            var result = _client.SendAsync(msg);
            result.ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// Post String
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> PostAsync(string body, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            try
            {
                JObject.Parse(body);
                var content = new StringContent(body, encoding, "application/json");
                var result = await PostAsync(content);
                return result;
            }
            catch (Exception)
            {
                var content = new StringContent(body, encoding, "application/x-www-form-urlencoded");
                var result = await PostAsync(content);
                return result;
            }


        }
        /// <summary>
        /// Post Object
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> PostAsync(object body, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var content = new StringContent(JsonConvert.SerializeObject(body), encoding);
            var result = await PostAsync(content);
            return result;
        }
        /// <summary>
        /// Post Jtoken
        /// </summary>
        /// <param name="JToken"></param>
        /// <param name="encoding"></param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> PostAsync(JToken JToken, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            JsonSerializerSettings val = new JsonSerializerSettings
            {
                NullValueHandling = (NullValueHandling)1
            };
            if (JToken == null) JToken = JToken.Parse("{}");
            string str = JsonConvert.SerializeObject(JToken, val);
            var content = new StringContent(str, encoding, MediaTypeHeaderValue.Parse("application/json"));
            var result = await PostAsync(content);
            return result;
        }
        /// <summary>
        /// Post Byte[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> PostAsync(byte[] bytes)
        {
            if (bytes == null) bytes = new byte[0];
            var content = new ByteArrayContent(bytes);
            var result = await PostAsync(content);
            return result;
        }
        /// <summary>
        /// Post String
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>string</returns>
        public async Task<string?> PostAsStringAsync(string body, Encoding? encoding = null)
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
        /// <summary>
        /// Post String
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>JToken</returns>
        public async Task<JToken> PostAsJTokenAsync(string body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        /// <summary>
        /// Post Jtoken
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>string</returns>
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
        /// <summary>
        /// Post Jtoken
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>JToken</returns>
        public async Task<JToken> PostAsJTokenAsync(JToken body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        /// <summary>
        /// Post Byte[]
        /// </summary>
        /// <param name="body"></param>
        /// <returns>string</returns>
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
        /// <summary>
        /// Post Byte[]
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Jtoken</returns>
        public async Task<JToken> PostAsJTokenAsync(byte[] body)
        {
            var str = await PostAsStringAsync(body);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        /// <summary>
        /// Post Object
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>string</returns>
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
        /// <summary>
        /// Post Object
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>Jtoken</returns>
        public async Task<JToken> PostAsJTokenAsync(object body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            JToken obj = JToken.Parse(str);
            return obj;
        }
        /// <summary>
        /// Post String
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>Byte[]</returns>
        public async Task<byte[]?> PostAsBytesAsync(string body, Encoding? encoding = null)
        {
            var result = await PostAsync(body, encoding);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var t = await result.Content.ReadAsByteArrayAsync();
            return t;
        }
        /// <summary>
        /// Post Byte[]
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Byte[]</returns>
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
        /// <summary>
        /// Post Object
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>Byte[]</returns>
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
        /// <summary>
        /// Post JToken
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>Byte[]</returns>
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
        /// <summary>
        /// Post String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>T</returns>
        public async Task<T?> PostAsObjectAsync<T>(string body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// Post JToken
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>T</returns>
        public async Task<T?> PostAsObjectAsync<T>(JToken body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// Post Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>T</returns>
        public async Task<T?> PostAsObjectAsync<T>(object body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// Post Byte[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <returns>T</returns>
        public async Task<T?> PostAsObjectAsync<T>(byte[] body)
        {
            var str = await PostAsStringAsync(body);
            if (str == null || str.IsNullOrEmpty()) return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        #endregion

        public CookieContainer cookieContainer = new CookieContainer();
        private void PackClient()
        {
            SocketsHttpHandler handler = new SocketsHttpHandler();
            handler.AllowAutoRedirect = _allowAutoRedirect;
            handler.EnableMultipleHttp2Connections = true;
            handler.AutomaticDecompression = System.Net.DecompressionMethods.All;
            handler.CookieContainer = cookieContainer;
            if (_client != null) _client.Dispose();
            _client = new System.Net.Http.HttpClient(handler);
            _client.BaseAddress = _url;
            _client.Timeout = Other.GetTimeSpan(_timeOut);
            var b = _client.DefaultRequestHeaders.UserAgent.TryParseAdd(_userAgent);
            Debug.WriteLine(b);
            _client.DefaultRequestHeaders.AcceptCharset.TryParseAdd("UTF-8");
            _client.DefaultRequestHeaders.Accept.TryParseAdd(_accept);

            string query = string.Join("&", _querys.ToList().Select(a => $"{a.Key}={Uri.EscapeDataString(a.Value)}"));
            if (query.StartsWith("?"))
            {
                _path += query;
            }
            else
            {
                _path += "?" + query;
            }

        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        public string Cookie
        {
            get
            {
                var cookieCollection = cookieContainer.GetCookies(_url);
                var cookieHeader = new StringBuilder();
                foreach (Cookie cookie in cookieCollection)
                {
                    if (cookieHeader.Length > 0)
                        cookieHeader.Append("; ");
                    cookieHeader.Append($"{cookie.Name}={cookie.Value}");
                }
                return cookieHeader.ToString();
            }
        }
    }
}
