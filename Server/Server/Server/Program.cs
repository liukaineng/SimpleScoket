using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dinosaur
{
    class Program
    {
        public static string IP = "127.0.0.1";
        public static int port = 7799;
        private static List<Client> clientlists = new List<Client>();
        private static List<Client> NotConnectClient = new List<Client>();
        private static int idIndex;
        static void Main(string[] args)
        {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(IP);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            tcpServer.Bind(ipEndPoint);
            tcpServer.Listen(100);
            Console.WriteLine("服务器已开启.....");
            Thread receiveInput = new Thread(ReceiveInput);
            receiveInput.Start();
            while (true)
            {
                Socket clientSocket = tcpServer.Accept(); //暂停等待客户端连接，连接后执行后面的代码
                int id = idIndex++;//每一个客户端分配一个标识id
                Console.WriteLine("有一个客户端连接过来,id为："+id);
                Client client = new Client(clientSocket,id); //连接后，客户端与服务器的操作封装到Client类中
                client.SendMessage(string.Format("分配ID : {0}", id));
                clientlists.Add(client); //放入集合
            }
        }
        static void ReceiveInput()
        {
            while (true)
            {
                string s = Console.ReadLine();
                BroadcastMessage(s);
            }

        }
        /// <summary>
        /// 广播信息
        /// </summary>
        /// <param name="message"></param>
        public static void BroadcastMessage(string message)
        {
            Console.WriteLine("广播消息：" + message);
            NotConnectClient.Clear();
            foreach (var client in clientlists)
            {
                if (client.Connected())//判断是否在线
                {
                    client.SendMessage(message);
                }
                else
                {
                    NotConnectClient.Add(client);
                }
            }
            //将掉线的客户端从集合里移除
            foreach (var temp in NotConnectClient)
            {
                clientlists.Remove(temp);
            }
        }
    }
}
