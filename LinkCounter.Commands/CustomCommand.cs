using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using LinkCounter.Models;

namespace LinkCounter.Commands
{ 
    /// <summary>
    /// Базовый класс комманды в проекте
    /// </summary>
    public abstract class CustomCommand: ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }
       
        public void Execute(object parameter)
        {
            CommandStart?.Invoke(this, new EventArgs());
            Task.Run(() =>
            {
                try
                {
                    InternalExecute(parameter);
                }
                catch (OperationCanceledException ex)
                {
                    CurrrentMessage = "Operation cancelled: " + ex.Message;
                }
                catch (Exception ex)
                {
                    CurrrentMessage = "Exception occured: " + ex.Message;
                }
            }).ContinueWith
                    (a =>
                        CommandEnd?.Invoke(this, new EventArgs())
                    );
        }

        public event EventHandler CanExecuteChanged
        {
            add { }

            remove { }
        }

        public event EventHandler<EventArgs> CommandStart;
        public event EventHandler<EventArgs> CommandMessage;
        public event EventHandler<EventArgs> CommandEnd;
        public event EventHandler<CommandProgressEventArgs> CommandProgress;

        private string _currentMessage;
        public string CurrrentMessage
        {
            get => _currentMessage;
            set
            {
                _currentMessage = value;
                CommandMessage?.Invoke(this, new EventArgs());
            }
        }

        public LinkCollection Links;

        public CancellationTokenSource CancelToken;

        public void RaiseProgress(int aVal)
        {
            CommandProgress?.Invoke(this, 
                new CommandProgressEventArgs(){Progress= aVal});

        }

        protected abstract void InternalExecute(object parameter);
    }

    public class CommandProgressEventArgs : EventArgs
    {
        public int Progress { get; set; }
    }

}
