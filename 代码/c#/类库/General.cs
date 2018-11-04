using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
//
namespace ProjectAutomationCreateWeb.Helper
{
    /// <summary>
    /// 常用方法
    /// </summary>
    public static class General
    {
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

        public static T DBFieldToObjField<T>(this object obj)
        {

            object d = null;
            if(typeof(T) == typeof(bool))
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
                    rr = Convert.ToDouble(obj.ToString());
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

    }
}
