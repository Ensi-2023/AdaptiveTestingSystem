using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaptiveTestingSystem.Data.JsonData
{
    public class Data_StatisticPacket: Data_Base
    {
        public int Index { get; set; } = 0;
    }

    public class Data_StatisticGeneral
    {
        public List<Data_AllScoreTest> AllScoreTests { get; set; } = new List<Data_AllScoreTest>();
        public List<Data_ClassroomScore_general> ClassroomScore_generals { get; set; } = new List<Data_ClassroomScore_general>();
        public List<Data_3MostTestedSubject> MostTested3Subjects { get; set; } = new List<Data_3MostTestedSubject>();
        public List<Data_5ClassRoomForAverageScore> AverageScores5ClassRoom { get; set; } = new List<Data_5ClassRoomForAverageScore>();

        public Data_MostActiveUser OneMostActiveUser { get; set; } = new Data_MostActiveUser();
        public List<Data_MostActiveUser> MostActiveUsers { get; set; } = new List<Data_MostActiveUser>();
    }
    public class Data_ClassroomScore_general
    {
        public string ClassRoomName { get; set; } = string.Empty;
        public double Count_Score2 { get; set; } = 0;
        public double Count_Score3 { get; set; } = 0;
        public double Count_Score4 { get; set; } = 0;
        public double Count_Score5 { get; set; } = 0;
    }
    public class Data_3MostTestedSubject
    {
        public string SubjectName { get; set; } = string.Empty;
        public int Count { get; set; } = 0;
    }
    public class Data_5ClassRoomForAverageScore
    {
        public string ClassRoomName { get; set; } = string.Empty;
        public double AverageScore { get; set; } = 0.0;
    }
    public class Data_AllScoreTest
    {
        public string TestName { get; set; } = string.Empty;
        public double Count_Score2_general { get; set; } = 0;
        public double Count_Score3_general { get; set; } = 0;
        public double Count_Score4_general { get; set; } = 0;
        public double Count_Score5_general { get; set; } = 0;
    }
    public class Data_MostActiveUser
    {
        public int Index { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public List<Data_UserDataInMonth> UserDataInMonth { get; set; }
    }  
    public class Data_UserDataInMonth
    {
        public Data_Month Dates { get; set; }
        public double AVG { get; set; }
        public double Count_Score2 { get; set; } = 0;
        public double Count_Score3 { get; set; } = 0;
        public double Count_Score4 { get; set; } = 0;
        public double Count_Score5 { get; set; } = 0;

    }
    public struct Data_Month
    {
        enum mesyac { Январь = 1, Февраль, Март, Апрель, Май, Июнь, Июль, Август, Сентябрь, Октябрь, Ноябрь, Декабрь };

        public int Month { get; private set; }
        public int Day { get; private set; }
        public int Year { get; private set; }

        public bool IsLeapYear { get; private set; }

        private DateTime Date = DateTime.Now;

        public Data_Month(int day, int month, int year) : this()
        {
            SetData(day, month, year);
        }

        public string GetNameMonth()
        {
            return ((mesyac)(Month)).ToString();
        }
        public Data_Month(DateTime date) :this() 
        {
            SetData(date.Day, date.Month, date.Year);
        }
        public DateTime GetDate() { return Date; }
        public void SetData(int day, int month, int year)
        {
            try
            {
                Date = DateTime.Parse($"{day}.{month}.{year}");
                if (DateTime.IsLeapYear(Date.Year))
                {
                    IsLeapYear = true;
                }
                else 
                {
                    IsLeapYear = false;
                }

                Month = month;
                Day = day;
                Year = year;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

    public class Data_StatisticCustom
    { 
        public List<Data_AllUserPacket> data_AllUsers { get; set; } =  new List<Data_AllUserPacket>();
    }

    public class Data_AllUserPacket 
    {
        public int Index { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateBirch { get; set; } = string.Empty;
    }

}
