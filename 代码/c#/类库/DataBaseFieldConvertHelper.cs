using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Huawei.CommonService.Client.HR;
using Huawei.CommonService.Client;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace DataService.Common
{
    /// <summary>
    /// 数据服务相关配置
    /// </summary>
    public static class General
    {
     

        public static string dateFormat = "YYYY-MM-DD HH24:MI:SS";
 
        /// <summary>
        /// 获得字符串的md5值
        /// </summary>
        /// <param name="sDataIn"></param>
        /// <returns></returns>
        static public string GetMD5WithString(string sDataIn)
        {
            string str = "";
            byte[] data = Encoding.GetEncoding("utf-8").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                str += bytes[i].ToString("x2");
            }
            return str;
        }

        /**
         * 利用正则表达式判断字符串是否是数字
         * @param str
         * @return
         */
        public static bool IsInt(string value)
        {
            string pattern = @"^\d+$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 数据库字段转换成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DBFieldToObjField<T>(this object obj)
        {

            object d = null;
            if (typeof(T) == typeof(bool))
            {
                if (DBNull.Value == obj || null == obj)
                {
                    d = false;
                }
                else
                {
                    d = "True".Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase) || "1".Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
            else if (typeof(T) == typeof(string))
            {
                if (DBNull.Value == obj || null == obj)
                {
                    d = string.Empty;
                }
                else
                {
                    d = obj.ToString();
                }
            }
            else if (typeof(T) == typeof(int))
            {
                int rr = 0;
                if (DBNull.Value != obj && null != obj)
                {
                    int.TryParse(obj.ToString(), out rr);
                }
                d = rr;
            }
            else if (typeof(T) == typeof(Double))
            {
                Double rr = 0;
                if (DBNull.Value != obj && null != obj)
                {
                   rr= Convert.ToDouble(obj.ToString());
                }
                d = rr;
            }
            else if (typeof(T) == typeof(Guid))
            {
                Guid rr = Guid.Empty;
                if (DBNull.Value != obj && null != obj)
                {
                    Guid.TryParse(obj.ToString(), out rr);
                }
                d = rr;
            }
            else if (typeof(T) == typeof(Decimal))
            {
                Decimal rr = 0;
                if (DBNull.Value != obj && null != obj)
                {
                   rr=Convert.ToDecimal(obj.ToString());
                }
                d = rr;
            }

            else if (typeof(T) == typeof(DateTime))
            {
                DateTime rr = DateTime.MinValue;
                try
                {
                    if (DBNull.Value != obj && null != obj)
                    {
                        DateTime.TryParse(obj.ToString(), out rr);
                    }
                }
                catch (Exception ex)
                {

                    //Common.Util.WriteLog("Ex" + ex.Message);
                }
                d = rr;
            }
            else if (typeof(T) == typeof(List<string>))
            {
                List<string> list = new List<string>();
                try
                {
                    list= JsonHelper.jc.Deserialize<List<string>>(obj.ToString());
                }
                catch (Exception ex)
                {
                    list = new List<string>();
                   
                }
                d = list;
            }
            else if (typeof(T) == typeof(Dictionary<string,string>))
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                try
                {
                    list = JsonHelper.jc.Deserialize<Dictionary<string, string>>(obj.ToString());
                }
                catch (Exception ex)
                {
                    list =new Dictionary<string, string>();

                }
                d = list;
            }
            return (T)d;
        }

        /// <summary>
        /// 数据库存储Null转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object SqlNull(this object obj)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }
            return obj;
        }

        /// <summary>
        /// 字符串转换成十六进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }

        /// <summary>
        /// 十六进制转换成字符串
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }


        /// <summary>
        /// 将文件转换成UTF-8不带Bom
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="encoding"></param>
        public static void ChangeEncoding(string filename, System.Text.Encoding encoding)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] flieByte = new byte[fs.Length];
            fs.Read(flieByte, 0, flieByte.Length);
            fs.Close();
            StreamWriter docWriter;
            System.Text.Encoding ec = System.Text.Encoding.GetEncoding("UTF-8");
            docWriter = new StreamWriter(filename, false, new UTF8Encoding(false));
            string v = encoding.GetString(flieByte);
            docWriter.Write(v);
            docWriter.Close();
        }


    }

   
}
