using System.Windows.Input;

namespace ECommerce_Project.Command;
public class RelayCommand : ICommand
{

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
    public bool CanExecute(object? parameter)
    {
        if (_canExecute is null)
            return true;
        return _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        if (_execute is not null)
            _execute(parameter);
    }
}