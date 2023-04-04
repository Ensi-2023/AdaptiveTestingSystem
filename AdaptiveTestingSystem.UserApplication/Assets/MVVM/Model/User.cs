using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model
{
    public class User:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

#nullable disable
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

        private string fio = "";

        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                OnPropertyChange("FIO");
            }
        }

        private string gender = "";

        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChange("Gender");
            }
        }

        private string datebirch = "";

        public string Datebirch
        {
            get { return datebirch; }
            set
            {
                datebirch = value;
                OnPropertyChange("Datebirch");
            }
        }

        private List<Data_Klass> klassesUser;

        public List<Data_Klass> KlassesUser
        {
            get { return klassesUser; }
            set { klassesUser = value; OnPropertyChange("KlassesUser"); }
        }

        public string Klass
        {
            get
            {
                if (KlassesUser.Count > 0)
                    return KlassesUser[0].Name;
                else
                    return "null";
            }
        }

        private string role;

        public string Role
        {
            get { return role; }
            set { role = value; OnPropertyChange("Role"); }
        }

        private bool teacher;

        public bool IsTeacher
        {
            get { return teacher; }
            set { teacher = value; OnPropertyChange("IsTeacher"); }
        }

        private string registrationdate;

        public string RegistrationDate
        {
            get { return registrationdate; }
            set { registrationdate = value; OnPropertyChange("RegistrationDate"); }
        }


        private string login;

        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChange("Login"); }
        }


    }
}
