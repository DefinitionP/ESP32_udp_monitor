using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;

using System.Threading;
using System.Net.NetworkInformation;



namespace udp_test
{
    internal class Program
    {
        public static int PORT = 8888;
        public static IPAddress desktop_ip = null;
        public static IPAddress device_ip = null;
        public static int device_port = 0;
        private static Socket socket;
        private static IPEndPoint ep;
        public static bool clear_flag = false;
        static void Main(string[] args)
        {
            RemoteNode.Nodes.Add(new RemoteNode(8888, "1", ConsoleColor.DarkGreen));
            RemoteNode.Nodes.Add(new RemoteNode(8889, "2", ConsoleColor.DarkBlue));
            RemoteNode.Nodes.Add(new RemoteNode(8890, "3", ConsoleColor.DarkMagenta));
            RemoteNode.Nodes.Add(new RemoteNode(8891, "4", ConsoleColor.DarkCyan));
            RemoteNode.Nodes.Add(new RemoteNode(8892, "5", ConsoleColor.DarkYellow));

            Console.WindowWidth = Console.LargestWindowWidth - 30;
            print_my_ip();
            Console.WriteLine();
            RemoteNode.StartListening();

            Thread thread = new Thread(key_parse);
            thread.IsBackground = true;
            thread.Start();

            while (true)
            {
                if (clear_flag == true)
                {
                    clear_flag = false;
                    Console.Clear();
                }
                foreach (var node in RemoteNode.Nodes)
                {
                    if (node.messages.Count() > 0)
                    {
                        Console.ForegroundColor = node.consoleColor;
                        Console.WriteLine(node.messages.First());
                        Console.ForegroundColor = ConsoleColor.White;
                        node.messages.RemoveAt(0);
                    }
                }
            }
        }
        static void key_parse()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (true)
            {

                if (key.Key == ConsoleKey.C)
                {
                    clear_flag = true;
                }
                key = Console.ReadKey(true);
            }
        }
        static void print_my_ip()
        {

            IPAddress[] ipaddress = Dns.GetHostAddresses(Dns.GetHostName());
            desktop_ip = ipaddress.Last();
            Console.Write($"IP устройства: {desktop_ip}");
        }
    }
}