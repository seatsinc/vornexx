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
using Gma.System.MouseKeyHook;
using System.IO;

namespace BarcodeScanner
{


    
    public partial class Form1 : Form
    {
        Mutex keyPressesMutex = new Mutex();

        private IKeyboardMouseEvents hook = Hook.GlobalEvents();

        List<string> barcodes = new List<string>();

        private string VORNEIP;
        private string wc;



        List<string> bcPrefixes = new List<string>();

        private string keyPresses = "";
        

        public Form1()
        {
            InitializeComponent();

            

            hook.KeyDown += Hook_KeyPress;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;


            this.loadConstants();
            this.loadbcPrefixes();

            this.Text = this.wc + " Barcode Scanner";

        }


        private void loadbcPrefixes()
        {
            try
            {
                this.bcPrefixes = File.ReadAllLines("PREFIXES.txt").ToList<string>();

                for (int i = 0; i < this.bcPrefixes.Count; ++i)
                {
                    this.bcPrefixes[i] = this.bcPrefixes[i].Trim();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }

            this.bcPrefixes.Sort((e1, e2) => e1.Length.CompareTo(e2.Length));
            this.bcPrefixes.Reverse();




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

                this.VORNEIP = sl[0][1].Trim();
                this.wc = sl[1][1].Trim();


            }
            catch (Exception exc)
            {
                MessageBox.Show("Error loading constants: " + exc.ToString());
                Application.Exit();
            }
        }




        private bool isDigit(char c)
        {
            string digits = "0123456789";
            
            foreach (char d in digits)
            {
                if (d == c)
                    return true;
            }

            return false;
        }


 

        private void Hook_KeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            
            keyPressesMutex.WaitOne();

            char lastKey = this.formatKeyPress(e.KeyCode);

            // backspace
            if (lastKey == '~')
            {
                if (this.keyPresses.Length > 0)
                {
                    this.keyPresses = this.keyPresses.Substring(0, this.keyPresses.Length - 1);
                }
            }
            else
            {
                this.keyPresses += lastKey;
            }
            

            string potentialBC = "";

            int serialIndex = -1;
            
            
            
            try
            {
                 if (this.keyPresses.Last<char>() == '\n')
                {
                    throw new Exception("newline detected");
                }
            }
            catch (Exception exc)
            {
                try
                {

                    int foundIndex = -1;

                    for (int i = 0; i < this.bcPrefixes.Count; ++i)
                    {
                        foundIndex = this.keyPresses.LastIndexOf(this.bcPrefixes[i]);

                        if (foundIndex > -1)
                        {
                            serialIndex = foundIndex + this.bcPrefixes[i].Length;
                            potentialBC += this.bcPrefixes[i];
                            break;
                        }
                    }




                    // get the potentialBC
                    bool validBC = false;

                    if (foundIndex != -1)
                    {
                        while (true)
                        {
                            potentialBC += this.keyPresses[serialIndex];

                            if (this.keyPresses[serialIndex] == '\n')
                            {
                                potentialBC = potentialBC.Substring(0, potentialBC.Length - 1);
                                validBC = true;
                                break;
                            }
                            else if (serialIndex > this.keyPresses.Length || !isDigit(this.keyPresses[serialIndex]))
                            {
                                validBC = false;
                                break;
                            }

                            serialIndex++;
                        }
                    }




                    // end of getting the potentialBC

                    
                    if (validBC)
                    {
                  

                        foreach (string bc in this.barcodes)
                        {

                            if (bc == potentialBC)
                            {
                                throw new Exception("Barcode already scanned!");
                            }

                        }

                        Console.WriteLine($"New barcode scanned: {potentialBC}");


                        this.barcodes.Add(potentialBC);




                        RestClient rc = new RestClient();

                        
                        // +1 good count query
                        // COMMENT OUT WHEN TESTING
                        
                        string message1 = $"http://{VORNEIP}/api/v0/inputs/1";
                        byte[] postData1 = Encoding.ASCII.GetBytes("{}");

                        rc.makeRequest(message1, httpVerb.POST, postData1);
                        
                        // end of good count query

                        

                        // send message to vorne to overlay an image +1

                        
                        string message2 = $"http://{VORNEIP}/api/v0/scoreboard/overlay";
                        
                        byte[] postData2 = Encoding.ASCII.GetBytes($"{{\"duration\":3,\"text\":[\"+1\",\"{potentialBC}\",\"New scan!\"]}}");

                        rc.makeRequest(message2, httpVerb.POST, postData2);
                        


                        // end of message

                    }

                    
                }
                catch (Exception exce)
                {

                    Console.WriteLine($"Barcode already scanned: {potentialBC}");

                    // send message to vorne to overlay an image +0

                    string message = $"http://{VORNEIP}/api/v0/scoreboard/overlay";
                    byte[] postData = Encoding.ASCII.GetBytes($"{{\"duration\":3,\"text\":[\"+0\",\"{potentialBC}\",\"Already scanned!\"]}}");

              

                    
                    RestClient rc = new RestClient();

                    rc.makeRequest(message, httpVerb.POST, postData);
                    



                    // end of message

                }
                finally
                {

                    keyPressesMutex.ReleaseMutex();

                    if (this.keyPresses.Length > 0)
                        this.keyPresses = "";
                }

            }
            
        }




