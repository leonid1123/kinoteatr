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
        public MainWindow()
        {
            InitializeComponent();
        }
        private SQLiteConnection sqlite;
        public void dbConnect(String dbLocation)
        {
            sqlite = new SQLiteConnection(String.Concat("Data Sourse=",dbLocation));
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
                Debug.WriteLine(dt.Columns.Count);

            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("что-то пошло не так");
            }
            sqlite.Close();
            return dt;
        }

        //дата, количество фильмов, название каждого фильма,картинка каждого фильма
        int[] filmsPerDay = new int[7] { 4, 3, 7, 7, 4, 8, 6 };
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<Canvas> canvasList = new List<Canvas>();
            canvasList.Add(canvas1);
            canvasList.Add(canvas2);
            canvasList.Add(canvas3);
            for (int j = 0; j < canvasList.Count; j++)
            {

                for (int i = 0; i < filmsPerDay[j]; i++)
                {
                    Button btn = new Button();
                    btn.FontSize = 14;
                    btn.Content = "Film" + i.ToString();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dbConnect("kino.db");
            DataTable ans =  dbSelectQuery("SELECT * FROM films");

        }
    }
}
