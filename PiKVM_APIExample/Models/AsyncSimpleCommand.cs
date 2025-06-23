using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PiKVM_APIExample.Models
{
    //public class AsyncSimpleCommand : ICommand
    //{
    //    private readonly Func<Task> _execute;
    //    private readonly Func<bool> _canExecute;

    //    public AsyncSimpleCommand(Func<Task> execute, Func<bool> canExecute = null)
    //    {
    //        _execute = execute;
    //        _canExecute = canExecute;
    //    }

    //    public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

    //    public async void Execute(object parameter) => await _execute();

    //    public event EventHandler CanExecuteChanged;
    //}
    public class AsyncSimpleCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Predicate<object> _canExecute;

        public AsyncSimpleCommand(Func<object, Task> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public async void Execute(object parameter) => await _execute(parameter);

        public event EventHandler CanExecuteChanged;
    }

}
