using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Media;
using System.Net.Sockets;
using System.Net;
using System.IO.Ports;
using System.Threading;



namespace VorneAPITest
{
    public partial class Form1 : Form
    {
        private Mutex mutex = new Mutex();

        // buffer size for server client relationship
        const int BUFFERSIZE = 1024 * 1024;
        
        // how long the lights will turn off to warn on takt time (takt time / ROLLDIVISOR)
        const int ROLLDIVISOR = 10;

        

        // ip address of the vorne machine
        const string VORNEIP = "10.119.12.15";
        const string WCNAME = "3915";

        // ipaddress IPAddress.Any if deploying
        // ...should be IPAddress.Loopback if on local computer
        readonly IPAddress SERVERIP = IPAddress.Any;
        const int SERVERPORT = 50010;

        // keeps track of the production state 
        private string ps; // production state
        private string pID; // part id
        private string psR; // process state reason

        // hour, min, sec are what the user has set on the time
        // tt stands for takt timer and represents the takt timer going down in seconds
        private int hour, min, sec, tt;

        private bool stopped = true;

        private int clients = 0;



        


        
        

        public Form1()
        {
            InitializeComponent();
        }

        


        private void Form1_Load(object sender, EventArgs e)
        {
            // initialize partID and productionState
            this.pID = "NA";
            this.ps = "NA";


            // makes sure the lights turn off if the program closes
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);


            this.lblWC.Text = WCNAME;

            this.hour = 0;
            this.min = 0;
            this.sec = 0;



            // make it so the screen full screen
            Rectangle wa = Screen.GetWorkingArea(this);
            this.Width = wa.Width / 5;
            this.Height = wa.Height / 3;
            this.Location = new Point(wa.Right - this.Width, wa.Bottom - this.Height);
            //this.Location = new Point(wa.Right - this.Width, wa.Top); // for testing

            // aligning
            this.lblClock.Location = new Point(wa.Left + this.Width / 2 - this.lblClock.Width / 2, wa.Top + this.Height / 2 - this.lblClock.Height / 2);

            this.lblPS.Location = new Point(wa.Left + 10, wa.Top + 10);
            this.lblPartID.Location = new Point(wa.Left + this.Width - this.lblPartID.Width - 10, wa.Top + 10);
            this.lblTime.Location = new Point(wa.Left + 10, wa.Top + this.Height - this.lblTime.Height - 10);
            this.lblWC.Location = new Point(wa.Left + this.Width - this.lblWC.Width - 10, wa.Top + this.Height - this.lblTime.Height - 10);

            this.btnHrInc.Location = new Point(this.lblClock.Location.X, this.lblClock.Location.Y - this.btnHrInc.Height);
            this.btnHrDec.Location = new Point(this.lblClock.Location.X, this.lblClock.Location.Y + this.lblClock.Height);
            this.btnMinInc.Location = new Point(wa.Left + this.Width / 2 - this.btnMinInc.Width / 2, this.lblClock.Location.Y - this.btnHrInc.Height);
            this.btnMinDec.Location = new Point(wa.Left + this.Width / 2 - this.btnMinDec.Width / 2, this.lblClock.Location.Y + this.lblClock.Height);
            this.btnSecInc.Location = new Point(this.lblClock.Location.X + this.lblClock.Width - this.btnSecInc.Width, this.lblClock.Location.Y - this.btnHrInc.Height);
            this.btnSecDec.Location = new Point(this.lblClock.Location.X + this.lblClock.Width - this.btnSecDec.Width, this.lblClock.Location.Y + this.lblClock.Height);

            this.btnStart.Location = new Point(wa.Left + this.Width / 2 - this.btnStart.Width, wa.Top + this.Height - this.btnStart.Height - 10);
            this.btnStop.Location = new Point(wa.Left + this.Width / 2, wa.Top + this.Height - this.btnStop.Height - 10);
            
            this.cbPorts.Location = new Point(wa.Left + this.Width / 2 - this.cbPorts.Width / 2, wa.Top + 10);

            this.lblClients.Location = new Point(this.cbPorts.Left, this.cbPorts.Bottom);
            

            // populate ports in combo box
            this.populatePorts();

            if (this.cbPorts.SelectedItem == null)
            {
                MessageBox.Show("Error: No serial ports detected. Plug in ports and restart.");
            }
            

            
            

            // make it so the application will always be on top
            this.BringToFront();
            this.TopMost = true;



            // listening loop
            Thread accepter = new Thread(this.accept);
            accepter.IsBackground = true;
            accepter.Start();


            // communicating loop
            Thread communicator = new Thread(this.queryVorne);
            communicator.IsBackground = true;
            communicator.Start();

            

