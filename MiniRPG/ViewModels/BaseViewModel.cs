using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiniRPG.ViewModels
{
    // BaseViewModel: implementa INotifyPropertyChanged per la binding UI
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Notifica la UI che una proprietà è cambiata
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}