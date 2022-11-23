using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _minhascompras__.Helper;
using System.IO;
using System.Threading;
using System.Globalization;


namespace _minhascompras__
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.db3"
                    );

                    _db = new SQLiteDatabaseHelper(path);
                }

                return _db;
            }
        }
        public App()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            MainPage = new NavigationPage(new View.Listagem()) { BarBackgroundColor = Color.FromHex("#322c8e"), BarTextColor = Color.White };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
