using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IPK_Calculator_Client
{
    class Udp
    {
        public static void communication(String line, Socket socket, EndPoint endpoint)
        {
            byte[] info = {0, (byte)line.Length};
            byte[] message = Encoding.ASCII.GetBytes(line);
            byte[] request = new Byte[info.Length + message.Length];
            info.CopyTo(request, 0);
            message.CopyTo(request, info.Length);
            socket.SendTo(request, 0, request.Length, SocketFlags.None, endpoint);
                        
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