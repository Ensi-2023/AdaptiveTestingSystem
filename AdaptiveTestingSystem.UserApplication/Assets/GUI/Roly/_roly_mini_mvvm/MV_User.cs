﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_mini_mvvm
{
    public class MV_User : INotifyPropertyChanged
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

    }
}
