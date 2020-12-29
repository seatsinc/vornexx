using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Media;
using System.Net;
using System.Threading;


namespace VorneAPITestC
{
    

    public partial class Form1 : Form
    {
        // buffer size for server client relationship
        const int BUFFERSIZE = 1024 * 1024;

        // random comment

        // SERVERIP 127.0.0.1 if on local computer
        // else the ip address of the target computer
        const string VORNEIP = "10.119.12.15";
        const string SERVERIP = "10.119.16.56";
        public static string WCNAME = "3915";

        const int SERVERPORT = 50010;

        const int ROLLTIME = 10;

        const int QUERYINTERVAL = 50; // miliseconds

        
        public string ps;
        public string color;
        public string pID;
        public int tt;

        public bool inRoll = false;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // aligning
            Rectangle wa = Screen.GetWorkingArea(this);
            this.Width = wa.Width / 5;
            this.Height = wa.Height / 5;
            this.Location = new Point(wa.Right - this.Width, wa.Bottom - this.Height);

            this.lblPS.Location = new Point(wa.Left + 10, wa.Top + 10);
            this.lblPartID.Location = new Point(wa.Left + this.Width - this.lblPartID.Width - 10, wa.Top + 10);
            this.lblTime.Location = new Point(wa.Left + 10, wa.Top + this.Height - this.lblTime.Height - 10);
            this.lblWC.Location = new Point(wa.Left + this.Width - this.lblWC.Width - 10, wa.Top + this.Height - this.lblTime.Height - 10);
            this.lblClock.Location = new Point(wa.Left + this.Width / 2 - this.lblClock.Width / 2, wa.Top + this.Height / 2 - this.lblClock.Height / 2);

            this.lblWC.Text = WCNAME;

            this.BringToFront();
            this.TopMost = true;

            Thread listener = new Thread(this.communicate);
            listener.IsBackground = true;
            listener.Start();

            
            timer.Start();

        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void communicate()
        {
            while (true)
            {

                try
                {
                    // connect to the server
                    TcpClient client = new TcpClient(SERVERIP, SERVERPORT);
                    

                    while (true)
                    {
                        NetworkStream stream = client.GetStream();

                        // send message to the server
                        byte[] writeBytes = Encoding.Unicode.GetBytes("Please send us production state, part id, and takt time, thank you.");
                        stream.Write(writeBytes, 0, writeBytes.Length);


                        // receive message from the server
                        byte[] readBytes = new byte[BUFFERSIZE];
                        do
                        {
                            stream.Read(readBytes, 0, readBytes.Length);
                        }
                        while (stream.DataAvailable);

                        Message message = JsonConvert.DeserializeObject<Message>(Encoding.Unicode.GetString(readBytes));

                        this.pID = message.pID;
                        this.tt = message.tt;
                        this.color = message.color;
                        this.ps = message.ps;

                        
                        Thread.Sleep(QUERYINTERVAL);

                        

                    }

                    
                }
                catch (Exception exc)
                {

                    this.pID = "";
                    this.ps = "CONNECTING...";
                    this.color = "BLACK*";
                    this.tt = 0;

                   

                }

                

                
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = DateTime.Now.ToLongTimeString();

            this.lblPartID.Text = this.pID;
            if (this.ps == "SERVER OFFLINE")
                this.lblClock.Text = "";
            else
                this.lblClock.Text = this.clockFromSec(this.tt);

            if (this.tt < ROLLTIME && this.color == "BLACK")
                this.lblPS.Text = "ROLL!";
            else
                this.lblPS.Text = this.ps;

            this.BackColor = this.colorFromPS(this.color);



            if (this.color != "BLACK" && this.inRoll == true)
            {
                SoundPlayer sp = new SoundPlayer("resources\\audio\\BEEP.wav");
                sp.Play();
            }

            if (this.color == "BLACK")
            {
                this.inRoll = true;
            }
            else
            {
                this.inRoll = false;
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

        private void lblClock_Click(object sender, EventArgs e)
        {
            Scoreboard sb = new Scoreboard(VORNEIP, WCNAME);

           

            sb.ShowDialog();

           
        }
    }

    

}
