using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IPK_Calculator_Client
{

    class Tcp
    {

        public static void connect(Socket socket, EndPoint endpoint, string server)
        {
            try
            {
                socket.Connect(endpoint);
            }
            catch
            {
                Console.Error.WriteLine("Conection refused from server " + server);
                Environment.Exit(1);
            }
            return;
        }
    }
}