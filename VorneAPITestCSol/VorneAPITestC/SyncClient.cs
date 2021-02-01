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

        public SyncClient(string rIP, int rPort)
        {
            this.remoteIP = rIP;
            this.remotePort = rPort;
        }


        public Message queryServer()
        {

            Message message = null;

            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = IPAddress.Parse(this.remoteIP);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, this.remotePort);

                // Create a TCP/IP  socket.  
                using (Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {



                    // Connect the socket to the remote endpoint. Catch any errors.  

                    sender.Connect(remoteEP);


                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);

                    message = JsonConvert.DeserializeObject<Message>(Encoding.ASCII.GetString(bytes));


                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            if (message == null)
            {
                return new Message("CONNECTING...", "BLACK*", "", 0);
            }
            else
            {
                return message;
            }
            
        }




    }
}
