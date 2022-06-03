using Lab3oopServer.BaseHandler;
using System;
using System.Net;
using System.Windows;

namespace Lab3OOP {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public class Server {
            public HttpListener listener;
            // класс, который определяет как будет изменяться HttpListenerContext
            public BaseHandler handler;
            bool isRunning = false;

            public Server(string url, BaseHandler h) {
                handler = h;
                listener = new HttpListener();
                listener.Prefixes.Add(url);
            }

            public void run() {
                isRunning = true;
                listener.Start();
                while (isRunning) {
                    // консоль
                    Console.WriteLine("SERVER STARTED!");

                    HttpListenerContext context = listener.GetContext();

                    // консоль
                    Console.WriteLine("REQUEST IS COMMING!");
                    if (handler.processRequest(context)) {
                        // консоль
                        Console.WriteLine("RESPONSE IS SENDED!");
                    }
                    else {
                        // консоль
                        Console.WriteLine("ERROR!!!");
                    }
                }
            }
            public void stop() {
                isRunning = false;
                listener.Stop();
            }
        }
    }
}
