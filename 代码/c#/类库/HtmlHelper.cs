using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Trados2015Plugin
{
   public static class HtmlHelper
    {
        /// <summary>
        /// Post方式请求一个URL地址
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="objP">对象作为参数(将对象转换成json字符串后以字节流传输到服务器)</param>
        /// <returns>str</returns>
        public static string PostString(string url, object objP)
        {
            JavaScriptSerializer j = new JavaScriptSerializer();
            string jsonStr = j.Serialize(objP);
            Encoding EncodeUTF_8 = Encoding.GetEncoding("UTF-8");
            byte[] postBytes = Encoding.UTF8.GetBytes(jsonStr);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.Proxy = TranslatorTM.Office.ProxyHelper.GetMyProxy();
            webRequest.ContentType = "application/json;charset=UTF-8";
            //webRequest.ContentType = "application / x - www - form - urlencoded;charset=UTF-8";
            //webRequest.Proxy = new WebProxy("proxy.huawei.com:8080", true);
            //webRequest.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            webRequest.Timeout = 5000;
            try
            {
                if (objP != null)
                {
                    webRequest.ContentLength = postBytes.Length;
                    using (Stream reqStream = webRequest.GetRequestStream())//写入参数
                    {
                        reqStream.Write(postBytes, 0, postBytes.Length);
                    }
                }
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), EncodeUTF_8))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                return "无法连接到服务器\r\n错误信息：" + ex.Message;
            }
        }

        /// <summary>
        /// Get方式请求一个URL地址(无需参数访问)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie">如果网页需要登录后才能访问时,请传入cookie</param>
        /// <returns></returns>
        public static string GetString(string url, CookieContainer cookie = null)
        {
            Encoding myEncode = Encoding.GetEncoding("UTF-8");
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            if (cookie != null)
            {
                webRequest.CookieContainer = cookie;
            }
            webRequest.Method = "GET";
            webRequest.Proxy = TranslatorTM.Office.ProxyHelper.GetMyProxy();
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            webRequest.Timeout = 20000;
            webRequest.KeepAlive = true;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    if (cookie != null)
                    {
                        response.Cookies = cookie.GetCookies(webRequest.RequestUri);
                    }
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }
            }
            catch (WebException ex)
            {
                return "无法连接到服务器\r\n错误信息：" + ex.Message;
            }
        }


        public static string GetResultByGet(string url)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            webClient.Proxy = TranslatorTM.Office.ProxyHelper.GetMyProxy();
            return webClient.DownloadString(url);
        }
    }
}
