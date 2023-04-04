using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model
{
    public class Testing : INotifyPropertyChanged
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


        private string _indexServer = string.Empty;

        public string IndexServer
        {
            get { return _indexServer; }
            set { _indexServer = value; OnPropertyChange("IndexServer"); }
        }

        private string _name = string.Empty;

        public string NameTest
        {
            get { return _name; }
            set { _name = value; OnPropertyChange("NameTest"); }
        }


        private string _desc = string.Empty;

        public string Description
        {
            get { return _desc; }
            set { _desc = value; OnPropertyChange("Description"); }
        }


        private int _indexCreator = 0;

        public int IndexCreator
        {
            get { return _indexCreator; }
            set { _indexCreator = value; OnPropertyChange("IndexCreator"); }
        }

        private string _nameCreator = string.Empty;

        public string NameCreator
        {
            get { return _nameCreator; }
            set { _nameCreator = value; OnPropertyChange("NameCreator"); }
        }


        private int _indexPredmet = 0;

        public int IndexPredmet
        {
            get { return _indexPredmet; }
            set { _indexPredmet = value; OnPropertyChange("IndexPredmet"); }
        }


        private string _namePredmet = string.Empty;

        public string NamePredmet
        {
            get { return _namePredmet; }
            set { _namePredmet = value; OnPropertyChange("NamePredmet"); }
        }


        private string _dateCrieting = string.Empty;

        public string DateCrieting
        {
            get { return _dateCrieting; }
            set { _dateCrieting = value; OnPropertyChange("DateCrieting"); }
        }

        private int _countQuest = 0;

        public int CountQuest
        {
            get { return _countQuest; }
            set { _countQuest = value; OnPropertyChange("CountQuest"); }
        }

        private string _pass = string.Empty;

        public string Password
        {
            get { return _pass; }
            set { _pass = value; OnPropertyChange("Password");

                if (value.Trim() == string.Empty) IsPassword = "Нет"; else IsPassword = "Да";
            }
        }


        private string chekPass;

        public string IsPassword
        {
            get { return chekPass; }
            set { chekPass = value; OnPropertyChange("IsPassword"); }
        }


        private bool _adaptive = false;

        public bool IsAdaptive
        {
            get { return _adaptive; }
            set { _adaptive = value; OnPropertyChange("IsAdaptive");
                if (value)
                    Adaptive = "Да";
                else
                    Adaptive = "Нет";
            }
        }


        private string _strAdaptive = string.Empty;

        public string Adaptive
        {
            get { return _strAdaptive; }
            set { _strAdaptive = value; OnPropertyChange("Adaptive"); }
        }


        private int _countUser = 0;

        public int CountUser
        {
            get { return _countUser; }
            set { _countUser = value; OnPropertyChange("CountUser"); }
        }

    }
}
