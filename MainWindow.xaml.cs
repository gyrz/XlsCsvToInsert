using Microsoft.Extensions.Primitives;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XlsCsvToInsert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable dataTable { get; set; }

        private string lastTableName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private static char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private bool isNumeric(string value)
        {
            foreach (var c in value.ToCharArray())
            {
                if (!numbers.Contains(c) && c != '.') return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                StringBuilder stringBuilder = new StringBuilder();

                var columnsNeeded = SampleQuery.Text.Substring(SampleQuery.Text.IndexOf('(') + 1, SampleQuery.Text.IndexOf(')') - SampleQuery.Text.IndexOf('(') - 1);
                columnsNeeded.Trim();

                foreach( DataRow row in dataTable.Rows)
                {
                    stringBuilder.Append(SampleQuery.Text);
                    foreach (var col in columnsNeeded.Split(','))
                    {
                        var col2Find = col.Trim();

                        try
                        {
                            var value = row[col2Find];
                            if (value is DateTime) stringBuilder.Append("'" + ((DateTime.Parse(value.ToString()))).ToString(DateTimeFormat.Text) + "'");
                            else if (value is string && (value.ToString().Contains(" AM") || value.ToString().Contains(" PM"))) stringBuilder.Append("'" + ((DateTime.Parse(value.ToString()))).ToString(DateTimeFormat.Text) + "'");
                            else if (value is string && !isNumeric(value.ToString())) stringBuilder.Append("'" + value.ToString() + "'");
                            else stringBuilder.Append(value.ToString());
                        }
                        catch { }

                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    stringBuilder.Append(" );\n");
                }
                // itt írd ki a fájlba az adatokat

                using (StreamWriter stream = new StreamWriter(filePath))
                {
                    stream.Write(stringBuilder.ToString());
                }
            }
        }

        private void reloadDataSource()
        {
            if (dataTable == null) return;

            Table.ItemsSource = null;
            Table.ItemsSource = dataTable.DefaultView;
            Table.Items.Refresh();
            DropDown.Items.Clear();
            foreach (DataColumn col in dataTable.Columns)
                DropDown.Items.Add(col.ColumnName);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Excel fájl kiválasztása
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|Csv files (*.csv)|*.csv|Old excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.Title = "Choose a file to upload";

            if (openFileDialog1.ShowDialog() == true)
            {
                if(openFileDialog1.FileName.Contains("xls"))
                {
                    dataTable = new XlsFileLoad().LoadData(openFileDialog1.FileName);
                }

                else if(openFileDialog1.FileName.Contains("csv"))
                {
                    dataTable = new CsvFileLoad().LoadData(openFileDialog1.FileName);
                }
            }

            reloadDataSource();
            StringBuilder builder = new StringBuilder();
            builder.Append($"INSERT INTO schema.TableName \n( ");
            foreach (DataColumn col in dataTable.Columns)
            {
                builder.Append(col.ColumnName);
                builder.Append(", ");
            }

            builder.Remove(builder.Length - 2, 2);
            builder.Append(" )\n VALUES ( ");
            SampleQuery.Text = builder.ToString();
        }

        private void DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = DropDown.SelectedItem;

            if(selectedItem != null) ColName.Text = selectedItem.ToString();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            SampleQuery.Text = SampleQuery.Text.Replace(lastTableName, textBox.Text);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            SampleQuery.Text = SampleQuery.Text.Replace(lastTableName, textBox.Text);
        }

        private void ColName_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = ColName.Text;
            var selectedItem = DropDown.SelectedItem;

            if (string.IsNullOrEmpty(text) || selectedItem == null) return;

            foreach(DataColumn col in dataTable.Columns)
            {
                if (col.ColumnName == selectedItem.ToString())
                {
                    SampleQuery.Text = SampleQuery.Text.Replace(col.ColumnName, text);
                    col.ColumnName = text;
                    break;
                }
            }

            reloadDataSource();
        }

    }
}
