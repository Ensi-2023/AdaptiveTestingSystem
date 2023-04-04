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
    public class VM_UserTestingViewer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        private ObservableCollection<MV_UserTesting> _user { get; set; }

        private ObservableCollection<MV_UserTesting>? _userCollectionViewer { get; set; }
        public ObservableCollection<MV_UserTesting>? UserCollectionViewer
        {
            get { return _userCollectionViewer; }

            set
            {
                _userCollectionViewer = value;
                OnPropertyChanged("UserCollectionViewer");
            }
        }

        private MV_UserTesting? selectedUser;
        public MV_UserTesting? SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (value == null) return;
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public VM_UserTestingViewer()
        {
            UserCollectionViewer = new ObservableCollection<MV_UserTesting>();
            _user = new ObservableCollection<MV_UserTesting>();
        }

        public async Task SetData(List<Data_MultyServerClient> data)
        {

            if (data.Count == 0) return;

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];

                var test = new MV_UserTesting()
                {
                    GUID = item.GUID,
                    Index = _user.Count + 1,
                    IsCode = item.IsCode==Code.NewClientConnect?Code.ConnectedToServer: item.IsCode,
                    NameUser = item.NameClient
                };

                var search = _user.FirstOrDefault(x => x.GUID == item.GUID);
                if (search == null)
                {
                    _user.Add(test);
                    UserCollectionViewer.Add(test);
                }

                await Task.Delay(10);
            }

            OnPropertyChanged("UserCollectionViewer");

        }

        public void AddOneUser(Data_MultyServerClient user)
        {
            var test = new MV_UserTesting()
            {
                GUID = user.GUID,
                Index = _user.Count + 1,
                IsCode = Code.ConnectedToServer,
                NameUser = user.NameClient
            };

            var search = _user.FirstOrDefault(x => x.GUID == user.GUID);
            if (search == null)
            {
                _user.Add(test);
                UserCollectionViewer.Add(test);
            }

            OnPropertyChanged("UserCollectionViewer");
        }

        public void RemoveUser(Data_MultyServerClient user)
        {

            var userSearch = _user.FirstOrDefault(x => x.GUID == user.GUID);
            if (userSearch != null)
            {
                if (userSearch.IsCode == Code.TestingCompleted) return;
                _user.Remove(userSearch);
                UserCollectionViewer.Remove(userSearch);
                OnPropertyChanged("UserCollectionViewer");
            }

            Search("");
        }


        public string GetNameuser(string guid)
        {
            var userSearch = _user.FirstOrDefault(x => x.GUID == guid);
            if (userSearch == null) return "";
            return userSearch.NameUser;
        }

        public void UpdateStatusUser(Data_MultyServerClient user)
        {
            var userSearch = _user.FirstOrDefault(x => x.GUID == user.GUID);
            if (userSearch != null)
            {
                userSearch.IsCode = user.IsCode;
                userSearch = UserCollectionViewer.FirstOrDefault(x => x.GUID == user.GUID);
                userSearch.IsCode = user.IsCode;
                OnPropertyChanged("UserCollectionViewer");
            }
        }
        public void Search(string isSearchString)
        {
            _Main.Instance.OverlayShow(true);
            UserCollectionViewer = new ObservableCollection<MV_UserTesting>();

            var filterd = _user.Where(x =>
                (
                  (x as MV_UserTesting).NameUser.ToLower().Trim().Contains(isSearchString.ToLower().Trim())
                ));

            for (int i = 0; i < filterd.Count(); i++)
            {

                var test = new MV_UserTesting()
                {
                    GUID = filterd.ToArray()[i].GUID,
                    Index = UserCollectionViewer.Count+1,
                    NameUser = filterd.ToArray()[i].NameUser,
                    IsCode = filterd.ToArray()[i].IsCode
                };

                UserCollectionViewer.Add(test);
            }
            OnPropertyChanged("UserCollectionViewer");
            _Main.Instance.OverlayShow(false);
        }


        internal void ViewInformation(object item)
        {

            if ((item as MV_UserTesting) != null)
            {
                // _Main.Instance.Manager.Next(new GUI_QuestViewer((item as MV_AllTesting).Index, index));
            }

        }

    }
}

