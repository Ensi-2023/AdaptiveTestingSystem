using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm
{
    public class MV_ResultDataTest : INotifyPropertyChanged
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
            set { index = value; OnPropertyChange("Index"); }
        }

        private string quest = string.Empty;

        public string Quest
        {
            get { return quest; }
            set { quest = value; OnPropertyChange("Quest"); }
        }

        private string correctAnswer = string.Empty;

        public string CorrectAnswer
        {
            get { return correctAnswer; }
            set { correctAnswer = value; OnPropertyChange("CorrectAnswer"); }
        }

        private string answerUser = string.Empty;

        public string AnswerUser
        {
            get { return answerUser; }
            set { answerUser = value; OnPropertyChange("AnswerUser"); }
        }


        private string questStringImage = string.Empty;

        public string QuestImage
        {
            get { return questStringImage; }
            set { questStringImage = value; OnPropertyChange("QuestImage"); }
        }

        private string answerImage = string.Empty;

        public string AnswerImage
        {
            get { return answerImage; }
            set { answerImage = value; OnPropertyChange("AnswerImage"); }
        }

        private string correctImage = string.Empty;

        public string CorrectImage
        {
            get { return correctImage; }
            set { correctImage = value; OnPropertyChange("CorrectImage"); }
        }


        private bool isQuestImage;

        public bool IsQuestImage
        {
            get { return isQuestImage; }
            set { isQuestImage = value; OnPropertyChange("IsQuestImage"); }
        }

        private bool isCorrectAnswerImage;

        public bool IsCorrectAnswerImage
        {
            get { return isCorrectAnswerImage; }
            set { isCorrectAnswerImage = value; OnPropertyChange("IsCorrectAnswerImage"); }
        }

        private bool isAnswerImage;

        public bool IsAnswerImage
        {
            get { return isAnswerImage; }
            set { isAnswerImage = value; OnPropertyChange("IsAnswerImage"); }
        }

    }
}
