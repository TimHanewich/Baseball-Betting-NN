using System;
using System.Net;
using System.Net.Http;

namespace ESPN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Scoreboard.RetrieveAsync().Wait();

        }
    }
}