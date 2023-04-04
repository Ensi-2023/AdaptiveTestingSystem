using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel
{
    public class modelPage_2 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }
        private List<modelPage_2_user> _selecUsers;

        public List<modelPage_2_user> SelectUsers
        {
            get { return _selecUsers; }
            set { _selecUsers = value; OnPropertyChange("SelectUsers"); }
        }

       private ObservableCollection<modelPage_2_user> db { get; set; }

        public ObservableCollection<modelPage_2_user> UserView { get; set; }

        public modelPage_2()
        {
            db = new ObservableCollection<modelPage_2_user>();
            UserView = new ObservableCollection<modelPage_2_user>();
        }

        public void SetUser(int index, string name, string gender, string dateBirch)
        {
            var user = new modelPage_2_user()
            {
                Index = index,
                Name = name,
                Gender = gender,
                DateBirch = DateTime.Parse(dateBirch).ToShortDateString(),
            };

            db.Add(user);
            UserView.Add(user);

            OnPropertyChange("UserView");
        }


        public System.Collections.IList Search(string search, System.Collections.IList selectedItems)
        {
            UserView = new ObservableCollection<modelPage_2_user>();
            List<modelPage_2_user> list = selectedItems.Cast<modelPage_2_user>().ToList();


            if (selectedItems.Count > 0)
            {
                foreach (var item in selectedItems)
                {
                    var obj = item as modelPage_2_user;
                    if (obj != null)
                    {
                        UserView.Add(obj);
                    }
                }
            }


            foreach (var item in db)
            {
                var obj = item as modelPage_2_user;

                if (UserView.FirstOrDefault(o => o.Index == item.Index)!=null) continue;

                if (search.Trim().Length > 0)
                {
                    if (item.Name.Trim().ToLower().Contains(search.Trim().ToLower())) UserView.Add(obj);
                }
                else UserView.Add(obj);
            }           

            OnPropertyChange("UserView");

            return list;
        }

        public void ViewSelect(IList selectedItems)
        {
            UserView = new ObservableCollection<modelPage_2_user>();
            List<modelPage_2_user> list = selectedItems.Cast<modelPage_2_user>().ToList();


            if (selectedItems.Count > 0)
            {
                foreach (var item in selectedItems)
                {
                    var obj = item as modelPage_2_user;
                    if (obj != null)
                    {
                        UserView.Add(obj);
                    }
                }
            }

            OnPropertyChange("UserView");
        }

        public void ViewAll()
        {
            UserView = new ObservableCollection<modelPage_2_user>();

            foreach (var item in db)
            {
                var obj = item as modelPage_2_user;

                UserView.Add(obj);
            }
            OnPropertyChange("UserView");
        }
    }
}
