using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FyLib.Http
{
    /// <summary>
    /// HttpClient池
    /// </summary>
    public class HttpClientPool : IDisposable
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<PooledHttpClientInfo>> _clientPool
            = new ConcurrentDictionary<string, ConcurrentQueue<PooledHttpClientInfo>>();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _cleanupTask;

        /// <summary>
        /// 队列长度
        /// </summary>
        public int Length
        {
            get
            {
                return _clientPool.Values.Sum(q => q.Count);
            }
        }

        /// <summary>
        /// HttpClientPool
        /// </summary>
        public HttpClientPool()
        {
            _cleanupTask = Task.Run(async () =>
            {
                await CheckTimeOutAsync();
            }, _cancellationTokenSource.Token);
        }

        private async Task CheckTimeOutAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(5000, _cancellationTokenSource.Token); // 每5秒检查一次

                    var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    foreach (var kvp in _clientPool)
                    {
                        var queue = kvp.Value;
                        var clientsToKeep = new ConcurrentQueue<PooledHttpClientInfo>();

                        while (queue.TryDequeue(out var clientInfo))
                        {
                            // 如果客户端超过180秒未使用，则释放它
                            if (now - clientInfo.LastUsedTime > 180)
                            {
                                clientInfo.Client?.Dispose();
                            }
                            else
                            {
                                clientsToKeep.Enqueue(clientInfo);
                            }
                        }

                        // 将有效的客户端放回队列
                        while (clientsToKeep.TryDequeue(out var clientInfo))
                        {
                            queue.Enqueue(clientInfo);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // 取消请求时正常退出
                    break;
                }
                catch
                {
                    // 忽略其他异常，确保循环继续运行
                }
            }
        }

        /// <summary>
        /// 添加HttpClient到池中
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="client"></param>
        public void Push(string baseUrl, HttpClient client)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentException("BaseUrl cannot be null or empty", nameof(baseUrl));

            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var clientInfo = new PooledHttpClientInfo
            {
                Client = client,
                LastUsedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var queue = _clientPool.GetOrAdd(baseUrl, _ => new ConcurrentQueue<PooledHttpClientInfo>());
            queue.Enqueue(clientInfo);
        }

        /// <summary>
        /// 从池中获取HttpClient
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public HttpClient? Pop(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                return null;

            if (_clientPool.TryGetValue(baseUrl, out var queue))
            {
                if (queue.TryDequeue(out var clientInfo))
                {
                    clientInfo.LastUsedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    return clientInfo.Client;
                }
            }

            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            // 停止清理任务
            _cancellationTokenSource.Cancel();

            try
            {
                // 等待清理任务完成（最多等待1秒）
                _cleanupTask?.Wait(1000);
            }
            catch
            {
                // 忽略等待过程中的异常
            }

            // 释放所有HttpClient实例
            foreach (var queue in _clientPool.Values)
            {
                while (queue.TryDequeue(out var clientInfo))
                {
                    clientInfo.Client?.Dispose();
                }
            }

            _clientPool.Clear();
            _cancellationTokenSource.Dispose();
        }
    }

    internal class PooledHttpClientInfo
    {
        public HttpClient Client { get; set; } = null!;
        public long LastUsedTime { get; set; }
    }
}