        private char formatKeyPress(System.Windows.Forms.Keys key)
        {
            switch (key)
            {
                case System.Windows.Forms.Keys.Enter:
                    return '\n';
                case System.Windows.Forms.Keys.Back:
                    return '~';
                case System.Windows.Forms.Keys.A:
                    return 'A';
                case System.Windows.Forms.Keys.B:
                    return 'B';
                case System.Windows.Forms.Keys.C:
                    return 'C';
                case System.Windows.Forms.Keys.D:
                    return 'D';
                case System.Windows.Forms.Keys.E:
                    return 'E';
                case System.Windows.Forms.Keys.F:
                    return 'F';
                case System.Windows.Forms.Keys.G:
                    return 'G';
                case System.Windows.Forms.Keys.H:
                    return 'H';
                case System.Windows.Forms.Keys.I:
                    return 'I';
                case System.Windows.Forms.Keys.J:
                    return 'J';
                case System.Windows.Forms.Keys.K:
                    return 'K';
                case System.Windows.Forms.Keys.L:
                    return 'L';
                case System.Windows.Forms.Keys.M:
                    return 'M';
                case System.Windows.Forms.Keys.N:
                    return 'N';
                case System.Windows.Forms.Keys.O:
                    return 'O';
                case System.Windows.Forms.Keys.P:
                    return 'P';
                case System.Windows.Forms.Keys.Q:
                    return 'Q';
                case System.Windows.Forms.Keys.R:
                    return 'R';
                case System.Windows.Forms.Keys.S:
                    return 'S';
                case System.Windows.Forms.Keys.T:
                    return 'T';
                case System.Windows.Forms.Keys.U:
                    return 'U';
                case System.Windows.Forms.Keys.V:
                    return 'V';
                case System.Windows.Forms.Keys.W:
                    return 'W';
                case System.Windows.Forms.Keys.X:
                    return 'X';
                case System.Windows.Forms.Keys.Y:
                    return 'Y';
                case System.Windows.Forms.Keys.Z:
                    return 'Z';
                case System.Windows.Forms.Keys.D0:
                case System.Windows.Forms.Keys.NumPad0:
                    return '0';
                case System.Windows.Forms.Keys.D1:
                case System.Windows.Forms.Keys.NumPad1:
                    return '1';
                case System.Windows.Forms.Keys.D2:
                case System.Windows.Forms.Keys.NumPad2:
                    return '2';
                case System.Windows.Forms.Keys.D3:
                case System.Windows.Forms.Keys.NumPad3:
                    return '3';
                case System.Windows.Forms.Keys.D4:
                case System.Windows.Forms.Keys.NumPad4:
                    return '4';
                case System.Windows.Forms.Keys.D5:
                case System.Windows.Forms.Keys.NumPad5:
                    return '5';
                case System.Windows.Forms.Keys.D6:
                case System.Windows.Forms.Keys.NumPad6:
                    return '6';
                case System.Windows.Forms.Keys.D7:
                case System.Windows.Forms.Keys.NumPad7:
                    return '7';
                case System.Windows.Forms.Keys.D8:
                case System.Windows.Forms.Keys.NumPad8:
                    return '8';
                case System.Windows.Forms.Keys.D9:
                case System.Windows.Forms.Keys.NumPad9:
                    return '9';
                default:
                    return '!';
            }
        }
    }
}
