using System.Collections.Generic;

namespace YiGuanCSharepSDK
{
    public interface Collecter
    {
        /// <summary>
        /// 发送/缓存消息 </summary>
        /// <param name="message"> 消息 </param>
        /// <returns> 是否成功 </returns>
        bool send(IDictionary<string, object> message);

        /// <summary>
        /// 上报数据 </summary>
        void upload();

        /// <summary>
        /// 刷新缓存 </summary>
        void flush();

        /// <summary>
        /// 关闭 </summary>
        void close();

        /// <summary>
        /// DEBUG </summary>
        /// <param name="debug"> 是否debug </param>
        void debug(bool debug);
    }
}
