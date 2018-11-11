using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;
using System.Collections.Generic;
using System.Threading;

namespace Trados2019Plugin.Core.Hepler
{
    /// <summary>   
    /// 适用与ZIP压缩   
    /// </summary>   
    public class ZipHelper
    {
        #region 压缩  
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceDir">要压缩的目录</param>
        /// <param name="destinationZipFilePath">保存的zip文件路径</param>
        public static void CreateZip(string sourceDir, string destinationZipFilePath)
        {
            if (sourceDir[sourceDir.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                sourceDir += System.IO.Path.DirectorySeparatorChar;
            ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
            zipStream.SetLevel(6);  // 压缩级别 0-9
            CreateZipFiles(sourceDir, zipStream, sourceDir);
            zipStream.Finish();
            zipStream.Close();
        }

        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
        /// ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream, string staticFile)
        {
            Crc32 crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
            foreach (string file in filesArray)
            {
                if (Directory.Exists(file))                     //如果当前是文件夹，递归
                {
                    CreateZipFiles(file, zipStream, staticFile);
                }
                else                                            //如果是文件，开始压缩
                {
                    try
                    {
                        FileStream fileStream = File.OpenRead(file);
                        byte[] buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, buffer.Length);
                        string tempFile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(tempFile);
                        entry.DateTime = DateTime.Now;
                        entry.Size = fileStream.Length;
                        fileStream.Close();
                        crc.Reset();
                        if (entry.Size != 0)
                        {
                            crc.Update(buffer);
                        }
                        entry.Crc = crc.Value;
                        zipStream.PutNextEntry(entry);
                        zipStream.Write(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {


                    }
                }
            }
        }


        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileNames">要打包的文件列表</param>
        /// <param name="GzipFileName">目标文件名</param>
        /// <param name="CompressionLevel">压缩品质级别（0~9）</param>
        /// <param name="SleepTimer">休眠时间（单位毫秒）</param>
        private static void Compress(List<FileInfo> fileNames, string GzipFileName, int CompressionLevel, int SleepTimer)
        {
            ZipOutputStream s = new ZipOutputStream(File.Create(GzipFileName));
            try
            {
                s.SetLevel(CompressionLevel);   //0 - store only to 9 - means best compression
                foreach (FileInfo file in fileNames)
                {
                    FileStream fs = null;
                    try
                    {
                        fs = file.Open(FileMode.Open, FileAccess.ReadWrite);
                    }
                    catch
                    { continue; }
                    //  方法二，将文件分批读入缓冲区
                    byte[] data = new byte[2048];
                    int size = 2048;
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file.Name));
                    entry.DateTime = (file.CreationTime > file.LastWriteTime ? file.LastWriteTime : file.CreationTime);
                    s.PutNextEntry(entry);
                    while (true)
                    {
                        size = fs.Read(data, 0, size);
                        if (size <= 0) break;
                        s.Write(data, 0, size);
                    }
                    fs.Close();
                    file.Delete();
                    Thread.Sleep(SleepTimer);
                }
            }
            finally
            {
                s.Finish();
                s.Close();
            }
        }
        #endregion

        #region 解压  
        /// <summary>
        /// 解压缩文件(当文件大小为0的文件将跳过)
        /// </summary>
        /// <param name="GzipFile">压缩包文件名</param>
        /// <param name="targetPath">解压缩目标路径 </param>    
        public static void Decompress(string GzipFile, string targetPath, bool IsDelete = false)
        {
            if (File.Exists(GzipFile))
            {
                string directoryName = targetPath;
                if (Directory.Exists(directoryName))
                    Directory.Delete(directoryName, true);
                Directory.CreateDirectory(directoryName);//生成解压目录
                string CurrentDirectory = directoryName;
                byte[] data = new byte[2048];
                int size = 2048;
                ZipEntry theEntry = null;
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(GzipFile)))
                {
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        //if (theEntry.Size == 0)
                        //{
                        //    continue;
                        //}
                        if (theEntry.IsDirectory)
                        {// 该结点是目录
                            if (!Directory.Exists(Path.Combine(CurrentDirectory, theEntry.Name))) Directory.CreateDirectory(Path.Combine(CurrentDirectory, theEntry.Name));
                        }
                        else
                        {
                            if (theEntry.Name != String.Empty)
                            {
                                string filePath = Path.Combine(CurrentDirectory, theEntry.Name);
                                var Direc = Path.GetDirectoryName(filePath);
                                if (!Directory.Exists(Direc))
                                    Directory.CreateDirectory(Direc);
                                //解压文件到指定的目录
                                if (theEntry.Size == 0)
                                {
                                    using (FileStream streamWriter = File.Create(Path.Combine(CurrentDirectory, theEntry.Name)))
                                    {
                                        streamWriter.Close();
                                    }
                                }
                                else
                                {
                                    using (FileStream streamWriter = File.Create(Path.Combine(CurrentDirectory, theEntry.Name)))
                                    {
                                        while (true)
                                        {
                                            size = s.Read(data, 0, data.Length);
                                            if (size <= 0) break;

                                            streamWriter.Write(data, 0, size);
                                        }
                                        streamWriter.Close();
                                    }
                                }
                            }
                        }

                    }
                    s.Close();
                }
                if (IsDelete)
                    File.Delete(GzipFile);
            }
        }
        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <returns>解压结果</returns>   
        public static bool UnZip(string fileToUnZip, string zipedFolder)
        {
            bool result = UnZip(fileToUnZip, zipedFolder, null);
            return result;
        }


        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        public static bool UnZip(string fileToUnZip, string zipedFolder, string password)
        {
            bool result = true;
            FileStream fs = null;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;

            if (!File.Exists(fileToUnZip))
                return false;

            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);

            try
            {
                zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        fileName = Path.Combine(zipedFolder, ent.Name);
                        fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi   

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        fs = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                fs.Write(data, 0, data.Length);
                            else
                                break;
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            return result;
        }
        #endregion
    }
}
