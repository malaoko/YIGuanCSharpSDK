using System;
using System.Collections.Generic;
using System.Windows.Forms;
using YiGuanCSharepSDK;

namespace WindowsFormsApp1
{
    /// <summary>
    /// CSharp SDK 测试
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string appkey = "你的key";
        string dataurl = "你的上数地址";
        
        private void button1_Click(object sender, EventArgs e)
        {
            var analysys = new AnalysysJavaSdk(new SyncCollecter(dataurl), appkey);
            analysys.DebugMode = DEBUG.OPENANDSAVE;
            string distinctid = "13676767888";
            bool islogin = true;
            string eventname = "Pay";
            string platform = "JS";
            var logininfo = new Dictionary<string, object>();
            logininfo.Add("AllPay", 350.00);
            analysys.track(distinctid, islogin, eventname, logininfo, platform);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var analysys = new AnalysysJavaSdk(new SyncCollecter(dataurl), appkey);
            analysys.DebugMode = DEBUG.OPENANDSAVE;
            string distinctid = "13676767888";
            bool islogin = true;
            string platform = "JS";
            var logininfo = new Dictionary<string, object>();
            logininfo.Add("ShengQu","川北省区");
            logininfo.Add("PianQu", "成资片区");
            analysys.profileSet(distinctid, islogin, logininfo, platform);
        }
    }
}
