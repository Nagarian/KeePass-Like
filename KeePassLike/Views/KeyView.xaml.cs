using KeePassLike.Lib.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassLike.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class KeyView : Page
    {
        private KeyEntry key;
        private KeyRing parentKeyRing;

        public KeyView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Refresh)
            {
                if (e.Parameter is KeyRing)
                {
                    newPanel.Visibility = Visibility.Visible;
                    parentKeyRing = (KeyRing)e.Parameter;
                    key = new KeyEntry();
                }
                else
                {
                    key = ((KeyEntry)e.Parameter);
                    passwordBox.Password = Setting.Current.DB.Decrypt(key.Password);
                }

                this.DataContext = key;
            }

            base.OnNavigatedTo(e);

            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += SystemNavigationManager_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= SystemNavigationManager_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            if (parentKeyRing == null)
            {
                key.Password = Setting.Current.DB.Encrypt(passwordBox.Password);
            }

            this.Frame.GoBack(new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (parentKeyRing != null)
            {
                parentKeyRing.Keys.Add(key);
            }

            key.Password = Setting.Current.DB.Encrypt(passwordBox.Password);

            this.Frame.GoBack(new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack(new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }
    }
}
