using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class sdsdsd:MVVM_Core, ICommandMVVM
    {

        public delegate void UpdatePredmetViewerHandler(string predmet);
    public event UpdatePredmetViewerHandler? UpdatePredmetViewer;


    public sdsdsd()
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

    public string PredmetStr { get; private set; } = "";

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
                if (PredmetStr.Trim() == string.Empty || PredmetStr.Trim() == "Все")
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
                    if (IsView) OnChangeDataOverlay((i + 1).ToString(), filterd.Count().ToString());
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
            Logger.Error($"ViewServerBrowserModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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
                    if (IsView) OnChangeDataOverlay((i + 1).ToString(), userList.Count.ToString());
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
            Logger.Error($"ViewServerBrowserModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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

    public void SetCollection(List<Data_ListMultyServer> obj)
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
                    Index = collection[i].IndexTest,
                    IndexServer = collection[i].IndexServer,
                    NameCreator = collection[i].NameCreator,
                    IndexCreator = collection[i].IndexCreator,
                    NamePredmet = collection[i].NamePredmet,
                    NameTest = collection[i].NameTest,
                    CountUser = collection[i].CountUser,
                    IsAdaptive = collection[i].IsAdaptive,
                    Password = collection[i].Password,
                });


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


    private void UpdateData(List<Data_ListMultyServer> obj, int count)
    {
        bool IsAppend = false;
        bool IsRemove = false;
        Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            for (int i = 0; i < count; i++)
            {
                var itemTesting = obj[i];
                var search = Collection.FirstOrDefault(o => (o as Testing).IndexServer == itemTesting.IndexServer);

                for (int j = 0; j < Collection.Count; j++)
                {
                    var delete = obj.FirstOrDefault(p => p.IndexServer == (Collection[j] as Testing).IndexServer);
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
                    (search as Testing).NameTest = itemTesting.NameTest == null ? "-" : itemTesting.NameTest;
                    (search as Testing).NamePredmet = itemTesting.NamePredmet == null ? "-" : itemTesting.NamePredmet;
                    (search as Testing).NameCreator = itemTesting.NameCreator == null ? "-" : itemTesting.NameCreator;
                    (search as Testing).IndexCreator = itemTesting.IndexCreator;
                    (search as Testing).IsAdaptive = itemTesting.IsAdaptive;
                    (search as Testing).Index = itemTesting.IndexTest;
                    (search as Testing).IndexServer = itemTesting.IndexServer;
                    (search as Testing).CountUser = itemTesting.CountUser;
                    (search as Testing).Password = itemTesting.Password;
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

            void Add(Data_ListMultyServer testing)
            {
                Collection.Add(new Testing()
                {
                    Index = testing.IndexTest,
                    IndexServer = testing.IndexServer,
                    NameCreator = testing.NameCreator,
                    IndexCreator = testing.IndexCreator,
                    NamePredmet = testing.NamePredmet,
                    NameTest = testing.NameTest,
                    CountUser = testing.CountUser,
                    IsAdaptive = testing.IsAdaptive,
                    Password = testing.Password,

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

