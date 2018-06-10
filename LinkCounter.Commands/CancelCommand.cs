namespace LinkCounter.Commands
{
    /// <summary>
    /// Комманда прекращения всех операций. (Кнопка Cancel)
    /// </summary>
    public class CancelCommand : CustomCommand
    {
        protected override void InternalExecute(object parameter)
        {
            CancelToken.Cancel();
        }
    }
}
