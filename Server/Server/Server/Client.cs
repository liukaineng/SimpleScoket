using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Dinosaur
{
    class Client
    {
        private Socket clientSocket;
        private int id;
        private byte[] data = new byte[1024];//存储数据

        public Client(Socket s,int id)
        {
            clientSocket = s;
            this.id = id;
            //开启一个线程 处理客户端的数据接收
            Thread thread = new Thread(ReceiveMessage);
            thread.Start();
        }
        private void ReceiveMessage()
        {
            //服务器一直接收客户端数据
            while (true)
            {

                //如果客户端掉线，直接出循环
                if (clientSocket.Poll(10, SelectMode.SelectRead))
                {
                    clientSocket.Close();
                    Console.WriteLine(string.Format("id为{0}的客户端断开连接！", id));
                    break;
                }
                try
                {
                    //接收信息
                    int length = clientSocket.Receive(data);
                    string message = Encoding.UTF8.GetString(data, 0, length);
                    Console.WriteLine(string.Format("接收到来自id为{0}的客户端消息：{1}", id,message));
                }
                catch
                {

                }
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }
        public bool Connected()
        {
            return clientSocket.Connected;
        }
    }
}
