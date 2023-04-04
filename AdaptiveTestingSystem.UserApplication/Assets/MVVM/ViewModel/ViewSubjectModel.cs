using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.MVVM.ViewModel
{
    public class ViewSubjectModel : MVVM_Core, ICommandMVVM
    {

        public ViewSubjectModel()
        {
            Collection = new ObservableCollection<object>();
            SubjectCollectionViewer = new ObservableCollection<Subject>();

        }


        private ObservableCollection<Subject>? _subjectCollectionViewer { get; set; }
        public ObservableCollection<Subject>? SubjectCollectionViewer
        {
            get { return _subjectCollectionViewer; }

            set
            {
                _subjectCollectionViewer = value;
                OnPropertyChanged("SubjectCollectionViewer");
            }
        }

        private Subject? selectedSubject;
        public Subject? SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                if (value == null) return;
                selectedSubject = value;
                OnPropertyChanged("SelectedSubject");
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
                   (obj) => SubjectCollectionViewer.Count > 0));
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
                    var item = obj as Subject;
                    if (item != null)
                    {
                        ViewerInformationSubject?.Invoke(item);
                    }
                }));
            }
        }


        public delegate void ViewerInformationSubjectHandler(Subject subject);
        public event ViewerInformationSubjectHandler? ViewerInformationSubject;


        public void DeleteData(System.Collections.IList deleteList)
        {
            //Подчистим данные из view. Если из бд не удаляется данные восстановятся
            List<Subject> list = deleteList.Cast<Subject>().ToList();
            list.ForEach(auto => SubjectCollectionViewer.Remove(auto));
            list.ForEach(auto => Collection.Remove(auto));
        }

        public void DeleteData(List<Subject> db,bool p)
        {
            //Подчистим данные из view. Если из бд не удаляется данные восстановятся

            db.ForEach(auto => SubjectCollectionViewer.Remove(auto));
            db.ForEach(auto => Collection.Remove(auto));
        }

        public override void Refresh()
        {
            if (IsSearch)
                RefreshWithFilter();
            else
                RefreshNoFilter();
        }

        public override async void RefreshWithFilter()
        {
            try
            {
                SubjectCollectionViewer = new ObservableCollection<Subject>();

                var userList = GetFilter();

                if (userList != null)
                {

                    var filterd = userList.Where(x =>
                    (
                      (x as Subject).Name.ToLower().Trim().Contains(IsSearchString.ToLower().Trim()) ||
                      (x as Subject).Employee.ToLower().Trim().Contains(IsSearchString.ToLower().Trim())
                    ));

                    totalItems = filterd.Count();

                    if (IsView) OnOverlayShowing(true);

                    for (int i = start; i < start + CountView && i < filterd.Count(); i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), filterd.Count().ToString());
                        SubjectCollectionViewer.Add((filterd.ToArray()[i] as Subject));
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
                    if (SubjectCollectionViewer.Count == 0)
                    {
                        CancelFilter.Execute(null);
                        _Main.Instance._Notification.Add("", "Данные не найдены", TypeNotification.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ViewSubjectModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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
                SubjectCollectionViewer = new ObservableCollection<Subject>();
                var userList = GetCollection(start, CountView, out totalItems);
                if (userList != null)
                {
                    if (IsView) OnOverlayShowing(true);
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (IsView) OnChangeDataOverlay((i+1).ToString(), userList.Count.ToString());
                        SubjectCollectionViewer.Add(userList[i] as Subject);
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
                Logger.Error($"ViewSubjectModel.RefreshNoFilter вызвал ошибку: {ex.Message}");
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


        public void SetCollection(List<Data_Subject> obj)
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
                    Collection.Add(new Subject()
                    {
                        Index = collection[i].Id_data,                
                        Name= collection[i].Name_data,
                        Employee = GetFullNameEmp(collection[i].Users),
                        Users = collection[i].Users

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

        private string GetFullNameEmp(List<Data_Subject_User> users)
        {
            string name = "";

            foreach (var item in users)
            {
                if (name.Contains(item.Name)) continue;                
                name += $"{item.Name}\n";
            }

            return name.Replace("NULL", "").Trim();
        }

        private void UpdateData(List<Data_Subject> obj, int count)
        {
            bool IsAppend = false;
            bool IsRemove = false;
            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                for (int i = 0; i < count; i++)
                {
                    var classSubject = obj[i];
                    var search = Collection.FirstOrDefault(o => (o as Subject).Index == classSubject.Id_data);

                    for (int j = 0; j < Collection.Count; j++)
                    {
                        var delete = obj.FirstOrDefault(p => p.Id_data == (Collection[j] as Subject).Index);
                        if (delete == null)
                        {

                            Collection.Remove((Collection[j] as Subject));
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
                        (search as Subject).Name = classSubject.Name_data == null ? "-" : classSubject.Name_data;
                        (search as Subject).Index = classSubject.Id_data;
                        (search as Subject).Employee = GetFullNameEmp(classSubject.Users);
                        (search as Subject).Users = classSubject.Users;
                    }

                    await Task.Delay(0);
                }

                if (IsAppend) Refresh();
                if (IsRemove) Refresh();

                SetupTimer();

                OnOverlayShowing(false);

                if (count == 0)
                {
                    if (Collection.Count > 0) { Collection = new ObservableCollection<object>(); SubjectCollectionViewer = new ObservableCollection<Subject>(); } 
                }

                void Add(Data_Subject subject)
                {
                    Collection.Add(new Subject()
                    {
                        Index = subject.Id_data,        
                        Employee = GetFullNameEmp(subject.Users),
                        Users = subject.Users,
                        Name = subject.Name_data,
                       
                    });
                }
            });
        }


    }
}
