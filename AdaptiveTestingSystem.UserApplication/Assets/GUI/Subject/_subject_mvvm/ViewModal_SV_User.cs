using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_mvvm
{
    public class ViewModal_SV_User : INotifyPropertyChanged
    {


        public delegate void LoadedData();
        public event LoadedData? Loaded;


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private Modal_SV_User selectedUser;
        private List<Data_Subject_User> userList;



        private ICommand _delete;
        public ICommand Delete
        {
            get
            {
                return _delete ?? (_delete = new RelayCommand(obj =>
                {
                    var startlist = (System.Collections.IList)obj;
                    if (startlist != null)
                    {
                        DeleteSelectedItem(startlist);
                    }
                },
                   (obj) => CollectionUser.Count > 0));
            }
        }

        private void DeleteSelectedItem(IList startlist)
        {
            if (startlist.Count > 0)
            {
                if (startlist != null)
                {
                    var list = new List<Data_UserList>();

                    foreach (var item in startlist)
                    {
                        var obj = item as Modal_CRV_User;
                        if (obj != null)
                        {
                            list.Add(new Data_UserList() { Id = obj.Index });
                        }
                    }

                    if (list.Count > 0)
                    {
                        if (MessageShow.Show($"Вы действительно хотите удалить {list.Count} записей?", "Удаление", MessageShow.Type.Question) == true)
                        {
                            var packet = new Data_FirstCommand()
                            {
                                Command = "Command_DeleteUserInSubject",
                                Json = JsonSerializer.Serialize(new Data_Klass_UserDelete()
                                {
                                    IsCode = Code.Null,
                                    Index = IndexSubject,
                                    Users = list
                                })
                            };

                            _Main.Instance.Client.Send(JsonSerializer.Serialize(packet));
                            _Main.Instance._Notification.Add("", "Запрос на удаление оправвлен", TypeNotification.Message);

                            DeleteItemView(startlist);

                        }
                    }
                }
            }
        }

        public void DeleteItemView(IList startlist)
        {
            for (int i = 0; i < startlist.Count; i++)
            {
                var obj = startlist[i] as Modal_CRV_User;
                if (obj != null)
                {
                    var searchItem = userList.FirstOrDefault(o => o.Index == obj.Index);
                    if (searchItem != null)
                    {
                        userList.Remove(searchItem);
                    }
                }
            }

            UpdateData();
        }

        public ViewModal_SV_User(List<Data_Subject_User> userList, int index)
        {
            this.userList = userList;
            this.IndexSubject = index;
            UpdateData();
        }


        public ViewModal_SV_User(int index)
        {
            this.IndexSubject = index;
        }

        public void SetUserList(List<Data_UserList> userListDB)
        {
            this.userList = new List<Data_Subject_User>();

            foreach (var obj in userListDB)
            {
                userList.Add(new Data_Subject_User()
                { 
                  Index = obj.Id,
                  DateBirch= obj.DateBirch,
                  Gender= obj.Gender,   
                  Name = obj.Name   
                });
            }

            UpdateData(false);
        }

        private void UpdateData(bool viewOverlay = true)
        {

            dbCollectionUser = new ObservableCollection<Modal_SV_User>();
            CollectionUser = new ObservableCollection<Modal_SV_User>();


            if (userList == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var item in userList)
                {
                    dbCollectionUser.Add(new Modal_SV_User()
                    {
                        DayBirch = item.DateBirch,
                        Gender = item.Gender,
                        Index = item.Index,
                        Name = item.Name,

                    });

                }

                Update(dbCollectionUser, viewOverlay);
            });




        }


        public void Add(int index, string name, string daybirch, string gender)
        {

            var searchUser = dbCollectionUser.FirstOrDefault(o => o.Index == index);
            if (searchUser != null) return;

            dbCollectionUser.Add(new Modal_SV_User()
            {
                DayBirch = daybirch,
                Gender = gender,
                Index = index,
                Name = name,
            });


            Update(dbCollectionUser);
        }

        private void Update(ObservableCollection<Modal_SV_User> _dbCollectionUser, bool viewOverlay = true)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                CollectionUser = new ObservableCollection<Modal_SV_User>();

                if (viewOverlay) _Main.Instance.OverlayShow(viewOverlay, TypeOverlay.loading, "Подождите...", "Обновляю коллекцию");

                foreach (var item in _dbCollectionUser)
                {

                    var searchUser = CollectionUser.FirstOrDefault(o => o.Index == item.Index);
                    if (searchUser != null) continue;

                    CollectionUser.Add(new Modal_SV_User()
                    {
                        DayBirch = item.DayBirch,
                        Gender = item.Gender,
                        Index = item.Index,
                        Name = item.Name,

                    });

                    await Task.Delay(10);
                }

                OnPropertyChanged("CollectionUser");

                _Main.Instance.OverlayShow(false);

                Loaded?.Invoke();
            });
        }

        internal void SearchClear(bool viewOverlay = true)
        {
            Update(dbCollectionUser, viewOverlay);
        }

        internal async void Search(string text, bool viewOverlay = true)
        {
            var list = await SearchData(text, viewOverlay);
            if (list == null) return;
            if (list.Count > 0)
            {
                Update(list, viewOverlay);
            }
            else
            {
                _Main.Instance._Notification.Add("", "Не найдено", TypeNotification.Warning);
            }
        }

        private async Task<ObservableCollection<Modal_SV_User>> SearchData(string text, bool viewOverlay = true)
        {
            var obj = new ObservableCollection<Modal_SV_User>();

            _Main.Instance.OverlayShow(viewOverlay, TypeOverlay.loading, "Подождите...", "Идет поиск");

            foreach (var item in dbCollectionUser)
            {
                if (item.Name.Trim().ToLower().Contains(text.Trim().ToLower()) ||
                    item.Gender.Trim().ToLower().Contains(text.Trim().ToLower()))
                    obj.Add(item);

                await Task.Delay(10);
            }

            _Main.Instance.OverlayShow(false);

            return obj;
        }

        internal void SaveData(int indexUser, string fio, string gender, string date)
        {
            foreach (var item in userList)
            {
                if (item.Index == indexUser)
                {
                    item.Gender = gender;
                    item.Name = fio;
                    item.DateBirch = date;
                    break;
                }
            }

            UpdateData(false);
        }


        public ObservableCollection<Modal_SV_User> CollectionUser { get; set; }
        private ObservableCollection<Modal_SV_User> dbCollectionUser;


        public Modal_SV_User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectUser");
            }
        }

        public int IndexSubject { get; private set; }
    }
}