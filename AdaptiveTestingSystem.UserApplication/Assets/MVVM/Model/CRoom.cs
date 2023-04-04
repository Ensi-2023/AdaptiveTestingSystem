///Class Room model
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model
{
    public class CRoom : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        private int index=0;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChange("Index");
            }
        }

        private string fio = "-";

        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                OnPropertyChange("FIO");
            }
        }


        private string datebirchEmployee = DateTime.Now.ToShortDateString();

        public string DatebirchEmployee
        {
            get { return datebirchEmployee; }
            set
            {
                datebirchEmployee = value;
                OnPropertyChange("DatebirchEmployee");
            }
        }


        private string genderEmployee = "-";

        public string GenderEmployee
        {
            get { return genderEmployee; }
            set
            {
                genderEmployee = value;
                OnPropertyChange("GenderEmployee");
            }
        }

        


        private int idEmployee = 0;

        public int EmployeeID
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChange("EmployeeID");
            }
        }


        private string className ="";

        public string ClassName
        {
            get { return className; }
            set { className = value; OnPropertyChange("ClassName"); }
        }

        private int countUser= 0;

        public int CountUser
        {
            get { return countUser; }
            set { countUser = value; OnPropertyChange("CountUser"); }
        }

        private List<Data_UserList> _userList  = new List<Data_UserList>();
        public List<Data_UserList> UserList
        {
            get { return _userList; }
            set { _userList = value; OnPropertyChange("UserList"); }
        }
    }
}
