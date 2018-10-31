using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Data;
using System.Data.SqlClient;

namespace ContenCheckClient
{

    public class ExcelHelper
    {
        public int currentIndext = 0;

        public int Commit =60000;

        public int totalCount = 0;

        public int Index = 0;

        public Microsoft.Office.Interop.Excel.Application app;


       public Microsoft.Office.Interop.Excel.Workbook wBook ;
       public Microsoft.Office.Interop.Excel.Worksheet wSheet;

        public ExcelHelper()
        {
            this.app = new Microsoft.Office.Interop.Excel.Application();
            this.wBook = app.Workbooks.Add(true);
            this.wSheet = (Excel.Worksheet)wBook.Sheets[1];
            this.wSheet.Name = "Sheet1";
        }

       /// <summary>
       /// 导入入口
       /// </summary>
       /// <param name="excelTable">数据DataTable</param>
       /// <param name="filePath">导出的文件路径</param>
       /// <returns></returns>
        public bool DTImpotToExcel(DataTable excelTable,string filePath)
        {
            try
            {
                int count = 0;
                bool importReuslt = false;
                bool isFirst = true;
                int insertIndext = 0;
                //设置
                DTImpotToExcelSetting();
                 this.totalCount = excelTable.Rows.Count;
                object[,] objData = new object[1, excelTable.Columns.Count];
                //首先将数据写入到一个二维数组中  
                for (int i = 0; i < excelTable.Columns.Count; i++)
                {
                    objData[0, i] = excelTable.Columns[i].ColumnName;
                }
                //导入表头
                importReuslt= DTImpotToExcelTitle(objData,1);
                if (importReuslt)
                {
                    insertIndext+=objData.GetLength(0);
                }
               
                //导入数据
                if (totalCount > 0)
                {
                    if (totalCount<=Commit)
                    {
                        objData = new object[totalCount, excelTable.Columns.Count];
                    }
                    else
                    {
                        objData = new object[Commit, excelTable.Columns.Count];
                    }
                    for (int i = 0; i < excelTable.Rows.Count; i++)
                    {
                        currentIndext++;
                        //循环对单元格赋值
                        for (int j = 0; j < excelTable.Columns.Count; j++)
                        {
                            if (excelTable.Rows[i][j].Equals(float.NaN))//查询过来的float.NaN
                                objData[count, j] = "-";
                            else if (String.IsNullOrEmpty(excelTable.Rows[i][j].ToString()) && excelTable.Rows[i][j].Equals(DBNull.Value))//有dbnull的数据，需要屏蔽掉——在数据源处理了
                                objData[count, j] = DataFilter(excelTable.Rows[i][j].ToString());
                            else
                                objData[count, j] = DataFilter(excelTable.Rows[i][j].ToString());
                        }
                        count++;
                        if (currentIndext%Commit==0&&!isFirst)
                        {
                            importReuslt = DTImpotToExcel(objData, insertIndext+1);
                            if (importReuslt)
                            {
                                insertIndext += objData.GetLength(0);
                                SaveExcel(filePath);
                                totalCount -= objData.GetLength(0);
                            }
                            if (totalCount <= Commit)
                            {
                                objData = new object[totalCount, excelTable.Columns.Count];
                            }
                            else
                            {
                                objData = new object[Commit, excelTable.Columns.Count];
                            }
                            count = 0;
                        }
                        isFirst = false;
                    }
                }
                if (excelTable.Rows.Count%Commit!=0&& excelTable.Rows.Count>0)
                {
                    importReuslt=DTImpotToExcel(objData, insertIndext+1);
                    if (importReuslt)
                    {
                        insertIndext += objData.GetLength(0);
                        SaveExcel(filePath);
                        totalCount -= objData.GetLength(0);
                    }
                    
                }
                return true;
            }
            catch (Exception err)//这里还有些问题，比如 对方安装的是WPS 不会提示中文错误，没有安装office 也不会弹出该错误
            {
                //强制结束excel进程
                //IntPtr t = new IntPtr(app.Hwnd);
                //int k = 0;
                ////GetWindowThreadProcessId(t, out k);
                //System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
                //p.Kill();
                //return false;
                throw new Exception(err.Message+"\r\n"+err.StackTrace);
            }
            finally
            {
                CloseExcel();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="excelTable"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool DTImpotToExcel(object[,] objData, int startRow)
        {
            int columnCount = objData.GetLength(1);
            int endRow = startRow + objData.GetLength(0) - 1;
            string temContent = string.Empty;
            string startCol = "A";//这里关键，计算要替换的区域
            int iCnt = ((columnCount - 1) / 26);//当列数是26时 不-1 会出现问题，自己试试就明白了
            string endColSignal = (iCnt == 0 ? "" : ((char)('A' + (iCnt - 1))).ToString());
            string endCol = endColSignal + ((char)('A' + columnCount - iCnt * 26 - 1)).ToString();
            Microsoft.Office.Interop.Excel.Range range = wSheet.get_Range(startCol + startRow, endCol + (endRow).ToString());
            range.Value = objData; //给Exccel中的Range整体赋值  
            range.EntireColumn.AutoFit(); //设定Excel列宽度自适应  
            wSheet.get_Range(startCol + "1", endCol + "1").Font.Bold = 1;//Excel文件列名 字体设定为Bold  
            GC.Collect();
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="excelTable"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private void DTImpotToExcelSetting()
        {

            this.app.Visible = false;
            this.app.DisplayAlerts = true;
            //Excel文件列名 字体设定为Bold  
            //设置禁止弹出保存和覆盖的询问提示框   
            this.app.DisplayAlerts = false;
            this.app.AlertBeforeOverwriting = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="excelTable"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool DTImpotToExcelTitle(object[,] objData, int startRow)
        {
            string temContent = string.Empty;
            string startCol = "A";//这里关键，计算要替换的区域
            int columnCount = objData.GetLength(1);
            int endRow = startRow + objData.GetLength(0)-1;
            int iCnt = ((columnCount - 1) / 26);//当列数是26时 不-1 会出现问题，自己试试就明白了
            string endColSignal = (iCnt == 0 ? "" : ((char)('A' + (iCnt - 1))).ToString());
            string endCol = endColSignal + ((char)('A' + columnCount - iCnt * 26 - 1)).ToString();
            Microsoft.Office.Interop.Excel.Range range = wSheet.get_Range(startCol + startRow.ToString(), endCol + (endRow).ToString());
            range.Value = objData; //给Exccel中的Range整体赋值  
            range.EntireColumn.AutoFit(); //设定Excel列宽度自适应  
            wSheet.get_Range(startCol + "1", endCol + "1").Font.Bold = 1;//Excel文件列名 字体设定为Bold  
            GC.Collect();
            return true;
        }




        public void SaveExcel(string filePath)
        {
            if (this.app != null)
            {
                wSheet.SaveAs(filePath);
            }
        }


        public void CloseExcel()
        {
            if (this.app!=null)
            {
                this.app.Quit();
                this.wBook.Close(false, Missing.Value, Missing.Value);
            }
        }


        /// <summary>
        /// 数据过滤处理
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public  string DataFilter(string content)
        {
            string result = string.Empty;
            try
            {
                if (content.StartsWith("@")|| content.StartsWith("=")||content.StartsWith("+"))
                {
                    content = string.Format("{0}{1}","\"",content);
                }
               content= content.Replace("machine-translated", "").Replace("Machine-translated", "");
               result = content;
            }
            catch (Exception)
            {

                
            }
            return result;
        }


    }
}
