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

            while (true)
            {



                // Establish the local endpoint for the socket.  
                // Dns.GetHostName returns the name of the
                // host running the application.
                IPAddress ipAddress = IPAddress.Any;
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.port);





                // Create a TCP/IP socket.  
                using (Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {

                    try
                    {

                        listener.Bind(localEndPoint);
                        listener.Listen(32);

                        while (true)
                        {

                            Socket handler = listener.Accept();

                            Thread handlerThread = new Thread(() => this.handle(handler));
                            handlerThread.IsBackground = true;
                            handlerThread.Start();
                        }


                    }
                    

                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());

                    }
                    finally
                    {
                        if (listener.Connected)
                        {
                            listener.Shutdown(SocketShutdown.Both);
                            listener.Close();
                        }
                    }
                }  
            }
        }


        

        private void handle(Socket handler)
        {

            using (handler)
            {
                try
                {


                    // Data buffer for incoming data.  
                    byte[] bytes = new Byte[1024];

                    handler.ReceiveTimeout = this.timeout;
                    handler.SendTimeout = this.timeout;

                    data = string.Empty;

                    // An incoming connection needs to be processed.  
                    do
                    {
                        if (handler.Connected)
                            data += Encoding.ASCII.GetString(bytes, 0, handler.Receive(bytes));
                        else
                            throw new Exception("Connection lost!");

                    } while (!data.Contains("<EOF>"));



                    if (this.relayMessage == null)
                    {
                        this.relayMessage = "";
                    }

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(this.relayMessage + "<EOF>");

                    if (handler.Connected)
                        handler.Send(msg);
                    else
                        throw new Exception("Connection lost!");
                }
                catch (Exception exc)
                {

                    Console.WriteLine(exc.ToString());

                }
                finally
                {
                    if (handler.Connected)
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
            }
            

            // do not close or shutdown handler socket or else it might throw WSACancelBlockingCall


        }


    }
}
