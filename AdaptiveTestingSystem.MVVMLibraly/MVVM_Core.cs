
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace AdaptiveTestingSystem.MVVMLibraly
{
    public abstract class MVVM_Core: DependencyObject, INotifyPropertyChanged, IDisposable, IHandler
    {
        public abstract void Search(string search);
        public abstract void Refresh();
        public abstract void RefreshWithFilter();
        public abstract void RefreshNoFilter();
 

        public event PropertyChangedEventHandler? PropertyChanged;
        public event IHandler.OverlayShowingHandler? OverlayShowing;
        public event IHandler.OverlayChangeInformationHandler? OverlayChangeInformation;
        public event IHandler.UpdateHandler? Update;
        public event IHandler.DeleteEventHandler? DeleteObjects;
#nullable disable
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        /// <summary>
        /// Количество элеметнов в коллекции
        /// </summary>
        public int totalItems = 0;
        public int start = 0;
        public int Start
        {
            get
            {
                if (start == 0) return 1;
                return (int)(End - (((double)totalItems - (double)start) / (double)CountView) + 1);
            }
        }
        public int End
        {
            get
            {
                return (int)Math.Ceiling((double)totalItems / (double)CountView);
            }
        }
        public int TotalItems { get { return totalItems; } }
        private int viewCount = 20;


        private int timeToUpdate = 20;
        public int CountView => viewCount;

        public bool IsLoad { get;  set; }
        public bool IsUpdate { get;  set; }
        public bool IsView { get; set; }
        private bool isFilter = false;
        public bool IsFilter
        {
            get { return isFilter; }
            set 
            { 
                isFilter = value;
                if (value)
                {
                    VB_CancelFilter = Visibility.Visible;
                    VB_ButtonFilter = Visibility.Collapsed;
                }
                else
                {
                    VB_CancelFilter = Visibility.Collapsed;
                    VB_ButtonFilter = Visibility.Visible;
                }

                OnPropertyChanged("VB_CancelFilter");
                OnPropertyChanged("VB_ButtonFilter");
            }
        }
        public bool IsConnect { get; private set; }

        public Visibility VB_CancelFilter { get; set; } = Visibility.Collapsed;
        public Visibility VB_ButtonFilter { get; set; } = Visibility.Visible;

        public DispatcherTimer TimeUpdate;
#nullable enable
        public ObservableCollection<object>? Collection = null;
        public ObservableCollection<object>? GetCollection(int _start, int itemCount, out int totalItems)
        {
#nullable disable
            totalItems = Collection.Count;
            ObservableCollection<object> itemsSource = new ObservableCollection<object>();
            for (int i = _start; i < _start + itemCount && i < totalItems; i++)
            {
                itemsSource.Add(Collection[i]);
            }
            return itemsSource;
        }

#nullable enable
        public ObservableCollection<object>? GetFilter()
        {
            //totalItems = Collection.Count;  
            return Collection;
        }


        public bool IsSearch { get; set; }
        public string IsSearchString { get; set; } = string.Empty;

#nullable disable
        public void DeleteItem(object item)
        {
            Collection.Remove(item);
            OnPropertyChanged("TotalItems");
        }

        public void Add(object item)
        {
            Collection.Add(item);
            OnPropertyChanged("TotalItems");
        }

        public void SetView(int count)
        {
            this.viewCount = count;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);

        }

        public ICommand firstCommand;

        public ICommand previousCommand;

        public ICommand nextCommand;

        public ICommand lastCommand;

        public ICommand removeCommand;
        public ICommand cancelfilterCommand;
        public ICommand viewInformation;

        public void OnOverlayShowing(bool value)
        {
            OverlayShowing?.Invoke(value);
        }


        public void DeleteSelectedItem(System.Collections.IList deleteList)
        {
            DeleteObjects?.Invoke(deleteList);
        }

        public void OnChangeDataOverlay(string fisrt, string last)
        { 
            OverlayChangeInformation?.Invoke(fisrt, last);
        }

        public async void OnUpdate(bool skipCheck=false)
        {
            await Task.Delay(150);
            if (this.IsView)
                Update?.Invoke(skipCheck);
        }

        public void OnConnect(bool value)
        {
            this.IsConnect = value;
        }


        public void SetupTimer()
        {
            if (TimeUpdate != null) TimeUpdate.Stop();
            timeToUpdate = 20;
            TimeUpdate = new DispatcherTimer();
            TimeUpdate.Tick += new EventHandler(timeReconnectr_Tick);
            TimeUpdate.Interval = TimeSpan.FromSeconds(1);
            TimeUpdate.Start();
        }



        private void timeReconnectr_Tick(object sender, EventArgs e)
        {
            if (IsView == false) TimeUpdate.Stop();
          
            if (timeToUpdate <= 0)
            {
                TimeUpdate.Stop();
                OnUpdate(true);    
            }

            timeToUpdate--;

        }
    }
}
