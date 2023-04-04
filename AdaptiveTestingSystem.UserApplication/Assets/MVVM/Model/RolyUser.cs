using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model
{
    public class RolyUser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }


        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChange("Index"); }
        }

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChange("Name"); }
        }

    }
}
