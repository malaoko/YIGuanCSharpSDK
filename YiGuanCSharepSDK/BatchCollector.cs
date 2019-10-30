
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace YiGuanCSharepSDK
{
    public class BatchCollecter : Collecter
	{
		private long sendTimer = -1;
		private bool isListen = true;
		private readonly string serverUrl;
		private const int DEFAULT_BATCH_NUM = 20;
		private const long DEFAULT_BATCH_SEC = 10;
		private readonly int batchNum;
		private readonly long batchSec;
		private readonly bool interrupt;
		private readonly IList<IDictionary<string, object>> batchMsgList;
		private Thread singleThread;
		private bool debug_Renamed;

		/// <summary>
		/// 构造方法 </summary>
		/// <param name="serverUrl"> 数据接收服务地址 </param>
		public BatchCollecter(string serverUrl) : this(serverUrl, DEFAULT_BATCH_NUM, DEFAULT_BATCH_SEC)
		{
		}

		/// <summary>
		/// 构造方法 </summary>
		/// <param name="serverUrl"> 数据接收服务地址 </param>
		/// <param name="interrupt"> 是否中断程序 </param>
		public BatchCollecter(string serverUrl, bool interrupt) : this(serverUrl, DEFAULT_BATCH_NUM, DEFAULT_BATCH_SEC, interrupt)
		{
		}

		/// <summary>
		/// 构造方法 </summary>
		/// <param name="serverUrl"> 数据接收服务地址 </param>
		/// <param name="batchNum"> 批量发送数量 </param>
		public BatchCollecter(string serverUrl, int batchNum) : this(serverUrl, batchNum, DEFAULT_BATCH_SEC)
		{
		}

		/// <summary>
		/// 构造方法 </summary>
		/// <param name="serverUrl"> 数据接收服务地址 </param>
		/// <param name="batchNum"> 批量发送数量 </param>
		/// <param name="batchSec"> 批量发送等待时间(秒) </param>
		public BatchCollecter(string serverUrl, int batchNum, long batchSec) : this(serverUrl, batchNum, batchSec, false)
		{
		}

		/// <summary>
		/// 构造方法 </summary>
		/// <param name="serverUrl"> 数据接收服务地址 </param>
		/// <param name="batchNum"> 批量发送数量 </param>
		/// <param name="batchSec"> 批量发送等待时间(秒) </param>
		/// <param name="interrupt"> 是否中断程序 </param>
		public BatchCollecter(string serverUrl, int batchNum, long batchSec, bool interrupt)
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
			this.batchNum = batchNum;
			this.batchSec = batchSec * 1000;
			this.batchMsgList = new List<IDictionary<string, object>>(this.batchNum);
            this.singleThread = new Thread(new ThreadStart(run));
		}

        private void run()
        {
            while(isListen)
            {
                try
                {
                    Thread.Sleep(1000);
                }
                catch(Exception e1)
                {
                    Console.WriteLine(e1);
                }
                if(sendTimer!=-1&& (DateTimeHelperClass.CurrentUnixTimeMillis() - sendTimer >= batchSec))
                {
                    try
                    {
                        upload();
                    }
                    catch(Exception )
                    { }
                }
            }
        }

		public virtual bool send(IDictionary<string, object> egCollectMessage)
		{
			lock (batchMsgList)
			{
				if (sendTimer == -1)
				{
					sendTimer = DateTimeHelperClass.CurrentUnixTimeMillis();
				}
				batchMsgList.Add(egCollectMessage);
				string xWhat = "xwhat";
				if (batchMsgList.Count >= batchNum || EventName.ALIAS.Value.Equals(egCollectMessage[xWhat]))
				{
					upload();
				}
			}
			return true;
		}

		public virtual void upload()
		{
			string jsonData = null;
			lock (batchMsgList)
			{
				if (batchMsgList != null && batchMsgList.Count > 0)
				{
					try
					{
                        //jsonData = ValidHandle.EgJsonMapper.writeValueAsString(batchMsgList);
                        jsonData = JsonConvert.SerializeObject(batchMsgList);
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
					}
					catch (JsonException e)
					{
						if (interrupt)
						{
							shutdown();
							throw new Exception("Json Serialize Error", e);
						}
						else
						{
							Console.WriteLine("Json Serialize Error" + e);
						}
					}
					catch (AnalysysException e)
					{
						if (interrupt)
						{
							shutdown();
							throw new Exception("Upload Data Error", e);
						}
						else
						{
							Console.WriteLine("Upload Data Error" + e);
						}
					}
					catch (IOException e)
					{
						if (interrupt)
						{
							shutdown();
							throw new Exception("Connect Server Error", e);
						}
						else
						{
							Console.WriteLine("Connect Server Error" + e);
						}
					}
					catch (Exception e)
					{
						Console.WriteLine("Send Data Error" + e);
					}
					finally
					{
						batchMsgList.Clear();
						resetTimer();
					}
				}
			}
		}

		public virtual void flush()
		{
			upload();
		}

		public virtual void close()
		{
			flush();
			shutdown();
		}

		private void shutdown()
		{
			this.isListen = false;
			try
			{
                this.singleThread.Abort();               
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				this.singleThread = null;
			}
		}

		private void resetTimer()
		{
			this.sendTimer = -1;
		}

		public virtual void debug(bool debug)
		{
			this.debug_Renamed = debug;
		}
	}
}
