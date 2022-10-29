using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HW12.Models
{
    internal class RelayCommand : ICommand
    {

        readonly Predicate<object?>? canExecute;
        readonly Action<object?>? methodExecute;

        public RelayCommand(Action<object?>? methodExecute, Predicate<object?>? canExecute = null)
        {
            this.methodExecute = methodExecute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            methodExecute?.Invoke(parameter);
        }
    }
}