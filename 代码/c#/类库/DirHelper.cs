using System;
using System.IO;

namespace TradosCommon
{
    /// <summary>
    /// 文件目录帮助类
    /// </summary>
    public class DirHelper
    {
        public static string currAppDir = AppDomain.CurrentDomain.BaseDirectory;
        public static string Local_Dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        public static string Roaming_Dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        public static string LJ_Desktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        public static string ABS_Desktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        public static string ProgramData_Dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        public static string MyDocument_Dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        private static string TempFileDir = @"D:\Trados 2019\Temp";
        private static string ProjectFileDir = @"D:\Trados 2019\Projects";
        private static string DefaultProjectTemplateFilePath = @"D:\Trados 2019\Project Templates\Default.sdltpl";

        public static string GetServiceAddress()
        {
            return IPHelper.GetLocalIP();
        }

        /// <summary>
        /// 获取AEM 临时目录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string GetTempFileDir(string account,string projeceCode)
        {
            //string result = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
            string result =string.Empty;
            try
            {
                if (string.IsNullOrEmpty(projeceCode))
                {
                    projeceCode= DateTime.Now.ToString("yyyyMMddHHmmssfffff");
                }
                string  re = Path.Combine(TempFileDir, account.ToLower(),projeceCode);
                if (!Directory.Exists(re))
                {
                    Directory.CreateDirectory(re);
                }
                if (Directory.Exists(re))
                {
                    result = re;
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        /// <summary>
        /// 获取AEM 项目目录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string GetProjectsFileDir(string account,string projeceCode)
        {

            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(projeceCode))
                {
                    projeceCode = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
                }
                string re = Path.Combine(ProjectFileDir, account.ToLower(), projeceCode);
                if (!Directory.Exists(re))
                {
                    Directory.CreateDirectory(re);
                }
                if (Directory.Exists(re))
                {
                    result = re;
                }
            }
            catch (Exception)
            {
            }
            return result;


           
        }



        /// <summary>
        /// 获取项目模板
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string GetProjectTemplateFilePath(string sourceLang, string[] targetLang)
        {
            return DefaultProjectTemplateFilePath;
        }


        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string GetDir(string dir)
        {
            string result = null;
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (Directory.Exists(dir))
                {
                    result = dir;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }


        
        /// <summary>
        /// 本地路径转换成服务器共享路径
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ServiceDirConvert(string source)
        {
            return source.Replace(new FileInfo(source).Directory.Root.ToString(), string.Format("\\\\{0}\\",IPHelper.GetLocalIP()));
        }

    }
}
