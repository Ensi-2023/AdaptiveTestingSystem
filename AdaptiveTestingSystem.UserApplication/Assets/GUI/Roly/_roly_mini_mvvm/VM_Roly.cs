using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window;
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
    public class VM_Roly : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }



        private ObservableCollection<MV_Roly> _roly { get; set; }

        private ObservableCollection<MV_Roly>? _rolyCollectionViewer { get; set; }
        public ObservableCollection<MV_Roly>? RolyCollectionViewer
        {
            get { return _rolyCollectionViewer; }

            set
            {
                _rolyCollectionViewer = value;
                OnPropertyChanged("RolyCollectionViewer");
            }
        }

        private MV_Roly? selectedRoly;
        public MV_Roly? SelectedRoly
        {
            get { return selectedRoly; }
            set
            {
                if (value == null) return;
                selectedRoly = value;
                OnPropertyChanged("SelectedRoly");
            }
        }

        public VM_Roly()
        {
            RolyCollectionViewer = new ObservableCollection<MV_Roly>();
            _roly = new ObservableCollection<MV_Roly>();
        }

        public async Task SetData(List<Data_Roly> data)
        {
            RolyCollectionViewer = new ObservableCollection<MV_Roly>();
            _roly = new ObservableCollection<MV_Roly>();
            if (data.Count == 0) return;

            for (int i = 0; i < data.Count; i++)
            {
                _roly.Add(new MV_Roly() { Index = data[i].Index,Name = data[i].Name });
                RolyCollectionViewer.Add(new MV_Roly() { Index = data[i].Index, Name = data[i].Name });


                GUI_RolyChanger.Instance.OverlayShow(true, TypeOverlay.loading, title: "Роль", subtitle: $"Обработано: {i+1} из {data.Count}");
                await Task.Delay(10);
            }

            OnPropertyChanged("RolyCollectionViewer");

            GUI_RolyChanger.Instance.OverlayShow(false);
        }

        public void Search(string isSearchString)
        {
            _Main.Instance.OverlayShow(true);
            RolyCollectionViewer = new ObservableCollection<MV_Roly>();

            var filterd = _roly.Where(x =>
                (
                  (x as MV_Roly).Name.ToLower().Trim().Contains(isSearchString.ToLower().Trim())

                ));

            for (int i = 0; i < filterd.Count(); i++)
            {
                RolyCollectionViewer.Add(new MV_Roly() { Index = filterd.ToArray()[i].Index, Name = filterd.ToArray()[i].Name });
            }
            OnPropertyChanged("RolyCollectionViewer");
            _Main.Instance.OverlayShow(false);
        }

    }
}
