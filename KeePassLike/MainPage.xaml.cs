using KeePassLike.Lib.Core;
using KeePassLike.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace KeePassLike
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Setting.Current.DB = new DBKeyRing(dbPassword.Password);

            try
            {
                Setting.Current.DB.Load(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.dat"));
            }
            catch (Exception)
            {
                return;
            }

            Frame.Navigate(typeof(DBKeyRingView));
        }
    }
}
