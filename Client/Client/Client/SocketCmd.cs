using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestExeController
{
    class SocketCmd
    {
        /*    exe客户端      */
        public string ipaddress = "127.0.0.1";
        public int port = 7799;
        private Socket messageSocket;
        private Thread messageThread;
        private byte[] data = new byte[1024];// 数据容器
        private string message;
        public event Action<string> OnReceiveMessage;

        public SocketCmd()
        {
            ConnectToServer();
        }
        void ConnectToServer()
        {
            messageSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //跟服务器连接
            messageSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));
            //客户端开启线程接收数据
            messageThread = new Thread(ReceiveMessage);
            messageThread.Start();
        }

        void ReceiveMessage()
        {
            while (true)
            {
                if (messageSocket.Connected == false)
                {
                    break;
                }
                try
                {
                    int length = messageSocket.Receive(data);
                    message = Encoding.UTF8.GetString(data, 0, length);
                    if (OnReceiveMessage != null)
                    {
                        OnReceiveMessage(message);
                    }
                }
                catch
                {

                }
            }
        }
        public void SendMessage(string message)
        {
            if (messageSocket.Connected == false)
            {
                return;
            }
            byte[] data = Encoding.UTF8.GetBytes(message);
            messageSocket.Send(data);
        }
        public void Destroy()
        {
            //关闭连接，分接收功能和发送功能，both为两者均关闭
            messageSocket.Shutdown(SocketShutdown.Both);
            messageSocket.Close();
        }

    }
}
