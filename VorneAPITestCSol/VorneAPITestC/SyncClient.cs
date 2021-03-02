using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;

namespace VorneAPITestC
{
    class SyncClient
    {

        private string remoteIP;
        private int remotePort;
        private int timeout;

        public SyncClient(string rIP, int rPort, int to)
        {
            this.remoteIP = rIP;
            this.remotePort = rPort;
            this.timeout = to;
        }


        public Message queryServer()
        {

            Message message = null;

            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse(this.remoteIP);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, this.remotePort);

            // Create a TCP/IP  socket.  
            using (Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    // Connect the socket to the remote endpoint. Catch any errors.  

                    sender.Connect(remoteEP);

                    sender.ReceiveTimeout = this.timeout;
                    sender.SendTimeout = this.timeout;


                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("<EOF>");

                    // Send the data through the socket.
                    if (sender.Connected)
                        sender.Send(msg);
                    else
                        throw new Exception("Connection lost!");

                    // Receive the response from the remote device.



                    string data = string.Empty;

                    // An incoming connection needs to be processed.  
                    do
                    {
                        if (sender.Connected)
                            data += Encoding.ASCII.GetString(bytes, 0, sender.Receive(bytes));
                        else
                            throw new Exception("Connection lost!");

                    } while (!data.Contains("<EOF>"));



                    message = JsonConvert.DeserializeObject<Message>(data.Substring(0, data.Length - 5));

                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());

                    message = null;
                }
            }


            if (message == null)
            {
                return new Message("OFFLINE", "BLACK**", "", 0);
            }
            else
            {
                return message;
            }
            
        }




    }
}
