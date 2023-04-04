using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class ViewUserModel:MVVM_Core, ICommandMVVM
    {

        public delegate void ViewerInformationUserHandler(User _user);
        public event ViewerInformationUserHandler? ViewerInformationUser;


        #region FiltredVariable
        public StructfilterUser FilterUser { get; private set; }
        public void SetFilterStruct(DateTime From, DateTime To, int gender = 1,bool filterDate = false)
        {
            FilterUser = new StructfilterUser(From, To, FilterUser.GetGender(gender), filterDate);
        }

        #endregion

        public ViewUserModel() 
        {
            Collection = new ObservableCollection<object>();
            UsersCollectionViewer = new ObservableCollection<User>();
            FilterUser = new StructfilterUser();
        }    
        

        #region Private
        private User? selectedUsers;
        #endregion


        #region Public
        private ObservableCollection<User>? _usersCollectionViewer { get; set; }      
        public ObservableCollection<User>? UsersCollectionViewer
        {
            get { return _usersCollectionViewer; }

            set {
                _usersCollectionViewer = value; 
                OnPropertyChanged("UsersCollectionViewer"); 
            }
        }      
        public User? SelectedUser
        {
            get { return selectedUsers; }
            set
            {
                if (value == null) return;
                selectedUsers = value;
                OnPropertyChanged("SelectedUser");
            }
        }
#nullable disable
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
        public ICommand RemoveItems
        {
            get
            {
                return removeCommand ?? (removeCommand = new RelayCommand(obj =>
                {
                    var startlist = (System.Collections.IList)obj;
                    if (startlist != null)
                    {
                        DeleteSelectedItem(startlist);
                    }
                },
                   (obj) => UsersCollectionViewer.Count > 0));
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
        public ICommand ViewInformation
        {
            get
            {
                return viewInformation ?? (viewInformation = new RelayCommand(obj =>
                {
                    var item = obj as User;
                    if (item != null)
                    {             
                        ViewerInformationUser?.Invoke(item);
                    }
                }));
            }
        }
        #endregion


        #region Private Method
        /// <summary>
        /// Обновлеине данных без пепресоздания коллекции
        /// </summary>
        /// <param name="obj">Список данных</param>
        /// <param name="count">Количество данных</param>
        public void UpdateData(List<Data_UserList> obj, int count)
        {
            bool IsAppend = false;
            bool IsRemove = false;
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                for (int i = 0; i < count; i++)
                {
                    var user = obj[i];
                    var search = Collection.FirstOrDefault(o => (o as User).Index == user.Id);

                    for (int j = 0; j < Collection.Count; j++)
                    {
                        var delete = obj.FirstOrDefault(p => p.Id == (Collection[j] as User).Index);
                        if (delete == null)
                        {

                            Collection.Remove((Collection[j] as User));
                            IsRemove = true;
                            continue;
                        }
                    }

                    if (search == null)
                    {
                        Add(user);
                        IsAppend = true;
                    }
                    else
                    {
                        //Меняем данные в основной коллекции
                        (search as User).FIO = user.Name;
                        (search as User).Datebirch = DateTime.Parse(user.DateBirch).ToShortDateString();
                        (search as User).Gender = user.Gender;
                        (search as User).IsTeacher = user.IsTeacher;
                        (search as User).KlassesUser = user.KlassesUser;
                        (search as User).Role = user.Role;
                        (search as User).Login = user.Login;

                    }

                    await Task.Delay(0);
                }

                if (IsAppend) Refresh();
                if (IsRemove) Refresh();

                SetupTimer();

                OnOverlayShowing(false);
                void Add(Data_UserList user)
                {
                    Collection.Add(new User()
                    {
                        FIO = user.Name,
                        Datebirch = DateTime.Parse(user.DateBirch).ToShortDateString(),
                        Gender = user.Gender,
                        Index = user.Id,
                        IsTeacher = user.IsTeacher,
                        KlassesUser = user.KlassesUser,
                        RegistrationDate = user.RegistrationData,
                        Role = user.Role,
                        Login = user.Login,
                    });
                }
            });
        }



        /// <summary>
        /// Метод обновления данных
        /// </summary>
        public override void Refresh()
        {
            if (IsSearch) 
                RefreshWithFilter();
            else
                RefreshNoFilter();
        }
        /// <summary>
        /// Обновление с фильтром
        /// </summary>
        public override async void RefreshWithFilter()
        {
            try
            {
                UsersCollectionViewer = new ObservableCollection<User>();
                          
                    var userList = GetFilter();

                    if (userList != null)
                    {

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
                            if (IsView) OnChangeDataOverlay((i+1).ToString(), filterd.Count().ToString());
                            UsersCollectionViewer.Add((filterd.ToArray()[i] as User));
                            await Task.Delay(1);
                        }
                    }

                    OnPropertyChanged("Start");
                    OnPropertyChanged("ViweItems");
                    OnPropertyChanged("End");
                    OnPropertyChanged("TotalItems");
                

                if (IsView) 
                {
                    OnOverlayShowing(false);
                    if(Start>1) FirstPage.Execute(null);
                    if (UsersCollectionViewer.Count == 0)
                    {
                        CancelFilter.Execute(null);
                        _Main.Instance._Notification.Add("","Данные не найдены",TypeNotification.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"User_VM.RefreshFiltred вызвал ошибку: {ex.Message}");
                if (IsView) OnOverlayShowing(false);
            }
        }
        /// <summary>
        /// Обычное обновление без использования фильтра
        /// </summary>
        public override async void RefreshNoFilter()
        {
            try
            {
                UsersCollectionViewer = new ObservableCollection<User>();
                var userList = GetCollection(start, CountView, out totalItems);
                if (userList != null)
                {
                    if (IsView) OnOverlayShowing(true);
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), userList.Count.ToString());
                        UsersCollectionViewer.Add(userList[i] as User);
                        await Task.Delay(5);
                    }
                }

                OnPropertyChanged("Start");
                OnPropertyChanged("ViweItems");
                OnPropertyChanged("End");
                OnPropertyChanged("TotalItems");

                if (IsView) OnOverlayShowing(false);
            }
            catch (Exception ex)
            {
                Logger.Error($"ViewUserModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
                if (IsView) OnOverlayShowing(false);
            }
        }

        #endregion

        #region Public Method

        public void DeleteData(System.Collections.IList deleteList)
        {
            //Подчистим данные из view. Если из бд не удаляется данные восстановятся
            List<User> list = deleteList.Cast<User>().ToList();
            list.ForEach(auto => UsersCollectionViewer.Remove(auto));
            list.ForEach(auto => Collection.Remove(auto));
        }

        /// <summary>
        /// Устанавливает базовую коллекцию
        /// </summary>
        /// <param name="obj">Список данных пришедший от сервера</param>
        public void SetCollection(Data_UserViewer userlist)
        {
            var obj = userlist.UserList;
            if (obj == null)
            {
                _closeFilter();
                return;
            }

            if (obj.Count == 0)
            {
                OnOverlayShowing(false);
                _closeFilter();
                return;
            }

            if (userlist.FilterUser != null)
            {
                SetFilterStruct
                (
                    userlist.FilterUser.From,
                    userlist.FilterUser.To,
                    userlist.FilterUser.Gender,
                    userlist.FilterUser.IsFilterData
                );

                IsFilter = userlist.IsFilter;
            }

        
            if (IsUpdate && !userlist.IsFilter)
            {
                UpdateData(obj, obj.Count);
                return;
            }
            
            //Асинхронно заполняем коллекцию
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
               //Создаем новую коллекцию
               Collection = new ObservableCollection<object>();

                for (int i = 0; i < obj.Count; i++)
                {
                    //Прокидываем данные количестве в оверлей
                  //  OnChangeDataOverlay(i.ToString(), $"{obj.Count}");
                    Collection.Add(new User()
                    {
                        FIO = obj[i].Name,
                        Datebirch = DateTime.Parse(obj[i].DateBirch).ToShortDateString(),
                        Gender = obj[i].Gender,
                        Index = obj[i].Id,
                        IsTeacher = obj[i].IsTeacher,
                        KlassesUser = obj[i].KlassesUser,
                        RegistrationDate = obj[i].RegistrationData,
                        Role = obj[i].Role,
                        Login = obj[i].Login,
                    });

  
                    //Нужен для ассинхронного испольнения метода
                    await Task.Delay(0);
                    //Обновим просматриваемую коллекцию
       
                }

                Refresh();
                //Запустим таймер на обновление

                SetupTimer();
                IsUpdate = true;

                OnPropertyChanged("VB_ButtonFilter");
                OnPropertyChanged("VB_CancelFilter");

            });
        
            //Local Method
            void _closeFilter() 
            {
                if (userlist.IsFilter)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _Main.Instance._Notification.Add("", "Данные не найдены", TypeNotification.Error);
                    });

                    CancelFilter.Execute(null);
                }
                else
                {
                    Collection = new ObservableCollection<object>();
                    UsersCollectionViewer = new ObservableCollection<User>();
                }
            }

        }

 
        /// <summary>
        /// Задает количество просматриваемых элементов
        /// </summary>
        /// <param name="count">Количество элементов</param>
        public void SetCountView(int count)
        {
            if (count != CountView)
            {
                this.SetView(count);
                Refresh();
            }
        }

        /// <summary>
        /// Запускает поиск (фильтрацию)
        /// </summary>
        /// <param name="search">поиск</param>
        public override void Search(string search)
        {
            if (search.Trim().Length > 0)
            {
                IsSearchString = search;
                IsSearch= true;
            }
            else
            {
                IsSearchString = string.Empty;
                IsSearch = false;
            }

  
            Refresh();
        }

        /// <summary>
        /// Управление фильтром
        /// </summary>
        /// <param name="value"></param>
        public void Filter(bool value)
        {
            IsFilter = value;
            if (value == false)
                FilterUser.Clear();
        }

        #endregion

    }
}
