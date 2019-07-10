using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestExeController
{

    class Program
    {
        static void Main(string[] args)
        {

            
            SocketCmd socketCmd = new SocketCmd();
            socketCmd.OnReceiveMessage += (s) =>
            {
                Console.WriteLine("服务端发来一条消息：" + s);
            };
            while (true)
            {
                string s = Console.ReadLine();
                socketCmd.SendMessage(s);
            }
        }
    }
}

