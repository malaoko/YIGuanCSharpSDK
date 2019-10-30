namespace YiGuanCSharepSDK
{
    public class DebugCollecter : SyncCollecter
    {
        /// <summary>
        /// 构造方法 </summary>
        /// <param name="serverUrl"> 数据接收服务地址 </param>
        public DebugCollecter(string serverUrl) : base(serverUrl, true)
        {
        }
    }
}
