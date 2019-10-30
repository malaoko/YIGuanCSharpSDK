using System.Collections.Generic;

namespace YiGuanCSharepSDK
{
    public sealed class EventName
    {
        //profile_set
        public static readonly EventName P_SET = new EventName("P_SET", InnerEnum.P_SET, "$profile_set");
        //profile_set_once
        public static readonly EventName P_SET_ONE = new EventName("P_SET_ONE", InnerEnum.P_SET_ONE, "$profile_set_once");
        //profile_increment
        public static readonly EventName P_IN = new EventName("P_IN", InnerEnum.P_IN, "$profile_increment");
        //profile_append
        public static readonly EventName P_APP = new EventName("P_APP", InnerEnum.P_APP, "$profile_append");
        //profile_unset
        public static readonly EventName P_UN = new EventName("P_UN", InnerEnum.P_UN, "$profile_unset");
        //profile_delete
        public static readonly EventName P_DEL = new EventName("P_DEL", InnerEnum.P_DEL, "$profile_delete");
        //alias
        public static readonly EventName ALIAS = new EventName("ALIAS", InnerEnum.ALIAS, "$alias");

        private static readonly IList<EventName> valueList = new List<EventName>();

        static EventName()
        {
            valueList.Add(P_SET);
            valueList.Add(P_SET_ONE);
            valueList.Add(P_IN);
            valueList.Add(P_APP);
            valueList.Add(P_UN);
            valueList.Add(P_DEL);
            valueList.Add(ALIAS);
        }

        public enum InnerEnum
        {
            P_SET,
            P_SET_ONE,
            P_IN,
            P_APP,
            P_UN,
            P_DEL,
            ALIAS
        }

        public readonly InnerEnum innerEnumValue;
        private readonly string nameValue;
        private readonly int ordinalValue;
        private static int nextOrdinal = 0;
        private readonly string value;
        private EventName(string name, InnerEnum innerEnum, string value)
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

        public static IList<EventName> values()
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

        public static EventName valueOf(string name)
        {
            foreach (EventName enumInstance in EventName.valueList)
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
