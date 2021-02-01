using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace VorneAPITest
{
    public class SyncServer
    {
        private Mutex pingsMutex = new Mutex();

        private int port, timeout;


  

        public SyncServer(int listenPort, int t)
        {
            this.port = listenPort;
            this.timeout = t;
        }


        // Incoming data from the client.  
        private string data = null;

        private string relayMessage = null;


        public void setRelayMessage(string rm)
        {
            this.relayMessage = rm;
        }

        public void listen()
        {

            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.
            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.port);

            // Create a TCP/IP socket.  
            using (Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {



                // Bind the socket to the local endpoint and
                // listen for incoming connections.  
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(20);

                    // Start listening for connections.  
                    while (true)
                    {


                        // Program is suspended while waiting for an incoming connection.  
                        using (Socket handler = listener.Accept())
                        {
                            handler.ReceiveTimeout = this.timeout;

                            data = null;

                            // An incoming connection needs to be processed.  
                            while (true)
                            {
                                int bytesRec = handler.Receive(bytes);
                                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                                if (data.IndexOf("<EOF>") > -1)
                                {
                                    break;
                                }
                            }



                            if (this.relayMessage == null)
                            {
                                this.relayMessage = "";
                            }

                            // Echo the data back to the client.  
                            byte[] msg = Encoding.ASCII.GetBytes(this.relayMessage);

                            handler.Send(msg);
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                        
                       
     

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
            }


        }


        
    }
}
