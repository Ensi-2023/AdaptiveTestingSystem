using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
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
    public class VM_AllTestingViewer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        private ObservableCollection<MV_AllTesting> _testing { get; set; }

        private ObservableCollection<MV_AllTesting>? _testingCollectionViewer { get; set; }
        public ObservableCollection<MV_AllTesting>? TestingCollectionViewer
        {
            get { return _testingCollectionViewer; }

            set
            {
                _testingCollectionViewer = value;
                OnPropertyChanged("TestingCollectionViewer");
            }
        }

        private MV_AllTesting? selectedTest;
        public MV_AllTesting? SelectedTest
        {
            get { return selectedTest; }
            set
            {
                if (value == null) return;
                selectedTest = value;
                OnPropertyChanged("SelectedTest");
            }
        }

        public VM_AllTestingViewer()
        {
            TestingCollectionViewer = new ObservableCollection<MV_AllTesting>();
            _testing = new ObservableCollection<MV_AllTesting>();
        }

        public async Task SetData(List<Data_AllTestForSB> data)
        {
            TestingCollectionViewer = new ObservableCollection<MV_AllTesting>();
            _testing = new ObservableCollection<MV_AllTesting>();
            if (data.Count == 0) return;

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
               
                var test = new MV_AllTesting() 
                {
                    CreateUser = item.NameCreate,
                    Index= item.Index,
                    NameTest = item.NameTest,
                    NamePredmet = item.NamePredmet,
                    CountQuest = item.MaxQuestCountForUser
                };

                _testing.Add(test);
                TestingCollectionViewer.Add(test);

                _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Вопросы для тестов", subtitle: $"Обработано: {i + 1} из {data.Count}");
                await Task.Delay(10);
            }

            OnPropertyChanged("QuestionCollectionViewer");

            _Main.Instance.OverlayShow(false);
        }

        public void Search(string isSearchString)
        {
            _Main.Instance.OverlayShow(true);
            TestingCollectionViewer = new ObservableCollection<MV_AllTesting>();

            var filterd = _testing.Where(x =>
                (
                  (x as MV_AllTesting).CreateUser.ToLower().Trim().Contains(isSearchString.ToLower().Trim()) ||
                  (x as MV_AllTesting).NameTest.ToLower().Trim().Contains(isSearchString.ToLower().Trim()) ||
                  (x as MV_AllTesting).NamePredmet.ToLower().Trim().Contains(isSearchString.ToLower().Trim()) 

                ));

            for (int i = 0; i < filterd.Count(); i++)
            {

                var test = new MV_AllTesting()
                {
                    CreateUser = filterd.ToArray()[i].CreateUser,
                    Index = filterd.ToArray()[i].Index,
                    NameTest = filterd.ToArray()[i].NameTest,
                    NamePredmet = filterd.ToArray()[i].NamePredmet,
                    CountQuest = filterd.ToArray()[i].CountQuest,
                };

                TestingCollectionViewer.Add(test);
            }
            OnPropertyChanged("TestingCollectionViewer");
            _Main.Instance.OverlayShow(false);
        }


        internal void ViewInformation(object item)
        {

            if ((item as MV_AllTesting) != null)
            {
               // _Main.Instance.Manager.Next(new GUI_QuestViewer((item as MV_AllTesting).Index, index));
            }

        }

    }
}
