using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace VorneAPITestC
{
    class SyncClient
    {

        private string remoteHostName;
        private int remotePort;
        private int timeout;

        public SyncClient(string rHN, int rPort, int to)
        {
            this.remoteHostName = rHN;
            this.remotePort = rPort;
            this.timeout = to;
        }


        public Message queryServer(int retries = 0)
        {

            Message message = null;

            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            


            using (Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {

                try
                {
                    // Create a TCP/IP  socket.  


                    sender.ReceiveTimeout = this.timeout;
                    sender.SendTimeout = this.timeout;

                    // Connect the socket to the remote endpoint. Catch any errors.  
                    sender.Connect(this.remoteHostName, this.remotePort);





                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("<EOF>");

                    // Send the data through the socket.
                    if (sender.Connected)
                        sender.Send(msg);
                    else
                        throw new Exception("Connection lost!");

                    // Receive the response from the remote device.



                    string data = string.Empty;


                    Stopwatch receiveTimer = new Stopwatch();
                    receiveTimer.Start();
                    
                    do
                    {
                        if (sender.Connected)
                            data += Encoding.ASCII.GetString(bytes, 0, sender.Receive(bytes));
                        else
                            throw new Exception("Connection lost!");

                        Console.WriteLine(data.Length);
                        
                        if (receiveTimer.ElapsedMilliseconds > this.timeout)
                        {
                            throw new Exception("receive timeout");
                        }
                        

                    } while (!data.Contains("<EOF>"));
                    receiveTimer.Reset();

                    Console.WriteLine(data.Length);
                    message = JsonConvert.DeserializeObject<Message>(data.Substring(0, data.Length - 5));



                }
                catch (Exception exc)
                {

              
 
                    message = null;
                }
            }




            if (message == null)
            {
                Console.WriteLine("hello");
                
                if (retries > 4)
                    return new Message("OFFLINE", "BLACK**", "", 0, "BEEP.wav");
                else
                    return this.queryServer(retries + 1);
                
            }
            else
            {
                Console.WriteLine("hello2");
                return message;
            }


        }

    }

    
}
