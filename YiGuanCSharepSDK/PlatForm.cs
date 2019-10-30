using System.Collections.Generic;

namespace YiGuanCSharepSDK
{
    public sealed class PlatForm
    {
        //Java
        public static readonly PlatForm Java = new PlatForm("Java", InnerEnum.Java, "Java");
        //python
        public static readonly PlatForm python = new PlatForm("python", InnerEnum.python, "python");
        //JS
        public static readonly PlatForm JS = new PlatForm("JS", InnerEnum.JS, "JS");
        //Node
        public static readonly PlatForm Node = new PlatForm("Node", InnerEnum.Node, "Node");
        //PHP
        public static readonly PlatForm PHP = new PlatForm("PHP", InnerEnum.PHP, "PHP");
        //WeChat
        public static readonly PlatForm WeChat = new PlatForm("WeChat", InnerEnum.WeChat, "WeChat");
        //Android
        public static readonly PlatForm Android = new PlatForm("Android", InnerEnum.Android, "Android");
        //iOS
        public static readonly PlatForm iOS = new PlatForm("iOS", InnerEnum.iOS, "iOS");
        public static readonly PlatForm DotNet = new PlatForm("DotNet", InnerEnum.DotNet, "DotNet");

        private static readonly IList<PlatForm> valueList = new List<PlatForm>();

        static PlatForm()
        {
            valueList.Add(Java);
            valueList.Add(python);
            valueList.Add(JS);
            valueList.Add(Node);
            valueList.Add(PHP);
            valueList.Add(WeChat);
            valueList.Add(Android);
            valueList.Add(iOS);
            valueList.Add(DotNet);
        }

        public enum InnerEnum
        {
            Java,
            python,
            JS,
            Node,
            PHP,
            WeChat,
            Android,
            iOS,
            DotNet
        }

        public readonly InnerEnum innerEnumValue;
        private readonly string nameValue;
        private readonly int ordinalValue;
        private static int nextOrdinal = 0;
        private readonly string value;
        private PlatForm(string name, InnerEnum innerEnum, string value)
        {
            this.value = value;

            nameValue = name;
            ordinalValue = nextOrdinal++;
            innerEnumValue = innerEnum;
        }
        public string Value
        {
            get
            {
                return value;
            }
        }

        public static IList<PlatForm> values()
        {
            return valueList;
        }

        public int ordinal()
        {
            return ordinalValue;
        }

        public override string ToString()
        {
            return nameValue;
        }

        public static PlatForm valueOf(string name)
        {
            foreach (PlatForm enumInstance in PlatForm.valueList)
            {
                if (enumInstance.nameValue == name)
                {
                    return enumInstance;
                }
            }
            throw new System.ArgumentException(name);
        }
    }
}
