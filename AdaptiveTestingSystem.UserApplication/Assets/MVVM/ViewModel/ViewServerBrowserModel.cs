using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class ViewServerBrowserModel : ViewTestModel
    {
        public new delegate void UpdatePredmetViewerHandler(string predmet);
        public new event UpdatePredmetViewerHandler? UpdatePredmetViewer;

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
                        IndexServer= collection[i].IndexServer,
                        NameCreator = collection[i].NameCreator,
                        IndexCreator = collection[i].IndexCreator,
                        NamePredmet = collection[i].NamePredmet,
                        NameTest = collection[i].NameTest,
                        CountUser = collection[i].CountUser,
                        IsAdaptive= collection[i].IsAdaptive,
                        Password = collection[i].Password,
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

                        if (itemTesting.NamePredmet != null)
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

    }
}

