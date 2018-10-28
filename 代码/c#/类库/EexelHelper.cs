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

namespace ContenCheckClient
{

    public class ExcelHelper
    {
        public int ExportCount = 0;

        public void AddData(Excel.Application excelApp, string filename, List<dynamic> qrs)
        {
            //set visible the Excel will run in background
            excelApp.Visible = false;
            //set false the alerts will not display
            excelApp.DisplayAlerts = false;

            Excel.Workbook workBook;


            if (File.Exists(filename))
            {
                workBook = excelApp.Workbooks.Open(filename, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            }
            else
            {
                workBook = excelApp.Workbooks.Add(true);

            }

            //new a worksheet
            Excel.Worksheet workSheet = workBook.ActiveSheet as Excel.Worksheet;

            //write data
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);//获得第i个sheet，准备写入
            workSheet.Cells[1, 3] = "(1,3)Content";

        }

        public void SaveAs(string filename, Excel.Workbook workBook)
        {
            workBook.SaveAs(filename);
            workBook.Close(false, Missing.Value, Missing.Value);
        }


        public void Exit(Excel.Application excelApp, Excel.Workbook workBook, Excel.Worksheet workSheet)
        {
            //quit and clean up objects
            if (excelApp != null)
            {
                excelApp.Quit();
            }
            if (workSheet != null)
            {
                workSheet = null;
            }
            if (excelApp != null)
            {
                workBook = null;
            }
            excelApp = null;
            GC.Collect();
        }


        public  void WriteExcel(string filename, List<dynamic> qrs)
        {
            //new an excel object
            Excel.Application excelApp = new Excel.ApplicationClass();
            if (excelApp == null)
            {
                // if equal null means EXCEL is not installed.
                MessageBox.Show("Excel is not properly installed!");
                return;
            }

            // open a workbook,if not exist, create a new one
            Excel.Workbook workBook;
            if (File.Exists(filename))
            {
                workBook = excelApp.Workbooks.Open(filename, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            }
            else
            {
                workBook = excelApp.Workbooks.Add(true);
            }

            //new a worksheet
            Excel.Worksheet workSheet = workBook.ActiveSheet as Excel.Worksheet;

            //write data
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);//获得第i个sheet，准备写入
            foreach (var qr in qrs)
            {
                workSheet.Cells[qr.No, 1] = qr.No;
                workSheet.Cells[qr.No, 2] = qr.Content ?? "";
                workSheet.Cells[qr.No, 3] = qr.IsExists;
                workSheet.Cells[qr.No, 4] = qr.Paragraph ?? "";
            }
            //workSheet.Cells[1, 3] = "(1,3)Content";

            //set visible the Excel will run in background
            excelApp.Visible = false;
            //set false the alerts will not display
            excelApp.DisplayAlerts = false;

            //workBook.SaveAs(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            workBook.SaveAs(filename);
            workBook.Close(false, Missing.Value, Missing.Value);

            //quit and clean up objects
            excelApp.Quit();
            workSheet = null;
            workBook = null;
            excelApp = null;
            GC.Collect();
        }


        public  bool DTImpotToExcel(string filename,DataTable excelTable,string filePath)
        {


            Microsoft.Office.Interop.Excel.Application app =
                new Microsoft.Office.Interop.Excel.Application();
            try
            {
                app.Visible = false;
                //set false the alerts will not display
                app.DisplayAlerts = true;
                Microsoft.Office.Interop.Excel.Workbook wBook = app.Workbooks.Add(true);
                Microsoft.Office.Interop.Excel.Worksheet wSheet = (Excel.Worksheet)wBook.Sheets[1];
                string temContent = string.Empty;
                //new a worksheet
                //Excel.Worksheet workSheet = wBook.ActiveSheet as Excel.Worksheet;
                wSheet.Name = filename;

                object[,] objData = new object[excelTable.Rows.Count + 1, excelTable.Columns.Count];
                //首先将数据写入到一个二维数组中  
                for (int i = 0; i < excelTable.Columns.Count; i++)
                {
                    objData[0, i] = excelTable.Columns[i].ColumnName;
                }
                if (excelTable.Rows.Count > 0)
                {
                    for (int i = 0; i < excelTable.Rows.Count; i++)
                    {
                        ExportCount++;
                        //循环对单元格赋值
                        for (int j = 0; j < excelTable.Columns.Count; j++)
                        {
                            if (excelTable.Rows[i][j].Equals(float.NaN))//查询过来的float.NaN
                                objData[i + 1, j] = "-";
                            else if (String.IsNullOrEmpty(excelTable.Rows[i][j].ToString()) && excelTable.Rows[i][j].Equals(DBNull.Value))//有dbnull的数据，需要屏蔽掉——在数据源处理了
                                objData[i + 1, j] =To(excelTable.Rows[i][j].ToString());
                            else
                                objData[i + 1, j] = To( excelTable.Rows[i][j].ToString());
                        }
                    }
                }

                string startCol = "A";//这里关键，计算要替换的区域
                int iCnt = ((excelTable.Columns.Count - 1) / 26);//当列数是26时 不-1 会出现问题，自己试试就明白了
                string endColSignal = (iCnt == 0 ? "" : ((char)('A' + (iCnt - 1))).ToString());
                string endCol = endColSignal + ((char)('A' + excelTable.Columns.Count - iCnt * 26 - 1)).ToString();
                Microsoft.Office.Interop.Excel.Range range = wSheet.get_Range(startCol + "1", endCol + (excelTable.Rows.Count + 1).ToString());

                range.Value = objData; //给Exccel中的Range整体赋值  
                range.EntireColumn.AutoFit(); //设定Excel列宽度自适应  
                wSheet.get_Range(startCol + "1", endCol + "1").Font.Bold = 1;//Excel文件列名 字体设定为Bold  


                //设置禁止弹出保存和覆盖的询问提示框   
                app.DisplayAlerts = false;
                app.AlertBeforeOverwriting = false;

                wSheet.SaveAs(filePath);
                wBook.Close(false, Missing.Value, Missing.Value);

                //quit and clean up objects
                app.Quit();
                GC.Collect();
                return true;
            }
            catch (Exception err)//这里还有些问题，比如 对方安装的是WPS 不会提示中文错误，没有安装office 也不会弹出该错误
            {
                MessageBox.Show("导出Excel出错！错误原因：" + err.Message, "提示信息");

                //强制结束excel进程
                IntPtr t = new IntPtr(app.Hwnd);
                int k = 0;
                //GetWindowThreadProcessId(t, out k);
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
                p.Kill();
                return false;
            }
            finally
            {
            }

            
        }

        private  string To(string content)
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
