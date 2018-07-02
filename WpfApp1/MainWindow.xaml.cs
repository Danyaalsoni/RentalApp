using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Database;
using System.Data.SQLite;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public string origdbloc;
        public string dbloc="";
        public MainWindow()
        {
            InitializeComponent();

            loadItems(); 
            

            
        }
        public void loadItems()
        {
            List<RentalAddress> rentalAddressesList = new List<RentalAddress>();
            rentalAddressesList.Clear();
            this.listView.Items.Clear();
            if (dbloc == "")
            {
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                dbloc = @"Data Source=|DataDirectory|\DataFile\RentalDatabase.db";
            }
            using (SQLiteConnection conn = new SQLiteConnection(dbloc))
            {
                conn.Open();
                // SQLiteCommand command = new SQLiteCommand("INSERT into RentalAddress (ID,Address,RenterID) Values (4,'ss',0)", conn);
                //command.ExecuteNonQuery();
                SQLiteCommand command1 = new SQLiteCommand("Select * from RentalAddress", conn);
                SQLiteDataReader reader = command1.ExecuteReader();

                while (reader.Read())
                {
                    //Console.WriteLine(reader["YourColumn"]);
                    rentalAddressesList.Add(new RentalAddress(Convert.ToInt32(reader["ID"]), reader["Address"].ToString()));

                }
                reader.Close();
                foreach (RentalAddress r in rentalAddressesList)
                {
                    this.listView.Items.Add(r);
                }
            }
        }
        public void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext;
            if (item != null)
            {
                MessageBox.Show(item.ToString()+"Item's Double Click handled!");
            }
            //load renter window
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddRentalAddress newWindow = new AddRentalAddress();
            newWindow.ShowDialog();
            loadItems();
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".db";
            Nullable<bool> res = dlg.ShowDialog();
            if (res.HasValue && res.Value)
            {
                dbloc = dlg.FileName;
                origdbloc = dbloc;
                dbloc = @"Data Source=" + dbloc;
                DatabaseLocationText.Text = dbloc;
                loadItems();
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            string data = string.Empty;
            int i=1, j = 0;
            bool flag = true;
            using (SQLiteConnection conn = new SQLiteConnection(dbloc))
            {
                conn.Open();
                string stm = "SELECT * FROM RentalAddress INNER JOIN Renter ON RentalAddress.ID=Renter.AddressID";
                using (SQLiteCommand cmd = new SQLiteCommand(stm, conn))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        
                        while (rdr.Read()) // Reading Rows
                        {
                            if (flag)
                            {
                                for (j = 0; j <= rdr.FieldCount - 1; j++)
                                {
                                    xlWorkSheet.Cells[i, j + 1] = rdr.GetName(j);
                                }
                                flag = false;
                            }
                            for (j = 0; j <= rdr.FieldCount - 1; j++) // Looping throw colums
                            {
                                data = rdr.GetValue(j).ToString();
                                xlWorkSheet.Cells[i + 1, j + 1] = data;
                            }
                            i++;
                        }
                    }
                }
                conn.Close();
            }
            xlWorkBook.SaveAs("Renter Datasheet.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }
        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
