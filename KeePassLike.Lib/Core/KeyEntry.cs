using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassLike.Lib.Core
{
    public class KeyEntry : INotifyPropertyChanged
    {
        private string title;
        private string username;
        private string password;
        private string description;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(nameof(Description)); }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
