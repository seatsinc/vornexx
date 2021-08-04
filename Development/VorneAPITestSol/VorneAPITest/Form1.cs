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
using NAudio.Wave;

namespace VorneAPITest
{
    public partial class Form1 : Form
    {
        public static Mutex mutex = new Mutex();


        // how long the lights will turn off to warn on takt time (takt time / ROLLDIVISOR)
        //const int ROLLDIVISOR = 10;



        private AsyncServer ss;


        // ip address of the vorne machine
        // these will be the values read in from a text file with Form1.loadConstants
        public static string WCNAME;
        string VORNEIP;
        int LISTENPORT; // the port that the server listens on
        string LIGHTPORT;

        private const int VORNETIMEOUT = 150;
        private const int VORNERETRIES = 30;
        private const int VORNEQUERYINTERVAL = 1000;
        private const int CLIENTTIMEOUT = 250;

        private string soundFileName = "BEEP.wav";
        
        

        // keeps track of the production state 
        private string ps; // production state
        private string pID; // part id
        private string psR; // process state reason

        // hour, min, sec are what the user has set on the time
        // tt stands for takt timer and represents the takt timer going down in seconds
        private int hour, min, sec, tt;

        private bool stopped = true;

        private SerialPort p = new SerialPort();


        private int playMinutesCount;

        public Form1()
        {
            InitializeComponent();
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            this.loadConstants();
            this.populateSounds();

            this.changeSound(this.cbSounds.Items[0].ToString());

            p.PortName = LIGHTPORT;
            p.BaudRate = 9600;
            p.DataBits = 8;
            p.Parity = Parity.None;
            p.StopBits = StopBits.One;
            p.DiscardNull = true;
            p.ReadTimeout = 1000;
            p.WriteTimeout = 1000;


            this.playMinutesCount = 0;
            this.lblShuffle.Visible = false;
            this.nShuffles.Visible = false;
            this.playTimer.Start();

            this.nEfficiency.Value = 100;


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
            this.cbSounds.Location = new Point(wa.Left + 10, wa.Top + this.lblPS.Height + 10);

            this.nShuffles.Location = new Point(wa.Left + this.Width - this.nShuffles.Width - 10, wa.Top + 10 + this.lblPartID.Height);
            this.lblShuffle.Location = new Point(wa.Left + this.Width - this.lblShuffle.Width - 10, wa.Top + 10 + this.lblPartID.Height + this.nShuffles.Height);
            this.checkShuffle.Location = new Point(wa.Left + this.Width - this.nShuffles.Width - this.checkShuffle.Width - 10, wa.Top + 10 + this.lblPartID.Height);

            this.cbSounds.DropDownWidth = this.DropDownWidth(this.cbSounds);
            this.lblWC.Location = new Point(wa.Left + this.Width - this.lblWC.Width - 10, wa.Top + this.Height - this.lblWC.Height - 10);



            this.btnCalcTT.Location = new Point(wa.Left + 10, wa.Top + this.Height - this.nEfficiency.Height - 10);
            this.nEfficiency.Location = new Point(wa.Left + 10, wa.Top + this.Height - this.nEfficiency.Height - 10 - this.btnCalcTT.Height);
            this.lblEfficiency.Location = new Point(wa.Left + this.nEfficiency.Width + 10, wa.Top + this.Height - this.nEfficiency.Height - 10 - this.btnCalcTT.Height);

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

        int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;
        }

