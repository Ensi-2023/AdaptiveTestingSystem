using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm
{
    public class VM_ResultDataTesting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }


        private ObservableCollection<MV_ResultDataTest> _item { get; set; }

        private ObservableCollection<MV_ResultDataTest>? _itemCollectionViewer { get; set; }
        public ObservableCollection<MV_ResultDataTest>? ItemCollectionViewer
        {
            get { return _itemCollectionViewer; }

            set
            {
                _itemCollectionViewer = value;
                OnPropertyChanged("ItemCollectionViewer");
            }
        }

        private MV_ResultDataTest? selectedItem;
        public MV_ResultDataTest? SelectedDataItem
        {
            get { return selectedItem; }
            set
            {
                if (value == null) return;
                selectedItem = value;
                OnPropertyChanged("SelectedDataItem");
            }
        }

        public VM_ResultDataTesting()
        {
            ItemCollectionViewer = new ObservableCollection<MV_ResultDataTest>();
            _item = new ObservableCollection<MV_ResultDataTest>();
        }

        public void AddOne(MV_ResultDataTest item)
        {
            var test = new MV_ResultDataTest()
            {
               Index = item.Index,
               AnswerImage= item.AnswerImage,
               CorrectImage= item.CorrectImage, 
               IsAnswerImage= item.IsAnswerImage,
               IsCorrectAnswerImage= item.IsCorrectAnswerImage, 
               IsQuestImage= item.IsQuestImage, 
               Quest= item.Quest,   
               QuestImage = item.QuestImage,
               AnswerUser= item.AnswerUser, 
               CorrectAnswer= item.CorrectAnswer,
            };


            _item.Add(test);
            ItemCollectionViewer.Add(test);
            

            OnPropertyChanged("ItemCollectionViewer");
        }


    }
}
