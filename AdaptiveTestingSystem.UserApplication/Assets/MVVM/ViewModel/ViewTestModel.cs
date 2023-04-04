using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class ViewTestModel : MVVM_Core, ICommandMVVM
    {

            public delegate void UpdatePredmetViewerHandler(string predmet);
        public event UpdatePredmetViewerHandler? UpdatePredmetViewer;


        public ViewTestModel()
        {
            Collection = new ObservableCollection<object>();
            TestingCollectionViewer = new ObservableCollection<Testing>();

        }

        private ObservableCollection<Testing>? _testingCollectionViewer { get; set; }
        public ObservableCollection<Testing>? TestingCollectionViewer
        {
            get { return _testingCollectionViewer; }

            set
            {
                _testingCollectionViewer = value;
                OnPropertyChanged("TestingCollectionViewer");
            }
        }
        private Testing? selectedTesting;
        public Testing? SelectedTesting
        {
            get { return selectedTesting; }
            set
            {
                if (value == null) return;
                selectedTesting = value;
                OnPropertyChanged("SelectedTesting");
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
                   (obj) => TestingCollectionViewer.Count > 0));
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
        private void Filter(bool v)
        {
            
        }

        public delegate void ViewerInformationTestingHandler(Testing testing);
        public event ViewerInformationTestingHandler? ViewerInformationTestng;
        public ICommand ViewInformation
        {
            get
            {
                return viewInformation ?? (viewInformation = new RelayCommand(obj =>
                {
                    var item = obj as Testing;
                    if (item != null)
                    {
                        ViewerInformationTestng?.Invoke(item);
                    }
                }));
            }
        }

        public string PredmetStr { get; private set; } = "Все";

        public void DeleteData(System.Collections.IList deleteList)
        {
            //Подчистим данные из view. Если из бд не удаляется данные восстановятся
            List<Testing> list = deleteList.Cast<Testing>().ToList();
            list.ForEach(auto => TestingCollectionViewer.Remove(auto));
            list.ForEach(auto => Collection.Remove(auto));
        }

        public override void Refresh()
        {
            if (IsSearch || PredmetStr.Trim() != "Все")
                RefreshWithFilter();
            else
                RefreshNoFilter();
        }

        public override async void RefreshWithFilter()
        {
            try
            {
                TestingCollectionViewer = new ObservableCollection<Testing>();

                var userList = GetFilter();

                if (userList != null)
                {
                    IEnumerable<object> filterd;


                    if (PredmetStr.Trim() == string.Empty || PredmetStr.Trim() =="Все")
                    {
                        filterd = userList.Where(x =>
                       (
                         (x as Testing).NameTest.ToLower().Trim().Contains(IsSearchString.ToLower().Trim()) ||
                         (x as Testing).NameCreator.ToLower().Trim().Contains(IsSearchString.ToLower().Trim()) ||
                         (x as Testing).NamePredmet.ToLower().Trim().Contains(IsSearchString.ToLower().Trim())
                       ));
                    }
                    else
                    {
                        filterd = userList.Where(x =>
                       (
                         ((x as Testing).NameTest.ToLower().Trim().Contains(IsSearchString.ToLower().Trim()) ||
                         (x as Testing).NameCreator.ToLower().Trim().Contains(IsSearchString.ToLower().Trim())) &&
                         (x as Testing).NamePredmet.ToLower().Trim().Contains(PredmetStr.ToLower().Trim())
                       ));
                    }

                    totalItems = filterd.Count();

                    if (IsView) OnOverlayShowing(true);

                    for (int i = start; i < start + CountView && i < filterd.Count(); i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), filterd.Count().ToString());
                        TestingCollectionViewer.Add((filterd.ToArray()[i] as Testing));
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
                    if (TestingCollectionViewer.Count == 0)
                    {
                        CancelFilter.Execute(null);
                        _Main.Instance._Notification.Add("", "Данные не найдены", TypeNotification.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ViewTestModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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
                TestingCollectionViewer = new ObservableCollection<Testing>();
                var userList = GetCollection(start, CountView, out totalItems);
                if (userList != null)
                {
                    if (IsView) OnOverlayShowing(true);
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), userList.Count.ToString());
                        TestingCollectionViewer.Add(userList[i] as Testing);
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
                Logger.Error($"ViewTestModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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


        public (string, int) ReturnRandomNameAndIndexTest()
        {
            var collection = Collection;
            if (collection == null)
            {
                return ("", 0);
            }

            if (collection.Count > 0)
            {
                Random rnd = new Random();
                var rndindex = rnd.Next(0, collection.Count);

                var item = collection[rndindex] as Data_Testing;
                if(item!=null)  return (item.Name, item.Index);
            }
          
            return ("", 0);
        }

        public void SetCollection(List<Data_Testing> obj)
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
                    Collection.Add(new Testing()
                    {
                        Index = collection[i].Index,
                        NameCreator= collection[i].NameCreator,
                        CountQuest= collection[i].CountQuest,
                        DateCrieting= collection[i].DateCrieting,
                        Description= collection[i].Description, 
                        IndexCreator= collection[i].IndexCreator,   
                        IndexPredmet= collection[i].IndexPredmet,   
                        NamePredmet= collection[i].NamePredmet,
                        NameTest = collection[i].Name

                    });

                    UpdatePredmetViewer?.Invoke(collection[i].NamePredmet);

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


        public void UpdateData(List<Data_Testing> obj, int count)
        {
            bool IsAppend = false;
            bool IsRemove = false;
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                for (int i = 0; i < count; i++)
                {
                    var itemTesting = obj[i];
                    var search = Collection.FirstOrDefault(o => (o as Testing).Index == itemTesting.Index);

                    for (int j = 0; j < Collection.Count; j++)
                    {
                        var delete = obj.FirstOrDefault(p => p.Index == (Collection[j] as Testing).Index);
                        if (delete == null)
                        {

                            Collection.Remove((Collection[j] as Testing));
                            IsRemove = true;
                            continue;
                        }
                    }

                    if (search == null)
                    {
                        Add(itemTesting);
                        IsAppend = true;
                    }
                    else
                    {
                        //Меняем данные в основной коллекции
                        (search as Testing).NameTest = itemTesting.Name == null ? "-" : itemTesting.Name;
                        (search as Testing).NamePredmet = itemTesting.NamePredmet == null ? "-" : itemTesting.NamePredmet;
                        (search as Testing).NameCreator = itemTesting.NameCreator == null ? "-" : itemTesting.NameCreator;
                        (search as Testing).IndexCreator = itemTesting.IndexCreator;
                        (search as Testing).IndexPredmet = itemTesting.IndexPredmet;
                        (search as Testing).Index = itemTesting.Index;
                        (search as Testing).CountQuest = itemTesting.CountQuest;
                        (search as Testing).DateCrieting = itemTesting.DateCrieting;
                        (search as Testing).Description = itemTesting.Description;

                        if(itemTesting.NamePredmet!=null)
                        UpdatePredmetViewer?.Invoke(itemTesting.NamePredmet);


                    }

                    await Task.Delay(0);
                }

                if (IsAppend) Refresh();
                if (IsRemove) Refresh();

                SetupTimer();

                OnOverlayShowing(false);

                if (count == 0)
                {
                    if (Collection.Count > 0) { Collection = new ObservableCollection<object>(); TestingCollectionViewer = new ObservableCollection<Testing>(); }
                }

                void Add(Data_Testing testing)
                {
                    Collection.Add(new Testing()
                    {
                        Index = testing.Index,
                        NameCreator = testing.NameCreator,
                        CountQuest = testing.CountQuest,
                        DateCrieting = testing.DateCrieting,
                        Description = testing.Description,
                        IndexCreator = testing.IndexCreator,
                        IndexPredmet = testing.IndexPredmet,
                        NamePredmet = testing.NamePredmet,
                        NameTest = testing.Name

                    });
                }
            });
        }

        public void SetPredmet(string caption)
        {
            this.PredmetStr = caption;
            Refresh();
        }
    }
}
