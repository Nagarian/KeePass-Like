using KeePassLike.Component;
using KeePassLike.Lib.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class DBKeyRingView : Page
    {
        public DBKeyRingView()
        {
            this.InitializeComponent();
            this.DB = Setting.Current.DB;
        }

        public DBKeyRing DB { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                this.Frame.BackStack.RemoveAt(this.Frame.BackStackDepth - 1);
            }

            this.DataContext = this;
        }

        private void KeyRingList_SelectedItemChanged(object sender, WinRTXamlToolkit.Controls.RoutedPropertyChangedEventArgs<object> e)
        {
            ContentFrame.Navigate(typeof(KeyRingView), e.NewValue, new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DB.Save(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.dat"));
        }

        private void DisplayRightMenu_Click(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(sender as FrameworkElement) ?? FlyoutBase.GetAttachedFlyout(e.OriginalSource as FrameworkElement);
            flyoutBase.ShowAt(e.OriginalSource as FrameworkElement);
        }

        #region Ring Management
        private async void AddRing_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new KeyRingTitleBox("Nouvel anneau");
            var context = ((FrameworkElement)sender).DataContext;
            if ((await dialog.ShowAsync()) == ContentDialogResult.Primary)

                if (context is DBKeyRingView)
                    DB.MasterKeyRing.SubKeyRing.Add(new KeyRing(dialog.KeyRingTitle));
                else if (context is KeyRing)
                    ((KeyRing)context).SubKeyRing.Add(new KeyRing(dialog.KeyRingTitle));
        }

        private async void RenameRing_Click(object sender, RoutedEventArgs e)
        {
            KeyRing ringToModify = ((FrameworkElement)sender).DataContext as KeyRing ?? DB.MasterKeyRing;

            var dialog = new KeyRingTitleBox(ringToModify.Title);
            if ((await dialog.ShowAsync()) == ContentDialogResult.Primary)
                ringToModify.Title = dialog.KeyRingTitle;
        }

        private async void DeleteRing_Click(object sender, RoutedEventArgs e)
        {
            var ring = ((FrameworkElement)sender).DataContext as KeyRing ?? DB.MasterKeyRing;

            var dialog = new MessageDialog($"Etes-vous sur de vouloir supprimer l'anneau {ring.Title} ?", $"Suppression de l'anneau {ring.Title}");
            dialog.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Annuler", Id = 1 });
            dialog.DefaultCommandIndex = 1;

            if ((int)(await dialog.ShowAsync()).Id == 0)
                DB.RemoveKeyRing(ring);
        }
        #endregion

        private void AddKey_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(KeyView), ((FrameworkElement)sender).DataContext as KeyRing ?? DB.MasterKeyRing, new Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo());
        }
    }

    public class DBKEyRingViewModel
    {
        public DBKEyRingViewModel()
        {
            DB = new DBKeyRing("coucou");
            DB.MasterKeyRing.Keys.Add(new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" });
            DB.MasterKeyRing.Keys.Add(new Lib.Core.KeyEntry { Title = "Lala", Username = "lala@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" });
            DB.MasterKeyRing.Keys.Add(new Lib.Core.KeyEntry { Title = "Tutu", Username = "tutu@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" });
            DB.MasterKeyRing.SubKeyRing.Add(new Lib.Core.KeyRing("mes sous-clés")
            {
                Keys =
                {
                    new Lib.Core.KeyEntry { Title = "Lala", Username = "lala@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                    new Lib.Core.KeyEntry { Title = "Tutu", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                },
                SubKeyRing = new ObservableCollection<KeyRing> {
                    new KeyRing("lala")
                    {
                        SubKeyRing = new ObservableCollection<KeyRing> {
                            new KeyRing("lolo")
                            {
                                Keys =
                                {
                                    new Lib.Core.KeyEntry { Title = "Lol", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                                }
                            }
                        },
                        Keys =
                        {
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                        }
                    },
                    new KeyRing("lala")
                    {
                        SubKeyRing = new ObservableCollection<KeyRing> {
                            new KeyRing("lolo")
                            {
                                Keys =
                                {
                                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                                }
                            }
                        },
                        Keys =
                        {
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                        }
                    }
                }
            });
            DB.MasterKeyRing.SubKeyRing.Add(new Lib.Core.KeyRing("mes sous-clés")
            {
                Keys =
                {
                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                },
                SubKeyRing = new ObservableCollection<KeyRing> {
                    new KeyRing("lala")
                    {
                        SubKeyRing = new ObservableCollection<KeyRing> {
                            new KeyRing("lolo")
                            {
                                Keys =
                                {
                                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                                }
                            }
                        },
                        Keys =
                        {
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                        }
                    },
                    new KeyRing("lala")
                    {
                        SubKeyRing = new ObservableCollection<KeyRing> {
                            new KeyRing("lolo")
                            {
                                Keys =
                                {
                                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                                    new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                                }
                            }
                        },
                        Keys =
                        {
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" },
                            new Lib.Core.KeyEntry { Title = "Toto", Username = "toto@yopmail.fr", Password = "tbr8pfzPs21dbj1hoLe7nA==" }
                        }
                    }
                }
            });
        }

        public DBKeyRing DB { get; set; }
    }
}
