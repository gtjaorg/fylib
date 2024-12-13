using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FyLib.Http
{
    /// <summary>
    /// HttpClient池
    /// </summary>
    public class HttpClientPool
    {
        private Lock Lock = new Lock();
        private List<HttpClientInfo> infos = new List<HttpClientInfo>();
        /// <summary>
        /// 队列长度
        /// </summary>
        public int Length { get { return infos.Count; } }
        /// <summary>
        /// HttpClientPool
        /// </summary>
        public HttpClientPool()
        {
            Task.Run(async () =>
            {
                await CheckTimeOut();
            });
        }
        private async Task CheckTimeOut()
        {
            while (true)
            {
                await Task.Delay(50);
                var ls = infos.Where(a => a.Time < Other.TimeStamp() - 180).ToList();
                this.Lock.Enter();
                foreach (var item in ls)
                {
                    infos.Remove(item);
                    item.Client.Dispose();
                }
                this.Lock.Exit();
            }
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~HttpClientPool()
        {
            this.Dispose();
        }
        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            this.Lock.Enter();
            foreach (var item in infos)
            {
                infos.Remove(item);
                item.Client.Dispose();
            }
            this.Lock.Exit();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="BaseUrl"></param>
        /// <param name="client"></param>
        public void Push(string BaseUrl, HttpClient client)
        {
            Lock.Enter();
            infos.Add(new HttpClientInfo()
            {
                Name = BaseUrl,
                Client = client,
                Time = Other.TimeStamp(),
                Status = 0
            });
            this.Lock.Exit();
        }
        /// <summary>
        /// 弹出
        /// </summary>
        /// <param name="BaseUrl"></param>
        /// <returns></returns>
        public HttpClient? Pop(string BaseUrl)
        {
            this.Lock.Enter();
            var client = infos.Where(info => info.Name == BaseUrl && info.Status == 0).FirstOrDefault();
            if (client != null)
            {
                infos.Remove(client);
                this.Lock.Exit();
                return client.Client;
            }
            this.Lock.Exit();
            return null;
        }
    }
    internal class HttpClientInfo
    {
        public string Name { get; set; }
        public HttpClient Client { get; set; }
        public int Time { get; set; }
        public int Status { get; set; }
    }
}
