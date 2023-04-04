using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class CustomPropertyChanged: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

#nullable disable
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
