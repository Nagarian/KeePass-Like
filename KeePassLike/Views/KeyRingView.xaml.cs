using KeePassLike.Lib.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class KeyRingView : Page
    {
        private DispatcherTimer timer;

        public KeyRingView()
        {
            this.InitializeComponent();
        }

        public bool CopyIsActive { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = e.Parameter;

            base.OnNavigatedTo(e);
        }

        #region Key Management
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(KeyView), (KeyRing)this.DataContext, new Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo());
        }

        private void CopyUsername_Click(object sender, TappedRoutedEventArgs e)
        {
            SetClipboard(((KeyEntry)((FrameworkElement)sender).DataContext).Username);
        }

        private void CopyPassword_Click(object sender, TappedRoutedEventArgs e)
        {
            SetClipboard(Setting.Current.DB.Decrypt(((KeyEntry)((FrameworkElement)sender).DataContext).Password));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var key = (KeyEntry)((FrameworkElement)sender).DataContext;
            this.Frame.Navigate(typeof(KeyView), key, new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var key = (KeyEntry)((FrameworkElement)sender).DataContext;

            var dialog = new MessageDialog($"Etes-vous sur de vouloir supprimer la clé {key.Title} ?", $"Suppression de la clé {key.Title}");
            dialog.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Annuler", Id = 1 });
            dialog.DefaultCommandIndex = 1;

            if ((int)(await dialog.ShowAsync()).Id == 0)
                ((KeyRing)this.DataContext).Keys.Remove(key);
        }
        #endregion

        #region Clipboard Management
        private void SetClipboard(string content)
        {
            Reset();

            var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
            dataPackage.SetText(content);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            Windows.ApplicationModel.DataTransfer.Clipboard.ContentChanged += Clipboard_ContentChanged;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += (s, e) =>
            {
                try
                {
                    Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
                }
                catch (Exception)
                {
                }

                Reset();
            };

            timer.Start();
        }

        private void Clipboard_ContentChanged(object sender, object e)
        {
            Reset();
        }

        private void Reset()
        {
            timer?.Stop();
            timer = null;
            Windows.ApplicationModel.DataTransfer.Clipboard.ContentChanged -= Clipboard_ContentChanged;
        } 
        #endregion
    }
}
