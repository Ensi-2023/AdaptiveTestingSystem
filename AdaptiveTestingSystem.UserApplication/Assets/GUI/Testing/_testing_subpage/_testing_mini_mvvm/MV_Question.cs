using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm
{
    public class MV_Question : INotifyPropertyChanged
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

        private int correctNumber;
        public int CorrectNumber
        {
            get { return correctNumber; }
            set
            {
                correctNumber = value;
                OnPropertyChange("CorrectNumber");
            }
        }


        private string question = "";

        public string Question
        {
            get { return question; }
            set
            {
                question = value;
                OnPropertyChange("Question");
            }
        }

        private bool isImaging;

        public bool IsImaging
        {
            get { return isImaging; }
            set { isImaging = value; OnPropertyChange("IsImaging"); }
        }

    }
}
