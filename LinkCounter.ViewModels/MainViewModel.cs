using System;
using System.Threading;
using LinkCounter.Commands;
using LinkCounter.Models;

namespace LinkCounter.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// ViewModel for Main Window
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        private CancelCommand _cancelCommand;
        private CountAnchorsCommand _countAnchorsCommand;
        private bool _isRunning;
        private string _message;
        private int _progress;
        private CancellationTokenSource _cancelToken;

        public CancellationTokenSource CancelToken => _cancelToken ?? (
                                                          _cancelToken = new CancellationTokenSource());

        /// <summary>
        /// Комманда подсчета кол-ва ссылок
        /// </summary>
        public CountAnchorsCommand CountAnchorsCommand => _countAnchorsCommand ??
                                                          (_countAnchorsCommand = (CountAnchorsCommand) MakeCommand(typeof(CountAnchorsCommand)));

        /// <summary>
        /// Комманда отмены действия
        /// </summary>
        public CancelCommand CancelCommand => _cancelCommand ?? (_cancelCommand = (CancelCommand)MakeCommand(typeof(CancelCommand)) );

        /// <summary>
        /// Создает экземпляр комманды
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected CustomCommand MakeCommand(Type t)
        {
            var c = (CustomCommand)Activator.CreateInstance(t);
            c.CommandStart += CommandStart;
            c.CommandEnd += CommandEnd;
            c.CommandMessage += CommandMessage;
            c.CommandProgress += CommandProgress;
            c.Links = Links;
            return c;
        }

        /// <summary>
        /// Вызывается, когда комманда стартует
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void CommandStart(object sender, EventArgs args)
        {
            var c = (CustomCommand)sender;
            c.CancelToken = CancelToken;
            IsRunning = true;
            Message = "Working...";
        }

        /// <summary>
        /// Вызывается, когда комманда заканчивает работу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void CommandEnd(object sender, EventArgs args)
        {
            if (sender.GetType() == typeof(CancelCommand))
                return;
            IsRunning = false;
            if (!CancelToken.IsCancellationRequested)
                Links.ShowWinners();
            Message = "Press button to start again.";
            CancelToken.Dispose();
            _cancelToken = null;
        }

        /// <summary>
        /// Вызывается, когда комманда делает сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void CommandMessage(object sender, EventArgs args)
        {
            Message = ((CustomCommand)(sender)).CurrrentMessage;
        }

        /// <summary>
        /// Вызывается, когда комманда вызывет общий прогресс события
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void CommandProgress(object sender, CommandProgressEventArgs args)
        {
            TotalProgress = args.Progress;
        }

        /// <summary>
        /// Запущена ли сейчас работа
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning == value)
                    return;
                _isRunning = value;
                RaisePropertyChangedEvent("IsRunning");
            }
        }

        /// <summary>
        /// Остновлена ли сейчас работа
        /// </summary>
        public bool IsReady
        {
            get => !IsRunning;
            set => IsRunning = !value;
        }

        /// <summary>
        /// Сообщение для интерфейса
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                RaisePropertyChangedEvent("Message");
            }
        }

        /// <summary>
        /// Общий прогресс выполнения
        /// </summary>
        public int TotalProgress
        {
            get => _progress;
            set
            {
                if (_progress == value)
                    return;
                _progress = value;
                RaisePropertyChangedEvent("TotalProgress");
            }
        }

        /// <summary>
        /// Collection of links loaded from input text file
        /// </summary>
        public LinkCollection Links { get; } = new LinkCollection();
    }
}
