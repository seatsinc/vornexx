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

    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;


        
    }

    public class AsyncServer
    {
        Mutex mutex = new Mutex();

        private int port, timeout;


        public static ManualResetEvent allDone = new ManualResetEvent(false);


        private Queue<Socket> clients = new Queue<Socket>();

        public AsyncServer(int listenPort, int t)
        {
            this.port = listenPort;
            this.timeout = t;
        }

        // Establish the local endpoint for the socket.  
        // The DNS name of the computer  
        // running the listener is "host.contoso.com".  
        IPHostEntry ipHostInfo;
        IPAddress ipAddress;
        IPEndPoint localEndPoint;

        // Create a TCP/IP socket.  
        Socket listener;


        private string relayMessage = null;


        public void setRelayMessage(string rm)
        {
            this.relayMessage = rm;
        }

        public async void listen()
        {

            await Task.Run(() =>
            {

                // Establish the local endpoint for the socket.  
                // The DNS name of the computer  
                // running the listener is "host.contoso.com".  
                ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                ipAddress = IPAddress.Any;
                localEndPoint = new IPEndPoint(ipAddress, this.port);

                // Create a TCP/IP socket.  
                listener = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);


                // Bind the socket to the local endpoint and listen for incoming connections.  
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(0);

                    while (true)
                    {

                    
                        // Set the event to nonsignaled state.  
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.  
                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        // Wait until a connection is made before continuing.  
                        allDone.WaitOne();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            });

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {


                // Signal the main thread to continue.  
                allDone.Set();


                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                handler.ReceiveTimeout = this.timeout;
                handler.SendTimeout = this.timeout;


                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {


                String content = String.Empty;

                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);


                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the
                        // client. Display it on the console.  
                        // Echo the data back to the client.  
                        //Send(handler);

                        this.mutex.WaitOne();
                        this.clients.Enqueue(handler);
                        this.mutex.ReleaseMutex();

                        //************************************
                    }
                    else
                    {
                        // Not all data received. Get more.  
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            
        }

        public async void dump()
        {
            await Task.Run(() => {

                this.mutex.WaitOne();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (this.clients.Count > 0)
                {
                    try
                    {
                        Send(this.clients.Peek());
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.ToString());
                    }
                    finally
                    {
                        this.clients.Dequeue();
                    }
                }

                sw.Stop();
                Console.WriteLine("Dump time: " + sw.ElapsedMilliseconds.ToString());
                sw.Reset();

                this.mutex.ReleaseMutex();
            });
        }

        private void Send(Socket handler)
        {

            


            try
            {


                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(this.relayMessage + "<EOF>");

                if (handler != null)
                {

                    // Begin sending the data to the remote device.  
                    handler.BeginSend(byteData, 0, byteData.Length, 0,
                        new AsyncCallback(SendCallback), handler);

                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            
            
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void exit()
        {
            try
            {
                this.dump();
                this.listener.Shutdown(SocketShutdown.Both);
                this.listener.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }




    }
}
