using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanReason
{
    public struct Data
    {
        public bool active;
        public double elapsed;
        public bool enabled;
        public string information_source;
        public string last_toggled;
        public string name;
        public string performance_impact;
        public int priority;
        public string process_state_reason;
        public string remaining;
    }

    public struct Meta
    {
        public string asset_id;
        public string data_version;
        public string device_id;
    }


    public class PSActive
    {
        public Data data;
        public Meta meta;
    }
}
