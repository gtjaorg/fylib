using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace FyLib
{
    /// <summary>
    /// 写入日志
    /// </summary>
    public class WriteLog
    {
        private string _fileName;

        private static Dictionary<long, long> lockDic = new Dictionary<long, long>();

        /// <summary>
        /// 获取或设置文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        public WriteLog(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Create(string fileName)
        {
            if (!File.Exists(fileName))
            {
                using var fileStream = File.Create(fileName);
                fileStream.Close();
            }
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="content">文本</param>
        /// <param name="newLine">换行</param>
        private void Write(string content, string newLine)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                throw new Exception("FileName不能为空！");
            }
            using var fileStream = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 8, FileOptions.Asynchronous);
            var bytes = Encoding.Default.GetBytes(content + newLine);
            fileStream.Seek(0, SeekOrigin.End);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        /// <summary>
        /// 写入文件内容
        /// </summary>
        /// <param name="content"></param>
        public void WriteLine(string content)
        {
            Write(content, Environment.NewLine);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="content"></param>
        public void Write(string content)
        {
            Write(content, "");
        }
    }
}
