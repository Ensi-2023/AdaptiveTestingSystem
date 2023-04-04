using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class StructList
    {
        public struct StructfilterUser
        {
            public bool IsDateFilter {get;set;}
            public DateTime DateBirch_From { get; set; }
            public DateTime DateBirch_To { get; set; }
            public enum GenderUserSelected
            {
                all = 1,
                male,
                female,
            }
            public GenderUserSelected GenderUser { get; set; } = GenderUserSelected.all;

            public StructfilterUser(DateTime From, DateTime To, GenderUserSelected Gender = GenderUserSelected.all,bool dateFilter=false)
            {
                this.DateBirch_From = From;
                this.DateBirch_To = To;
                this.GenderUser = Gender;
                this.IsDateFilter = dateFilter;
            }

            public GenderUserSelected GetGender(int gender)
            {
                return (GenderUserSelected)gender;
            }

            public void Clear()
            {
                DateBirch_From = DateTime.MinValue;
                DateBirch_To = DateTime.MinValue;
                GenderUser = GenderUserSelected.all;
            }
        }
    }
}
