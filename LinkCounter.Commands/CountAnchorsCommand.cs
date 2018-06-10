using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkCounter.Models;
using System.Windows;

namespace LinkCounter.Commands
{   
    /// <summary>
    /// Основная комманда приложения. Считает ссылки
    /// </summary>
    public class CountAnchorsCommand : CustomCommand
    {
        /// <summary>
        /// Всего страниц
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Всего страниц обратано
        /// </summary>
        public int Processed { get; set; }

        protected override void InternalExecute(object parameter)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Links.Clear();
            });
            var result = LinkCollection.FromFile(InputFile);
            Processed = 0;
            Total = result.Count;
            var no = 0;
            var itemtasks = new List<Task>();
            var c = CancelToken;
            foreach (var item in result)
            {
                Helper.Sleep();
                c.Token.ThrowIfCancellationRequested();
                var xx = item;
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Links.Add((LinkModel)xx);
                });
                ++no;
                item.No = no;
                var x = item;
                var itemtask = Task.Factory.StartNew(() => { x.CountAnchors(c); }, c.Token)
                    .ContinueWith(a => 
                        TaskCompelted()
                        );
                itemtasks.Add(itemtask);
            }
            Task.WaitAll(itemtasks.ToArray(),c.Token);
        }

        protected void TaskCompelted()
        {
            Processed++;
            RaiseProgress(Processed * 100 / Total);
        }

        /// <summary>
        /// Return Input text file name to work with
        /// </summary>
        protected string InputFile => AppDomain.CurrentDomain.BaseDirectory + "urls.txt";
    }
}
