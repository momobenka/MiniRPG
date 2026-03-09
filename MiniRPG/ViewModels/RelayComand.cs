using System;
using System.Windows.Input;

namespace MiniRPG.ViewModels
{
    // Comando WPF generico per eseguire azioni dalla UI
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;           // azione da eseguire
        private readonly Predicate<object?>? _canExecute;    // condizione per eseguire comando

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter); // se null, sempre eseguibile
        }

        public void Execute(object? parameter)
        {
            _execute(parameter); // esegue azione
        }

        // Evento richiesto da ICommand per notificare cambiamento stato eseguibile
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}