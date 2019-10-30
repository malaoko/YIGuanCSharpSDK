using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace YiGuanCSharepSDK
{
    public class MessageSender
    {
        private readonly bool? isEncode;
        private readonly string serverUrl;
        private readonly IDictionary<string, string> egHeaderParams;
        private readonly string jsonData;

        /// <summary>
        /// 构造方法 </summary>
        /// <param name="serverUrl"> 数据接收服务地址 </param>
        /// <param name="egHeaderParams"> HTTP消息头信息 </param>
        /// <param name="jsonData"> HTTP消息体 </param>
        public MessageSender(string serverUrl, IDictionary<string, string> egHeaderParams, string jsonData) : this(serverUrl, egHeaderParams, jsonData, false)
        {
            //默认不压缩
        }

        /// <summary>
        /// 构造方法 </summary>
        /// <param name="serverUrl"> 数据接收服务地址 </param>
        /// <param name="egHeaderParams"> HTTP消息头信息 </param>
        /// <param name="jsonData"> HTTP消息体 </param>
        /// <param name="isEncode"> 是否对消息体进行编码,默认true </param>
        public MessageSender(string serverUrl, IDictionary<string, string> egHeaderParams, string jsonData, bool? isEncode)
        {
            this.serverUrl = serverUrl;
            this.egHeaderParams = egHeaderParams;
            this.jsonData = jsonData;
            this.isEncode = isEncode;
        }

        /// <summary>
        /// 发送消息至接收服务器 </summary>
        /// <returns> HttpResponse </returns>
        public virtual string send()
        {
            HttpClient httpclient =new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                httpclient.DefaultRequestHeaders.Add("User-Agent", "Analysys Java SDK");
                if (this.egHeaderParams != null)
                {
                    foreach (KeyValuePair<string, string> entry in this.egHeaderParams.SetOfKeyValuePairs())
                    {
                        httpclient.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                    }
                }
                HttpContent egRequest = null;
                if (isEncode.Value)
                {
                    egRequest = new StringContent(AnalysysEncoder.encode(AnalysysEncoder.compress(jsonData)));
                }
                else
                {
                    egRequest = new StringContent(jsonData);
                    egRequest.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                response = httpclient.PostAsync(this.serverUrl,egRequest).Result;
                string message = response.Content.ReadAsStringAsync().Result;
                //try
                //{
                //    message = AnalysysEncoder.uncompress(AnalysysEncoder.decode(message));
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}
                //printLog(message, jsonData);
                if (response.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    if (!string.ReferenceEquals(message, null) && message.Contains("\"code\":200"))
                    {
                        return message;
                    }
                    else
                    {
                        throw new AnalysysException(message);
                    }
                }
                else
                {
                    throw new AnalysysException(message);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
                if (httpclient != null)
                {
                    httpclient.Dispose();
                }
            }
        }
        private void printLog(string message, string jsonData)
        {
            if (!string.ReferenceEquals(message, null) && !message.Contains("\"code\":200"))
            {
                Console.WriteLine("Data Upload Fail: " + jsonData);
            }
        }      
    }
}
