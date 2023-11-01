

// FyLib.Winhttp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FyLib;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
///
/// </summary>
public class Winhttp
{
    private HttpClient client;

    /// <summary>
    /// 浏览器类型
    /// </summary>
    public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62";

    /// <summary>
    /// 接收格式. 默认json
    /// </summary>
    public string Accept = "application/json";

    /// <summary>
    /// cookie
    /// </summary>
    public string Cookie = "";

    private string domain = "";

    /// <summary>
    /// 超时
    /// </summary>
    public int Timeout = 120;

    /// <summary>
    /// 是否重定向
    /// </summary>
    public bool Redirect;

    /// <summary>
    /// 返回结构体
    /// </summary>
    public HttpResponseMessage Response = new HttpResponseMessage();

    /// <summary>
    /// 发送结构体
    /// </summary>
    public HttpRequestMessage Request = new HttpRequestMessage();

    /// <summary>
    /// 状态码
    /// </summary>
    public HttpStatusCode StatusCode = HttpStatusCode.OK;

    /// <summary>
    /// 发送类型
    /// </summary>
    public string ContentType = "application/json";

    public bool HttpVersion2 = true;

    /// <summary>
    /// 附加协议头
    /// </summary>
    public Map<string, string> Headers = new Map<string, string>();

    /// <summary>
    /// 基础域名 以http或https开头 /结尾
    /// </summary>
    public string Domain
    {
        get
        {
            return domain;
        }
        set
        {
            domain = value;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public Winhttp()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    private void MakeRequest(HttpRequestMessage request)
    {
        if (HttpVersion2)
        {
            request.Version = new Version("2.0");
        }
        foreach (KeyValuePair<string, string> item in Headers.ToList())
        {
            request.Headers.Add(item.Key, item.Value);
        }
        if (Cookie != "")
        {
            request.Headers.Add("cookie", Cookie);
        }
        Request = request;
    }
    private async Task<HttpResponseMessage> GetResponseAsync(string path)
    {
        MakeHeader();
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, path);
        MakeRequest(httpRequestMessage);
        var response = await client.SendAsync(httpRequestMessage).ConfigureAwait(false);
        Response = response;
        StatusCode = response.StatusCode;
        return response;
    }
    /// <summary>
    /// 获取Get文本返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetAsString(string path = "")
    {
        var result = GetAsStringAsync(path).Result;
        return result;
    }
    /// <summary>
    /// 获取返回文本
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<string> GetAsStringAsync(string path)
    {
        var response = await GetResponseAsync(path);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
    /// <summary>
    /// 获取返回bytes
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<byte[]> GetAsBytesAsync(string path)
    {
        var response = await GetResponseAsync(path);
        var result = await response.Content.ReadAsByteArrayAsync();
        return result;
    }
    /// <summary>
    /// 获取Get 字节集返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] GetAsBytes(string path = "")
    {
        var result = GetAsBytesAsync(path).Result;
        return result;
    }

    /// <summary>
    /// 获取get json返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public JObject GetAsJson(string path = "")
    {
        
        var result = GetAsString(path);
        try
        {
            return JObject.Parse(result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
    /// <summary>
    /// 获取Json返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<JObject> GetAsJsonAsync(string path)
    {
        var result = await GetAsStringAsync(path);
        try
        {
            return JObject.Parse(result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T GetAsObject<T>(string path = "")
    {
        string asString = GetAsString(path);
        try
        {
            return JsonConvert.DeserializeObject<T>(asString);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return default;
        }
    }
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<T> GetAsObjectAsync<T>(string path)
    {
        string asString = await GetAsStringAsync(path);
        try
        {
            return JsonConvert.DeserializeObject<T>(asString);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return default;
        }
    }



    /// <summary>
    /// Post 返回文本
    /// </summary>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <returns>String</returns>
    public async Task<string> PostAsStringAsync(string path,object body)
    {
        if(body==null) return await GetAsStringAsync(path);
        MakeHeader();
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path);
        MakeRequest(httpRequestMessage);
        JsonSerializerSettings val = new JsonSerializerSettings
        {
            NullValueHandling = (NullValueHandling)1
        };
        if (body.GetType() == typeof(string))
        {
            httpRequestMessage.Content = new StringContent((string)body, Encoding.UTF8, ContentType);
        }
        else if (body.GetType() == typeof(byte[]))
        {
            httpRequestMessage.Content = new ByteArrayContent((byte[])body);
        }
        else
        {
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(body, val), Encoding.UTF8, ContentType);
        }
        HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage).ConfigureAwait(false);
        Response = httpResponseMessage;
        var result  = await httpResponseMessage.Content.ReadAsStringAsync();
        return result;

    }
    /// <summary>
    /// Post 返回文本
    /// </summary>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public string PostAsString(string path, object body)
    {
        var result = PostAsStringAsync(path, body).Result;
        return result;
    }
    /// <summary>
    /// Post
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <returns>Object</returns>
    public async Task< T> PostAsObjectAsync<T>(string path, object body)
    {
        string text = await PostAsStringAsync(path, body);
        try
        {
            return JsonConvert.DeserializeObject<T>(text);
        }
        catch (Exception)
        {
            return default;
        }
    }
    /// <summary>
    /// Post 返回自定义对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public T PostAsObject<T>(string path, object body)
    {
        string text = PostAsString(path, body);
        try
        {
            return JsonConvert.DeserializeObject<T>(text);
        }
        catch (Exception)
        {
            return default;
        }
    }
    /// <summary>
    /// Post
    /// </summary>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <returns>JObject</returns>
    public async Task< JObject> PostAsJsonAsync(string path, object body)
    {
        string text = await PostAsStringAsync(path, body);
        try
        {
            return JObject.Parse(text);
        }
        catch (Exception)
        {
            return null;
        }

    }
    /// <summary>
    /// Post 返回Json
    /// </summary>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public JObject PostAsJson(string path, object body)
    {
        string text = PostAsString(path, body);
        try
        {
            return JObject.Parse(text);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Head请求, 一般用于判断是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool Head(string path = "")
    {
        MakeHeader();
        HttpRequestMessage request = (Request = new HttpRequestMessage(HttpMethod.Head, path));
        Task<HttpResponseMessage> task = client.SendAsync(request);
        task.ConfigureAwait(false);
        task.Wait();
        StatusCode = (Response = task.Result).StatusCode;
        return StatusCode == HttpStatusCode.OK;
    }

    private void MakeHeader()
    {
            if (client == null)
            {
                SocketsHttpHandler handler = new SocketsHttpHandler();
                handler.AllowAutoRedirect = Redirect;
                handler.EnableMultipleHttp2Connections = true;
                handler.AutomaticDecompression = System.Net.DecompressionMethods.All;
                client = new HttpClient(handler);
                client.BaseAddress = new Uri(domain);
                client.Timeout = Other.GetTimeSpan(Timeout*1000);
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                client.DefaultRequestHeaders.AcceptCharset.TryParseAdd("UTF-8");
                client.DefaultRequestHeaders.Accept.TryParseAdd(Accept);
            }
    }
}
