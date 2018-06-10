using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace LinkCounter
{
    public partial class App
    {
        /// <summary>
        /// The best method ever
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static DateTime Tommorow()
        {
            Thread.Sleep(24 * 60 * 60 * 1000);
            return DateTime.Now;
        }
    }
}
