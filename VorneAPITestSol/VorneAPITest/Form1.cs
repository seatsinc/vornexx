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


namespace VorneAPITest
{
    public partial class Form1 : Form
    {
        
        
        

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            

            // make it so the screen is in the bottom right corner
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);

            // make it so the application will always be on top
            this.TopMost = true;


            // add the work centers to the work center combo box
            cbWC.Items.Add("3910");
            cbWC.Items.Add("3920");
            cbWC.Items.Add("3915");

            cbWC.SelectedIndex = 0;


            timer.Start();

        }

        private string getWCIP(string wc)
        {
            if (wc == "3910")
            {
                return "10.119.12.14";
            }
            else if (wc == "3920")
            {
                return "10.119.12.13";
            }
            else if (wc == "3915")
            {
                return "10.119.12.15";
            }
            else
            {
                return null;
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

        private void updateHUD()
        {
            RestClient client = new RestClient();


            // make requests to vorne
            string requestPS = client.makeRequest("http://" + this.getWCIP(this.cbWC.SelectedItem.ToString()) + "/api/v0/process_state/active", httpVerb.GET);
            string requestPR = client.makeRequest("http://" + this.getWCIP(this.cbWC.SelectedItem.ToString()) + "/api/v0/part_run", httpVerb.GET);
            string requestT = client.makeRequest("http://" + this.getWCIP(this.cbWC.SelectedItem.ToString()) + "/api/v0/team", httpVerb.GET);


            // deserialize objects
            PSActiveNS.PSActive psActive = JsonConvert.DeserializeObject<PSActiveNS.PSActive>(requestPS);
            PartRunNS.PartRun partRun = JsonConvert.DeserializeObject<PartRunNS.PartRun>(requestPR);
            TeamNS.Team team = JsonConvert.DeserializeObject<TeamNS.Team>(requestT);

            // update HUD

            double sec = psActive.data.elapsed;

            double taktTime = partRun.data.takt_time;
            string ps = Util.replAwBStr(psActive.data.name.ToUpper(), '_', ' ');
            this.lblPS.Text = ps;

            

            this.lblTime.Text = DateTime.Now.ToLongTimeString();

            if (ps == "RUNNING")
            {
                

                this.lblPartID.Text = partRun.data.part_id.ToUpper();


                

                // update clock (based on interval time
                int cc = -((Convert.ToInt32(Math.Floor(sec)) % Convert.ToInt32(Math.Floor(taktTime))) - Convert.ToInt32(Math.Floor(taktTime))) - 1;


                this.BackColor = System.Drawing.Color.PaleGreen;

                this.lblClock.Text = this.clockFromSec(cc);


                Console.WriteLine('\r' + Math.Round(sec, 1).ToString());
                
                if (Math.Round(sec, 1) % Math.Round(taktTime, 1) == 0.0)
                {
                    Console.WriteLine(Math.Round(sec, 1).ToString() + "-----------" + Math.Round(taktTime, 1).ToString());

                    SoundPlayer sp = new SoundPlayer("resources\\audio\\BEEP.wav");
                    
                    sp.Play();
                }
                
            }
            else if (ps == "DOWN")
            {
                this.lblPartID.Text = partRun.data.part_id.ToUpper();

                string downReason = psActive.data.process_state_reason.ToUpper();

                this.lblPS.Text = Util.replAwBStr(downReason, '_', ' ');

                this.BackColor = System.Drawing.Color.Crimson;

                // update clock
                this.lblClock.Text = this.clockFromSec(sec);
            }   
            else if (ps == "CHANGEOVER")
            {
                this.lblPartID.Text = partRun.data.part_id.ToUpper();


                this.BackColor = System.Drawing.Color.Orange;

                // update clock
                this.lblClock.Text = this.clockFromSec(sec);
            }
            else
            {

                this.BackColor = System.Drawing.Color.DeepSkyBlue;
               

                this.lblPartID.Text = "";
                

                

                // update clock
                this.lblClock.Text = this.clockFromSec(sec);
            }
             



        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.updateHUD();
        }

       
    }
}
