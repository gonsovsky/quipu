using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace LinkCounter.Models
{
    /// <summary>
    /// Helper class
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Fix "The request was aborted: Could not create SSL/TLS secure channel"
        /// </summary>
        public static void NoSslProblems()
        {
            ServicePointManager.ServerCertificateValidationCallback += AlwaysGoodCertificate;
        }

        private static bool AlwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        internal static Random R = new Random();

        /// <summary>
        /// Рандом чтобы задерживать работу
        /// </summary>
        public static int TinyRandom => R.Next(1, 10);

        /// <summary>
        /// Sleep a little bit
        /// </summary>
        public static void Sleep()
        {
            Thread.Sleep(TinyRandom);
        }

    }
}
