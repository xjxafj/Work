using System;

namespace Trados2019Plugin.Core.Hepler
{
    /// <summary>
    /// Process帮助类
    /// </summary>
    public class ProcessHelper
    {
        /// <summary>
        /// 打开自定的文件目录并选中，文件存在时选中文件
        /// 文件不存在是选中文件所在的目录，目录不存在时
        /// 无操作
        /// </summary>
        /// <param name="fileFullName"></param>
        public static void OpenFolderOrSelectFile(String fileFullName)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileFullName);
                if (fileInfo.Exists)
                {
                    psi.Arguments = "/e,/select," + fileFullName;
                }
                else if (System.IO.Directory.Exists(fileInfo.DirectoryName))
                {
                    psi.Arguments = fileInfo.DirectoryName;
                }
                else
                {
                    //打开失败
                    return;
                }
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                //传入文件路径有误

            }
        }
    }
}
