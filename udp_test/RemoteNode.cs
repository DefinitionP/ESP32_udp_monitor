using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;


namespace udp_test
{
    internal class RemoteNode
    {
        public static List<RemoteNode> Nodes = new List<RemoteNode>();
        public static IPAddress desktop_ip = IPAddress.None;

        public ConsoleColor consoleColor;
        public int port;
        public UdpClient udp;
        public IPEndPoint endPoint;
        public string name;
        public Thread listenThread;
        public List<string> messages = new List<string>();
        public RemoteNode(int port, string name, ConsoleColor consoleColor)
        {
            if (desktop_ip == IPAddress.None)
            {
                IPAddress[] ipaddress = Dns.GetHostAddresses(Dns.GetHostName());
                desktop_ip = ipaddress.Last();
            }
            this.consoleColor = consoleColor;
            this.port = port;
            this.name = name;
            udp = new UdpClient(port);
            endPoint = new IPEndPoint(desktop_ip, port);
            listenThread = new Thread(Listen);
            listenThread.Priority = ThreadPriority.Highest;
        }

        public static void StartListening()
        {
            foreach (var node in Nodes)
            {
                node.listenThread.Start();
            }
        }
        public bool Available()
        {
            return udp.Available > 0;
        }
        public string ReadMessage()
        {
            byte[] data = udp.Receive(ref endPoint);
            DateTime dateTime = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(dateTime.ToString());
            sb.Append($" recieved from \"{name}\": ");
            sb.Append($"\"{Encoding.ASCII.GetString(data, 0, data.Length)}\"");
            return sb.ToString();
        }
        public void Listen()
        {
            while (true)
            {
                if (Available()) messages.Add(ReadMessage());
            }
        }
    }
}
