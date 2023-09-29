

// FyLib.Winhttp
using System;
using System.Collections.Generic;
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
    }

    /// <summary>
    /// 获取Get文本返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetAsString(string path = "")
    {
        MakeHeader();
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, path);
        foreach (KeyValuePair<string, string> item in Headers.ToList())
        {
            httpRequestMessage.Headers.Add(item.Key, item.Value);
        }
        if (Cookie != "")
        {
            httpRequestMessage.Headers.Add("cookie", Cookie);
        }
        Task<HttpResponseMessage> task = client.SendAsync(httpRequestMessage);
        task.Wait();
        HttpResponseMessage httpResponseMessage = (Response = task.Result);
        StatusCode = httpResponseMessage.StatusCode;
        if (((httpResponseMessage.StatusCode == HttpStatusCode.Found) & Redirect) && httpResponseMessage.Headers.TryGetValues("location", out var values))
        {
            string text = values.ToArray()[0];
            Domain = text;
            return GetAsString(path);
        }
        Task<string> task2 = httpResponseMessage.Content.ReadAsStringAsync();
        task2.Wait();
        return task2.Result;
    }

    /// <summary>
    /// 获取Get 字节集返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] GetAsBytes(string path = "")
    {
        MakeHeader();
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, path);
        foreach (KeyValuePair<string, string> item in Headers.ToList())
        {
            httpRequestMessage.Headers.Add(item.Key, item.Value);
        }
        if (Cookie != "")
        {
            httpRequestMessage.Headers.Add("cookie", Cookie);
        }
        Request = httpRequestMessage;
        Task<HttpResponseMessage> task = client.SendAsync(httpRequestMessage);
        task.Wait();
        HttpResponseMessage httpResponseMessage = Response = task.Result;
        StatusCode = httpResponseMessage.StatusCode;
        if (((httpResponseMessage.StatusCode == HttpStatusCode.Found) & Redirect) && httpResponseMessage.Headers.TryGetValues("location", out var values))
        {
            string text = values.ToArray()[0];
            Domain = text;
            return GetAsBytes(path);
        }
        Task<byte[]> task2 = httpResponseMessage.Content.ReadAsByteArrayAsync();
        task2.Wait();
        return task2.Result;
    }

    /// <summary>
    /// 获取get json返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public JObject GetAsJson(string path = "")
    {
        string asString = GetAsString(path);
        try
        {
            return JObject.Parse(asString);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 获取get 自定义数据
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
        catch (Exception)
        {
            return default(T);
        }
    }

    /// <summary>
    /// Post 返回文本
    /// </summary>
    /// <param name="path"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public string PostAsString(string path, object body)
    {
        //IL_0088: Unknown result type (might be due to invalid IL or missing references)
        //IL_008d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0095: Expected O, but got Unknown
        MakeHeader();
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path);
        foreach (KeyValuePair<string, string> item in Headers.ToList())
        {
            httpRequestMessage.Headers.Add(item.Key, item.Value);
        }
        if (Cookie != "")
        {
            httpRequestMessage.Headers.Add("cookie", Cookie);
        }
        JsonSerializerSettings val = new JsonSerializerSettings
        {
            NullValueHandling = (NullValueHandling)1
        };
        if (body.GetType() == typeof(string))
        {
            httpRequestMessage.Content = new StringContent(body.ToString(), Encoding.UTF8, ContentType);
        }
        else if (body.GetType() == typeof(byte[]))
        {
            httpRequestMessage.Content = new ByteArrayContent(body as byte[]);
        }
        else
        {
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(body, val), Encoding.UTF8, ContentType);
        }
        Request = httpRequestMessage;
        Task<HttpResponseMessage> task = client.SendAsync(httpRequestMessage);
        task.Wait();
        HttpResponseMessage httpResponseMessage = (Response = task.Result);
        StatusCode = httpResponseMessage.StatusCode;
        if (((httpResponseMessage.StatusCode == HttpStatusCode.Found) & Redirect) && httpResponseMessage.Headers.TryGetValues("location", out var values))
        {
            string text = values.ToArray()[0];
            Domain = text;
            return PostAsString(path, body);
        }
        Task<string> task2 = httpResponseMessage.Content.ReadAsStringAsync();
        task2.Wait();
        return task2.Result;
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
            return default(T);
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

    public bool PostFile(string path, string filename)
    {
        if (!File.Exists(filename))
        {
            return false;
        }
        MakeHeader();
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path);
        foreach (KeyValuePair<string, string> item in Headers.ToList())
        {
            httpRequestMessage.Headers.Add(item.Key, item.Value);
        }
        if (Cookie != "")
        {
            httpRequestMessage.Headers.Add("cookie", Cookie);
        }
        MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent("------WebKitFormBoundarygV3AiwcDMOzPLs0P");
        multipartFormDataContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
        string fileName = Path.GetFileName(filename);
        StringContent stringContent = new StringContent("123465");
        stringContent.Headers.ContentType = null;
        multipartFormDataContent.Add(stringContent, "sign");
        stringContent = new StringContent("true");
        stringContent.Headers.ContentType = null;
        multipartFormDataContent.Add(stringContent, "create_media");
        StreamContent streamContent = new StreamContent(File.OpenRead(filename));
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
        multipartFormDataContent.Add(streamContent, "file", fileName);
        httpRequestMessage.Content = multipartFormDataContent;
        client.SendAsync(httpRequestMessage);
        return false;
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
        task.Wait();
        StatusCode = (Response = task.Result).StatusCode;
        return StatusCode == HttpStatusCode.OK;
    }

    private void MakeHeader()
    {
        checked
        {
            if (client == null)
            {
                if (HttpVersion2)
                {
                    client = new HttpClient(new FyHttpHandler());
                    client.BaseAddress = new Uri(domain);
                    client.Timeout = Other.GetTimeSpan(Timeout * 1000);
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                    client.DefaultRequestHeaders.AcceptCharset.TryParseAdd("UTF-8");
                    client.DefaultRequestHeaders.Accept.TryParseAdd(Accept);
                }
                else
                {
                    client = new HttpClient();
                    client.BaseAddress = new Uri(domain);
                    client.Timeout = Other.GetTimeSpan(Timeout * 1000);
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgent);
                    client.DefaultRequestHeaders.AcceptCharset.TryParseAdd("UTF-8");
                    client.DefaultRequestHeaders.Accept.TryParseAdd(Accept);
                }
            }
        }
    }

    internal class FyHttpHandler : WinHttpHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = new Version("2.0");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
