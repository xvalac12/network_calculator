using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IPK_Calculator_Client
{
    /// <summary>
    /// Class with methods used for TCP communication.
    /// </summary>
    class Tcp
    {

        /// <summary>
        /// Function will try connect to server or print error.
        /// </summary>
        /// <param name="socket">Structure with information about type of connection.</param>
        /// <param name="endPoint">Info about host (Host and port), which will participate in communication.</param>
        /// <param name="server">Hostname or IP address of server, which we are tryin connect to.</param>
        public static void connect(Socket socket, EndPoint endPoint, string server)
        {
            try
            {
                socket.Connect(endPoint);
            }
            catch
            {
                Console.Error.WriteLine("Conection refused from server " + server);
                Environment.Exit(2);
            }
            return;
        }

        /// <summary>
        /// Sending messages from user to client and receiving response.
        /// </summary>
        /// <param name="line">Command entered by User.</param>
        /// <param name="socket">Structure with information about type of connection.</param>
        /// <returns>1 in case of end of communication, otherwiese 0.</returns>
        public static int communication(String line, Socket socket)
        {
            socket.Send(Encoding.ASCII.GetBytes(line));

            byte[] output = new byte[1024];
            string controlVar = Encoding.ASCII.GetString(output, 0, socket.Receive(output));
            Console.Write(controlVar);

            if (controlVar == "BYE\n") // server exits communication
            {
                return 1;  
            }
            return 0;
        }

        /// <summary>
        /// Exiting communication in case of interrupt signal.
        /// </summary>
        /// <param name="socket">Structure with information about type of connection.</param>
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