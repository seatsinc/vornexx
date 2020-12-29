using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;

namespace VorneAPITestC
{
    public partial class Scoreboard : Form
    {

        private string VORNEIP;

        private string shift, wc;
        

        private string score;

        
        private List<List<List<string>>> board = new List<List<List<string>>>();

        private int count = 0;



        public Scoreboard(string vip, string workCenter)
        {
            InitializeComponent();

            this.VORNEIP = vip;
            this.wc = workCenter;
        }

        private string center(string s, int width)
        {
            if (s.Length >= width)
            {
                return s;
            }

            int leftPadding = (width - s.Length) / 2;
            int rightPadding = width - s.Length - leftPadding;

            return new string(' ', leftPadding) + s + new string(' ', rightPadding);
        }

        private void Scoreboard_Load(object sender, EventArgs e)
        {

            this.lblScoreboard.Text = "LOADING...";

            Rectangle wa = Screen.GetWorkingArea(this);
            this.Width = wa.Width;
            this.Height = wa.Height;
            this.Location = new Point(0, 0);


            this.refr();
            


        }

        
        private List<List<List<string>>> queryScore()
        {
            
            
            List<List<List<string>>> sb = new List<List<List<string>>>();

            RestClient client = new RestClient();

            // center("TIME", w), center("PART", w), center("GOOD COUNT", w), center("LABOR EFF.", w), center("OEE", w), center("AVAIL.", w), center("PERF.", w), center("QUALITY", w));

            // make requests to vorne

            try
            {
               

                string shiftQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=shift&limit=15&sort=-event_id", httpVerb.GET);
                shiftQuery = Util.jsonMakeover(shiftQuery);
                Console.WriteLine(shiftQuery);
                ShiftQueryNS.ShiftQuery sq = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(shiftQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                string startQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=start_time&limit=15&sort=-event_id&", httpVerb.GET);
                ShiftQueryNS.ShiftQuery st = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(startQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string timeQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=end_time&limit=15&sort=-event_id", httpVerb.GET);
                ShiftQueryNS.ShiftQuery tq = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(timeQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string partQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=part&limit=15&sort=-event_id", httpVerb.GET);
                partQuery = Util.jsonMakeover(partQuery);
                ShiftQueryNS.ShiftQuery part = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(partQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string goodCountQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=good_count&limit=20&sort=-event_id", httpVerb.GET);
                ShiftQueryNS.ShiftQuery gc = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(goodCountQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string lbrEffQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=labor_efficiency&limit=20&sort=-event_id", httpVerb.GET);
                ShiftQueryNS.ShiftQuery le = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(lbrEffQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string oeeQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=oee&limit=20&sort=-event_id", httpVerb.GET);
                ShiftQueryNS.ShiftQuery oee = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(oeeQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string availabilityQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=availability&limit=20&sort=-event_id", httpVerb.GET);
                ShiftQueryNS.ShiftQuery a = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(availabilityQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string performanceQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=performance&limit=20&sort=-event_id", httpVerb.GET);
                ShiftQueryNS.ShiftQuery p = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(performanceQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string qualityQuery = client.makeRequest("http://" + VORNEIP + "/api/v0/channels/shift_hour/events?fields=quality&limit=20&sort=-event_id", httpVerb.GET);
                ShiftQueryNS.ShiftQuery q = JsonConvert.DeserializeObject<ShiftQueryNS.ShiftQuery>(qualityQuery, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });







                this.shift = Util.replAwBStr(sq.data.events.ElementAt<List<string>>(0).Last<string>(), '_', ' ').ToUpper();


                int lines = 0;

                bool end = false;

                while (!end)
                {
                    end = true;

                    for (int i = 0; i < sq.data.events.ElementAt<List<string>>(lines).Count; ++i)
                    {
                        if (sq.data.events.ElementAt<List<string>>(lines)[i] != "null")
                            end = false;
                    }

                    if (!end)
                        lines++;
                }
                    


                // center("TIME", w), center("PART", w), center("GOOD COUNT", w), center("LABOR EFF.", w), center("OEE", w), center("AVAIL.", w), center("PERF.", w), center("QUALITY", w));

                for (int i = 0; i < lines; ++i)
                {

                    sb.Add(new List<List<string>>());

                    for (int j = 0; j < 9; ++j)
                        sb.ElementAt<List<List<string>>>(i).Add(new List<string>());


                    // start time
                    for (int j = 0; j < st.data.events.ElementAt(i).Count; ++j)
                    {
                        string str = st.data.events.ElementAt<List<string>>(i).ElementAt<string>(j).Substring(11, 5);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(0).Add(DateTime.Parse(str).ToShortTimeString());
                    }


                    // end time
                    for (int j = 0; j < tq.data.events.ElementAt(i).Count; ++j)
                    {
                        string str = tq.data.events.ElementAt<List<string>>(i).ElementAt<string>(j).Substring(11, 5);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(1).Add(DateTime.Parse(str).ToShortTimeString());
                    }


                    // part
                    for (int j = 0; j < part.data.events.ElementAt(i).Count; ++j)
                    {
                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(2).Add(part.data.events.ElementAt<List<string>>(i).ElementAt<string>(j));
                    }


                    // good count
                    for (int j = 0; j < gc.data.events.ElementAt(i).Count; ++j)
                    {
                        string d = gc.data.events.ElementAt<List<string>>(i).ElementAt<string>(j);

                        double goodCountTemp = Convert.ToDouble(d);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(3).Add(Math.Round(goodCountTemp, 0).ToString());
                    }

                    // labor efficiency
                    for (int j = 0; j < le.data.events.ElementAt(i).Count; ++j)
                    {
                        string d = le.data.events.ElementAt<List<string>>(i).ElementAt<string>(j);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(4).Add(Util.percentRound(d, 1) + '%');
                    }

                    // oee
                    for (int j = 0; j < oee.data.events.ElementAt(i).Count; ++j)
                    {
                        string d = oee.data.events.ElementAt<List<string>>(i).ElementAt<string>(j);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(5).Add(Util.percentRound(d, 1) + '%');
                    }

                    // availability
                    for (int j = 0; j < a.data.events.ElementAt(i).Count; ++j)
                    {
                        string d = a.data.events.ElementAt<List<string>>(i).ElementAt<string>(j);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(6).Add(Util.percentRound(d, 1) + '%');
                    }

                    // performance
                    for (int j = 0; j < p.data.events.ElementAt(i).Count; ++j)
                    {
                        string d = p.data.events.ElementAt<List<string>>(i).ElementAt<string>(j);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(7).Add(Util.percentRound(d, 1) + '%');
                    }

                    // quality
                    for (int j = 0; j < q.data.events.ElementAt(i).Count; ++j)
                    {
                        string d = q.data.events.ElementAt<List<string>>(i).ElementAt<string>(j);

                        sb.ElementAt<List<List<string>>>(i).ElementAt<List<string>>(8).Add(Util.percentRound(d, 1) + '%');
                    }






                }

                sb.Reverse();


                this.count++;


                return sb;
            }
            catch (Exception exc)
            {
                return null;
            }
            

        }




       

        private void writeScore()
        {
            

            this.score = "";


            int w = 15;

            string line = "";

            for (int i = 0; i < 144; ++i)
            {
                line += '-';
            }


            this.wc = Form1.WCNAME;


            score += DateTime.Now.ToShortDateString() + " - " + shift + " - " + wc + '\n';

            score += string.Format("{0,0}|{1,0}|{2,0}|{3,0}|{4,0}|{5,0}|{6,0}|{7,0}|{8,0}\n", center("START TIME", w), center("END TIME", w), center("PART", w), center("GOOD COUNT", w), center("LABOR EFF.", w), center("OEE", w), center("AVAIL.", w), center("PERF.", w), center("QUALITY", w));
            score += line + '\n';


            // writing the queried scorebaord to the label string


            for (int i = 0; i < this.board.Count; ++i)
            {
                List<List<string>> currentLine = this.board.ElementAt<List<List<string>>>(i);


                bool newLine = true;


                while (newLine)
                {

                    newLine = false;

                    for (int j = 0; j < currentLine.Count; ++j)
                    {
                        if (currentLine[j].Count > 1)
                            newLine = true;


                        if (currentLine[j].Count >= 1)
                        // only one element
                        {
                            this.score += string.Format(center(currentLine[j][0], w));
                            currentLine[j].RemoveAt(0);

                        }
                        else
                        // 0 elements
                        {
                            this.score += string.Format(center(" ", w));
                        }




                        // handle the '|'
                        if (j < currentLine.Count - 1)
                            this.score += "|";



                    }

                    // write a dotted line
                    this.score += '\n';

                    if (newLine == false)
                        this.score += line + '\n';



                }



                
            }


            this.lblScoreboard.Text = score;


            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.refr();
        }


        private void refr()
        {
            this.Hide();

            this.lblScoreboard.Text = "LOADING...";
            this.board = this.queryScore();

            if (this.board == null)
            {
                this.lblScoreboard.Text = "ERROR";
                return;
            }

            this.writeScore();

            this.Show();
        }

        
    }
}
