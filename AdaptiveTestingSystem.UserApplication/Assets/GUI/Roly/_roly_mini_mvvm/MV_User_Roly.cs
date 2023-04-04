using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_mini_mvvm
{
    public class MV_User_Roly : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }


        private ObservableCollection<MV_User> _users { get; set; }

        private ObservableCollection<MV_User>? _rolyUserCollectionViewer { get; set; }
        public ObservableCollection<MV_User>? RolyUserCollectionViewer
        {
            get { return _rolyUserCollectionViewer; }

            set
            {
                _rolyUserCollectionViewer = value;
                OnPropertyChanged("RolyUserCollectionViewer");
            }
        }

        private MV_User? selectedRolyUser;
        public MV_User? SelectedRolyUser
        {
            get { return selectedRolyUser; }
            set
            {
                if (value == null) return;
                selectedRolyUser = value;
                OnPropertyChanged("SelectedRolyUser");
            }
        }

        public MV_User_Roly()
        {
            RolyUserCollectionViewer = new ObservableCollection<MV_User>();
            _users=new ObservableCollection<MV_User>();
        }

        public async Task SetData(List<Data_RolyUser> data)
        {
            RolyUserCollectionViewer = new ObservableCollection<MV_User>();
            _users = new ObservableCollection<MV_User>();
            if (data.Count == 0) { _Main.Instance.OverlayShow(false); return; };

            for (int i = 0; i < data.Count; i++)
            {
                _users.Add(new MV_User() { Index= data[i].IndexUser, FIO = data[i].Name});
                RolyUserCollectionViewer.Add(new MV_User() { Index= data[i].IndexUser, FIO = data[i].Name});
                _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Пользователи", subtitle: $"Обработано: {i+1} из {data.Count}");
                await Task.Delay(10);
            }

            OnPropertyChanged("RolyUserCollectionViewer");

            _Main.Instance.OverlayShow(false);
        }

        public void Search(string isSearchString)
        {
            _Main.Instance.OverlayShow(true);
            RolyUserCollectionViewer = new ObservableCollection<MV_User>();

            var filterd = _users.Where(x =>
                (
                  (x as MV_User).FIO.ToLower().Trim().Contains(isSearchString.ToLower().Trim()) 
           
                ));

            for (int i = 0; i < filterd.Count(); i++)
            {
                RolyUserCollectionViewer.Add(new MV_User() { Index = filterd.ToArray()[i].Index,FIO = filterd.ToArray()[i].FIO});
            }
            OnPropertyChanged("RolyUserCollectionViewer");
            _Main.Instance.OverlayShow(false);
        }

        internal void ViewInformation(object item)
        {
           var obj = (MV_User)item;

            if (obj != null)
            {
                _Main.Instance.Manager.Next(new GUI_User_Viewer(obj.Index));
            }

        }
    }
}
