using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class Account
    {
        public delegate void DisconnectHandler();

        public event DisconnectHandler? Disconnect;

        public bool IsActive { get; private set; }


        public string GUI { get; private set; } = "";

        public int ID { get; set; } = 0;
        public string Login { get; set; } = "null";

        public string Surname { get; set; } = "null";
        public string Firstname { get; set; } = "null";
        public string Middlemane { get; set; } = "null";
        public string Datebirch { get; set; } = "null";
        public string Gender { get; set; } = "null";

        public int IDRoly { get; set; } = 0;

        public string NameRoly { get; set; } = "null";
        public bool AllRoly { get; set; } = false;
        public bool ReadUser { get; set; } = false;
        public bool ReadSotrud { get; set; } = false;
        public bool ReadClass { get; set; } = false;
        public bool ReadPredmet { get; set; } = false;
        public bool CreateAndViewReport { get; set; } = false;
        public bool TestReady { get; set; } = false;
        public bool CreateTest { get; set; } = false;
        public bool CreateGroup { get; set; } = false;
        public bool ConnectGroup { get; set; } = false;

        public bool AddSotrudForPredmet { get; set; } = false;
        public bool DeleteSotrudForPredmet { get; set; } = false;

        public bool ViewDataUser { get; set; } = false;
        public bool ViewDataSotrud { get; set; } = false;
        public bool ViewPrepmet { get; set; } = false;
        public bool ViewClass { get; set; } = false;


        public double CountAssessment2 { get; set; } = 0;
        public double CountAssessment3 { get; set; } = 0;
        public double CountAssessment4 { get; set; } = 0;
        public double CountAssessment5 { get; set; } = 0;

        public Account()
        {


        }

  
        public void Set(Data_Access access, string login,string guid)
        {
            this.GUI = guid;

            this.ID = access.IdUser;
            this.IDRoly = access.IdRoly;
            this.Login = login;
            this.Surname = access.Surname;
            this.Firstname = access.Firstname;
            this.Middlemane = access.Middlemane;
            this.Datebirch = DateTime.Parse(access.Datebirch).ToShortDateString();
            this.Gender = access.Gender;

            this.NameRoly = access.NameRoly;
            this.AllRoly = access.AllRoly;
            this.ReadUser = access.ReadUser;
            this.ReadSotrud = access.ReadSotrud;
            this.ReadClass = access.ReadClass;
            this.ReadPredmet = access.ReadPredmet;
            this.CreateAndViewReport = access.CreateAndViewReport;
            this.TestReady = access.TestReady;
            this.CreateTest = access.CreateTest;
            this.CreateGroup = access.CreateGroup;
            this.ConnectGroup = access.ConnectGroup;
            this.AddSotrudForPredmet = access.AddSotrudForPredmet;
            this.DeleteSotrudForPredmet = access.DeleteSotrudForPredmet;

            this.ViewDataUser = access.ViewDataUser;
            this.ViewDataSotrud = access.ViewDataSotrud;
            this.ViewPrepmet = access.ViewPrepmet;
            this.ViewClass = access.ViewClass;

            this.IsActive = true;
        }

        /// <summary>
        /// Получить Фамилию и Имя акканута
        /// </summary>
        /// <returns>Surname and Firstname</returns>
        public string GetName()
        {
            return $"{Surname} {Firstname}";
        }

        /// <summary>
        /// Получить роль аккаунта
        /// </summary>
        /// <returns>NameRoly</returns>
        public string GetRole()
        {
            return $"{NameRoly}";
        }

        public void Dispose()
        {
            this.IsActive = false;
            Disconnect?.Invoke();
        }
    }
}