            timer.Start();
            lightTimer.Start();

            

        }

        

        private int rollTime()
        {
            return this.calcSec(this.hour, this.min, this.sec) / ROLLDIVISOR;
        }

        private void accept()
        {
            

            while (true)
            {
                try
                {


                    // server end point and listener
                    IPEndPoint ep;
                    TcpListener listener;

                    
                    // initialize server enpoint and listenter
                    ep = new IPEndPoint(SERVERIP, SERVERPORT);
                    listener = new TcpListener(ep);
                    
                    listener.Start(); // Start listening

                    /* Can only proceed from here
                            * if a client makes a request to our application.
                            * Once receives a request, should proceed from this line.
                            */


                    TcpClient temp = new TcpClient();

                    temp = listener.AcceptTcpClient();

                    Thread new_thread = new Thread(() => handleClient(ref temp));
                    new_thread.Start();



                    listener.Stop();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }

            

        }

        private void handleClient(ref TcpClient c)
        {
            mutex.WaitOne();
            this.clients++;
            mutex.ReleaseMutex();

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[BUFFERSIZE];

                    NetworkStream stream = c.GetStream();

                    do
                    {
                        stream.Read(buffer, 0, buffer.Length);
                    }
                    while (stream.DataAvailable);


                    // send message back to client
                    Message message = new Message(this.ps, this.getColor(), this.pID, this.tt);
                    byte[] messageBytes = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(message));
                    stream.Write(messageBytes, 0, messageBytes.Length);

                }
                catch (Exception e)
                {
                    c.Close();
                    break;

                }
            }

            mutex.WaitOne();
            this.clients--;
            mutex.ReleaseMutex();


        }

        private void queryVorne()
        {
            while (true)
            {
                try
                {


                    // vorne communication
                    RestClient client = new RestClient();


                    // make requests to vorne
                    string requestPS = client.makeRequest("http://" + VORNEIP + "/api/v0/process_state/active", httpVerb.GET);
                    string requestPR = client.makeRequest("http://" + VORNEIP + "/api/v0/part_run", httpVerb.GET);



                    // deserialize objects
                    PSActiveNS.PSActive psActive = JsonConvert.DeserializeObject<PSActiveNS.PSActive>(requestPS);
                    PartRunNS.PartRun partRun = JsonConvert.DeserializeObject<PartRunNS.PartRun>(requestPR);


                    this.pID = partRun.data.part_id;
                    this.psR = psActive.data.process_state_reason.ToUpper();


                    this.ps = Util.replAwBStr(psActive.data.name.ToUpper(), '_', ' ');
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(250);


            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
            // turns lights off when the program exits
        {
            this.changeColor("BLACK");

            Environment.Exit(Environment.ExitCode);
        }

       

        private string getColor()
        {
            this.ps = Util.replAwBStr(this.ps, '_', ' ');

            if (this.stopped == false && this.tt < this.rollTime())
            {
                return "BLACK";
            }  
            else if (this.ps == "RUNNING")
            {
                return "GREEN";
            }
            else if (this.ps == "DOWN")
            {
                return "RED";
            }
            else if (this.ps == "CHANGEOVER")
            {
                return "YELLOW";
            }
            else if (this.ps == "BREAK"
                || this.ps == "MEETING"
                || this.ps == "DETECTING STATE")
            {
                return "BLUE";
            }
            else
            {
                return "BLACK*"; // lights off but not during takt timer
            }
        }

        private void changeColor(string color)
        {

            if (this.cbPorts.SelectedItem == null)
            {
                return;
            }

            try
            {
                SerialPort p = new SerialPort(this.cbPorts.SelectedItem.ToString(), 9600);

                p.Open();

                if (color == "BLACK*")
                {
                    color = "BLACK";
                }

                p.Write(color);
                p.Close();

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private System.Drawing.Color colorFromPS(string ps)
        {
            if (ps == "GREEN")
            {
                return System.Drawing.Color.PaleGreen;
            }
            else if (ps == "RED")
            {
                return System.Drawing.Color.Crimson;
            }
            else if (ps == "YELLOW")
            {
                return System.Drawing.Color.Yellow;
            }
            else if (ps == "BLUE")
            {
                return System.Drawing.Color.DeepSkyBlue;
            }
            else
            {
                return System.Drawing.Color.LightGray;
            }
            

        }

        private void populatePorts()
        {

            try
            {

                foreach (string portName in SerialPort.GetPortNames())
                {
                    this.cbPorts.Items.Add(portName);
                    this.cbPorts.SelectedIndex = 0;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }


        private string clockFromSec(double secs)
        {
            int seconds = Convert.ToInt32(Math.Floor(secs));

            string clock = "";

            int hours = (seconds / 3600);

            if (hours < 10)
                clock += '0';

            clock += hours.ToString() + ':';

            seconds %= 3600;

            int minutes = seconds / 60;

            if (minutes < 10)
                clock += '0';

            clock += minutes.ToString() + ':';

            seconds %= 60;

            if (seconds < 10)
                clock += '0';

            clock += seconds.ToString();

            return clock;

        }


        private void btnHrInc_Click(object sender, EventArgs e)
        {
            if (this.hour == 23)
                this.hour = 0;
            else
                this.hour++;

            // update takt timer in seconds
            this.tt = this.calcSec(this.hour, this.min, this.sec);

            // update clock
            this.lblClock.Text = this.clockFromSec(this.tt);
        }

        private void btnHrDec_Click(object sender, EventArgs e)
        {
            if (this.hour == 0)
                this.hour = 23;
            else
                this.hour--;

            // update takt timer in seconds
            this.tt = this.calcSec(this.hour, this.min, this.sec);

            // update clock
            this.lblClock.Text = this.clockFromSec(this.tt);
        }

        private void btnMinInc_Click(object sender, EventArgs e)
        {
            if (this.min == 59)
                this.min = 0;
            else
                this.min++;

            // update takt timer in seconds
            this.tt = this.calcSec(this.hour, this.min, this.sec);

            // update clock
            this.lblClock.Text = this.clockFromSec(this.tt);
        }

        private void btnMinDec_Click(object sender, EventArgs e)
        {
            if (this.min == 0)
                this.min = 59;
            else
                this.min--;

            // update takt timer in seconds
            this.tt = this.calcSec(this.hour, this.min, this.sec);

            // update clock
            this.lblClock.Text = this.clockFromSec(this.tt);
        }

        private void btnSecInc_Click(object sender, EventArgs e)
        {
            if (this.sec == 59)
                this.sec = 0;
            else
                this.sec++;

            // update takt timer in seconds
            this.tt = this.calcSec(this.hour, this.min, this.sec);

            // update clock
            this.lblClock.Text = this.clockFromSec(this.tt);
        }

        private void btnSecDec_Click(object sender, EventArgs e)
        {
            if (this.sec == 0)
                this.sec = 59;
            else
                this.sec--;

            // update takt timer in seconds
            this.tt = this.calcSec(this.hour, this.min, this.sec);

            // update clock
            this.lblClock.Text = this.clockFromSec(this.tt);
        }

        

        private void taktTimer_Tick(object sender, EventArgs e)
        { 

            if (this.tt == 0)
                this.tt = this.calcSec(this.hour, this.min, this.sec - 1);
            else
                this.tt--;

            this.lblClock.Text = this.clockFromSec(this.tt);

            
            

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            

            if (this.calcSec(this.hour, this.min, this.sec) <= 10)
            {
                MessageBox.Show("Error: Takt time must be greater than 10 seconds");
                return;
            }

            this.taktTimer.Start();

            this.stopped = false;

            

            this.btnHrInc.Visible = false;
            this.btnHrDec.Visible = false;
            this.btnMinInc.Visible = false;
            this.btnMinDec.Visible = false;
            this.btnSecInc.Visible = false;
            this.btnSecDec.Visible = false;

            

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.taktTimer.Stop();

            this.stopped = true;

            

            this.tt = this.calcSec(this.hour, this.min, this.sec);
            this.lblClock.Text = this.clockFromSec(this.tt);

            this.btnHrInc.Visible = true;
            this.btnHrDec.Visible = true;
            this.btnMinInc.Visible = true;
            this.btnMinDec.Visible = true;
            this.btnSecInc.Visible = true;
            this.btnSecDec.Visible = true;


        }

        private int calcSec(int hour, int min, int sec)
        {
            return (hour * 3600) + (min * 60) + sec;
        }

        private void lightTimer_Tick(object sender, EventArgs e)
        {
            this.changeColor(this.getColor());
        }

        private void cbPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void updateHUD()
        {



            if (this.tt < this.rollTime() && this.stopped == false)
                this.lblPS.Text = "ROLL!";
            else
                this.lblPS.Text = this.ps;

            this.lblPartID.Text = this.pID;

            this.lblTime.Text = DateTime.Now.ToLongTimeString();


            if (this.ps == "DOWN")
            {
                // special case
                string downReason = this.psR;

                this.lblPS.Text = Util.replAwBStr(downReason, '_', ' ');

            }

            this.BackColor = this.colorFromPS(this.getColor());

            this.lblClients.Text = "Clients: " + this.clients;

            
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            this.updateHUD();
        }

       
    }
}
