using System.Net;
using System.Net.Sockets;

namespace IPK_Calculator_Client
{
    /// <summary>
    /// A main class of client.
    /// </summary>
    class Client
    {

         /// <summary>
        /// Loop for communication
        /// </summary>
        /// <param name="socket">Structure with information about type of connection.</param>
        /// <param name="endPoint">Info about host (Host and port), which will participate in communication.</param>
        /// <param name="protocol">Protocol used for network communication.</param>
        static void comunication(Socket socket, EndPoint endPoint, string protocol)
        {
            string? line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                if (line == null) continue;
                
                if (protocol == "tcp")
                {
                    if (Tcp.communication(line, socket) == 1)
                    {
                        break;
                    }
                }
                else if (protocol == "udp")
                {
                    Udp.communication(line, socket, endPoint);
                }
            }
            if (protocol == "tcp")
            {
                socket.Shutdown(SocketShutdown.Both);
            }
        }

        /// <summary>
        /// Check the correctness of command line arguments.
        /// </summary>
        /// <param name="args">Array with command line arguments.</param>
        /// <returns>Port number of server.</returns>
        static int cla_handling(string[] args)
        {
            int portNum = 0;
            if (args.Length != 6)
            {
                Console.Error.WriteLine("Wrong number of arguments. Number of arguments is " + args.Length + " insted of 6.");
                Environment.Exit(1);
            }
            if (args[0] != "-h" || args[2] != "-p" || args[4] != "-m")
            {
                Console.Error.WriteLine("Wrong argument entered. Usage: ./ipkcpc -h <host> -p <port> -m <mode>");
                Environment.Exit(1);
            }
            else if ((args[5] != "udp") && (args[5] != "tcp"))
            {
                Console.Error.WriteLine("Mode can be only UDP or TCP. You entered " + args[5]);
                Environment.Exit(1);
            }
            try 
            {
                portNum = int.Parse(args[3]);
            }
            catch
            {
                Console.Error.WriteLine("Wrong port entered.");
                Environment.Exit(1);
            }

            return portNum;
            
        }

        /// <summary>
        /// Main method of client.
        /// </summary>
        /// <param name="args">Array with command line arguments.</param>
        public static void Main(string[] args)
        {
            int port = cla_handling(args);
            string mode = args[5];
            IPAddress ipAddress;
            IPEndPoint endPoint;

            try
            {
                ipAddress = IPAddress.Parse(args[1]);
                endPoint = new IPEndPoint(ipAddress, port);
            }
            catch
            {
                IPAddress[] ipAddresses = Dns.GetHostAddresses(args[1]); // https://www.c-sharpcorner.com/UploadFile/1e050f/getting-ip-address-and-host-name-using-dns-class/
                endPoint = new IPEndPoint(ipAddresses[0], port);
                
            }

            // IPV4, unreliable connection, protocol UDP
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if (mode == "tcp")
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Tcp.connect(socket, endPoint, args[1]);
            }
            
            Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e)  // https://learn.microsoft.com/en-us/dotnet/api/system.console.cancelkeypress?view=net-7.0
            {
                if (mode == "tcp")
                {
                    Tcp.sigint(socket);
                }
                socket.Close();
                Environment.Exit(0);
            };

            comunication(socket, endPoint, mode);

            socket.Close();
            Environment.Exit(0);              
        }

    }
}