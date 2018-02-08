using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TaskManager.BLL
{
    public class MyNPOI
    {
        public static void ExportExcel(DataTable dt, string fileName = "")
        {
            //生成Excel
            IWorkbook book = BuildWorkbook(dt);

            //web 下载
            if (fileName == "")
                fileName = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
            fileName = fileName.Trim();
            string ext = Path.GetExtension(fileName);

            if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                fileName = fileName.Replace(ext, string.Empty);

            HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
            httpResponse.Clear();
            httpResponse.Buffer = true;
            httpResponse.Charset = Encoding.UTF8.BodyName;
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");
            httpResponse.ContentEncoding = Encoding.UTF8;
            httpResponse.ContentType = "application/vnd.ms-excel; charset=UTF-8";
            book.Write(httpResponse.OutputStream);
            httpResponse.End();
        }

        //65536判断处理
        public static HSSFWorkbook BuildWorkbook(DataTable dt)
        {
            var book = new HSSFWorkbook();

            ISheet sheet1 = book.CreateSheet("Sheet1");
            ISheet sheet2 = book.CreateSheet("Sheet2");

            //设置列头  
            int columnCount = dt.Columns.Count;//列数 
            IRow row = sheet1.CreateRow(0);//excel第一行设为列头 

            for (int c = 0; c < columnCount; c++)
            {
                ICell cell = row.CreateCell(c);
                cell.SetCellValue(dt.Columns[c].ColumnName);
            }

            //填充数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < 65536)
                {
                    IRow drow = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = drow.CreateCell(j, CellType.String);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
                if (i >= 65536)
                {
                    IRow drow = sheet2.CreateRow(i - 65536);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = drow.CreateCell(j, CellType.String);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

            }

            //自动列宽
            for (int i = 0; i <= dt.Columns.Count; i++)
            {
                sheet1.AutoSizeColumn(i, true);
                sheet2.AutoSizeColumn(i, true);
            }
            return book;
        }
    }
}
