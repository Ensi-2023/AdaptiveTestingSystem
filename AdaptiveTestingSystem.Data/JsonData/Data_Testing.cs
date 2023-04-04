using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Data.JsonData
{

    public class Data_ResultTesting
    {
        public int Index { get; set; } = 0;
        public double CountAssessment2 { get; set; } = 0;
        public double CountAssessment3 { get; set; } = 0;
        public double CountAssessment4 { get; set; } = 0;
        public double CountAssessment5 { get; set; } = 0;
        public double avg { get; set; } = 0.0;
    }

    public class Data_ConnectTestingServer
    {
        public string Hash { get; set; } = string.Empty;
        public string IndexServer { get; set; } = string.Empty;
    }

    public class Data_SendTesting: Data_Base
    {
        public bool IsEdit { get; set; } = false;
    }


    public class Data_TestingView : Data_Base
    {
        public int Index { get; set; } = 0;

    }

    public  class Data_TestRun
    { 
        public int Index { get; set; }
        public List<Data_TestRun_Qeust> List_Qeusts { get; set; } = new List<Data_TestRun_Qeust>();

        public int Count { get; set; }
        public int CountAnswer { get; set; }

        public int IndexUser { get; set; }
        public int Assessment { get; set; }
        public bool IsEarly { get; set; }
        public string DateTimeTest { get; set; } = string.Empty;
        public int CountCorrect { get; set; }
        public int CountNotCorrect { get; set; }
    }

    public class Data_TestRun_Qeust
    {
        public int Index { get; set; }
        public int IndexAnswer { get; set; }
        public int IndexCorrectAnswer { get; set; }

    }

    public class Data_TestRun_Answer
    {
        public int Index { get; set; }
        public int Number { get; set; }
        public int CorrectNumber { get; set; }

    }

    public class Data_Testing: Data_Base
    {
        public int Index { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int IndexCreator { get; set; } = 0;
        public string NameCreator { get; set; } = string.Empty;
        public int IndexPredmet { get; set; } = 0;
        public string NamePredmet { get; set; } = string.Empty;
        public string DateCrieting { get; set; } = string.Empty;

        public int CountQuest { get; set; } = 0;

        public List<Data_Question> Questions { get; set; } = new List<Data_Question>();



        public List<int> DeleteQuestIndex { get; set; } = new List<int>(); 
        public List<int> DeleteAnswerIndex { get; set; } = new List<int>(); 
    }


    public class Data_TestingPacket
    {
        public Data_Testing Testing { get; set; } = new Data_Testing();
        public List<Data_Question> Questions { get; set; } = new List<Data_Question>(); 
    }

    public class Data_Question
    { 
        public int Index { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageFormat { get; set; } = string.Empty;
        public bool IsImaging { get; set; }
        public int CorrecrNumber { get; set; }
        public int Complexity { get; set; }
        public List<Data_Answer> Answer { get; set; } = new List<Data_Answer>();
        
    }

    public class Data_QuestionEditOrInsert
    { 
        public Data_Question Question { get; set; } = new Data_Question();
        public int IndexTest { get; set; }
        public List<int> DeleteAnswerIndex { get; set; } = new List<int>();
    }

    public class Data_Answer
    { 
        public int Index { get; set; }
        public string Answer { get; set;} = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageFormat { get; set; } = string.Empty;
        public bool IsImaging { get; set; }
        public int Number { get; set; }
    }

    public class Data_DeleteQuestions
    {
        public int Index { get; set; } = 0;
        public List<Data_Question> Questions { get; set; } = new List<Data_Question>();
    }


    public class Data_QuestDataViewer: Data_Base
    { 
        public int Index { get; set; }
        public bool IsEdit { get; set; } = false;
    }


    public class Data_AllTestForSB:Data_Base
    {
        public int Index { get; set; } = 0;
        public string NameCreate { get; set; } = string.Empty;
        public string NameTest { get; set; } = string.Empty;
        public string NamePredmet { get; set; } = string.Empty;
        public string MaxQuestCountForUser { get; set; } = string.Empty;
    }
}
