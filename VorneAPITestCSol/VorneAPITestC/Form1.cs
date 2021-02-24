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
using System.Drawing.Imaging;
using System.IO;

namespace VorneAPITestC
{
    

    public partial class Form1 : Form
    {

        // SERVERIP 127.0.0.1 if on local computer
        // else the ip address of the target computer
        // Form1.loadConstants will read in these values
        // ONLY CHANGE THESE VALUES
        string VORNEIP;
        string SERVERIP;
        public static string WCNAME;
        int SERVERPORT; // the port the server LISTENS on

        const int QUERYINTERVAL = 16;
        const int TIMEOUT = 150;
        
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

            this.loadConstants();




            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // aligning
            Rectangle wa = Screen.GetWorkingArea(this);
            this.Width = wa.Width / 5;
            this.Height = wa.Height / 5;
            this.Location = new Point(wa.Right - this.Width, wa.Bottom - this.Height);


            // displaying the logo


            
            Bitmap image = (Bitmap)Image.FromFile(@"resources\images\logo.png");


            pbLogo.Image = image;
            pbLogo.BackColor = Color.Transparent;



            pbLogo.Location = new Point((this.Width / 2) - (this.pbLogo.Width / 2), this.Height / 25);



            // end of displaying the image

            this.lblTime.Text = "0:00:00 AM";
            this.lblWC.Text = WCNAME;


            this.lblPS.Location = new Point(wa.Left + 10, wa.Top + this.Height - this.lblPS.Height - 10);
            this.lblPartID.Location = new Point(wa.Left + 10, wa.Top + 10);
            this.lblTime.Location = new Point(wa.Left + this.Width - this.lblTime.Width - 10, wa.Top + this.Height - this.lblTime.Height - 10);
            this.lblWC.Location = new Point(wa.Left + this.Width - this.lblWC.Width - 10, wa.Top + 10);
            this.lblClock.Location = new Point(wa.Left + this.Width / 2 - this.lblClock.Width / 2, wa.Top + this.Height / 2 - this.lblClock.Height / 2);




            

            this.BringToFront();
            this.TopMost = true;

          

            this.communicate();


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

                Form1.WCNAME = sl[0][1].Trim();
                this.VORNEIP = sl[1][1].Trim();
                this.SERVERIP = sl[2][1].Trim();
                this.SERVERPORT = Int32.Parse(sl[3][1].Trim());


            }
            catch (Exception exc)
            {
                MessageBox.Show("Error loading constants: " + exc.ToString());
                Application.Exit();
            }
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private async void communicate()
        {

            await Task.Run(() =>
            {

                SyncClient sc = new SyncClient(SERVERIP, SERVERPORT, TIMEOUT);


                Message message = sc.queryServer();

                this.pID = message.pID;
                this.tt = message.tt;
                this.color = message.color;
                this.ps = message.ps;

                try
                {
                    this.Invoke((System.Action)(this.updateHUD));
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
                finally
                {
                    Thread.Sleep(QUERYINTERVAL);


                    this.communicate();
                }

                
                
            });


        }

        private void updateHUD()
        {
            this.lblTime.Text = DateTime.Now.ToLongTimeString();

            this.lblPartID.Text = this.pID;

            if (this.ps == "OFFLINE")
                this.lblClock.Text = "        ";
            else
                this.lblClock.Text = this.clockFromSec(this.tt);

            if (this.inRoll)
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

            
    }

    

}
