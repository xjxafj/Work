using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MagicKit
{
    class WindowsHelper
    {
        /// <summary>
        /// 删除计算机指定进程
        /// </summary>
        /// <param name="processName"></param>
        public static void KillProcessApp(String processName)
        {
            var processes = Process.GetProcesses();
            IEnumerable<Process> wordProcesses = processes.Where(o =>
                 //o.ProcessName.IndexOf("POWERPNT", StringComparison.OrdinalIgnoreCase) >= 0
                 o.ProcessName.IndexOf(processName, StringComparison.OrdinalIgnoreCase) >= 0
            );
            wordProcesses.ToList().ForEach((i) => { i.Kill(); });
        }

        /// <summary>
        /// 判断计算机是否在运行指定进程
        /// </summary>
        /// <param name="processName"></param>
        public static bool IsExistsProcessApp(String processName)
        {
            bool result = false;
            var processes = Process.GetProcesses();
            IEnumerable<Process> wordProcesses = processes.Where(o =>
                 //o.ProcessName.IndexOf("POWERPNT", StringComparison.OrdinalIgnoreCase) >= 0
                 o.ProcessName.IndexOf(processName, StringComparison.OrdinalIgnoreCase) >= 0
            );
            wordProcesses.ToList().ForEach((i) =>
            {
                result = true;
                //break;
            });
            return result;
        }
        
        
         /// <summary>
        /// 判断计算机是否在运行指定进程
        /// </summary>
        /// <param name="processName"></param>
        public static bool Is64BitOperatingSystem(String processName)
        {
            bool result = System.Environment.Is64BitOperatingSystem;
            
            return result;
        }
        
        
        /// <summary>
        /// 运行cmd命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="isShowWindows"></param>
        /// <returns></returns>
        public static bool RunCmd(string cmd, bool isShowWindows)
        {
            bool result = false;
            try
            {
                //实例化一个进程类 
                Process p = new Process();
                //获得系统信息，使用的是 systeminfo.exe 这个控制台程序 
                p.StartInfo.FileName = "cmd.exe";
                //将cmd的标准输入和输出全部重定向到.NET的程序里  
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true; //标准输入 
                //p.StartInfo.RedirectStandardOutput = true; //标准输出  
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                //p.EnableRaisingEvents = true;
                //p.Exited += (s, e) => AfterInstallTool();
                //不显示命令行窗口界面 
                p.StartInfo.CreateNoWindow = isShowWindows;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                p.Start(); //启动进程              
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.WriteLine("exit");
                //Console.WriteLine("ddd");
                p.WaitForExit();//等待控制台程序执行完成
                var errorStr = p.StandardError.ReadToEnd();
                //var msg= p.StandardOutput.ReadToEnd();
                if (String.IsNullOrEmpty(errorStr))
                {
                    result = true;
                }
                p.Close();//关闭该进程 

            }
            catch (Exception)
            {

                result = false;
            }
            return result;
            //if (string.IsNullOrEmpty(errorStr))
            //    return true;
            //else
            //{
            //    Console.WriteLine(errorStr);
            //    return false;
            //}
        }
    }
}
