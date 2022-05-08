using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace WPF_stock_server
{
    class MySocket
    {
        public static Socket socket;
        public static string data = null;
        public Thread listenthread;

        public MySocket(Socket s)
        {
            socket = s;
            StockTimer();
        }

        public string receive()
        {
            byte[] bytes = new Byte[1024];
            int bytesRec = socket.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            
            return data;
        }

        public void listener()
        {
            listenthread = new Thread(listen);
            listenthread.Start();
        }

        public void listen()
        {
            try
            {
                while (true)
                {
                    String line = receive();

                    if (line.IndexOf("<EOF>") > -1)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        break;
                    }
                }
            }
            catch (Exception ee)
            {

            }
        }

        public static void StockTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 1000; //執行間隔時間,單位為毫秒; 這裡實際間隔為10分鐘  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(SendStock);
        }

        private static void SendStock(object source, ElapsedEventArgs e)
        {
            try
            {
                byte[] msg = Encoding.UTF8.GetBytes("測試排程事件時間: ");
                socket.Send(msg);
                Console.WriteLine("測試排程事件時間: " + DateTime.Now.ToString());
            }
            catch
            {

            }
        }
        public void SendMsg(String data)
        {
            byte[] msg = Encoding.UTF8.GetBytes(data);

            socket.Send(msg);
        }

    }
}
