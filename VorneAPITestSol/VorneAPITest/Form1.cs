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
using System.IO;
using System.Diagnostics;


namespace VorneAPITest
{
    public partial class Form1 : Form
    {
        public static Mutex mutex = new Mutex();


        // how long the lights will turn off to warn on takt time (takt time / ROLLDIVISOR)
        const int ROLLDIVISOR = 10;

        private AsyncServer ss;


        // ip address of the vorne machine
        // these will be the values read in from a text file with Form1.loadConstants
        public static string WCNAME;
        string VORNEIP;
        int LISTENPORT; // the port that the server listens on
        string LIGHTPORT;

        private const int VORNETIMEOUT = 150;
        private const int VORNEQUERYINTERVAL = 1000;
        private const int CLIENTTIMEOUT = 150;
        private const int CLIENTQUERYINTERVAL = 250;



        

        // keeps track of the production state 
        private string ps; // production state
        private string pID; // part id
        private string psR; // process state reason

        // hour, min, sec are what the user has set on the time
        // tt stands for takt timer and represents the takt timer going down in seconds
        private int hour, min, sec, tt;

        private bool stopped = true;

        private SerialPort p = new SerialPort();




        public Form1()
        {
            InitializeComponent();
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            this.loadConstants();


            p.PortName = LIGHTPORT;
            p.BaudRate = 9600;
            p.DataBits = 8;
            p.Parity = Parity.None;
            p.StopBits = StopBits.One;
            p.DiscardNull = true;
            p.ReadTimeout = VORNEQUERYINTERVAL;
            p.WriteTimeout = VORNEQUERYINTERVAL;
           





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

            // adding an image
            Bitmap image = (Bitmap)Image.FromFile(@"resources\images\logo.png");


            pbLogo.Image = image;
            pbLogo.BackColor = Color.Transparent;



            pbLogo.Location = new Point((this.Width / 2) - (this.pbLogo.Width / 2), this.Height / 30);
            // end of adding image



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

           






            // make it so the application will always be on top
            this.BringToFront();
            this.TopMost = true;


            this.queryVorneAsync();

            this.updateHUDAsync();


           
            this.changeColorAsync();


            this.ss = new AsyncServer(LISTENPORT, CLIENTTIMEOUT);
            this.ss.listen();





        }



        private void loadConstants()
        {
            try
            {


                string fileName = "CONSTANTS.txt";

                List<string> lines = new List<string>();

                lines = File.ReadAllLines(fileName).ToList<string>();


                List<List<string>> sl = new List<List<string>>();

                foreach (string line in lines)
                    sl.Add(line.Split(':').ToList<string>());

                WCNAME = sl[0][1].Trim();
                this.VORNEIP = sl[1][1].Trim();
                this.LISTENPORT = Int32.Parse(sl[2][1].Trim());
                this.LIGHTPORT = sl[3][1].Trim();


            }
            catch (Exception exc)
            {
                MessageBox.Show("Error loading constants: " + exc.ToString());
                Application.Exit();
            }
        }



        private int rollTime()
        {
            return this.calcSec(this.hour, this.min, this.sec) / ROLLDIVISOR;
        }



        private async void queryVorneAsync()
        {
            
            await Task.Run(() =>
            {

                
                // vorne communication
                RestClient client = new RestClient(150, 30);



                try
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    string requestPS =  client.makeRequest("http://" + VORNEIP + "/api/v0/process_state/active", httpVerb.GET);
                    string requestPR = client.makeRequest("http://" + VORNEIP + "/api/v0/part_run", httpVerb.GET);
                    timer.Stop();
                    Console.WriteLine(timer.ElapsedMilliseconds);

                    if (requestPS == string.Empty || requestPR == string.Empty)
                        throw new Exception("Could not make http request!");


                    // deserialize objects
                    PSActiveNS.PSActive psActive = JsonConvert.DeserializeObject<PSActiveNS.PSActive>(requestPS);
                    PartRunNS.PartRun partRun = JsonConvert.DeserializeObject<PartRunNS.PartRun>(requestPR);


                    this.pID = partRun.data.part_id;
                    this.psR = psActive.data.process_state_reason.ToUpper();


                    this.ps = Util.replAwBStr(psActive.data.name.ToUpper(), '_', ' ');


                }
                catch (Exception e)
                {


                    this.pID = "";
                    this.psR = "";
                    this.ps = "";


                }
                finally
                {
                    Thread.Sleep(VORNEQUERYINTERVAL);
                    this.queryVorneAsync();

                }


            });

            
            

            


        }


        private void OnApplicationExit(object sender, EventArgs e)
        // turns lights off when the program exits
        {



            try
            {
                if (!p.IsOpen)
                    p.Open();

                if (p.IsOpen)
                    p.Write('x'.ToString());
                else
                    throw new Exception("Serial port is not open");

                p.Close();
            }
            catch (Exception exc)
            {
                //Console.WriteLine(exc.ToString());
            }
            finally
            {
                Process.GetCurrentProcess().Kill();
            }

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

        private char getArduinoColor(string color)
        {
            if (color == "RED")
            {
                return 'r';
            }
            else if (color == "GREEN")
            {
                return 'g';
            }
            else if (color == "BLUE")
            {
                return 'b';
            }
            else if (color == "YELLOW")
            {
                return 'y';
            }
            else
            {
                return 'x';
            }

        }

        private async void changeColorAsync()
        {
            await Task.Run(() =>
            {
                
                char color = this.getArduinoColor(this.getColor());

                try
                {

                    

                    if (!p.IsOpen)
                        p.Open();


                    if (p.IsOpen)
                    {
                        p.Write(color.ToString());
                    }
                    else
                        throw new Exception("Serial port is not open");


                    Console.WriteLine((char)p.ReadChar());

                    

                }
                catch (Exception exc)
                {
                    Thread.Sleep(VORNEQUERYINTERVAL);
                }
                finally
                {

                    this.changeColorAsync();
                }

            });

            
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

            this.btnStart.Visible = false;

            this.taktTimer.Start();

            this.stopped = false;



            this.btnHrInc.Visible = false;
            this.btnHrDec.Visible = false;
            this.btnMinInc.Visible = false;
            this.btnMinDec.Visible = false;
            this.btnSecInc.Visible = false;
            this.btnSecDec.Visible = false;



        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void lblClock_Click(object sender, EventArgs e)
        {
            this.Hide();

            try
            {
                using (Scoreboard sb = new Scoreboard(VORNEIP, WCNAME))
                {
                    sb.ShowDialog();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            finally
            {
                this.Show();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnStart.Visible = true;

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


       
        private async void updateHUDAsync()
        {
            
            
            await Task.Run(() =>
            {
                try
                {


                    this.Invoke((System.Action)(() =>
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

                    }));
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
                finally
                {
                    Message message = new Message(this.ps, this.getColor(), this.pID, this.tt);
                    this.ss.setRelayMessage(JsonConvert.SerializeObject(message));



                    Thread.Sleep(CLIENTQUERYINTERVAL);

                    this.updateHUDAsync();
                }

                
                

            });

            


        }



    }
}
