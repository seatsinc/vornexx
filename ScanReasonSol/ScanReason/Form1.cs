using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Newtonsoft.Json;

namespace ScanReason
{

    


    public partial class Form1 : Form
    {

        const string VORNEIP = "10.119.12.15";

        SoundPlayer sp;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            System.IO.Stream str = Properties.Resources.ALARM;

            sp = new SoundPlayer(str);

            timer.Start();

            sp.Play();

        }

        

        private bool isMissingReason()
        {

            try
            {
                RestClient client = new RestClient();

                // make requests to vorne
                string requestPS = client.makeRequest("http://" + VORNEIP + "/api/v0/process_state/active", httpVerb.GET);



                // deserialize objects
                PSActive psActive = JsonConvert.DeserializeObject<PSActive>(requestPS);

                if (psActive.data.process_state_reason == "down")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                return false;
            }
        }


        private void missingReasonTick()
        {
            this.WindowState = FormWindowState.Maximized;


            this.BackColor = System.Drawing.Color.Red;

            this.lblScan.Location = new Point((this.Width / 2) - (this.lblScan.Width / 2), (this.Height / 2) - this.lblScan.Height);
            this.lblDownReason.Location = new Point((this.Width / 2) - (this.lblDownReason.Width / 2), this.Height / 2);

            sp.Play();
        }

        private void allGoodTick()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            if (this.isMissingReason())
            {
                this.missingReasonTick();
            }
            else
            {
                this.allGoodTick();
            }
        }
    }
}
