using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XlsCsvToInsert
{
    internal class XlsFileLoad : FileLoad
    {
        public override DataTable LoadData(string fileName)
        {
            try
            {
                using (var file = new FileStream(fileName, FileMode.Open))
                {
                    var dt = new DataTable();
                    using (ExcelPackage pck = new ExcelPackage(file))
                    {
                        var sheet = pck.Workbook.Worksheets.FirstOrDefault();

                        int totalColumns = sheet.Dimension.End.Column;
                        for (int i = 1; i <= totalColumns; i++)
                        {
                            dt.Columns.Add(sheet.Cells[1, i].Value?.ToString() ?? $"Column{i}");
                        }

                        int totalRows = sheet.Dimension.End.Row;
                        for (int i = 2; i <= totalRows; i++)
                        {
                            DataRow row = dt.NewRow();
                            for (int j = 1; j <= totalColumns; j++)
                            {
                                if (sheet.Cells[i, j].Value == null) row[j - 1] = DBNull.Value;
                                else
                                {
                                    var dateStr = sheet.Cells[i, j].Style.Numberformat.Format;
                                    if (dateStr.Contains("d") || dateStr.Contains("y") || dateStr.Contains("m"))
                                    {
                                        double dateValue = 0;
                                        if (Double.TryParse(sheet.Cells[i, j].Value.ToString(), out dateValue))
                                        {
                                            DateTime dateTime = new DateTime(1900, 1, 1).AddDays(dateValue - 2);
                                            row[j - 1] = dateTime;
                                        }
                                    }

                                    row[j - 1] = sheet.Cells[i, j].Value.ToString();
                                }
                            }
                            dt.Rows.Add(row);
                        }
                    }
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
