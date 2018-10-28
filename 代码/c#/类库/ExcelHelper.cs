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

namespace ContenCheckClient
{

    public class ExcelHelper
    {
        public void AddData(Excel.Application excelApp,string filename,List<QueryResult> qrs)
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
            if (excelApp!=null)
            {
                excelApp.Quit();
            }
            if (workSheet!=null)
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


        public static void WriteExcel(string filename,List<QueryResult> qrs)
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
            foreach (QueryResult qr in qrs)
            {
                workSheet.Cells[qr.No, 1] = qr.No;
                workSheet.Cells[qr.No, 2] = qr.Content??"";
                workSheet.Cells[qr.No, 3] = qr.IsExists;
                workSheet.Cells[qr.No, 4] = qr.Paragraph??"";
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


    }
}
