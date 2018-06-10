using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace LinkCounter.Models
{
    public class LinkCollection : ObservableCollection<LinkModel>
    {
        /// <summary>
        /// Находит в списке победителей по числу ссылок
        /// </summary>
        public void ShowWinners()
        {
            var pp = Items.OrderByDescending(a => a.Counted).ToArray();
            if (!pp.Any())
                return;
            var max = pp.First().Counted;
            if (max <= 0)
                return;
            var px = pp.TakeWhile(a => a.Counted == max);
            foreach (var x in px)
            {
                x.Winner = true;
                x.Status = "Winner!";
            }
        }


        /// <summary>
        /// Generate List of LinkModels from text file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<LinkModel> FromFile(string file)
        {
            var lines = File.ReadAllLines(file);
            var result = (from line in lines
                    select new LinkModel()
                    {
                        Url = line,
                        Percent = 0,
                        Status = "download..."

                    }
                ).ToList();
            return result;
        }
    }
}
