using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YiGuanCSharepSDK
{
    public class AnalysysJavaSdk
    {
        private readonly string SDK_VERSION = "4.1.0";
        private readonly Collecter collecter;
        private readonly string appId;
        private readonly IDictionary<string, object> egBaseProperties;
        private readonly IDictionary<string, object> xcontextSuperProperties;
        private int debugMode = DEBUG.CLOSE.Code;

        /// <summary>
        /// 构造方法 </summary>
        /// <param name="collecter"> 消息收集器 </param>
        /// <param name="appId"> 用户AppId </param>
        public AnalysysJavaSdk(Collecter collecter, string appId) : this(collecter, appId, false)
        {
        }

        /// <summary>
        /// 构造方法 </summary>
        /// <param name="collecter"> 消息收集器 </param>
        /// <param name="appId"> 用户AppId </param>
        /// <param name="autoDelParam"> 是否自动删除校验不通过的属性 </param>
        public AnalysysJavaSdk(Collecter collecter, string appId, bool? autoDelParam)
        {
            this.collecter = collecter;
            this.appId = appId;
            this.egBaseProperties = new Dictionary<string, object>(3);
            this.xcontextSuperProperties = new ConcurrentDictionary<string, object>();
            //ValidHandle.DelNotValidParam = autoDelParam.Value;
            initBaseProperties();
        }

        /// <summary>
        /// Debug模式 </summary>
        /// <param name="debug"> Debug级别 </param>
        public virtual DEBUG DebugMode
        {
            set
            {
                this.debugMode = value.Code;
            }
        }

        private bool Debug
        {
            get
            {
                return debugMode == DEBUG.OPENNOSAVE.Code || debugMode == DEBUG.OPENANDSAVE.Code;
            }
        }

        /// <summary>
        /// 初始化基础属性
        /// </summary>
        public virtual void initBaseProperties()
        {
            this.egBaseProperties.Clear();
            this.egBaseProperties["$lib"] = PlatForm.Java.Value;
            this.egBaseProperties["$lib_version"] = SDK_VERSION;
        }

        /// <summary>
        /// 注册超级属性,注册后每次发送的消息体中都包含该属性值 </summary>
        /// <param name="params"> 属性 </param>
        public virtual void registerSuperProperties(IDictionary<string, object> @params)
        {
            int num = 100;
            if (@params.SetOfKeyValuePairs().Count> num)
            {
                throw new AnalysysException("Too many super properties. max number is 100.");
            }
            //ValidHandle.checkParam("", @params);
            foreach (string key in @params.Keys)
            {
                this.xcontextSuperProperties[key] = @params[key];
            }
            if (Debug)
            {
                Console.WriteLine("registerSuperProperties success");
            }
        }

        /// <summary>
        /// 移除超级属性 </summary>
        /// <param name="key"> 属性key </param>
        public virtual void unRegisterSuperProperty(string key)
        {
            if (this.xcontextSuperProperties.ContainsKey(key))
            {
                this.xcontextSuperProperties.Remove(key);
            }
            if (Debug)
            {
                Console.WriteLine(string.Format("unRegisterSuperProperty {0} success", key));
            }
        }

        /// <summary>
        /// 获取超级属性 </summary>
        /// <param name="key"> 属性KEY </param>
        /// <returns> 该KEY的超级属性值 </returns>
        public virtual object getSuperPropertie(string key)
        {
            if (this.xcontextSuperProperties.ContainsKey(key))
            {
                return this.xcontextSuperProperties[key];
            }
            return null;
        }

        /// <summary>
        /// 获取超级属性 </summary>
        /// <returns> 所有超级属性 </returns>
        public virtual IDictionary<string, object> SuperProperties
        {
            get
            {
                return this.xcontextSuperProperties;
            }
        }

        /// <summary>
        /// 清除超级属性
        /// </summary>
        public virtual void clearSuperProperties()
        {
            this.xcontextSuperProperties.Clear();
            if (Debug)
            {
                Console.WriteLine("clearSuperProperties success");
            }
        }

        /// <summary>
        /// 立即发送所有收集的信息到服务器
        /// </summary>
        public virtual void flush()
        {
            this.collecter.flush();
        }

        public virtual void shutdown()
        {
            this.collecter.close();
        }

        /// <summary>
        /// 设置用户的属性 </summary>
        /// <param name="distinctId"> 用户ID </param>
        /// <param name="isLogin"> 用户ID是否是登录 ID </param>
        /// <param name="properties"> 用户属性 </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void profileSet(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform)
        {
            upload(distinctId, isLogin, EventName.P_SET.Value, properties, platform, null);
        }
        public virtual void profileSet(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform, string xwhen)
        {
            upload(distinctId, isLogin, EventName.P_SET.Value, properties, platform, xwhen);
        }

        /// <summary>
        /// 首次设置用户的属性,该属性只在首次设置时有效 </summary>
        /// <param name="distinctId"> 用户ID </param>
        /// <param name="isLogin"> 用户ID是否是登录 ID </param>
        /// <param name="properties"> 用户属性 </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void profileSetOnce(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform)
        {
            upload(distinctId, isLogin, EventName.P_SET_ONE.Value, properties, platform, null);
        }
        public virtual void profileSetOnce(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform, string xwhen)
        {
            upload(distinctId, isLogin, EventName.P_SET_ONE.Value, properties, platform, xwhen);
        }

        /// <summary>
        /// 为用户的一个或多个数值类型的属性累加一个数值 </summary>
        /// <param name="distinctId"> 用户ID </param>
        /// <param name="isLogin"> 用户ID是否是登录 ID </param>
        /// <param name="properties"> 用户属性 </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void profileIncrement(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform)
        {
            upload(distinctId, isLogin, EventName.P_IN.Value, properties, platform, null);
        }
        public virtual void profileIncrement(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform, string xwhen)
        {
            upload(distinctId, isLogin, EventName.P_IN.Value, properties, platform, xwhen);
        }

        /// <summary>
        /// 追加用户列表类型的属性 </summary>
        /// <param name="distinctId"> 用户ID </param>
        /// <param name="isLogin"> 用户ID是否是登录 ID </param>
        /// <param name="properties"> 用户属性 </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void profileAppend(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform)
        {
            upload(distinctId, isLogin, EventName.P_APP.Value, properties, platform, null);
        }
        public virtual void profileAppend(string distinctId, bool isLogin, IDictionary<string, object> properties, string platform, string xwhen)
        {
            upload(distinctId, isLogin, EventName.P_APP.Value, properties, platform, xwhen);
        }

        /// <summary>
        /// 删除用户某一个属性 </summary>
        /// <param name="distinctId"> 用户ID </param>
        /// <param name="isLogin"> 用户ID是否是登录 ID </param>
        /// <param name="property"> 用户属性名称 </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void profileUnSet(string distinctId, bool isLogin, string property, string platform)
        {
            IDictionary<string, object> properties = new Dictionary<string, object>(2);
            properties[property] = "";
            upload(distinctId, isLogin, EventName.P_UN.Value, properties, platform, null);
        }
        public virtual void profileUnSet(string distinctId, bool isLogin, string property, string platform, string xwhen)
        {
            IDictionary<string, object> properties = new Dictionary<string, object>(2);
            properties[property] = "";
            upload(distinctId, isLogin, EventName.P_UN.Value, properties, platform, xwhen);
        }

        /// <summary>
        /// 删除用户所有属性 </summary>
        /// <param name="distinctId"> 用户ID </param>
        /// <param name="isLogin"> 用户ID是否是登录 ID </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void profileDelete(string distinctId, bool isLogin, string platform)
        {
            upload(distinctId, isLogin, EventName.P_DEL.Value, new Dictionary<string, object>(1), platform, null);
        }
        public virtual void profileDelete(string distinctId, bool isLogin, string platform, string xwhen)
        {
            upload(distinctId, isLogin, EventName.P_DEL.Value, new Dictionary<string, object>(1), platform, xwhen);
        }

        /// <summary>
        /// 关联用户匿名ID和登录ID </summary>
        /// <param name="aliasId"> 用户登录ID </param>
        /// <param name="distinctId"> 用户匿名ID </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void alias(string aliasId, string distinctId, string platform)
        {
            IDictionary<string, object> param = new Dictionary<string, object>(2);
            param["$original_id"] = distinctId;
            upload(aliasId, true, EventName.ALIAS.Value, param, platform, null);
        }
        public virtual void alias(string aliasId, string distinctId, string platform, string xwhen)
        {
            IDictionary<string, object> param = new Dictionary<string, object>(2);
            param["$original_id"] = distinctId;
            upload(aliasId, true, EventName.ALIAS.Value, param, platform, xwhen);
        }

        /// <summary>
        /// 追踪用户多个属性的事件 </summary>
        /// <param name="distinctId"> 用户ID </param>
        /// <param name="isLogin"> 用户ID是否是登录 ID </param>
        /// <param name="eventName"> 事件名称 </param>
        /// <param name="properties"> 事件属性 </param>
        /// <param name="platform"> 平台类型 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        public virtual void track(string distinctId, bool isLogin, string eventName, IDictionary<string, object> properties, string platform)
        {
            upload(distinctId, isLogin, eventName, properties, platform, null);
        }
        public virtual void track(string distinctId, bool isLogin, string eventName, IDictionary<string, object> properties, string platform, string xwhen)
        {
            upload(distinctId, isLogin, eventName, properties, platform, xwhen);
        }

        /// <summary>
        /// 上传数据,首先校验相关KEY和VALUE,符合规则才可以上传 </summary>
        /// <param name="distinctId"> 用户标识 </param>
        /// <param name="isLogin"> 是否登陆 </param>
        /// <param name="eventName"> 事件名称 </param>
        /// <param name="properties"> 属性 </param>
        /// <param name="platform"> 平台类型 </param>
        /// <param name="xwhen"> 时间戳 </param>
        /// <exception cref="AnalysysException"> 自定义异常 </exception>
        private void upload(string distinctId, bool isLogin, string eventName, IDictionary<string, object> properties, string platform, string xwhen)
        {
            Dictionary<string, object> targetProperties = new Dictionary<string, object>();
            if (properties != null)
            {
                foreach(var item in properties)
                {
                    targetProperties.Add(item.Key, item.Value);
                }                
            }
            //ValidHandle.checkProperty(distinctId, eventName, targetProperties, this.xcontextSuperProperties.Count);
            IDictionary<string, object> eventMap = new Dictionary<string, object>(8);
            eventMap["xwho"] = distinctId;
            if (!string.ReferenceEquals(xwhen, null) && xwhen.Trim().Length > 0)
            {
                if (xwhen.Trim().Length != 13)
                {
                    throw new AnalysysException(string.Format("The param xwhen {0} not a millisecond timestamp.", xwhen.Trim()));
                }
                try
                {
                    long when = Convert.ToInt64(xwhen.Trim());

                    //(new SimpleDateFormat("yyyy-MM-dd HH:mm:ss SSS")).format(new DateTime(when));
                    //eventMap["xwhen"] = when;

                    DateTime dtstart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    long ltime = long.Parse(dtstart + "0000");
                    TimeSpan tonow = new TimeSpan(ltime);
                    DateTime dtresult = dtstart.Add(tonow);
                    when =long.Parse( dtresult.ToString("yyyyMMddHHmmssSSS"));
                    eventMap["xwhen"] = when;

                }
                catch (Exception error)
                {
                    throw new AnalysysException(string.Format("The param xwhen {0} not a timestamp.", xwhen.Trim()));
                }
            }
            else
            {
                if (EventName.ALIAS.Value.Equals(eventName))
                {
                    eventMap["xwhen"] = DateTimeHelperClass.CurrentUnixTimeMillis() - 3;
                }
                else
                {
                    eventMap["xwhen"] = DateTimeHelperClass.CurrentUnixTimeMillis();
                    //测试时间
                    //DateTime time = System.DateTime.MinValue;
                    //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    //time = startTime.AddMilliseconds((double)((long)eventMap["xwhen"]));
                }
            }
            eventMap["xwhat"] = eventName;
            eventMap["appid"] = appId;
            IDictionary<string, object> newProperties = new Dictionary<string, object>(16);
            string profile = "$profile";
            if (!eventName.StartsWith(profile, StringComparison.Ordinal) && !eventName.StartsWith(EventName.ALIAS.Value, StringComparison.Ordinal))
            {
                //JAVA TO C# CONVERTER CRACKED BY X-CRACKER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
                foreach(var item in xcontextSuperProperties)
                {
                    newProperties.Add(item.Key, item.Value);
                }
                
            }
            newProperties["$debug"] = debugMode;
            if (targetProperties != null)
            {
                foreach(var item in targetProperties)
                {
                    newProperties.Add(item.Key, item.Value);
                }
            }
            foreach(var item in egBaseProperties)
            {
                newProperties.Add(item.Key, item.Value);
            }

            newProperties["$is_login"] = isLogin;
            string newPlatForm = getPlatForm(platform);
            if (!string.ReferenceEquals(newPlatForm, null) && newPlatForm.Trim().Length > 0)
            {
                newProperties["$platform"] = newPlatForm;
            }
            eventMap["xcontext"] = newProperties;
            this.collecter.debug(Debug);
            bool ret = this.collecter.send(eventMap);
            if (eventName.StartsWith(profile, StringComparison.Ordinal) && Debug && ret)
            {
                Console.WriteLine(string.Format("{0} success.", eventName.Substring(1)));
            }
        }

        private string getPlatForm(string platform)
        {
            if (PlatForm.JS.Value.Equals(platform,StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.JS.Value;
            }
            if (PlatForm.WeChat.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.WeChat.Value;
            }
            if (PlatForm.Android.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.Android.Value;
            }
            if (PlatForm.iOS.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.iOS.Value;
            }
            Console.WriteLine(string.Format("Warning: param platform:{0}  Your input are not:iOS/Android/JS/WeChat.", string.ReferenceEquals(platform, null) ? "" : platform));
            if (PlatForm.Java.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.Java.Value;
            }
            if (PlatForm.python.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.python.Value;
            }
            if (PlatForm.Node.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.Node.Value;
            }
            if (PlatForm.PHP.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.PHP.Value;
            }

            if (PlatForm.DotNet.Value.Equals(platform, StringComparison.OrdinalIgnoreCase))
            {
                return PlatForm.DotNet.Value;
            }
            if (string.ReferenceEquals(platform, null) || platform.Trim().Length == 0)
            {
                Console.WriteLine(string.Format("Warning: param platform is empty, will use default value: {0}.", PlatForm.Java.Value));
                return PlatForm.Java.Value;
            }
            return platform;
        }
    }
}
