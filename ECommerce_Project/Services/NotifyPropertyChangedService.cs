using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ECommerce_Project.Services;
public class NotifyPropertyChangedService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
