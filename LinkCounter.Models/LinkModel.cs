using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace LinkCounter.Models
{
    /// <summary>
    /// Очередная ссылка из текстового файла
    /// </summary>
    public class LinkModel : ObservableObject
    {
        /// <summary>
        /// Index number
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// Url from input text file
        /// </summary>
        public string Url { get; set; }

        private string _status;
        private int _total;
        private int _counted;
        private int _percent;
        private bool _winner;

        /// <summary>
        /// Return current 'display' status: ready/downloading/calculating/completed
        /// </summary>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status == value)
                    return;
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
        }

        /// <summary>
        /// Total number of anchors found at this Url
        /// </summary>
        public int Total
        {
            get => _total;
            set
            {
                if (_total == value)
                    return;
                _total = value;
                RaisePropertyChangedEvent("Total");
            }
        }

        /// <summary>
        /// Currently counted achors for this LinkModel
        /// </summary>
        public int Counted
        {
            get => _counted;
            set
            {
                if (_counted == value)
                    return;
                _counted = value;
                RaisePropertyChangedEvent("Counted");
            }
        }

        /// <summary>
        /// Counted / Total * 100
        /// </summary>
        public int Percent
        {
            get => _percent;
            set
            {
                if (_percent == value)
                    return;
                _percent = value;
                RaisePropertyChangedEvent("Percent");
            }
        }

        /// <summary>
        /// Is this item leading with number of anchors
        /// </summary>
        public bool Winner
        {
            get => _winner;
            set
            {
                if (_winner == value)
                    return;
                _winner = value;
                RaisePropertyChangedEvent("Winner");
            }
        }

        /// <summary>
        /// Count anchors for this LinkModel
        /// </summary>
        /// <returns></returns>
        public void CountAnchors(CancellationTokenSource cancelToken)
        {
            Helper.Sleep();
            Status = "calc..";
            try
            {
                var html = (new WebClient()).DownloadString(Url);
                cancelToken.Token.ThrowIfCancellationRequested();
                Percent = 0;
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var anchors = doc.DocumentNode.SelectNodes("//a[@href]").ToList();
                Total = anchors.Count;
                for (var i = 0; i <= Total - 1; i++)
                {
                    Helper.Sleep();
                    Counted += 1;
                    Percent = (int)(Counted * 100.00 / Total);
                    cancelToken.Token.ThrowIfCancellationRequested();
                }
                Status = "complete";
            }
            catch (Exception)
            {
                Status = "error";            
            }
        }
    }
}
