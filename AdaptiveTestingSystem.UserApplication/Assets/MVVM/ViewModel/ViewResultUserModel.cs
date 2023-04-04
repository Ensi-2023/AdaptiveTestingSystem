using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class ViewResultUserModel : MVVM_Core, ICommandMVVM
    {

        public delegate void ViewerInformationUserHandler(User user);
        public event ViewerInformationUserHandler? ViewerInformationUser;



        public delegate void SelectUserUserHandler(List<User> users);
        public event SelectUserUserHandler? SelectUsersInGrid;

        #region Private
        private User? selectedUser;
        #endregion


        private ObservableCollection<User>? _userCollectionViewer { get; set; }
        public ObservableCollection<User>? UserCollectionViewer
        {
            get { return _userCollectionViewer; }

            set
            {
                _userCollectionViewer = value;
                OnPropertyChanged("UserCollectionViewer");
            }
        }
        public User? SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (value == null) return;
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }



        public ICommand FirstPage
        {
            get
            {
                if (firstCommand == null)
                {
                    firstCommand = new RelayCommand
                    (
                        param =>
                        {
                            start = 0;
                            Refresh();
                        },
                        param =>
                        {
                            return start - CountView >= 0 ? true : false;
                        }
                    );
                }

                return firstCommand;
            }
        }
        public ICommand LastPage
        {
            get
            {
                if (lastCommand == null)
                {
                    lastCommand = new RelayCommand
                    (
                        param =>
                        {
                            start = (totalItems / CountView - 1) * CountView;
                            start += totalItems % CountView == 0 ? 0 : CountView;
                            Refresh();
                        },
                        param =>
                        {
                            return start + CountView < totalItems ? true : false;
                        }
                    );
                }

                return lastCommand;
            }
        }
        public ICommand NextPage
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand
                    (
                        param =>
                        {
                            start += CountView;
                            Refresh();
                        },
                        param =>
                        {
                            return start + CountView < totalItems ? true : false;
                        }
                    );
                }

                return nextCommand;
            }
        }
        public ICommand PrevPage
        {
            get
            {
                if (previousCommand == null)
                {
                    previousCommand = new RelayCommand
                    (
                        param =>
                        {
                            start -= CountView;
                            Refresh();
                        },
                        param =>
                        {
                            return start - CountView >= 0 ? true : false;
                        }
                    );
                }

                return previousCommand;
            }
        }
        public ICommand CancelFilter
        {
            get
            {
                return cancelfilterCommand ?? (cancelfilterCommand = new RelayCommand(obj =>
                {
                    Filter(false);
                }));
            }
        }
        public ICommand ViewInformation => throw new NotImplementedException();
        public ICommand RemoveItems => throw new NotImplementedException();

        List<User> usersSelect { get; set; } = new List<User>();

        public override void Refresh()
        {
            if (IsSearch)
                RefreshWithFilter();
            else
                RefreshNoFilter();
        }

        public void AddSelect(IList selectedItems)
        {
            usersSelect = selectedItems.Cast<User>().ToList();
        }

        public async override void RefreshNoFilter()
        {
            try
            {
                   
                var userList = GetCollection(start, CountView, out totalItems);
                UserCollectionViewer = new ObservableCollection<User>();

                if (userList != null)
                {
                    if (usersSelect.Count > 0)
                    {
                        foreach (var item in usersSelect)
                        {
                            var obj = item as User;
                            if (obj != null)
                            {
                                UserCollectionViewer.Add(obj);
                            }
                        }
                    }



                    if (IsView) OnOverlayShowing(true);
                    for (int i = 0; i < userList.Count; i++)
                    {
                        var item = userList[i] as User;
                        if (IsView) OnChangeDataOverlay((i + 1).ToString(), userList.Count.ToString());
                        if (UserCollectionViewer.FirstOrDefault(o => o.Index == item.Index) != null) continue;
                        UserCollectionViewer.Add(item);
                        await Task.Delay(5);
                    }
                }

                OnPropertyChanged("Start");
                OnPropertyChanged("ViweItems");
                OnPropertyChanged("End");
                OnPropertyChanged("TotalItems");

                if (IsView) 
                {
                    SelectUsersInGrid?.Invoke(usersSelect);
                    OnOverlayShowing(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ViewResultUserModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
                if (IsView) OnOverlayShowing(false);
            }
        }

        public async override void RefreshWithFilter()
        {
            try
            {

                UserCollectionViewer = new ObservableCollection<User>();

                var userList = GetFilter();


                if (userList != null)
                {

                    if (usersSelect.Count > 0)
                    {
                        foreach (var item in usersSelect)
                        {
                            var obj = item as User;
                            if (obj != null)
                            {
                                UserCollectionViewer.Add(obj);
                            }
                        }
                    }



                    var filterd = userList.Where(x =>
                    (
                      (x as User).FIO.ToLower().Trim().Contains(IsSearchString.ToLower().Trim()) ||
                      (x as User).Gender.ToLower().Trim().Contains(IsSearchString.ToLower().Trim()) ||
                      (x as User).Datebirch.Contains(IsSearchString.ToLower().Trim())
                    ));

                    totalItems = filterd.Count();

                    if (IsView) OnOverlayShowing(true);

                    for (int i = start; i < start + CountView && i < filterd.Count(); i++)
                    {
                        var item = filterd.ToArray()[i] as User;

                        if (IsView) OnChangeDataOverlay((i + 1).ToString(), filterd.Count().ToString());
                        if (UserCollectionViewer.FirstOrDefault(o => o.Index == item.Index) != null) continue;
                        UserCollectionViewer.Add(item);
                        await Task.Delay(1);
                    }
                }

                OnPropertyChanged("Start");
                OnPropertyChanged("ViweItems");
                OnPropertyChanged("End");
                OnPropertyChanged("TotalItems");


                if (IsView)
                {
                    SelectUsersInGrid?.Invoke(usersSelect);
                    OnOverlayShowing(false);
                    if (Start > 1) FirstPage.Execute(null);
                    if (UserCollectionViewer.Count == 0)
                    {
                        CancelFilter.Execute(null);
                        _Main.Instance._Notification.Add("", "Данные не найдены", TypeNotification.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ViewResultUserModel.RefreshFiltred вызвал ошибку: {ex.Message}");
                if (IsView) OnOverlayShowing(false);
            }
        }

      

        public override void Search(string search)
        {
            if (search.Trim().Length > 0)
            {
                IsSearchString = search;
                IsSearch = true;
                            
            }
            else
            {
                IsSearchString = string.Empty;
                IsSearch = false;
             

            }

            Refresh();
        }


        private void Filter(bool v)
        {

        }

        public void SetCollection(List<Data_AllUserPacket> obj)
        {
            var collection = obj;
            if (collection == null)
            {
                OnOverlayShowing(false);
                return;
            }


            if (IsUpdate)
            {
                UpdateData(obj, obj.Count);
                return;
            }
 

            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                //Создаем новую коллекцию
                Collection = new ObservableCollection<object>();
                for (int j = 0;                      j < 100;   j++)
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        //Прокидываем данные количестве в оверлей
                        // OnChangeDataOverlay(i.ToString(), $"{collection.Count}");
                        Collection.Add(new User()
                        {
                            Index = randomIndex(collection[i].Index),
                            Datebirch = collection[i].DateBirch,
                            Gender = collection[i].Gender,
                            FIO = randomStr()
                        }); ;

                        await Task.Delay(0);
                    }
                }
                Refresh();
                IsUpdate = true;
            });

        }

        public void SetCountView(int count)
        {
            if (count != CountView)
            {
                this.SetView(count);
                Refresh();
            }
        }


        private int randomIndex(int startIndex)
        {
            Random rand = new Random();
            int newindex = 0;
            do
            {
                newindex = rand.Next(startIndex,999999999);
            }
            while (newindex == startIndex);
            return newindex;
        }

        private string randomStr()
        {
            Random rand = new Random();

            // Choosing the size of string
            // Using Next() string
            int stringlen = rand.Next(4, 15);
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {

                // Generating a random number.
                randValue = rand.Next(0, 26);

                // Generating random character by converting
                // the random number into character.
                letter = Convert.ToChar(randValue + 65);

                // Appending the letter to string.
                str = str + letter;
            }

            return str;
        }


        private void UpdateData(List<Data_AllUserPacket> obj, int count)
        {
            bool IsAppend = false;
            bool IsRemove = false;
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                for (int i = 0; i < count; i++)
                {
                    var classRoom = obj[i];
                    var search = Collection.FirstOrDefault(o => (o as User).Index == classRoom.Index);

                    for (int j = 0; j < Collection.Count; j++)
                    {
                        var delete = obj.FirstOrDefault(p => p.Index == (Collection[j] as User).Index);
                        if (delete == null)
                        {

                            Collection.Remove((Collection[j] as User));
                            IsRemove = true;
                            continue;
                        }
                    }

                    if (search == null)
                    {
                        Add(classRoom);
                        IsAppend = true;
                    }
                    else
                    {
                        //Меняем данные в основной коллекции
                        (search as User).FIO = classRoom.Name == null ? "-" : classRoom.Name;
                        (search as User).Index = classRoom.Index;
                        (search as User).Datebirch = classRoom.DateBirch;
                        (search as User).Gender = classRoom.Gender;

                    }

                    await Task.Delay(0);
                }

                if (IsAppend) Refresh();
                if (IsRemove) Refresh();

                SetupTimer();

                OnOverlayShowing(false);
                void Add(Data_AllUserPacket _user)
                {
                    Collection.Add(new User()
                    {
                        Index = _user.Index,
                        Gender = _user.Gender,
                        FIO = _user.Name,
                        Datebirch = _user.DateBirch
                    });
                }
            });
        }

        public void DeleteSelectUser(User? user)
        {
            foreach (var item in usersSelect)
            {
                if (item.Index == user.Index)
                {
                    usersSelect.Remove(item);
                    return;
                }
            }
        }

        public async void ViewAllSelectUser()
        {
            if (usersSelect.Count > 0)
            {
                UserCollectionViewer = new ObservableCollection<User>();
                if (IsView) OnOverlayShowing(true);
                foreach (var item in usersSelect)
                {
                    var obj = item as User;
                    if (obj != null)
                    {
                        UserCollectionViewer.Add(obj);
                    }

                    await Task.Delay(5);
                }

                OnPropertyChanged("Start");
                OnPropertyChanged("ViweItems");
                OnPropertyChanged("End");
                OnPropertyChanged("TotalItems");

                if (IsView)
                {
                    OnOverlayShowing(false);
                    SelectUsersInGrid?.Invoke(usersSelect);
                }
            }
            else
            {
                _Main.Instance._Notification.Add("", "Данные не найдены", TypeNotification.Warning);
            }
        }
    }
}
