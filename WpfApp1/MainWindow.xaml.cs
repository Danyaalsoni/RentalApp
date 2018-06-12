using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
using WpfApp1.Database;
using System.Data.SQLite;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

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
        public MainWindow()
        {
            InitializeComponent();
            
                
                List<RentalAddress> rentalAddressesList = new List<RentalAddress>();
                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\danya\source\repos\WpfApp1\WpfApp1\DataFile\RentalDatabase.db;"))
                {
                    conn.Open();
                   // SQLiteCommand command = new SQLiteCommand("INSERT into RentalAddress (ID,Address,RenterID) Values (4,'ss',0)", conn);
                    //command.ExecuteNonQuery();
                    SQLiteCommand command1 = new SQLiteCommand("Select * from RentalAddress", conn);
                    SQLiteDataReader reader = command1.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        //Console.WriteLine(reader["YourColumn"]);
                        rentalAddressesList.Add(new RentalAddress(Convert.ToInt32(reader["ID"].ToString()), reader["Address"].ToString(), Convert.ToInt32(reader["RenterID"].ToString())));
                      
                    }
                    reader.Close();
                    foreach(RentalAddress r in rentalAddressesList)
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddRentalAddress newWindow = new AddRentalAddress();
            newWindow.ShowDialog();
           
        }
    }
}
