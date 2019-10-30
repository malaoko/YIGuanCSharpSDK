using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YiGuanCSharepSDK
{
    public class SyncCollecter : Collecter
    {
        private readonly string serverUrl;
        private readonly bool interrupt;
        private bool debug_Renamed;

        /// <summary>
        /// 构造方法 </summary>
        /// <param name="serverUrl"> 数据接收服务地址 </param>
        public SyncCollecter(string serverUrl) : this(serverUrl, false)
        {
        }

        /// <summary>
        /// 构造方法 </summary>
        /// <param name="serverUrl"> 数据接收服务地址 </param>
        /// <param name="interrupt"> 是否中断程序 </param>
        public SyncCollecter(string serverUrl, bool interrupt)
        {
            if (string.ReferenceEquals(serverUrl, null) || serverUrl.Trim().Length == 0)
            {
                throw new Exception("Server URL is empty");
            }
            else
            {
                if (serverUrl.Contains("/up"))
                {
                    serverUrl = serverUrl.Substring(0, serverUrl.IndexOf("/up", StringComparison.Ordinal));
                }
            }
            this.serverUrl = serverUrl + "/up";
            this.interrupt = interrupt;
        }

        public virtual bool send(IDictionary<string, object> egCollectMessage)
        {
            string jsonData = null;
            try
            {
                IList<IDictionary<string, object>> egMsgList = new List<IDictionary<string, object>>();
                egMsgList.Add(egCollectMessage);
                jsonData = JsonConvert.SerializeObject (egMsgList);
                IDictionary<string, string> headParam = new Dictionary<string, string>(1);
                if (debug_Renamed)
                {
                    Console.WriteLine(string.Format("Send message to server: {0} \ndata: {1}", serverUrl, jsonData));
                }
                string retMsg = (new MessageSender(serverUrl, headParam, jsonData)).send();
                if (debug_Renamed)
                {
                    Console.WriteLine(string.Format("Send message success,response: {0}", retMsg));
                }
                return true;
            }
            catch (JsonException e)
            {
                if (interrupt)
                {
                    throw new Exception("Json Serialize Error: ", e);
                }
                else
                {
                    Console.WriteLine("Json Serialize Error: " + e);
                }
            }
            catch (AnalysysException e)
            {
                if (interrupt)
                {
                    throw new Exception("Upload Data Error: ", e);
                }
                else
                {
                    Console.WriteLine("Upload Data Error: " + e);
                }
            }
            catch (IOException e)
            {
                if (interrupt)
                {
                    throw new Exception("Connect Server Error: ", e);
                }
                else
                {
                    Console.WriteLine("Connect Server Error: " + e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sync Send Data Error: " + e);
            }
            return false;
        }

        public virtual void upload()
        {
        }
        public virtual void flush()
        {
        }
        public virtual void close()
        {
        }
        public virtual void debug(bool debug)
        {
            this.debug_Renamed = debug;
        }
    }
}
