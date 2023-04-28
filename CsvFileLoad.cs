using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsCsvToInsert
{
    internal class CsvFileLoad : FileLoad
    {
        public static char[] TrailingChars = { ',', ';' };

        public override DataTable LoadData(string fileName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var file = new StreamReader(fileName))
                {
                    var trailingChar = ',';
                    foreach(var c in TrailingChars)
                    {
                        if(file.ReadLine().Contains(c))
                        {
                            trailingChar = c;
                            break;
                        }
                    }

                    string[] headers = file.ReadLine().Split(trailingChar);
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header.Trim());
                    }
                    while (!file.EndOfStream)
                    {
                        string[] rows = file.ReadLine().Split(trailingChar);
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
