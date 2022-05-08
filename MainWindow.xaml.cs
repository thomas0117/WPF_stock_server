using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_stock_server
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(RunSocket);
            thread.Start();

            richTextBox.AppendText("等待連線中...\n");
        }

        public void AddMsgToRichTextBox(string msg)
        {
            richTextBox.AppendText(msg + '\n');
        }

        public void RunSocket()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);
            listener.Listen(10);

            while (true)
            {
                MySocket client = new MySocket(listener.Accept());

                this.Dispatcher.Invoke(() =>
                {
                    richTextBox.AppendText("接受一個新連線\n");
                });

                client.listener();
            }
        }
    }
}
