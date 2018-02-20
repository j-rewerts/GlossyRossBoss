// Cool Image Effects

using System;
using System.Windows.Input;

namespace CoolImageEffects.Command {

    public class DelegateCommand : ICommand {

        Action<object> _Execute;
        Predicate<object> _CanExecute;

        public DelegateCommand(Action<object> executeCommand, Predicate<object> canExecute) {
            this._Execute = executeCommand;
            this._CanExecute = canExecute;
        }

        public DelegateCommand(Action<object> executeCommand) {
            this._Execute = executeCommand;
        }

        public bool CanExecute(object parameter) {
            if (_CanExecute == null)
                return true;
            else {
                return _CanExecute(parameter);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            _Execute(parameter);
        }

        public void RaiseCanExecuteChanged() {
            if (CanExecuteChanged != null) {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}