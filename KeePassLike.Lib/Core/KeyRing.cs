using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassLike.Lib.Core
{
    public class KeyRing : INotifyPropertyChanged
    {
        private string title;

        public KeyRing()
        {
        }

        public KeyRing(string title)
        {
            this.Title = title;
            SubKeyRing = new ObservableCollection<KeyRing>();
            Keys = new ObservableCollection<KeyEntry>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }
        
        public ObservableCollection<KeyRing> SubKeyRing { get; set; }

        public ObservableCollection<KeyEntry> Keys { get; set; }
        
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
