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
                Environment.Exit(2);
            }
            return;
        }

        
        public static int communication(String line, Socket socket)
        {
            socket.Send(Encoding.ASCII.GetBytes(line));

            byte[] output = new byte[1024];
            string controlVar = Encoding.ASCII.GetString(output, 0, socket.Receive(output));
            Console.Write(controlVar);
            if (controlVar == "BYE\n")
            {
                return 1;  
            }
            return 0;
        }

        public static void sigint(Socket socket)
        {
            socket.Send(Encoding.ASCII.GetBytes("BYE\n"));
            byte[] output = new byte[1024];
            Console.Write(Encoding.ASCII.GetString(output, 0, socket.Receive(output)));
            socket.Shutdown(SocketShutdown.Both);
            return;
        }
    }
}