using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel
{

    public class modelPage_2_user : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChange("Index"); }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChange("Name"); }
        }

        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChange("Gender"); }
        }


        private string _datebirch;

        public string DateBirch
        {
            get { return _datebirch; }
            set { _datebirch = value; OnPropertyChange("DateBirch"); }
        }

    }


    public class modelPage_2_UserList : MVVM_Core, ICommandMVVM
    {

        public delegate void ViewerInformationUserHandler(modelPage_2_user room);
        public event ViewerInformationUserHandler? ViewerInformationRoom;


        #region Private
        private modelPage_2_user? selectedUser;
        #endregion


        private ObservableCollection<modelPage_2_user>? _userCollectionViewer { get; set; }
        public ObservableCollection<modelPage_2_user>? UserCollectionViewer
        {
            get { return _userCollectionViewer; }

            set
            {
                _userCollectionViewer = value;
                OnPropertyChanged("UserCollectionViewer");
            }
        }
        public modelPage_2_user? SelectedUser
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

        public override void Refresh()
        {
            if (IsSearch)
                RefreshWithFilter();
            else
                RefreshNoFilter();
        }

        public override void RefreshNoFilter()
        {
            throw new NotImplementedException();
        }

        public override void RefreshWithFilter()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

                for (int i = 0; i < collection.Count; i++)
                {
                    //Прокидываем данные количестве в оверлей
                    // OnChangeDataOverlay(i.ToString(), $"{collection.Count}");
                    Collection.Add(new modelPage_2_user()
                    {
                        Index = collection[i].Index,
                        DateBirch= collection[i].DateBirch,
                        Gender= collection[i].Gender,   
                        Name = collection[i].Name   
                            
                    }); ;

                   await Task.Delay(0);
                }


                Refresh();                
                IsUpdate = true;
            });

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
                    var search = Collection.FirstOrDefault(o => (o as modelPage_2_user).Index == classRoom.Index);

                    for (int j = 0; j < Collection.Count; j++)
                    {
                        var delete = obj.FirstOrDefault(p => p.Index == (Collection[j] as modelPage_2_user).Index);
                        if (delete == null)
                        {

                            Collection.Remove((Collection[j] as modelPage_2_user));
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
                        (search as modelPage_2_user).Name = classRoom.Name == null ? "-" : classRoom.Name;
                        (search as modelPage_2_user).Index = classRoom.Index;
                        (search as modelPage_2_user).DateBirch = classRoom.DateBirch;
                        (search as modelPage_2_user).Gender = classRoom.Gender;
       
                    }

                    await Task.Delay(0);
                }

                if (IsAppend) Refresh();
                if (IsRemove) Refresh();

                SetupTimer();

                OnOverlayShowing(false);
                void Add(Data_AllUserPacket _user)
                {
                    Collection.Add(new modelPage_2_user()
                    {
                        Index = _user.Index,
                        Gender= _user.Gender,
                        Name = _user.Name,
                        DateBirch = _user.DateBirch                   
                    });
                }
            });
        }



    }
}
