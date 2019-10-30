using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Text;

namespace YiGuanCSharepSDK
{
	public class AnalysysEncoder
    {
        public const string GZIP_ENCODE_UTF_8 = "UTF-8";

        /// <summary>
        /// 编码 </summary>
        /// <param name="str"> 待处理字符串 </param>
        /// <returns> 字节数组 </returns>
        public static sbyte[] compress(string str)
        {
            return compress(str, GZIP_ENCODE_UTF_8);
        }

        /// <summary>
        /// 解码 </summary>
        /// <param name="bytes"> 待处理字节 </param>
        /// <returns> 解码后的字符串 </returns>
        /// <exception cref="IOException"> IO异常 </exception>
        public static string uncompress(sbyte[] bytes)
        {
            return uncompress(bytes, GZIP_ENCODE_UTF_8);
        }

        /// <summary>
        /// 对字符按照指定编码格式进行编码 </summary>
        /// <param name="str"> 待处理字符串 </param>
        /// <param name="encoding"> 编码格式 </param>
        /// <returns> 编码后的字节数组 </returns>
        public static sbyte[] compress(string str, string encoding)
        {
            if (string.ReferenceEquals(str, null) || str.Length == 0)
            {
                return null;
            }
            System.IO.MemoryStream @out = new System.IO.MemoryStream();
            GZipOutputStream gzip;
            try
            {
                gzip = new GZipOutputStream(@out);
                gzip.Write(Encoding.UTF8.GetBytes(str),0,str.Length);
                gzip.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return (sbyte[])(object)@out.ToArray();
        }

        /// <summary>
        /// 对字节按照指定编码格式进行解码 </summary>
        /// <param name="bytes"> 待处理字节码 </param>
        /// <param name="encoding"> 编码格式 </param>
        /// <returns> 解码后的字符串 </returns>
        /// <exception cref="IOException"> IO异常 </exception>
        public static string uncompress(sbyte[] bytes, string encoding)
        {
            //System.IO.MemoryStream @out = new System.IO.MemoryStream();
            //System.IO.MemoryStream @in = new System.IO.MemoryStream((object)bytes as byte[]);
            //GZipInputStream ungzip = new GZipInputStream(@in);
            //sbyte[] buffer = new sbyte[256];
            //int n;
            //while ((n = ungzip.Read((object)buffer as byte[],0,256)) >= 0)
            //{
            //    @out.Write((object)buffer as byte[], 0, n);
            //}
            //byte[] b = @out.ToArray();

            //return Encoding.UTF8.GetString(b, 0, b.Length);
            
            System.IO.MemoryStream @out = new System.IO.MemoryStream();
            byte[] inbytes = new byte[bytes.Length];
            Buffer.BlockCopy(bytes, 0, inbytes, 0, bytes.Length);
            System.IO.MemoryStream @in = new System.IO.MemoryStream(inbytes);
            Stream ungzip = new GZipInputStream(@in);
            int size = 256;
            byte[] buffer = new byte[size];

            while (true)
            {
                size = ungzip.Read(buffer, 0, size);
                if (size > 0)
                {
                    @out.Write(buffer, 0, size);
                }
                else
                {
                    break;
                }
            }

            ungzip.Close();
            byte[] b = @out.ToArray();
            return Encoding.UTF8.GetString(b, 0, b.Length);

        }

        /// <summary>
        /// Base64解码 </summary>
        /// <param name="base64str"> 待处理的Base64编码的字符串 </param>
        /// <returns> 解码后的字符串 </returns>
        public static string ozBase64ToStr(string base64str)
        {
            string base64Codep;
            try
            {
                base64Codep = StringHelperClass.NewString(AnalysysEncoder.decode(base64str));
                return base64Codep;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Base64编码 </summary>
        /// <param name="str"> 待处理字符串 </param>
        /// <returns> 编码后的字符串 </returns>
        public static string ozStrToBase64(string str)
        {
            string basestr = AnalysysEncoder.encode(str.GetBytes());
            return basestr;
        }

        private static char[] base64EncodeChars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/' };

        private static sbyte[] base64DecodeChars = new sbyte[] { (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, 62, (sbyte)-1, (sbyte)-1, (sbyte)-1, 63, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1, (sbyte)-1 };

        public static string encode(sbyte[] data)
        {
            StringBuilder sb = new StringBuilder();
            int len = data.Length;
            int i = 0;
            int b1, b2, b3;
            while (i < len)
            {
                b1 = data[i++] & 0xff;
                if (i == len)
                {
                    sb.Append(base64EncodeChars[(int)((uint)b1 >> 2)]);
                    sb.Append(base64EncodeChars[(b1 & 0x3) << 4]);
                    sb.Append("==");
                    break;
                }
                b2 = data[i++] & 0xff;
                if (i == len)
                {
                    sb.Append(base64EncodeChars[(int)((uint)b1 >> 2)]);
                    sb.Append(base64EncodeChars[((b1 & 0x03) << 4) | ((int)((uint)(b2 & 0xf0) >> 4))]);
                    sb.Append(base64EncodeChars[(b2 & 0x0f) << 2]);
                    sb.Append("=");
                    break;
                }
                b3 = data[i++] & 0xff;
                sb.Append(base64EncodeChars[(int)((uint)b1 >> 2)]);
                sb.Append(base64EncodeChars[((b1 & 0x03) << 4) | ((int)((uint)(b2 & 0xf0) >> 4))]);
                sb.Append(base64EncodeChars[((b2 & 0x0f) << 2) | ((int)((uint)(b3 & 0xc0) >> 6))]);
                sb.Append(base64EncodeChars[b3 & 0x3f]);
            }
            return sb.ToString();
        }
        public static sbyte[] decode(string str)
        {
            StringBuilder sb = new StringBuilder();
            sbyte[] data = str.GetBytes(Encoding.ASCII);
            int len = data.Length;
            int i = 0;
            int b1, b2, b3, b4;
            try
            {
                while (i < len)
                {
                    do
                    {
                        b1 = base64DecodeChars[data[i++]];
                    } while (i < len && b1 == -1);

                    if (b1 == -1)
                    {
                        break;
                    }

                    do
                    {
                        b2 = base64DecodeChars[data[i++]];
                    } while (i < len && b2 == -1);

                    if (b2 == -1)
                    {
                        break;
                    }

                    sb.Append((char) ((b1 << 2) | ((int) ((uint) (b2 & 0x30) >> 4))));
                    do
                    {
                        b3 = data[i++];
                        if (b3 == 61)
                        {
                            return sb.ToString().GetBytes("iso8859-1");
                            //return sb.ToString().GetBytes("utf-8");
                        }

                        b3 = base64DecodeChars[b3];
                    } while (i < len && b3 == -1);

                    if (b3 == -1)
                    {
                        break;
                    }

                    sb.Append((char) (((b2 & 0x0f) << 4) | ((int) ((uint) (b3 & 0x3c) >> 2))));
                    do
                    {
                        b4 = data[i++];
                        if (b4 == 61)
                        {
                            return sb.ToString().GetBytes("iso8859-1");
                           //return sb.ToString().GetBytes("utf-8");
                        }

                        b4 = base64DecodeChars[b4];
                    } while (i < len && b4 == -1);

                    if (b4 == -1)
                    {
                        break;
                    }

                    sb.Append((char) (((b3 & 0x03) << 6) | b4));
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            return sb.ToString().GetBytes("iso8859-1");
            //return sb.ToString().GetBytes("utf-8");
        }
    }
}
