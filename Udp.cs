using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IPK_Calculator_Client
{
    /// <summary>
    /// Class with methods used for UDP communication.
    /// </summary>
    class Udp
    {
        /// <summary>
        /// Sending messages from user to client and receiving response + handling format of response and request.
        /// </summary>
        /// <param name="line">Command entered by User.</param>
        /// <param name="socket">Structure with information about type of connection.</param>
        /// <param name="endPoint">Info about server (Address and port), which will participate in communication.</param>
        public static void communication(String line, Socket socket, EndPoint endPoint)
        {
            byte[] info = {0, (byte)line.Length};
            byte[] message = Encoding.ASCII.GetBytes(line);
            byte[] request = new Byte[info.Length + message.Length];
            info.CopyTo(request, 0);
            message.CopyTo(request, info.Length);
            socket.SendTo(request, 0, request.Length, SocketFlags.None, endPoint);
                        
            byte[] output = new byte[1024];
            int num = socket.Receive(output);
            if (output[0] == 1)
            {
                if (output[1] == 1)
                {
                    String controlVar = Encoding.ASCII.GetString(output, 3, num);
                    Console.Write("ERR:" + controlVar);
                }
                else 
                {
                    string controlVar = Encoding.ASCII.GetString(output, 3, num);
                    Console.WriteLine("OK:" + controlVar);
                }
            }
            return;
        }
    }
}