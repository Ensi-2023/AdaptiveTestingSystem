using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm
{
    public class MV_AllTesting : INotifyPropertyChanged
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

        private string nameTest = "";

        public string NameTest
        {
            get { return nameTest; }
            set
            {
                nameTest = value;
                OnPropertyChange("NameTest");
            }
        }

        private string createUser = "";

        public string CreateUser
        {
            get { return createUser; }
            set
            {
                createUser = value;
                OnPropertyChange("CreateUser");
            }
        }

        private string namePredmet = "";

        public string NamePredmet
        {
            get { return namePredmet; }
            set
            {
                namePredmet = value;
                OnPropertyChange("NamePredmet");
            }
        }


        private string countQuest = "";
        public string CountQuest
        {
            get { return countQuest; }
            set
            {
                countQuest = value;
                OnPropertyChange("CountQuest");
            }
        }
    }
}
