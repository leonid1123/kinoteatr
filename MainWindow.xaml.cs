using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Data.SQLite;
using System.Data;



namespace kinoteatr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection sqlite;
   
        public MainWindow()
        {
            InitializeComponent();
        }
        public void dbConnect(string dbLocation)
        {
            sqlite = new SQLiteConnection(String.Concat("Data Source=",dbLocation));
        }
        public DataTable dbSelectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();  //Initiate connection to the db
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource

            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("что-то пошло не так с базой данных");
            }
            sqlite.Close();
            return dt;
        }

        //дата, количество фильмов, название каждого фильма,картинка каждого фильма
        int[] filmsPerDay = new int[7] { 40, 3, 7, 7, 4, 8, 6 };
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dbConnect("kino.db");
            DataTable ans = dbSelectQuery("SELECT Name FROM films");

            filmsPerDay[0] = ans.Rows.Count;
            List<Canvas> canvasList = new List<Canvas>();
            canvasList.Add(canvas1);
            canvasList.Add(canvas2);
            canvasList.Add(canvas3);
            for (int j = 0; j < 1; j++)
            {

                for (int i = 0; i < filmsPerDay[j]; i++)
                {
                    Button btn = new Button();
                    btn.FontSize = 14;
                    btn.Content = ans.Rows[i]["Name"].ToString();
                    btn.Name = "Film" + i.ToString();
                    btn.Width = 100;
                    btn.Height = 100;
                    btn.Click += new RoutedEventHandler(btnClk);
                    Canvas.SetTop(btn, 10);
                    Canvas.SetLeft(btn, btn.Width * i);
                    canvasList[j].Children.Add(btn);
                }
            }

        }
        void btnClk(object sender, RoutedEventArgs e)
        {
            string sourceName = ((FrameworkElement)e.Source).Name;
            string senderName = ((FrameworkElement)sender).Name;

            Debug.WriteLine($"Routed event handler attached to {senderName}, " +
                $"triggered by the Click routed event raised by {sourceName}.");
        }
    }
}

