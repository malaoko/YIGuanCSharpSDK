using System.Collections.Generic;

namespace YiGuanCSharepSDK
{
    /// <summary>
	/// @author admin
	/// </summary>
	public sealed class DEBUG
    {
        //0
        public static readonly DEBUG CLOSE = new DEBUG("CLOSE", InnerEnum.CLOSE, 0);
        //1
        public static readonly DEBUG OPENNOSAVE = new DEBUG("OPENNOSAVE", InnerEnum.OPENNOSAVE, 1);
        //2
        public static readonly DEBUG OPENANDSAVE = new DEBUG("OPENANDSAVE", InnerEnum.OPENANDSAVE, 2);

        private static readonly IList<DEBUG> valueList = new List<DEBUG>();

        static DEBUG()
        {
            valueList.Add(CLOSE);
            valueList.Add(OPENNOSAVE);
            valueList.Add(OPENANDSAVE);
        }

        public enum InnerEnum
        {
            CLOSE,
            OPENNOSAVE,
            OPENANDSAVE
        }

        public readonly InnerEnum innerEnumValue;
        private readonly string nameValue;
        private readonly int ordinalValue;
        private static int nextOrdinal = 0;
        private int code;
        private DEBUG(string name, InnerEnum innerEnum, int code)
        {
            this.code = code;

            nameValue = name;
            ordinalValue = nextOrdinal++;
            innerEnumValue = innerEnum;
        }
        public int Code
        {
            get
            {
                return code;
            }
        }

        public static IList<DEBUG> values()
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

        public static DEBUG valueOf(string name)
        {
            foreach (DEBUG enumInstance in DEBUG.valueList)
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
