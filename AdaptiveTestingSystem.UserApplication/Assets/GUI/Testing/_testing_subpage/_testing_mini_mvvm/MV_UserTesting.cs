using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm
{
    public class MV_UserTesting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }


        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChange("Index");
            }
        }

        private string nameUser = "";

        public string NameUser
        {
            get { return nameUser; }
            set
            {
                nameUser = value;
                OnPropertyChange("NameUser");
            }
        }

        private string guid = "";

        public string GUID
        {
            get { return guid; }
            set
            {
                guid = value;
                OnPropertyChange("GUID");
            }
        }



        private Code userCode;

        public Code IsCode
        {
            get { return userCode; }
            set { userCode = value; OnPropertyChange("IsCode"); }
        }


    }
}

