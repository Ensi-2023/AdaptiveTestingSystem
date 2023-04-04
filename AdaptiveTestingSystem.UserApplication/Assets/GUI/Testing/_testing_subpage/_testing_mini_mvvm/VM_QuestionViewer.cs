using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_mini_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
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
    public class VM_QuestionViewer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        private ObservableCollection<MV_Question> _question { get; set; }

        private ObservableCollection<MV_Question>? _questionCollectionViewer { get; set; }
        public ObservableCollection<MV_Question>? QuestionCollectionViewer
        {
            get { return _questionCollectionViewer; }

            set
            {
                _questionCollectionViewer = value;
                OnPropertyChanged("QuestionCollectionViewer");
            }
        }

        private MV_Question? selectedQuestion;
        public MV_Question? SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                if (value == null) return;
                selectedQuestion = value;
                OnPropertyChanged("SelectedQuestion");
            }
        }

        public VM_QuestionViewer()
        {
            QuestionCollectionViewer = new ObservableCollection<MV_Question>();
            _question = new ObservableCollection<MV_Question>();
        }

        public async Task SetData(List<Data_Question> data)
        {
            QuestionCollectionViewer = new ObservableCollection<MV_Question>();
            _question = new ObservableCollection<MV_Question>();
            if (data.Count == 0) return;

            for (int i = 0; i < data.Count; i++)
            {
                string name = data[i].Question;
                if (data[i].IsImaging) name = "Вопрос изображением";


                _question.Add(new MV_Question() { Index = data[i].Index, Question = name, CorrectNumber = data[i].CorrecrNumber, IsImaging = data[i].IsImaging });
                QuestionCollectionViewer.Add(new MV_Question() { Index = data[i].Index, Question = name, CorrectNumber = data[i].CorrecrNumber, IsImaging = data[i].IsImaging });


                _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Вопросы для тестов", subtitle: $"Обработано: {i+1} из {data.Count}");
                await Task.Delay(10);
            }

            OnPropertyChanged("QuestionCollectionViewer");

            _Main.Instance.OverlayShow(false);
        }

        public void Search(string isSearchString)
        {
            _Main.Instance.OverlayShow(true);
            QuestionCollectionViewer = new ObservableCollection<MV_Question>();

            var filterd = _question.Where(x =>
                (
                  (x as MV_Question).Question.ToLower().Trim().Contains(isSearchString.ToLower().Trim())

                ));

            for (int i = 0; i < filterd.Count(); i++)
            {
                string name = filterd.ToArray()[i].Question;
                if (filterd.ToArray()[i].IsImaging) name = "Вопрос изображением";

                QuestionCollectionViewer.Add(new MV_Question() { Index = filterd.ToArray()[i].Index, Question = name,
                CorrectNumber = filterd.ToArray()[i].CorrectNumber,IsImaging = filterd.ToArray()[i].IsImaging});
            }
            OnPropertyChanged("QuestionCollectionViewer");
            _Main.Instance.OverlayShow(false);
        }


        internal void ViewInformation(object item,int index)
        {
          
            if ((item as MV_Question) != null)
            {
                _Main.Instance.Manager.Next(new GUI_QuestViewer((item as MV_Question).Index, index));
            }

        }

    }
}
