using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_mvvm
{
    public class Modal_CRV_User: INotifyPropertyChanged
    {

       public event PropertyChangedEventHandler? PropertyChanged;
       public void OnPropertyChanged([CallerMemberName] string prop = "")
       {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
       }
        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged("Index"); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private string _daybirch;

        public string DayBirch
        {
            get { return _daybirch; }
            set { _daybirch = value; OnPropertyChanged("DayBirch"); }
        }

        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged("Gender"); }
        }
    }
}
