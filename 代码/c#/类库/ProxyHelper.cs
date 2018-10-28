using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TranslatorTM.Office
{
   public class ProxyHelper
    {
        public static readonly string CATProxySetFile = @"D:\CATProxySet.txt";

        private static IWebProxy myproxy = null;
        public static IWebProxy GetMyProxy()
        {
            if (myproxy == null)
            {
                //string fileName = @"D:\CATProxySet.dat";
                //if (File.Exists(fileName))
                //{
                //    Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //    BinaryFormatter binFormat = new BinaryFormatter();
                //    Proxy result = (Proxy)binFormat.Deserialize(fStream);
                //    fStream.Close();
                //    return result;
                //}
                try
                {
                    if (File.Exists(CATProxySetFile))
                    {
                        string[] lines = File.ReadAllLines(CATProxySetFile, Encoding.UTF8);
                        if (lines != null && lines.Length > 0)
                        {
                            string[] pps = lines[0].Split('@');
                            myproxy = new WebProxy(pps[0], true);
                            if (pps.Length > 1)
                            {
                                string[] uus = pps[1].Split(':');
                                if (uus.Length > 1)
                                    myproxy.Credentials = new NetworkCredential(uus[0], uus[1]);
                            }
                        }

                    }

                }
                catch (Exception)
                {


                }
                if (myproxy == null)
                    myproxy = WebRequest.GetSystemWebProxy();
            }
            return myproxy;
        }
    }
}
