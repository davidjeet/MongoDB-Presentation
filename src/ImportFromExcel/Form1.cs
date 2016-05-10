using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportFromExcel
{
    using System.IO;

    using Excel;

    using MongoDB.Driver;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            server = new MongoClient(ConnectionString).GetServer();
            database = server.GetDatabase("test");
        }


        private readonly MongoServer server;
        private readonly MongoDatabase database;
        private const string ConnectionString = "mongodb://localhost/?safe=true";


        private void button1_Click(object sender, EventArgs e)
        {
            const string FilePath = @"C:\mongodb\bin\TrafficStopsSample.xls";
            FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);

            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
            using (IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
            {
                //...
                //////2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                ////IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
                //////3. DataSet - The result of each spreadsheet will be created in the result.Tables
                ////DataSet result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                ////excelReader.IsFirstRowAsColumnNames = true;
                ////DataSet result = excelReader.AsDataSet();

                //this.textBox1.Text = string.Format("Rows imported: {0}", result.Tables[0].Rows.Count);

                int i = 0;
                //////5. Data Reader methods
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                excelReader.Read(); //to get past header row
                while (excelReader.Read())
                {

                    var data = new CaryData
                                   {
                                       Id = i++,
                                       CAD_Call = excelReader.GetString(0),
                                       Call_Type = excelReader.GetString(1),
                                       Address = excelReader.GetString(2),
                                       Dt = new DateTime(2012, excelReader.GetInt32(5), excelReader.GetInt32(6)),
                                       Time = excelReader.GetString(4),
                                       Month = Int32.Parse(excelReader.GetString(5)),
                                       Day = Int32.Parse(excelReader.GetString(6)),
                                       Disposition = excelReader.GetString(7),
                                       Streetno = excelReader.GetString(8),
                                       Streetonly = excelReader.GetString(9),
                                       Location = new[] { excelReader.GetDouble(10), excelReader.GetDouble(11) }
                                   };

                    var col = database.GetCollection<CaryData>("trafficstops");
                    col.Save(data);
                }

                stopwatch.Stop();
                this.textBox1.Text = "Done! Operation took " + stopwatch.Elapsed.TotalSeconds + " seconds.";
            }

            //6. Free resources (IExcelDataReader is IDisposable)
            //excelReader.Close();
        }


        public class CaryData
        {

            public int Id { get; set; }

            public string CAD_Call { get; set; }

            public string Call_Type { get; set; }

            public string Address { get; set; }

            public DateTime Dt { get; set; }

            public string Time { get; set; }

            public int Month { get; set; }

            public int Day { get; set; }

            public string Disposition { get; set; }

            public string Streetno { get; set; }

            public string Streetonly { get; set; }

            public double [] Location { get; set; }

        }
    }
}
