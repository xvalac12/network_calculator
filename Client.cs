using System.Net;
using System.Net.Sockets;

namespace IPK_Calculator_Client
{
    class Client
    {
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

        public static void Main(string[] args)
        {
            int port = cla_handling(args);

            IPAddress ipAddress;
            IPEndPoint endpoint;

            try
            {
                ipAddress = IPAddress.Parse(args[2]);
                endpoint = new IPEndPoint(ipAddress, port);
            }
            catch
            {
                IPAddress[] ipAddresses = Dns.GetHostAddresses(args[2]); // https://www.c-sharpcorner.com/UploadFile/1e050f/getting-ip-address-and-host-name-using-dns-class/
                endpoint = new IPEndPoint(ipAddresses[0], port);
                
            }       
        }

    }
}