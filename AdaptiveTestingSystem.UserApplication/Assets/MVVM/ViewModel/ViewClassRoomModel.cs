using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class ViewClassRoomModel : MVVM_Core, ICommandMVVM
    {
#nullable disable


        public ViewClassRoomModel()
        {
            Collection = new ObservableCollection<object>();
            RoomCollectionViewer = new ObservableCollection<CRoom>();
           
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
                   (obj) => RoomCollectionViewer.Count > 0));
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


        public override void Refresh()
        {
            if (IsSearch)
                RefreshWithFilter();
            else
                RefreshNoFilter();
        }
        private void Filter(bool value)
        {

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

        public void SetCollection(List<Data_Klass> obj)
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

                for (int i = 0; i < collection.Count; i++)
                {
                    //Прокидываем данные количестве в оверлей
                   // OnChangeDataOverlay(i.ToString(), $"{collection.Count}");
                    Collection.Add(new CRoom()
                    {
                        Index = collection[i].Id,
                        ClassName = collection[i].Name,
                        FIO = collection[i].Employee == null?"-": collection[i].Employee,
                        CountUser = collection[i].CountUser,
                        EmployeeID = collection[i].EmployeeID,
                        UserList = collection[i].Users,
                        DatebirchEmployee = collection[i].DatebirchEmployee,
                        GenderEmployee = collection[i].GenderEmployee
                    });;


                    //Нужен для ассинхронного испольнения метода
                    await Task.Delay(0);
                    //Обновим просматриваемую коллекцию
        

                    //OnPropertyChanged("VB_ButtonFilter");
                    //OnPropertyChanged("VB_CancelFilter");
                }


                Refresh();
                //Запустим таймер на обновление

                SetupTimer();
                IsUpdate = true;
            });



        }

        private void UpdateData(List<Data_Klass> obj, int count)
        {
            bool IsAppend = false;
            bool IsRemove = false;
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                for (int i = 0; i < count; i++)
                {
                    var classRoom = obj[i];
                    var search = Collection.FirstOrDefault(o => (o as CRoom).Index == classRoom.Id);

                    for (int j = 0; j < Collection.Count; j++)
                    {
                        var delete = obj.FirstOrDefault(p => p.Id == (Collection[j] as CRoom).Index);
                        if (delete == null)
                        {

                            Collection.Remove((Collection[j] as CRoom));
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
                        (search as CRoom).FIO = classRoom.Employee == null ? "-" : classRoom.Employee;
                        (search as CRoom).EmployeeID = classRoom.EmployeeID;
                        (search as CRoom).ClassName = classRoom.Name;
                        (search as CRoom).CountUser = classRoom.CountUser;
                        (search as CRoom).UserList = classRoom.Users;
                        (search as CRoom).GenderEmployee = classRoom.GenderEmployee;
                        (search as CRoom).DatebirchEmployee = classRoom.DatebirchEmployee;
   
                    }

                    await Task.Delay(0);
                }

                if (IsAppend) Refresh();
                if (IsRemove) Refresh();

                SetupTimer();

                OnOverlayShowing(false);
                void Add(Data_Klass cRoom)
                {
                    Collection.Add(new CRoom()
                    {
                        Index = cRoom.Id,
                        ClassName = cRoom.Name,
                        FIO = cRoom.Employee== null ? "-": cRoom.Employee,
                        CountUser = cRoom.CountUser,
                        EmployeeID = cRoom.EmployeeID,
                        UserList = cRoom.Users,
                        DatebirchEmployee= cRoom.DatebirchEmployee,
                        GenderEmployee = cRoom.GenderEmployee,
                    });
                }
            });
        }

        /// <summary>
        /// Обновление с фильтром
        /// </summary>
        public override async void RefreshWithFilter()
        {
            try
            {
                RoomCollectionViewer = new ObservableCollection<CRoom>();

                var userList = GetFilter();

                if (userList != null)
                {

                    var filterd = userList.Where(x =>
                    (
                      (x as CRoom).FIO.ToLower().Trim().Contains(IsSearchString.ToLower().Trim()) ||
                      (x as CRoom).ClassName.Contains(IsSearchString.ToLower().Trim()) 
                    ));

                    totalItems = filterd.Count();

                    if (IsView) OnOverlayShowing(true);

                    for (int i = start; i < start + CountView && i < filterd.Count(); i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), filterd.Count().ToString());
                        RoomCollectionViewer.Add((filterd.ToArray()[i] as CRoom));
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
                    if (Start > 1) FirstPage.Execute(null);
                    if (RoomCollectionViewer.Count == 0)
                    {
                        CancelFilter.Execute(null);
                        _Main.Instance._Notification.Add("", "Данные не найдены", TypeNotification.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ViewClassRoomModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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
                RoomCollectionViewer = new ObservableCollection<CRoom>();
                var userList = GetCollection(start, CountView, out totalItems);
                if (userList != null)
                {
                    if (IsView) OnOverlayShowing(true);
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), userList.Count.ToString());
                        RoomCollectionViewer.Add(userList[i] as CRoom);
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
                Logger.Error($"ViewClassRoomModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
                if (IsView) OnOverlayShowing(false);
            }
        }


        public ICommand ViewInformation
        {
            get
            {
                return viewInformation ?? (viewInformation = new RelayCommand(obj =>
                {
                    var item = obj as CRoom;
                    if (item != null)
                    {
                        ViewerInformationRoom?.Invoke(item);
                    }
                }));
            }
        }


        public delegate void ViewerInformationUserHandler(CRoom room);
        public event ViewerInformationUserHandler? ViewerInformationRoom;


        #region Private
        private CRoom? selectedRoom;
        #endregion


        private ObservableCollection<CRoom>? _roomCollectionViewer { get; set; }
        public ObservableCollection<CRoom>? RoomCollectionViewer
        {
            get { return _roomCollectionViewer; }

            set
            {
                _roomCollectionViewer = value;
                OnPropertyChanged("RoomCollectionViewer");
            }
        }
        public CRoom? SelectedRoom
        {
            get { return selectedRoom; }
            set
            {
                if (value == null) return;
                selectedRoom = value;
                OnPropertyChanged("SelectedRoom");
            }
        }


    }
}
