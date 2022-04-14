namespace DexPlayer.MVVM;

using System;
using System.Windows.Input;

public class Command : ICommand
{
    readonly Action execute = null;
    readonly Predicate<object> canExecute = null;

    public event EventHandler CanExecuteChanged;

    public Command(Action execute)
        : this(execute, null) { }

    public Command(Action execute, Predicate<object> canExecute)
    {
        this.execute = execute ??
            throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter) =>
        canExecute == null || canExecute(parameter);

    public void Execute(object parameter) =>
        execute();

    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class Command<T> : ICommand
{
    readonly Action<T> execute = null;
    readonly Predicate<T> canExecute = null;

    public event EventHandler CanExecuteChanged;

    public Command(Action<T> execute)
        : this(execute, null) { }

    public Command(Action<T> execute, Predicate<T> canExecute)
    {
        this.execute = execute ??
            throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter) =>
        canExecute == null || canExecute((T)parameter);

    public void Execute(object parameter) =>
        execute((T)parameter);

    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
