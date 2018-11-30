using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]//使用注解加载配置
namespace Trados2019Plugin.Core.log
{
    /// <summary>
    /// 日志定义类
    /// </summary>
    public class LogHelper
    {
        #region 定义日志名称
        /// <summary>
        /// 定义日志名称
        /// </summary>
        public static string myLogName = "myLog";
        #endregion

        #region 加载编码方式加载配置
        /// <summary>
        /// 加载编码方式加载配置
        /// </summary>
        static LogHelper()
        {
            //编码方式加载配置
            UseCodeConfig();
        }
        #endregion

        #region 使用编码方式加载配置
        /// <summary>
        /// 使用编码方式加载配置
        /// </summary>
        public static void UseCodeConfig()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();

            //定义日志格式
            patternLayout.ConversionPattern = "[%date] %thread  %-5level %logger - %message%newline";
            //patternLayout.ConversionPattern = "[%date]  %-5level %logger - %message%newline";
            //patternLayout.ConversionPattern = "% property{ log4net: HostName} :: % level:: % message % newlineLogger: % logger % newlineThread: % thread % newlineDate: % date % newlineNDC: % property{ NDC}% newline % newline";
            //HTML样式
            //patternLayout.ConversionPattern = "&lt;HR COLOR=blue&gt;%n日志时间：%d [%t] &lt;BR&gt;%n日志级别：%-5p &lt;BR&gt;%n日 志 类：%c [%x] &lt;BR&gt;%n%m &lt;BR&gt;%n &lt;HR Size=1&gt;";

            patternLayout.ActivateOptions();
            RollingFileAppender roller = new RollingFileAppender();
            //是否追加日志
            roller.AppendToFile = true;
            roller.File = string.Format(@"D:\\Trados 2019\\logs\\ProjectAutomationCreate_log\\CreateProcee{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;

            //创建定义自定义对象
            Logger log = hierarchy.LoggerFactory.CreateLogger(myLogName);
            log.Level = Level.All;
            log.AddAppender(roller);
        }
        #endregion

        #region 使用文件配置log
        /// <summary>
        /// 使用文件配置log
        /// </summary>
        public static void UseFileConfig()
        {
            //使用文件加载配置
            FileInfo configFile = new FileInfo(@"C:\Program Files (x86)\SDL\SDL Trados Studio\Studio15\ProjectAutomationCreate.exe.config");
            if (configFile.Exists)
            {
                log4net.Config.XmlConfigurator.Configure(configFile);
            }
            else
            {
            }
        }
        #endregion

        #region  WriteLog
        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t">日志发生类</param>
        /// <param name="ex"></param>
        public static void WriteLog(Type t, Exception ex)
        {
            var logger = LogManager.GetLogger(t);
            logger.Error("Error", ex);
        }
        /// <summary>
        /// WriteLog
        /// </summary>
        /// <param name="ex">日志异常</param>
        public static void WriteLog(Exception ex)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(myLogName);
            logger.Error("Error", ex);
        }
        #endregion

        #region WriteLog_info
        /// <summary>
        /// info
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="msg">日志内容</param>
        public static void WriteLog_info(Type t, string msg)
        {
            var logger = LogManager.GetLogger(t);
            logger.Info(msg);
        }
        /// <summary>
        /// info
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void WriteLog_info(string msg)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(myLogName);
            logger.Info(msg);
        }
        #endregion

        #region WriteLog_error
        /// <summary>
        /// error
        /// </summary>
        /// <param name="t">日志输出来源类</param>
        /// <param name="ex">日志内容</param>
        public static void WriteLog_error(Type t, Exception ex)
        {
            var logger = LogManager.GetLogger(t);
            logger.Error(ex);
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="ex">异常对象</param>
        public static void WriteLog_error(Exception ex)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(myLogName);
            logger.Error(ex);
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 自定义Log
        /// </summary>
        /// <param name="msg"></param>
        public static void MyWriteLog(string msg)
        {
            try
            {
                string logFloder = @"D:\HuaweiTranslationProvider\Log";
                if (!Directory.Exists(logFloder))
                {
                    Directory.CreateDirectory(logFloder);
                }
                string fileName = string.Format("HuaweiTranslationProviderLog_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                string logPath = Path.Combine(@"D:\HuaweiTranslationProvider\Log", fileName);
                StreamWriter sw = new StreamWriter(logPath, true);
                string result = string.Format("{0} {1}", DateTime.Now.ToString(), msg);
                sw.WriteLine(result);
                sw.Close();
            }
            catch
            {

            }
        }
        #endregion
    }
}