        private void changeSound(string fileName)
        {
            this.soundFileName = fileName;

            WaveFileReader wf = new WaveFileReader($"resources\\audio\\{fileName}");

            try
            {


                SoundPlayer sp = new SoundPlayer($"resources\\audio\\{this.soundFileName}");

                sp.Play();
            }
            catch (Exception exc)
            {
                MessageBox.Show("media not found");
            }
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




        private async void queryVorneAsync()
        {
            
            await Task.Run(() =>
            {

                
                // vorne communication
                RestClient client = new RestClient(VORNETIMEOUT, VORNERETRIES);



                try
                {
                    
                    string requestPS =  client.makeRequest("http://" + VORNEIP + "/api/v0/process_state/active", httpVerb.GET);
                    string requestPR = client.makeRequest("http://" + VORNEIP + "/api/v0/part_run", httpVerb.GET);
    


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
        

        private void populateSounds()
        {
            foreach (string fileName in Directory.GetFiles(@"resources\audio"))
            {
                this.cbSounds.Items.Add(parseFileName(fileName));
            }

            this.cbSounds.SelectedIndex = 0;
        }

        private string parseFileName(string path)
        {
            string parsedFileName = "";

            Stack<char> stack = new Stack<char>();

            int i = path.Length - 1;

            while (path[i] != '\\')
            {
                stack.Push(path[i]);
                i--;
            }

            while (stack.Count > 0)
            {
                parsedFileName += stack.Peek();
                stack.Pop();
            }

            return parsedFileName;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        // turns lights off when the program exits
        {
            try
            {
                this.ss.exit();
            }
            catch
            {
                Console.WriteLine("asdf");
            }


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

            if (this.stopped == false && this.tt < 10)
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

        private void updateMinPlayMinutes()
        {
            this.nShuffles.Minimum = (this.hour * 60) + this.min + 1;
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

            this.updateMinPlayMinutes();
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

            this.updateMinPlayMinutes();
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

            this.updateMinPlayMinutes();
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

            this.updateMinPlayMinutes();
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

            this.updateMinPlayMinutes();
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

            this.updateMinPlayMinutes();
        }



        private void taktTimer_Tick(object sender, EventArgs e)
        {

            Message message = new Message(this.ps, this.getColor(), this.pID, this.tt, this.soundFileName);

            this.lblClock.Text = this.clockFromSec(this.tt);

            this.ss.setRelayMessage(JsonConvert.SerializeObject(message));

            this.ss.dump();

            if (this.tt == 0)
                this.tt = this.calcSec(this.hour, this.min, this.sec - 1);
            else
                this.tt--;

            

            


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

        private void cbSounds_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.changeSound(this.cbSounds.SelectedItem.ToString());
        }

        private void checkShuffle_CheckedChanged(object sender, EventArgs e)
        {
            this.playMinutesCount = 0;

            if (this.checkShuffle.Checked)
            {
                this.lblShuffle.Visible = true;
                this.nShuffles.Visible = true;
            }
            else
            {
                this.lblShuffle.Visible = false;
                this.nShuffles.Visible = false;
            }
        }

        private void playTimer_Tick(object sender, EventArgs e)
        {
            this.playMinutesCount++;

            if (this.playMinutesCount >= this.nShuffles.Value && this.checkShuffle.Checked)
            {

                Random random = new Random();

                this.cbSounds.SelectedIndex = random.Next(0, this.cbSounds.Items.Count);

                this.playMinutesCount = 0;
            }
        }

        private void nShuffles_ValueChanged(object sender, EventArgs e)
        {
            this.playMinutesCount = 0;
        }

        private async void btnCalcTT_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    // vorne communication
                    RestClient client = new RestClient(VORNETIMEOUT, VORNERETRIES);

                    string requestPR = client.makeRequest("http://" + VORNEIP + "/api/v0/part_run", httpVerb.GET);
                    string requestT = client.makeRequest("http://" + VORNEIP + "/api/v0/team", httpVerb.GET);




                    if (requestT == string.Empty || requestPR == string.Empty)
                        throw new Exception("Could not make http request!");


                    // deserialize objects
                    TeamNS.Team team = JsonConvert.DeserializeObject<TeamNS.Team>(requestT);
                    PartRunNS.PartRun partRun = JsonConvert.DeserializeObject<PartRunNS.PartRun>(requestPR);

                    string pid = partRun.data.part_id;
                    double tlpp = partRun.data.target_labor_per_piece;
                    double teamSize = team.data.team_size;
                    double eff = Convert.ToDouble(this.nEfficiency.Value);

                    if (teamSize <= 0.0)
                    {
                        MessageBox.Show("Error! Team size must be greater than 0!");
                    }
                    else
                    {
                        double sph = Math.Ceiling((3600.0 / tlpp) * teamSize * (eff / 100.0));

                        MessageBox.Show($"" +
                            $"Part: {pid}\n" +
                            $"Target labor per piece: {this.clockFromSec(Math.Ceiling(tlpp))}\n" +
                            $"Team size: {teamSize}\n" +
                            $"\n" +
                            $"<< GOALS >>\n" +
                            $"Labor efficiency: {eff}%\n" +
                            $"Target labor per piece: {this.clockFromSec(Math.Ceiling(tlpp * (100.0 / eff)))}\n" +
                            $"Takt time: {this.clockFromSec(Math.Ceiling(3600.0 / sph))}\n" +
                            $"Seats per hour: {sph}");
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Could not make request!");
                }

            });
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


                        if (this.tt < 10 && this.stopped == false)
                            this.lblPS.Text = "ROLL!";
                        else
                            this.lblPS.Text = this.ps;

                        this.lblPartID.Text = this.pID;

          


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
                    


                    
                    Thread.Sleep(1000);


                    this.updateHUDAsync();

                    

                    if (this.taktTimer.Enabled == false)
                    {
                        Message message = new Message(this.ps, this.getColor(), this.pID, this.tt, this.soundFileName);

                        this.ss.setRelayMessage(JsonConvert.SerializeObject(message));

                        this.ss.dump();

                    }
                }

                
                

            });

            


        }



    }
}
