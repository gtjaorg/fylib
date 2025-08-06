using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FyLib.FyLib;

namespace FyLib.Http
{
    /// <summary>
    /// 快速Http对象
    /// </summary>
    public class QuickHttp
    {

        private readonly Uri _url;
        private string _path;
        private int _timeOut = 10000;
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings();
        private string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0";
        private string _accept = "application/json";
        private readonly Map<string, string> _headers = new Map<string, string>();
        private bool _allowAutoRedirect = true;
        private readonly Map<string, string> _params = new Map<string, string>();
        private System.Security.Authentication.SslProtocols? _sslProtocols = null;
        private WebProxy? _webProxy = null;
        private bool _useWebProxy = false;
        private bool _useHttp2 = false;
        /// <summary>
        /// HttpClient客户端
        /// </summary>
        public HttpClient? Client { get; private set; }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public QuickHttp SetProxy(string ip, int port)
        {
            this._useWebProxy = true;
            this._webProxy = new WebProxy(ip, port);
            return this;
        }
        /// <summary>
        /// 设置 SSL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public QuickHttp SetSslProtocols(System.Security.Authentication.SslProtocols value)
        {
            _sslProtocols = value;
            return this;
        }
        /// <summary>
        /// 是否使用 HTTP2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public QuickHttp UseHttp2(bool value = false)
        {
            this._useHttp2 = value;
            return this;
        }
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
                    _params.Add(a[0], a[1]);
                });
        }
        /// <summary>
        /// 设置超时, 默认10000
        /// </summary>
        /// <param name="timeout">毫秒</param>
        /// <returns></returns>
        public QuickHttp SetTimeout(int timeout)
        {
            this._timeOut = timeout;
            return this;
        }
        /// <summary>
        /// 设置UA头
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public QuickHttp SetUserAgent(string userAgent)
        {
            this._userAgent = userAgent;
            return this;
        }
        /// <summary>
        /// 设置Accept 默认为[application/json]
        /// </summary>
        /// <param name="accept"></param>
        /// <returns></returns>
        public QuickHttp SetAccept(string accept)
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
        public QuickHttp AddHeader(string key, string value)
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
        public QuickHttp AddQuery(string key, string value)
        {
            _params.Add(key, value);
            return this;
        }
        /// <summary>
        /// 移除协议头
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public QuickHttp RemoveHeader(string key)
        {
            _headers.Remove(key);
            return this;
        }
        /// <summary>
        /// 设置重定向, 默认为真
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public QuickHttp SetAutoRedirect(bool value)
        {
            _allowAutoRedirect = value;
            return this;
        }

        /// <summary>
        /// 设置Json序列化设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public QuickHttp SetJsonSerializerSettings(JsonSerializerSettings settings)
        {
            _jsonSerializerSettings = settings;
            return this;
        }
        /// <summary>
        /// 设置引用页面
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public QuickHttp SetReferer(string value)
        {
            AddHeader("Referer", value);
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
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>HttpResponseMessage</returns>
        public Task<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            PackClient();
            var msg = new HttpRequestMessage(HttpMethod.Get, _path);
            if (this._useHttp2)
            {
                msg.Version = new Version(2, 0);
            }
            foreach (var item in _headers.ToList())
            {
                msg.Headers.Add(item.Key, item.Value);
            }
            var result = Client.SendAsync(msg, cancellationToken);
            if (!_useWebProxy)
                Project.HttpPool.Push(_url.ToString(), Client);
            return result;
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <returns>string</returns>
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>string</returns>
        public async Task<string?> GetAsStringAsync(CancellationToken cancellationToken = default)
        {
            HttpResponseMessage? result;
            try
            {
                result = await GetAsync(cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.InnerException.Message);
                }
                else
                {
                    Debug.WriteLine(ex.Message);
                }
                return null;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Request was canceled");
                return null;
            }

            this.ResponseMessage = result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            if (result.Content.Headers.ContentType != null)
            {
                if (result.Content.Headers.ContentType.CharSet == "GB2312")
                {
                    var bytes = await result.Content.ReadAsByteArrayAsync(cancellationToken);
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("GB2312").GetString(bytes);
                }
            }
            return await result.Content.ReadAsStringAsync(cancellationToken);
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <returns>byte[]</returns>
        public async Task<byte[]?> GetAsBytesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await GetAsync(cancellationToken);
                this.ResponseMessage = result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                var t = await result.Content.ReadAsByteArrayAsync(cancellationToken);
                return t;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Request was canceled");
                return null;
            }
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
        public async Task<T?> GetAsObjectAsync<T>(CancellationToken cancellationToken = default)
        {
            var str = await GetAsStringAsync(cancellationToken);
            if (str == null || str.IsNullOrEmpty()) return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str, _jsonSerializerSettings);
        }
        #endregion
        #region Post
        private Task<HttpResponseMessage> PostAsync(HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            PackClient();
            var msg = new HttpRequestMessage(HttpMethod.Post, _path);
            if (_useHttp2)
            {
                msg.Version = new Version(2, 0);
            }
            msg.Content = content;
            foreach (var item in _headers.ToList())
            {
                msg.Headers.Add(item.Key, item.Value);
            }
            var result = Client.SendAsync(msg, cancellationToken);
            if (_useWebProxy == false)
                Project.HttpPool.Push(_url.ToString(), Client);
            result.ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// Post String
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>HttpResponseMessage</returns>
        /// <summary>
        /// Post String
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage?> PostAsync(string body, Encoding? encoding = null, CancellationToken cancellationToken = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            StringContent content;

            try
            {
                JObject.Parse(body);
                content = new StringContent(body, encoding, "application/json");
            }
            catch (JsonReaderException)
            {
                content = new StringContent(body, encoding, "application/x-www-form-urlencoded");
            }
            try
            {
                var result = await PostAsync(content, cancellationToken);
                this.ResponseMessage = result;
                return result;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.InnerException.Message);
                }
                else
                {
                    Debug.WriteLine(ex.Message);
                }
                return null;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Request was canceled");
                return null;
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
            var content = new StringContent(JsonConvert.SerializeObject(body), encoding, "application/json");
            try
            {
                var result = await PostAsync(content);
                this.ResponseMessage = result;
                return result;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.InnerException.Message);
                }
                else
                {
                    Debug.WriteLine(ex.Message);
                }
                return null;
            }

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
            var val = new JsonSerializerSettings
            {
                NullValueHandling = (NullValueHandling)1
            };
            if (JToken == null) JToken = JToken.Parse("{}");
            var str = JsonConvert.SerializeObject(JToken, val);
            var content = new StringContent(str, encoding, MediaTypeHeaderValue.Parse("application/json"));
            try
            {
                var result = await PostAsync(content);
                this.ResponseMessage = result;
                return result;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.InnerException.Message);
                }
                else
                {
                    Debug.WriteLine(ex.Message);
                }
                return null;
            }

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
            try
            {
                var result = await PostAsync(content);
                this.ResponseMessage = result;
                return result;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.InnerException.Message);
                }
                else
                {
                    Debug.WriteLine(ex.Message);
                }
                return null;
            }

        }
        /// <summary>
        /// Post String
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>string</returns>
        public async Task<string?> PostAsStringAsync(string body, Encoding? encoding = null, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync(body, encoding, cancellationToken);
            if (result == null) return null;
            this.ResponseMessage = result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            if (result.Content.Headers.ContentType != null)
            {
                if (result.Content.Headers.ContentType.CharSet == "GB2312")
                {
                    var bytes = await result.Content.ReadAsByteArrayAsync(cancellationToken);
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("GB2312").GetString(bytes);
                }
            }
            return await result.Content.ReadAsStringAsync(cancellationToken);
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
            var obj = JToken.Parse(str);
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
            if (result == null) return null;
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
        /// Post Jtoken
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>JToken</returns>
        public async Task<JToken> PostAsJTokenAsync(JToken body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            var obj = JToken.Parse(str);
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
            if (result == null) return null;
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
        /// Post Byte[]
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Jtoken</returns>
        public async Task<JToken> PostAsJTokenAsync(byte[] body)
        {
            var str = await PostAsStringAsync(body);
            if (str == null) return JToken.Parse("{}");
            var obj = JToken.Parse(str);
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
            if (result == null) return null;
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
        /// Post Object
        /// </summary>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <returns>Jtoken</returns>
        public async Task<JToken> PostAsJTokenAsync(object body, Encoding? encoding = null)
        {
            var str = await PostAsStringAsync(body, encoding);
            if (str == null) return JToken.Parse("{}");
            var obj = JToken.Parse(str);
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
            if (result == null) return null;
            this.ResponseMessage = result;
            if (result.StatusCode != HttpStatusCode.OK)
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
            if (result == null) return null;
            this.ResponseMessage = result;
            if (result.StatusCode != HttpStatusCode.OK)
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
            if (result == null) return null;
            this.ResponseMessage = result;
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
            if (result == null) return null;
            this.ResponseMessage = result;
            if (result.StatusCode != HttpStatusCode.OK)
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
        /// <summary>
        /// Post String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>T</returns>
        public async Task<T?> PostAsObjectAsync<T>(string body, Encoding? encoding = null, CancellationToken cancellationToken = default)
        {
            var str = await PostAsStringAsync(body, encoding, cancellationToken);
            if (str == null || str.IsNullOrEmpty()) return default;
            return JsonConvert.DeserializeObject<T>(str, _jsonSerializerSettings);
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
            return JsonConvert.DeserializeObject<T>(str);
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
            return JsonConvert.DeserializeObject<T>(str);
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
            return JsonConvert.DeserializeObject<T>(str);
        }
        #endregion
        /// <summary>
        /// Cookie 容器
        /// </summary>
        public readonly CookieContainer CookieContainer = new CookieContainer();
        private void PackClient()
        {
            _path = _url.LocalPath; // 重置_path为原始路径
            HttpClient? pooledClient = null;
            if (_useWebProxy == false)
            {
                pooledClient = Project.HttpPool.Pop(_url.ToString());
            }

            if (pooledClient == null)
            {
                // 创建新的HttpClient
                var handler = new SocketsHttpHandler();
                handler.AllowAutoRedirect = _allowAutoRedirect;
                if (_useHttp2)
                {
                    handler.EnableMultipleHttp2Connections = true;
                    handler.MaxConnectionsPerServer = 256;
                }
                if (_sslProtocols != null)
                {
                    handler.SslOptions.EnabledSslProtocols = (System.Security.Authentication.SslProtocols)_sslProtocols;
                }
                if (_useWebProxy && _webProxy != null)
                {
                    handler.Proxy = _webProxy;
                    handler.UseProxy = _useWebProxy;
                }

                handler.AutomaticDecompression = DecompressionMethods.All;
                handler.CookieContainer = CookieContainer;

                if (Client != null) Client.Dispose();
                Client = new HttpClient(handler);
            }
            else
            {
                // 使用池中的HttpClient
                Client = pooledClient;
            }
            // 重新配置客户端（适用于新创建的和从池中获取的）
            Client.BaseAddress = _url;
            Client.Timeout = Other.GetTimeSpan(_timeOut);
            Client.DefaultRequestHeaders.UserAgent.Clear();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.AcceptCharset.Clear();
            Client.DefaultRequestHeaders.UserAgent.TryParseAdd(_userAgent);
            Client.DefaultRequestHeaders.AcceptCharset.TryParseAdd("UTF-8");
            Client.DefaultRequestHeaders.Accept.TryParseAdd(_accept);
            // 在这里添加查询参数
            var query = string.Join("&", _params.ToList().Select(a => $"{a.Key}={Uri.EscapeDataString(a.Value)}"));
            if (!string.IsNullOrEmpty(query))
            {
                if (query.StartsWith("?"))
                {
                    _path += query;
                }
                else
                {
                    _path += "?" + query;
                }

            }
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        public string Cookie
        {
            get
            {
                var cookieCollection = CookieContainer.GetCookies(_url);
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
