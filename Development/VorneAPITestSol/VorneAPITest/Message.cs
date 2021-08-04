using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorneAPITest
{
    public class Message
    {
        public string ps;
        public string color;
        public string pID;
        public int tt;
        public string audioFile;


        public Message(string productionState, string c, string partID, int taktTime, string audiof)
        {
            this.ps = productionState;
            this.color = c;
            this.pID = partID;
            this.tt = taktTime;
            this.audioFile = audiof;
        }
    }
}
