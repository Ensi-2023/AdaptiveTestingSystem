using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class ViewRolyUserModel : MVVM_Core, ICommandMVVM
    {

        private ObservableCollection<RolyUser>? _rolyUserCollectionViewer { get; set; }
        public ObservableCollection<RolyUser>? RolyUserCollectionViewer
        {
            get { return _rolyUserCollectionViewer; }

            set
            {
                _rolyUserCollectionViewer = value;
                OnPropertyChanged("RolyUserCollectionViewer");
            }
        }

        private RolyUser? selectedRolyUser;
        public RolyUser? SelectedRolyUser
        {
            get { return selectedRolyUser; }
            set
            {
                if (value == null) return;
                selectedRolyUser = value;
                OnPropertyChanged("SelectedRolyUser");
            }
        }



        public delegate void ViewerInformationRolyUserHandler(RolyUser rolyUser);
        public event ViewerInformationRolyUserHandler? ViewerInformationRolyUser;


        public ViewRolyUserModel()
        {
            Collection = new ObservableCollection<object>();
        }

        public ICommand ViewInformation
        {
            get
            {
                return viewInformation ?? (viewInformation = new RelayCommand(obj =>
                {
                    var item = obj as RolyUser;
                    if (item != null)
                    {
                        ViewerInformationRolyUser?.Invoke(item);
                    }
                }));
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
                   (obj) => RolyUserCollectionViewer.Count > 0));
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



        public override void Refresh()
        {
            if (IsSearch)
                RefreshWithFilter();
            else
                RefreshNoFilter();
        }

        public override async void RefreshNoFilter()
        {
            try
            {
                RolyUserCollectionViewer = new ObservableCollection<RolyUser>();
                var userList = GetCollection(start, CountView, out totalItems);
                if (userList != null)
                {
                    if (IsView) OnOverlayShowing(true);
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), userList.Count.ToString());
                        RolyUserCollectionViewer.Add(userList[i] as RolyUser);
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
                Logger.Error($"ViewRolyUserModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
                if (IsView) OnOverlayShowing(false);
            }
        }

        public override async void RefreshWithFilter()
        {
            try
            {
                RolyUserCollectionViewer = new ObservableCollection<RolyUser>();

                var userList = GetFilter();

                if (userList != null)
                {

                    var filterd = userList.Where(x =>
                    (
                      (x as RolyUser).Name.ToLower().Trim().Contains(IsSearchString.ToLower().Trim())                     
                    ));

                    totalItems = filterd.Count();

                    if (IsView) OnOverlayShowing(true);

                    for (int i = start; i < start + CountView && i < filterd.Count(); i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), filterd.Count().ToString());
                        RolyUserCollectionViewer.Add((filterd.ToArray()[i] as RolyUser));
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
                    if (RolyUserCollectionViewer.Count == 0)
                    {
                        CancelFilter.Execute(null);
                        _Main.Instance._Notification.Add("", "Данные не найдены", TypeNotification.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ViewRolyUserModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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
        private void Filter(bool value)
        {

        }


        public void DeleteData(System.Collections.IList deleteList)
        {
            //Подчистим данные из view. Если из бд не удаляется данные восстановятся
            List<RolyUser> list = deleteList.Cast<RolyUser>().ToList();
            list.ForEach(auto => RolyUserCollectionViewer.Remove(auto));
            list.ForEach(auto => Collection.Remove(auto));
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

        public void SetCollection(List<Data_Roly> obj)
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


                    if (_Main.Instance.MyAccount.AllRoly != true)
                        if (collection[i].Name.Trim().ToLower() == "системный администратор") continue;



                    Collection.Add(new RolyUser()
                    {
                        Index = collection[i].Index,
                        Name = collection[i].Name,
                   
                    }); ;


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
        private void UpdateData(List<Data_Roly> obj, int count)
        {
            bool IsAppend = false;
            bool IsRemove = false;
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                for (int i = 0; i < count; i++)
                {
                    var classSubject = obj[i];

                    if (_Main.Instance.MyAccount.AllRoly!=true)
                    if (classSubject.Name.Trim().ToLower() == "системный администратор") continue;




             
                    var search = Collection.FirstOrDefault(o => (o as RolyUser).Index == classSubject.Index);

                    for (int j = 0; j < Collection.Count; j++)
                    {
                        var delete = obj.FirstOrDefault(p => p.Index == (Collection[j] as RolyUser).Index);
                        if (delete == null)
                        {

                            Collection.Remove((Collection[j] as RolyUser));
                            IsRemove = true;
                            continue;
                        }
                    }

                    if (search == null)
                    {
                        Add(classSubject);
                        IsAppend = true;
                    }
                    else
                    {
                        //Меняем данные в основной коллекции
                        (search as RolyUser).Name = classSubject.Name;
                        (search as RolyUser).Index = classSubject.Index;
 
                    }

                    await Task.Delay(0);
                }

                if (IsAppend) Refresh();
                if (IsRemove) Refresh();

                SetupTimer();

                OnOverlayShowing(false);

                if (count == 0)
                {
                    if (Collection.Count > 0) { Collection = new ObservableCollection<object>(); RolyUserCollectionViewer = new ObservableCollection<RolyUser>(); }
                }

                void Add(Data_Roly roly)
                {
                    Collection.Add(new RolyUser()
                    {
                        Index = roly.Index,
                        Name = roly.Name,

                    });
                }
            });
        }
    }
}
