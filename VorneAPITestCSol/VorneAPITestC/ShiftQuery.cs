using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorneAPITestC
{
    namespace ShiftQueryNS
    {
        public struct Data
        {
            public List<List<string>> events;
        }

        public struct Meta
        {
            string asset_id;
            double data_version;
            string device_id;
        }

        public class ShiftQuery
        {
            public Data data;
            public Meta meta;

        }
    }
    
}
