using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace UiCore
{
    public class Program
    {
        public static void Main()
        {
            string baseUrl = "http://localhost:4242/";
            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseUrl))
            {
                var process = System.Diagnostics.Process.Start(baseUrl);
                Console.ReadLine();
            }
        }
    }
}
