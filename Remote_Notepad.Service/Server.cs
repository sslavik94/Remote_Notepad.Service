using Microsoft.Owin.Hosting;
using System;
using System.Data;
using System.Data.OleDb;


namespace Remote_Notepad.Service
{
    public class Server
    {
        public static IDisposable SignalR { get; set; }
        const string ServerURI = "http://192.168.1.104:8080";

        public static void StartServer()
        {
            SignalR = WebApp.Start<Startup>(url: ServerURI);
            Console.WriteLine("Server started at " + ServerURI);
            Console.Read();
        }
    }
}
