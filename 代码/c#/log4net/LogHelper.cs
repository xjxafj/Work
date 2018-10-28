using System;
using System.IO;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace ApplicationLog
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("","loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("","logerror");

        static LogHelper()
        {
            SetConfig();
        }
        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure(null,new Uri(""));
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(null,new FileInfo("te"));
        }

        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);
            }
        }
    }
}
